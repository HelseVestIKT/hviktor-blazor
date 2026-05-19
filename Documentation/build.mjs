#!/usr/bin/env node
/**
 * Build script for Documentation.
 * Usage: node build.mjs [--configuration Debug|Release]
 *
 * Runs in order:
 * - Generates index.html and 404.html from entry.template.html
 * - Compiles CSS
 */

import { dirname } from "node:path";
import { fileURLToPath } from "node:url";

import { build } from "../tools/build.mjs";
import { generateHtmlEntries } from "./generate-html-entries.mjs";

const __dirname = dirname(fileURLToPath(import.meta.url));

try {
  generateHtmlEntries();
} catch (error) {
  console.error(`HTML generation failed: ${error.message}`);
  process.exit(1);
}

try {
  build({ projectDir: __dirname, requirePostcss: true });
} catch (error) {
  console.error(`CSS build failed: ${error.message}`);
  process.exit(1);
}
