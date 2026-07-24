#!/usr/bin/env node

import { createHash } from "node:crypto";
import { existsSync, readFileSync, readdirSync, statSync } from "node:fs";
import { extname, join } from "node:path";
import { glob } from "glob";

/** Normalizes path separators to forward slashes. */
export const normalizeSlashes = (p) => p.replaceAll("\\", "/");

const toGlob = (p) => normalizeSlashes(join(...[p].flat()));

/**
 * Creates a file cache to avoid duplicate stat/hash calls.
 * @returns {object} Cache with `clear`, `hashFile`, and `getStats` methods.
 */
export function createFileCache() {
  const cache = new Map();

  const getFileInfo = (filePath) => {
    const normalized = normalizeSlashes(filePath);
    if (!cache.has(normalized)) {
      const stats = statSync(filePath);
      const hash = createHash("sha256")
        .update(readFileSync(filePath))
        .digest("hex")
        .toUpperCase();
      cache.set(normalized, { stats, hash });
    }
    return cache.get(normalized);
  };

  return {
    clear: (filePath) => cache.delete(normalizeSlashes(filePath)),
    hashFile: (filePath) => getFileInfo(filePath).hash,
    getStats: (filePath) => getFileInfo(filePath).stats,
  };
}

/**
 * Finds files matching a glob pattern.
 * @param {string|string[]} pattern - Glob pattern or path segments.
 * @returns {string[]} Normalized file paths.
 */
export const findFiles = (pattern) =>
  glob.sync(toGlob(pattern), { nodir: true }).map(normalizeSlashes);

/**
 * Finds a file in a directory whose content matches a regex.
 * @param {string} dir - Directory to search.
 * @param {string} pattern - Glob pattern for filenames.
 * @param {RegExp} contentMatch - Regex to match file content.
 * @returns {string|null} Matched file path, or null.
 */
export function findFileWithContent(dir, pattern, contentMatch) {
  for (const file of findFiles([dir, pattern])) {
    if (contentMatch.test(readFileSync(file, "utf8"))) {
      return file;
    }
  }
  return null;
}

/**
 * Recursively collects files from a directory matching specified extensions.
 * @param {string} dir - Directory to scan.
 * @param {string[]} [exts] - File extensions to match.
 * @returns {string[]} Normalized file paths.
 */
export function collectDistFiles(dir, exts = [".js", ".css", ".json"]) {
  const results = [];
  if (!existsSync(dir)) {
    return results;
  }

  for (const entry of readdirSync(dir, { withFileTypes: true })) {
    const fullPath = join(dir, entry.name);
    if (entry.isDirectory()) {
      results.push(...collectDistFiles(fullPath, exts));
    } else if (exts.includes(extname(entry.name))) {
      results.push(normalizeSlashes(fullPath));
    }
  }
  return results;
}
