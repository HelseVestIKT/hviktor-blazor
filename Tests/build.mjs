#!/usr/bin/env node
/**
 * CSS Build Script for Documentation
 * Usage: node build.mjs [--configuration Debug|Release]
 */

import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import {
  checkIfBuildNeeded,
  collectOutputFiles,
  createFileCache,
  finalizeManifest,
  generateManifest,
  getSourceFiles,
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

const configuration = parseConfiguration(process.argv.slice(2));
const fileCache = createFileCache();

const sourcePatterns = [
  { pattern: [stylesDir, "**/*.{css,scss}"], type: "style" },
  { pattern: [__dirname, "Components/**/*.{razor,razor.cs}"], type: "razor" },
];

if (configuration === "Release") {
  process.env.NODE_ENV = "production";
}

function build() {
  const startTime = new Date();
  log.header(`CSS Build - ${configuration} Configuration`);

  log.section("Checking if rebuild is needed");
  const buildCheck = checkIfBuildNeeded({
    manifestPath,
    configuration,
    sourcePatterns,
    fileCache,
  });

  if (!buildCheck.needed) {
    log.success("CSS build skipped - no changes detected");
    process.exit(0);
  }

  log.info(buildCheck.reason);

  // PostCSS (Tailwind)
  const postcss = runBuildStep(
    postcssStep(stylesDir, rootPath, __dirname, fileCache),
  );

  // Sass
  const sass = runBuildStep(
    sassStep(stylesDir, rootPath, __dirname, fileCache),
  );

  // Collect output files
  log.section("Collecting output files");
  const outputFiles = collectOutputFiles(
    [postcss.outputPath, sass.outputPath],
    rootPath,
    fileCache,
  );

  // Generate manifest
  log.section("Generating manifest");
  const sourceFileMap = getSourceFiles(sourcePatterns, [
    postcss.outputPath,
    sass.outputPath,
  ]);
  const tailwindFile = normalizeSlashes(
    postcss.outputPath.replace(".min.css", ".css"),
  );
  const sassFile = normalizeSlashes(sass.outputPath.replace(".css", ".scss"));

  const manifest = generateManifest({
    sourceFileMap,
    tailwindFile,
    sassFile,
    commandResults: [postcss, sass],
    outputFiles,
    configuration,
    rootPath,
    scriptPath: __filename,
    fileCache,
    version: "1.5",
  });

  finalizeManifest(manifest, manifestPath, startTime, "CSS Build");
}

try {
  build();
} catch (error) {
  log.error(`CSS build failed: ${error.message}`);
  process.exit(1);
}
