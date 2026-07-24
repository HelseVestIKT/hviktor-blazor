#!/usr/bin/env node

import { existsSync, readFileSync } from "node:fs";
import { basename } from "node:path";
import { log } from "./log.mjs";
import { normalizeSlashes } from "./fs.mjs";
import { getSourceFiles } from "./sources.mjs";

/**
 * Checks whether output files listed in the manifest have changed.
 * @param {object} manifest - Parsed build manifest.
 * @param {object} fileCache - File cache instance.
 * @returns {{needed: boolean, reason?: string}} Whether a rebuild is needed.
 */
export function checkOutputFiles(manifest, fileCache) {
  for (const output of manifest.OutputFiles) {
    if (!existsSync(output.Path)) {
      return { needed: true, reason: `Output missing: ${output.FileName}` };
    }
    if (fileCache.hashFile(output.Path) !== output.Hash) {
      return { needed: true, reason: `Output changed: ${output.FileName}` };
    }
  }
  return { needed: false };
}

/**
 * Checks whether source files listed in the manifest have changed or new files appeared.
 * @param {object} manifest - Parsed build manifest.
 * @param {Array<{pattern: string[], type: string}>} sourcePatterns - Source patterns.
 * @param {object} fileCache - File cache instance.
 * @param {Map} [extraSourceFiles] - Additional source files to check (e.g., Vite sources).
 * @returns {{needed: boolean, reason?: string}} Whether a rebuild is needed.
 */
export function checkSourceFiles(
  manifest,
  sourcePatterns,
  fileCache,
  extraSourceFiles = new Map(),
) {
  const outputPaths = manifest.OutputFiles.map((f) => f.Path);
  const manifestFiles = new Map(
    manifest.SourceFiles.map((f) => [normalizeSlashes(f.Path), f]),
  );

  for (const source of manifest.SourceFiles) {
    if (!existsSync(source.Path)) {
      return { needed: true, reason: `Source removed: ${source.FileName}` };
    }
    const stats = fileCache.getStats(source.Path);
    if (stats.mtime.toISOString() !== source.LastModified) {
      if (fileCache.hashFile(source.Path) !== source.Hash) {
        return { needed: true, reason: `Source changed: ${source.FileName}` };
      }
    }
  }

  const currentFiles = getSourceFiles(sourcePatterns, outputPaths);
  for (const [path] of extraSourceFiles) {
    if (!currentFiles.has(path)) {
      currentFiles.set(path, extraSourceFiles.get(path));
    }
  }

  for (const [path] of currentFiles) {
    if (!manifestFiles.has(path)) {
      return { needed: true, reason: `Source added: ${basename(path)}` };
    }
  }

  return { needed: false };
}

/**
 * Full change detection: checks manifest existence, config, outputs, and sources.
 * @param {object} options - Options.
 * @param {string} options.manifestPath - Path to `build-manifest.json`.
 * @param {string} options.configuration - Current build configuration.
 * @param {Array} options.sourcePatterns - Source file patterns.
 * @param {object} options.fileCache - File cache instance.
 * @param {Map} [options.extraSourceFiles] - Additional source files (e.g., Vite).
 * @returns {{needed: boolean, reason?: string}} Whether a rebuild is needed.
 */
export function checkIfBuildNeeded({
  manifestPath,
  configuration,
  sourcePatterns,
  fileCache,
  extraSourceFiles = new Map(),
}) {
  if (!existsSync(manifestPath)) {
    return { needed: true, reason: "No manifest found" };
  }

  try {
    const manifest = JSON.parse(readFileSync(manifestPath, "utf8"));
    if (manifest.BuildInfo.Config !== configuration) {
      return {
        needed: true,
        reason: `Configuration changed: ${manifest.BuildInfo.Config} -> ${configuration}`,
      };
    }

    const outputResult = checkOutputFiles(manifest, fileCache);
    if (outputResult.needed) {
      return outputResult;
    }

    const sourceResult = checkSourceFiles(
      manifest,
      sourcePatterns,
      fileCache,
      extraSourceFiles,
    );
    if (sourceResult.needed) {
      return sourceResult;
    }

    log.success("Build not needed - all files are up to date");
    log.info(`Last build: ${manifest.BuildInfo.EndTime}`);
    manifest.OutputFiles.forEach((o) =>
      log.info(`Output: ${o.FileName} (${o.SizeKB} KB)`),
    );
    return { needed: false };
  } catch (e) {
    return { needed: true, reason: `Manifest error: ${e.message}` };
  }
}
