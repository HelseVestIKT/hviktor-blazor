#!/usr/bin/env node

import { basename, extname, join } from "node:path";
import { existsSync } from "node:fs";
import { findFiles, normalizeSlashes } from "./fs.mjs";

/**
 * Returns a human-readable file type string for script files based on extension.
 * @param {string} ext File extension (e.g., `.ts`).
 * @returns {string} Human-readable file type.
 */
export function getScriptFileType(ext) {
  switch (ext) {
    case ".ts":
      return "TypeScript";
    case ".tsx":
      return "TypeScript React";
    case ".js":
      return "JavaScript";
    case ".jsx":
      return "JavaScript React";
    default:
      return "Script";
  }
}

/**
 * Scans for source files matching the given patterns, excluding specified paths.
 * @param {Array<{pattern: string[], type: string}>} sourcePatterns Patterns to match.
 * @param {string[]} [excludePaths] Paths to exclude.
 * @returns {Map<string, {path: string, name: string, type: string}>} Source file map keyed by normalized path.
 */
export function getSourceFiles(sourcePatterns, excludePaths = []) {
  const excluded = new Set(excludePaths.map(normalizeSlashes));
  const files = new Map();

  for (const { pattern, type } of sourcePatterns) {
    for (const file of findFiles(pattern)) {
      if (!excluded.has(file)) {
        files.set(file, { path: file, name: basename(file), type });
      }
    }
  }

  return files;
}

/**
 * Scans directories for Vite-related source files (TS/JS) and config files.
 * @param {string} projectDir Project directory (`__dirname` of the calling script).
 * @param {string[]} sourceDirs Relative directory names to scan for scripts.
 * @param {string[]} configFiles Config filenames to track.
 * @returns {Map<string, {path: string, name: string, type: string}>} Vite source file map.
 */
export function getViteSourceFiles(projectDir, sourceDirs, configFiles) {
  const files = new Map();

  for (const dir of sourceDirs) {
    const absDir = join(projectDir, dir);
    if (!existsSync(absDir)) {
      continue;
    }

    for (const file of findFiles([absDir, "**/*.{ts,tsx,js,jsx}"])) {
      files.set(file, { path: file, name: basename(file), type: "script" });
    }
  }

  for (const configFile of configFiles) {
    const absPath = normalizeSlashes(join(projectDir, configFile));
    if (existsSync(absPath)) {
      files.set(absPath, { path: absPath, name: configFile, type: "config" });
    }
  }

  return files;
}
