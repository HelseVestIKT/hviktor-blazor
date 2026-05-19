#!/usr/bin/env node
/**
 * Reusable CSS + Vite Build Script
 *
 * Can be invoked directly or imported as a library.
 * When imported, call `build(options)` with project-specific configuration.
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

import {
  buildViteAssets,
  checkIfBuildNeeded,
  collectOutputFiles,
  collectViteOutputFiles,
  createFileCache,
  ensurePnpmDependencies,
  finalizeManifest,
  generateManifest,
  getSourceFiles,
  getViteSourceFiles,
  log,
  normalizeSlashes,
  parseConfiguration,
  postcssStep,
  runBuildStep,
  sassStep,
} from "./build-utils.mjs";

/**
 * Runs a CSS (+ optional Vite) build for a project.
 *
 * @param {object} options: Build configuration.
 * @param {string} options.projectDir: Absolute path to the project directory.
 * @param {string} [options.rootPath]: Absolute path to the repository root. Defaults to parent of projectDir.
 * @param {string} [options.configuration]: "Debug" or "Release". Defaults to parsing process.argv.
 * @param {string} [options.manifestVersion]: Manifest schema version. Defaults to "2.0" with Vite, "1.5" without.
 * @param {Array<{pattern: string[], type: string}>} [options.sourcePatterns]: Source file glob patterns.
 * @param {string[]} [options.viteSourceDirs]: Directories containing Vite source files.
 * @param {string[]} [options.viteConfigFileNames]: Vite-related config file names to watch.
 * @param {boolean} [options.enableVite]: Whether to run Vite build. Defaults to auto-detect via vite.config.js.
 * @param {boolean} [options.requirePostcss]: If true, fail when no PostCSS input is found. Defaults to false.
 */
export function build(options = {}) {
  const {
    projectDir,
    rootPath = dirname(projectDir),
    configuration = parseConfiguration(process.argv.slice(2)),
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

  if (configuration === "Release") {
    process.env.NODE_ENV = "production";
  }

  const startTime = new Date();
  const buildLabel = enableVite ? "CSS + Vite Build" : "CSS Build";
  log.header(`${buildLabel} - ${configuration} Configuration`);

  log.section("Checking if rebuild is needed");
  const viteSourceFiles = enableVite
    ? getViteSourceFiles(projectDir, viteSourceDirs, viteConfigFileNames)
    : new Map();

  const buildCheck = checkIfBuildNeeded({
    manifestPath,
    configuration,
    sourcePatterns,
    fileCache,
    extraSourceFiles: viteSourceFiles,
  });

  if (!buildCheck.needed) {
    log.success(`${buildLabel} skipped - no changes detected`);
    process.exit(0);
  }

  log.info(buildCheck.reason);
  const commandResults = [];

  // PostCSS (Tailwind)
  const postcssOptions = postcssStep(
    stylesDir,
    rootPath,
    projectDir,
    fileCache,
  );
  const hasPostcssInput = !!postcssOptions.findInput();
  let postcss = null;

  if (hasPostcssInput) {
    postcss = runBuildStep(postcssOptions);
    commandResults.push(postcss);
  } else if (requirePostcss) {
    log.error("No Tailwind entry file found but PostCSS is required");
    process.exit(1);
  } else {
    log.section("PostCSS (Tailwind)");
    log.info("No Tailwind entry file found - skipping PostCSS step");
  }

  // Sass
  const sass = runBuildStep(
    sassStep(stylesDir, rootPath, projectDir, fileCache),
  );
  commandResults.push(sass);

  // Vite
  if (enableVite) {
    const npmResult = ensurePnpmDependencies(projectDir);
    if (npmResult) commandResults.push(npmResult);

    const viteResult = buildViteAssets(projectDir, configuration, distDir);
    if (viteResult) commandResults.push(viteResult);
  }

  // Collect output files
  log.section("Collecting output files");
  const cssOutputPaths = postcss
    ? [postcss.outputPath, sass.outputPath]
    : [sass.outputPath];
  const cssOutputs = collectOutputFiles(cssOutputPaths, rootPath, fileCache);
  const viteOutputs = enableVite
    ? collectViteOutputFiles(distDir, rootPath, fileCache)
    : [];
  const outputFiles = [...cssOutputs, ...viteOutputs];

  // Generate manifest
  log.section("Generating manifest");
  const allOutputPaths = outputFiles.map((f) => f.Path);
  const sourceFileMap = getSourceFiles(sourcePatterns, allOutputPaths);
  for (const [path, info] of viteSourceFiles) {
    if (!sourceFileMap.has(path)) sourceFileMap.set(path, info);
  }

  const tailwindFile = postcss
    ? normalizeSlashes(postcss.outputPath.replace(".min.css", ".css"))
    : undefined;
  const sassFile = normalizeSlashes(sass.outputPath.replace(".css", ".scss"));

  const manifest = generateManifest({
    sourceFileMap,
    tailwindFile,
    sassFile,
    commandResults,
    outputFiles,
    configuration,
    rootPath,
    scriptPath: join(projectDir, "build.mjs"),
    fileCache,
    version: manifestVersion,
  });

  finalizeManifest(manifest, manifestPath, startTime, buildLabel);
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
