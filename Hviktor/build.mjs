#!/usr/bin/env node
/**
 * CSS + Vite Build Script for Hviktor
 * Usage: node build.mjs [--configuration Debug|Release]
 */

import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import { build } from "../tools/build.mjs";

const __dirname = dirname(fileURLToPath(import.meta.url));

try {
  build({
    projectDir: __dirname,
    enableSass: false,
    requirePostcss: true,
    postcssInput: join(__dirname, "wwwroot/styles/index.css"),
  });
} catch (error) {
  console.error(`Build failed: ${error.message}`);
  process.exit(1);
}
