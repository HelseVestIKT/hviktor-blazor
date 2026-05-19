#!/usr/bin/env node
/**
 * CSS Build Script for Documentation
 * Usage: node build.mjs [--configuration Debug|Release]
 */

import { dirname } from "node:path";
import { fileURLToPath } from "node:url";

import { build } from "../tools/build.mjs";

const __dirname = dirname(fileURLToPath(import.meta.url));

try {
  build({ projectDir: __dirname, requirePostcss: true });
} catch (error) {
  console.error(`CSS build failed: ${error.message}`);
  process.exit(1);
}
