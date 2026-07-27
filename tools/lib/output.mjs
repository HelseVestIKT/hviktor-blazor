#!/usr/bin/env node

import { existsSync } from "node:fs";
import { basename, extname, relative } from "node:path";
import { log } from "./log.mjs";
import { collectDistFiles, normalizeSlashes } from "./fs.mjs";

/**
 * Collects output file metadata for CSS build outputs.
 * @param {string[]} outputPaths Absolute paths to output files.
 * @param {string} rootPath Repository root for relative paths.
 * @param {object} fileCache File cache instance.
 * @returns {object[]} Output file metadata array.
 */
export function collectOutputFiles(outputPaths, rootPath, fileCache) {
  return outputPaths.filter(existsSync).map((filePath) => {
    const stats = fileCache.getStats(filePath);
    const sizeKB = Number((stats.size / 1024).toFixed(2));
    log.info(`Found: ${basename(filePath)} (${sizeKB} KB)`);
    return {
      Path: normalizeSlashes(filePath),
      RelativePath: normalizeSlashes(relative(rootPath, filePath)),
      FileName: basename(filePath),
      SizeBytes: stats.size,
      SizeKB: sizeKB,
      LastModified: stats.mtime.toISOString(),
      Hash: fileCache.hashFile(filePath),
      Type: filePath.endsWith(".min.css") ? "PostCSS Output" : "Sass Output",
    };
  });
}

/**
 * Collects Vite dist output files with metadata for the manifest.
 * @param {string} distDir Vite output directory.
 * @param {string} rootPath Repository root for relative paths.
 * @param {object} fileCache File cache instance.
 * @returns {object[]} Output file metadata array.
 */
export function collectViteOutputFiles(distDir, rootPath, fileCache) {
  const viteOutputs = [];
  if (!existsSync(distDir)) {
    return viteOutputs;
  }

  for (const filePath of collectDistFiles(distDir)) {
    fileCache.clear(filePath);
    const stats = fileCache.getStats(filePath);
    const sizeKB = Number((stats.size / 1024).toFixed(2));
    const ext = extname(filePath);
    let fileType = "Vite Asset";
    if (ext === ".js") {
      fileType = "Vite JavaScript";
    } else if (ext === ".css") {
      fileType = "Vite CSS Asset";
    } else if (ext === ".json") {
      fileType = "Vite Manifest";
    }

    log.info(`Found: ${basename(filePath)} (${sizeKB} KB) [${fileType}]`);
    viteOutputs.push({
      Path: normalizeSlashes(filePath),
      RelativePath: normalizeSlashes(relative(rootPath, filePath)),
      FileName: basename(filePath),
      SizeBytes: stats.size,
      SizeKB: sizeKB,
      LastModified: stats.mtime.toISOString(),
      Hash: fileCache.hashFile(filePath),
      Type: fileType,
    });
  }

  return viteOutputs;
}
