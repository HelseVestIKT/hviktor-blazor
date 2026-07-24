#!/usr/bin/env node
/**
 * Reusable CSS + Vite Build Script
 *
 * Can be invoked directly or imported as a library.
 * When imported, call `build(options)`, `buildDev(options)`, or `buildProd(options)`.
 *
 * @example
 * ```js
 * import { build } from "../tools/build.mjs";
 * build({ projectDir: __dirname });
 * ```
 */

import { existsSync } from "node:fs";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";
import { log } from "./lib/log.mjs";
import { createFileCache, normalizeSlashes } from "./lib/fs.mjs";
import { parseConfiguration } from "./lib/config.mjs";
import { getSourceFiles, getViteSourceFiles } from "./lib/sources.mjs";
import { checkIfBuildNeeded } from "./lib/detection.mjs";
import {
  runBuildStep,
  postcssStep,
  sassStep,
  ensurePnpmDependencies,
  cleanInstallPnpmDependencies,
  buildViteAssets,
} from "./lib/steps.mjs";
import { collectOutputFiles, collectViteOutputFiles } from "./lib/output.mjs";
import { generateManifest, finalizeManifest } from "./lib/manifest.mjs";

// --- Shared helpers ---

/**
 * Resolves common path and cache values from raw options.
 * @param {object} options - Raw build options.
 * @returns {object} Resolved context shared by dev and prod builds.
 */
function resolveContext(options) {
  const {
    projectDir,
    rootPath = dirname(projectDir),
    sourcePatterns: customSourcePatterns,
    viteSourceDirs = [
      "wwwroot/scripts",
      "Components",
      "Models",
      "Services",
      "Utility",
      "Accessors",
      "Reactive",
      "Extensions",
    ],
    viteConfigFileNames = ["entry.ts", "vite.config.js", "tsconfig.json"],
    enableVite = existsSync(join(projectDir, "vite.config.js")),
    requirePostcss = false,
    enableSass = true,
    postcssInput,
    configuration,
  } = options;

  const manifestVersion =
    options.manifestVersion ?? (enableVite ? "2.0" : "1.5");
  const manifestPath = join(projectDir, "build-manifest.json");
  const stylesDir = join(projectDir, "wwwroot/styles");
  const distDir = join(projectDir, "wwwroot/dist");
  const fileCache = createFileCache();

  const sourcePatterns = customSourcePatterns ?? [
    { pattern: [stylesDir, "**/*.{css,scss}"], type: "style" },
    {
      pattern: [projectDir, "Components/**/*.{razor,razor.cs}"],
      type: "razor",
    },
  ];

  return {
    projectDir,
    rootPath,
    configuration,
    manifestVersion,
    manifestPath,
    stylesDir,
    distDir,
    fileCache,
    sourcePatterns,
    viteSourceDirs,
    viteConfigFileNames,
    enableVite,
    requirePostcss,
    enableSass,
    postcssInput,
  };
}

/**
 * Runs the PostCSS step and pushes the result into `commandResults`.
 * @param {object} ctx - Resolved build context.
 * @param {Array} commandResults - Mutable results array.
 * @returns {object|null} PostCSS result, or null if skipped.
 */
function runPostcssBuild(ctx, commandResults) {
  const options = postcssStep(
    ctx.stylesDir,
    ctx.rootPath,
    ctx.projectDir,
    ctx.fileCache,
    ctx.postcssInput,
  );
  const hasInput = !!options.findInput();

  if (hasInput) {
    const result = runBuildStep(options);
    commandResults.push(result);
    return result;
  }

  if (ctx.requirePostcss) {
    log.error("No PostCSS entry file found but PostCSS is required");
    process.exit(1);
  }

  log.section("PostCSS");
  log.info("No PostCSS entry file found - skipping PostCSS step");
  return null;
}

/**
 * Runs the Sass step and pushes the result into `commandResults`.
 * @param {object} ctx - Resolved build context.
 * @param {Array} commandResults - Mutable results array.
 * @returns {object|null} Sass result, or null if disabled.
 */
function runSassBuild(ctx, commandResults) {
  if (!ctx.enableSass) {
    return null;
  }
  const result = runBuildStep(
    sassStep(ctx.stylesDir, ctx.rootPath, ctx.projectDir, ctx.fileCache),
  );
  commandResults.push(result);
  return result;
}

/**
 * Runs the Vite + pnpm steps and pushes results into `commandResults`.
 * @param {object} ctx - Resolved build context.
 * @param {Array} commandResults - Mutable results array.
 * @param {boolean} [clean=false] - When true, always runs a frozen-lockfile install (for production).
 */
function runViteBuild(ctx, commandResults, clean = false) {
  const installResult = clean
    ? cleanInstallPnpmDependencies(ctx.projectDir)
    : ensurePnpmDependencies(ctx.projectDir);
  if (installResult) {
    commandResults.push(installResult);
  }

  const viteResult = buildViteAssets(
    ctx.projectDir,
    ctx.configuration,
    ctx.distDir,
  );
  if (viteResult) {
    commandResults.push(viteResult);
  }
}

// --- Public API ---

/**
 * Development build: runs incremental change detection, compiles assets, and writes a manifest.
 *
 * @param {object} options - Build options (see `build` for full parameter docs).
 */
