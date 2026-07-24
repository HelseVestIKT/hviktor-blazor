#!/usr/bin/env node

import { log } from "./log.mjs";

/**
 * Parses `--configuration` (or `-c`) from CLI arguments.
 * @param {string[]} argv - Process arguments (typically `process.argv.slice(2)`).
 * @returns {"Debug"|"Release"} Resolved configuration name.
 */
export function parseConfiguration(argv) {
  const configIndex = argv.findIndex(
    (a) => a === "--configuration" || a === "-c",
  );
  const configuration = configIndex >= 0 ? argv[configIndex + 1] : "Debug";

  if (!["Debug", "Release"].includes(configuration)) {
    log.error("Invalid configuration. Use Debug or Release.");
    process.exit(1);
  }

  return configuration;
}
