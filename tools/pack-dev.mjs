#!/usr/bin/env node

/**
 * Packs all Hviktor NuGet projects using dotnet pack.
 *
 * Usage:
 *   pnpm pack:dev -- --version 1.2.0
 *   pnpm pack:dev -- --version 1.2.0 --output ./dist --configfile nuget.custom.config
 */

import { spawnSync } from "child_process";
import { resolve, dirname } from "path";
import { fileURLToPath } from "url";
import { homedir } from "os";
import { rmSync, existsSync } from "fs";

const ROOT = resolve(dirname(fileURLToPath(import.meta.url)), "..");

const PROJECTS = [
  "Hviktor.Icons.Abstractions/Hviktor.Icons.Abstractions.csproj",
  "Hviktor.Abstractions/Hviktor.Abstractions.csproj",
  "Hviktor.Extensions/Hviktor.Extensions.csproj",
  "Hviktor.Icons/Hviktor.Icons.csproj",
  "Hviktor/Hviktor.csproj",
];

const args = process.argv.slice(2);

let version = "";
let configfile = "nuget.Development.config";

// Default output: per-user local feed
const LOCAL_FEED = resolve(homedir(), ".nuget", "local-packages");

// Resolve NuGet global cache to clear stale package versions
const nugetResult = spawnSync(
  "dotnet",
  ["nuget", "locals", "global-packages", "--list"],
  {
    shell: true,
    encoding: "utf8",
    cwd: ROOT,
  },
);
let globalCache = "";
if (nugetResult.status === 0) {
  const match = nugetResult.stdout.match(/global-packages:\s*(.+)/);
  if (match) globalCache = match[1].trim();
}

let customOutput = "";

for (let i = 0; i < args.length; i++) {
  if (args[i] === "--version") version = args[++i];
  else if (args[i] === "--output") customOutput = args[++i];
  else if (args[i] === "--configfile") configfile = args[++i];
  else {
    console.error(`Unknown argument: ${args[i]}`);
    console.error(
      "Usage: pnpm pack:dev -- --version <version> [--output <path>] [--configfile <path>]",
    );
    process.exit(1);
  }
}

if (!version) {
  console.error("Error: --version is required.");
  console.error(
    "Usage: pnpm pack:dev -- --version <version> [--output <path>] [--configfile <path>]",
  );
  process.exit(1);
}

// Clear all Hviktor packages from global cache upfront to avoid NU1403 hash mismatch
if (globalCache) {
  for (const project of PROJECTS) {
    const packageName = project.split("/")[0].toLowerCase();
    const cachedPath = resolve(globalCache, packageName, version);
    if (existsSync(cachedPath)) {
      console.log(
        `Clearing cached ${packageName}@${version} from global cache...`,
      );
      rmSync(cachedPath, { recursive: true, force: true });
    }
  }
}

// Restore all projects to ensure consistent package graph after cache clear
console.log("Restoring all projects...");
const restoreResult = spawnSync(
  "dotnet",
  ["restore", "--configfile", configfile],
  { stdio: "inherit", shell: true, cwd: ROOT },
);
if (restoreResult.status !== 0) {
  console.error("Restore failed.");
  process.exit(restoreResult.status ?? 1);
}

for (const project of PROJECTS) {
  const packageName = project.split("/")[0].toLowerCase();
  const output = customOutput || LOCAL_FEED;

  console.log(`Packing ${project} -> ${output}`);
  const result = spawnSync(
    "dotnet",
    [
      "pack",
      project,
      "-c",
      "Release",
      `-p:Version=${version}`,
      "--output",
      output,
      "--configfile",
      configfile,
    ],
    { stdio: "inherit", shell: true, cwd: ROOT },
  );
  if (result.status !== 0) {
    console.error(`Failed to pack ${project}`);
    process.exit(result.status ?? 1);
  }
}

console.log("\nAll packages packed.");
