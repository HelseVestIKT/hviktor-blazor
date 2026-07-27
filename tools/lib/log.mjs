#!/usr/bin/env node

const color = (c, msg) => `\x1b[${c}m${msg}\x1b[0m`;

/** Structured logger with colored console output. */
export const log = {
  header: (msg) =>
    console.log(
      `\n${color(36, "=".repeat(60))}\n${color(36, msg)}\n${color(36, "=".repeat(60))}`,
    ),
  section: (msg) => console.log(`\n${color(33, "► " + msg)}`),
  info: (msg) => console.log(`  ${msg}`),
  success: (msg) => console.log(color(32, "✓ " + msg)),
  warning: (msg) => console.log(color(33, "⚠ " + msg)),
  error: (msg) => console.error(color(31, "✗ " + msg)),
  debug: (msg) => console.log(color(2, "  " + msg)),
};
