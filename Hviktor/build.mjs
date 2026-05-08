#!/usr/bin/env node
/**
 * CSS + Vite Build Script for Hviktor
 * Usage: node build.mjs [--configuration Debug|Release]
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
} from "../tools/build-utils.mjs";

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const rootPath = dirname(__dirname);
const manifestPath = join(__dirname, "build-manifest.json");
const stylesDir = join(__dirname, "wwwroot/styles");
const distDir = join(__dirname, "wwwroot/dist");
const hasVite = existsSync(join(__dirname, "vite.config.js"));

const configuration = parseConfiguration(process.argv.slice(2));
const fileCache = createFileCache();

const sourcePatterns = [
  { pattern: [stylesDir, "**/*.{css,scss}"], type: "style" },
  { pattern: [__dirname, "Components/**/*.{razor,razor.cs}"], type: "razor" },
];

const viteSourceDirs = [
  "wwwroot/scripts",
  "Components",
  "Models",
  "Services",
  "Utility",
  "Accessors",
  "Reactive",
  "Extensions",
];
const viteConfigFileNames = ["entry.ts", "vite.config.js", "tsconfig.json"];

function build() {
  const startTime = new Date();
  const buildLabel = hasVite ? "CSS + Vite Build" : "CSS Build";
  log.header(`${buildLabel} - ${configuration} Configuration`);

  log.section("Checking if rebuild is needed");
  const viteSourceFiles = hasVite
    ? getViteSourceFiles(__dirname, viteSourceDirs, viteConfigFileNames)
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

  const postcss = runBuildStep(
    postcssStep(stylesDir, rootPath, __dirname, fileCache),
  );
  commandResults.push(postcss);

  const sass = runBuildStep(
    sassStep(stylesDir, rootPath, __dirname, fileCache),
  );
  commandResults.push(sass);

  const npmResult = ensurePnpmDependencies(__dirname);
  if (npmResult) commandResults.push(npmResult);

  const viteResult = buildViteAssets(__dirname, configuration, distDir);
  if (viteResult) commandResults.push(viteResult);

  log.section("Collecting output files");
  const cssOutputs = collectOutputFiles(
    [postcss.outputPath, sass.outputPath],
    rootPath,
    fileCache,
  );
  const viteOutputs = collectViteOutputFiles(distDir, rootPath, fileCache);
  const outputFiles = [...cssOutputs, ...viteOutputs];

  log.section("Generating manifest");
  const allOutputPaths = outputFiles.map((f) => f.Path);
  const sourceFileMap = getSourceFiles(sourcePatterns, allOutputPaths);
  for (const [path, info] of viteSourceFiles) {
    if (!sourceFileMap.has(path)) sourceFileMap.set(path, info);
  }

  const tailwindFile = normalizeSlashes(
    postcss.outputPath.replace(".min.css", ".css"),
  );
  const sassFile = normalizeSlashes(sass.outputPath.replace(".css", ".scss"));

  const manifest = generateManifest({
    sourceFileMap,
    tailwindFile,
    sassFile,
    commandResults,
    outputFiles,
    configuration,
    rootPath,
    scriptPath: __filename,
    fileCache,
    version: "2.0",
  });

  finalizeManifest(manifest, manifestPath, startTime, buildLabel);
}

try {
  build();
} catch (error) {
  log.error(`Build failed: ${error.message}`);
  process.exit(1);
}