export function buildDev(options = {}) {
  const ctx = resolveContext(options);
  const buildLabel = ctx.enableVite ? "CSS + Vite Build" : "CSS Build";

  log.header(`${buildLabel} - Development`);

  const viteSourceFiles = ctx.enableVite
    ? getViteSourceFiles(
        ctx.projectDir,
        ctx.viteSourceDirs,
        ctx.viteConfigFileNames,
      )
    : new Map();

  log.section("Checking if rebuild is needed");
  const buildCheck = checkIfBuildNeeded({
    manifestPath: ctx.manifestPath,
    configuration: ctx.configuration,
    sourcePatterns: ctx.sourcePatterns,
    fileCache: ctx.fileCache,
    extraSourceFiles: viteSourceFiles,
  });

  if (!buildCheck.needed) {
    log.success(`${buildLabel} skipped - no changes detected`);
    process.exit(0);
  }

  log.info(buildCheck.reason);

  const startTime = new Date();
  const commandResults = [];

  const postcss = runPostcssBuild(ctx, commandResults);
  const sass = runSassBuild(ctx, commandResults);
  if (ctx.enableVite) {
    runViteBuild(ctx, commandResults);
  }

  log.section("Collecting output files");
  const cssOutputPaths = [postcss?.outputPath, sass?.outputPath].filter(
    Boolean,
  );
  const cssOutputs = collectOutputFiles(
    cssOutputPaths,
    ctx.rootPath,
    ctx.fileCache,
  );
  const viteOutputs = ctx.enableVite
    ? collectViteOutputFiles(ctx.distDir, ctx.rootPath, ctx.fileCache)
    : [];
  const outputFiles = [...cssOutputs, ...viteOutputs];

  log.section("Generating manifest");
  const allOutputPaths = outputFiles.map((f) => f.Path);
  const sourceFileMap = getSourceFiles(ctx.sourcePatterns, allOutputPaths);
  for (const [path, info] of viteSourceFiles) {
    if (!sourceFileMap.has(path)) {
      sourceFileMap.set(path, info);
    }
  }

  const entryFile = postcss
    ? normalizeSlashes(postcss.outputPath.replace(".min.css", ".css"))
    : undefined;
  const sassFile = sass
    ? normalizeSlashes(sass.outputPath.replace(".css", ".scss"))
    : undefined;

  const manifest = generateManifest({
    sourceFileMap,
    entryFile,
    sassFile,
    commandResults,
    outputFiles,
    configuration: ctx.configuration,
    rootPath: ctx.rootPath,
    scriptPath: join(ctx.projectDir, "build.mjs"),
    fileCache: ctx.fileCache,
    version: ctx.manifestVersion,
  });

  finalizeManifest(manifest, ctx.manifestPath, startTime, buildLabel);
}

/**
 * Production build: compiles all assets without incremental detection or manifest generation.
 * Faster than `buildDev` because it skips all bookkeeping not needed in CI.
 *
 * @param {object} options - Build options (see `build` for full parameter docs).
 */
export function buildProd(options = {}) {
  process.env.NODE_ENV = "production";
  const ctx = resolveContext(options);
  const buildLabel = ctx.enableVite ? "CSS + Vite Build" : "CSS Build";

  log.header(`${buildLabel} - Production`);

  const startTime = new Date();
  const commandResults = [];

  runPostcssBuild(ctx, commandResults);
  runSassBuild(ctx, commandResults);
  if (ctx.enableVite) {
    runViteBuild(ctx, commandResults, true);
  }

  const duration = ((new Date() - startTime) / 1000).toFixed(2);
  log.success(`${buildLabel} completed in ${duration}s`);
  console.log("\n" + "=".repeat(60) + "\n");
}

/**
 * Runs a CSS (+ optional Vite) build for a project.
 * Delegates to `buildDev` for Debug and `buildProd` for Release.
 *
 * @param {object} options - Build configuration.
 * @param {string} options.projectDir - Absolute path to the project directory.
 * @param {string} [options.rootPath] - Absolute path to the repository root. Defaults to parent of projectDir.
 * @param {string} [options.configuration] - "Debug" or "Release". Defaults to parsing process.argv.
 * @param {string} [options.manifestVersion] - Manifest schema version. Defaults to "2.0" with Vite, "1.5" without.
 * @param {Array<{pattern: string[], type: string}>} [options.sourcePatterns] - Source file glob patterns.
 * @param {string[]} [options.viteSourceDirs] - Directories containing Vite source files.
 * @param {string[]} [options.viteConfigFileNames] - Vite-related config file names to watch.
 * @param {boolean} [options.enableVite] - Whether to run Vite build. Defaults to auto-detect via vite.config.js.
 * @param {boolean} [options.requirePostcss] - If true, fail when no PostCSS input is found. Defaults to false.
 * @param {boolean} [options.enableSass] - Whether to run the Sass build step. Defaults to true.
 * @param {string} [options.postcssInput] - Explicit PostCSS entry file path, bypassing content-based detection.
 */
export function build(options = {}) {
  const configuration =
    options.configuration ?? parseConfiguration(process.argv.slice(2));
  const resolved = { ...options, configuration };
  return configuration === "Release" ? buildProd(resolved) : buildDev(resolved);
}

// When run directly, build from the current working directory
const __filename = fileURLToPath(import.meta.url);
const isDirectRun =
  process.argv[1] === __filename ||
  process.argv[1]?.replaceAll("\\", "/") === __filename.replaceAll("\\", "/");

if (isDirectRun) {
  try {
    build({ projectDir: process.cwd() });
  } catch (error) {
    log.error(`Build failed: ${error.message}`);
    process.exit(1);
  }
}
