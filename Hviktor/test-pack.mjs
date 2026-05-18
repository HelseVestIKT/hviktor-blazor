#!/usr/bin/env node
/**
 * test-pack.mjs - Verifies Vite assets are included in the NuGet package.
 *
 * Usage:
 *   node test-pack.mjs [options]
 *
 * Options:
 *   --configuration, -c  Debug|Release  Build configuration (default: Debug)
 *   --skip-build                        Skip clean + Vite build (use existing dist)
 *   --keep-package                      Do not delete the test .nupkg after success
 */

import { execFileSync } from "node:child_process";
import {
  existsSync,
  mkdirSync,
  readdirSync,
  readFileSync,
  rmSync,
  statSync,
  writeFileSync,
} from "node:fs";
import { Dirent } from "node:fs";
import { inflateRawSync } from "node:zlib";
import { basename, dirname, extname, join, relative } from "node:path";
import { fileURLToPath } from "node:url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// Arguments
const argv = process.argv.slice(2);
const flagIndex = argv.findIndex((a) => a === "--configuration" || a === "-c");
const configuration = flagIndex >= 0 ? argv[flagIndex + 1] : "Debug";
const skipBuild = argv.includes("--skip-build");
const keepPackage = argv.includes("--keep-package");

if (!["Debug", "Release"].includes(configuration)) {
  console.error("Invalid --configuration. Use Debug or Release.");
  process.exit(1);
}

// Logging
const color = (code, msg) => `\x1b[${code}m${msg}\x1b[0m`;
const log = {
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

/** Recursively collect files matching the given extensions from a directory. */
function collectFiles(dir, exts = [".js", ".css"]) {
  const results = [];
  if (!existsSync(dir)) {
    return results;
  }

  const entries = /** @type {Dirent[]} */ (
    readdirSync(dir, { withFileTypes: true })
  );
  for (const entry of entries) {
    const full = join(dir, entry.name);
    if (entry.isDirectory()) {
      results.push(...collectFiles(full, exts));
    } else if (exts.includes(extname(entry.name))) {
      results.push(full);
    }
  }
  return results;
}

/**
 * Run an external command, exiting with code 1 on failure.
 * Returns trimmed stdout.
 *
 * NOTE: `shell` is intentionally omitted. execFileSync resolves executables
 * from PATH directly and passes the args array to the process without going
 * through a shell, so paths with spaces are safe without quoting.
 * Using shell:true would cause the shell to re-tokenise the joined arg string,
 * splitting paths that contain spaces (e.g. "Helse Vest IKT").
 */
function run(cmd, args, { label = cmd } = {}) {
  // Quote args with spaces only for the human-readable debug line
  const displayArgs = args.map((a) => (a.includes(" ") ? `"${a}"` : a));
  log.debug(`Command: ${cmd} ${displayArgs.join(" ")}`);
  try {
    const out = execFileSync(cmd, args, {
      cwd: __dirname,
      stdio: ["ignore", "pipe", "pipe"],
    });
    return out?.toString().trim() ?? "";
  } catch (err) {
    const stderr = err.stderr?.toString().trim();
    log.error(`${label} failed: ${stderr || err.message}`);
    throw err; // bubble up so callers inside try/finally can clean up
  }
}

/**
 * Extract a ZIP/nupkg to destDir using only Node built-ins (node:fs + node:zlib).
 * Supports DEFLATE (method 8) and STORED (method 0) entries.
 * Throws a descriptive Error on failure.
 *
 * @param {string} zipPath  - Path to the .zip / .nupkg file.
 * @param {string} destDir  - Directory to extract into (created if absent).
 */
function extractZip(zipPath, destDir) {
  /** @type {Buffer} */
  const buf = Buffer.from(readFileSync(zipPath));

  // Locate End of Central Directory record (signature 0x06054b50).
  // Scan backwards from the end to handle ZIP comment fields.
  const EOCD_SIG = 0x06054b50;
  let eocdOffset = -1;
  for (let i = buf.length - 22; i >= 0; i--) {
    if (buf.readUInt32LE(i) === EOCD_SIG) {
      eocdOffset = i;
      break;
    }
  }
  if (eocdOffset === -1) {
    throw new Error(
      `extractZip: no End-of-Central-Directory record found in ${zipPath}`,
    );
  }

  const cdCount = buf.readUInt16LE(eocdOffset + 10); // total entries in CD
  const cdOffset = buf.readUInt32LE(eocdOffset + 16); // offset of CD start

  const CD_SIG = 0x02014b50;
  let pos = cdOffset;

  for (let i = 0; i < cdCount; i++) {
    if (buf.readUInt32LE(pos) !== CD_SIG) {
      throw new Error(
        `extractZip: invalid Central Directory entry at offset ${pos}`,
      );
    }

    const method = buf.readUInt16LE(pos + 10);
    const compSize = buf.readUInt32LE(pos + 20);
    const uncompSize = buf.readUInt32LE(pos + 24);
    const nameLen = buf.readUInt16LE(pos + 28);
    const extraLen = buf.readUInt16LE(pos + 30);
    const commentLen = buf.readUInt16LE(pos + 32);
    const localOffset = buf.readUInt32LE(pos + 42);
    const name = buf.toString("utf8", pos + 46, pos + 46 + nameLen);

    pos += 46 + nameLen + extraLen + commentLen;

    // Skip directory entries
    if (name.endsWith("/")) {
      continue;
    }

    // Read local file header to find actual data offset
    const LFH_SIG = 0x04034b50;
    if (buf.readUInt32LE(localOffset) !== LFH_SIG) {
      throw new Error(`extractZip: invalid Local File Header for "${name}"`);
    }

    const localNameLen = buf.readUInt16LE(localOffset + 26);
    const localExtraLen = buf.readUInt16LE(localOffset + 28);
    const dataOffset = localOffset + 30 + localNameLen + localExtraLen;

    const compData = buf.subarray(dataOffset, dataOffset + compSize);

    let data;
    if (method === 0) {
      // STORED — no compression
      data = compData;
    } else if (method === 8) {
      // DEFLATE
      data = inflateRawSync(compData);
    } else {
      throw new Error(
        `extractZip: unsupported compression method ${method} for "${name}"`,
      );
    }

    if (data.length !== uncompSize) {
      throw new Error(
        `extractZip: size mismatch for "${name}" (expected ${uncompSize}, got ${data.length})`,
      );
    }

    const outPath = join(destDir, ...name.split("/"));
    mkdirSync(dirname(outPath), { recursive: true });
    writeFileSync(outPath, data);
  }
}

/**
 * Generate a unique test version string.
 * Format: 2.1.99-test-YYYYMMDD-HHmmss
 * Includes the date so back-to-back runs never collide.
 */
function makeTestVersion() {
  const now = new Date();
  const YYYY = now.getFullYear();
  const MM = String(now.getMonth() + 1).padStart(2, "0");
  const DD = String(now.getDate()).padStart(2, "0");
  const hh = String(now.getHours()).padStart(2, "0");
  const mm = String(now.getMinutes()).padStart(2, "0");
  const ss = String(now.getSeconds()).padStart(2, "0");
  return `2.1.99-test-${YYYY}${MM}${DD}-${hh}${mm}${ss}`;
}

/** Elapsed seconds since `start`, formatted to 2 decimal places. */
const elapsed = (start) => ((Date.now() - start) / 1000).toFixed(2);

function main() {
  const totalStart = Date.now();
  log.header(`Testing NuGet Package Creation - ${configuration}`);

  if (skipBuild) {
    log.warning("--skip-build active: skipping clean and Vite build.");
  }

  const distDir = join(__dirname, "wwwroot", "dist");

  // Clean
  if (!skipBuild) {
    const t = Date.now();
    log.section("Step 1: Cleaning");
    run("dotnet", ["clean", "-c", configuration, "-v", "quiet"], {
      label: "dotnet clean",
    });
    if (existsSync(distDir)) {
      rmSync(distDir, { recursive: true, force: true });
      log.info("Removed old dist folder");
    }
    log.success(`Clean complete (${elapsed(t)}s)`);
  } else {
    log.section("Step 1: Cleaning - skipped");
  }

  // Build Vite assets
  if (!skipBuild) {
    const t = Date.now();
    log.section("Step 2: Building Vite assets");
    log.debug(`Running: node build.mjs --configuration ${configuration}`);
    run("node", ["build.mjs", "--configuration", configuration], {
      label: "build.mjs",
    });

    // Fail fast: verify dist was produced before continuing to pack
    if (!existsSync(distDir)) {
      log.error("wwwroot/dist was not created by the build. Aborting.");
      process.exit(1);
    }
    log.success(`Build complete (${elapsed(t)}s)`);
  } else {
    log.section("Step 2: Building Vite assets - skipped");
    if (!existsSync(distDir)) {
      log.error(
        "--skip-build was set but wwwroot/dist does not exist. Run without --skip-build first.",
      );
      process.exit(1);
    }
  }

  // Verify dist files (fail-fast before spending time on pack)
  log.section("Step 3: Checking dist files created by Vite");
  const distFiles = collectFiles(distDir);
  if (distFiles.length === 0) {
    log.error(
      "No .js/.css files found in wwwroot/dist - build produced no output.",
    );
    process.exit(1);
  }
  log.info(`Vite produced ${distFiles.length} file(s) in wwwroot/dist:`);
  for (const f of distFiles) {
    log.debug(`  - ./${relative(__dirname, f).replaceAll("\\", "/")}`);
  }

  // Pack with test version
  const t4 = Date.now();
  log.section("Step 4: Packing with test version");
  const testVersion = makeTestVersion();
  log.info(`Version: ${testVersion}`);

  const nugetOutput = join(__dirname, "..", "..", "./nupkg");
  mkdirSync(nugetOutput, { recursive: true }); // ensure output dir exists

  const packagePath = join(nugetOutput, `Hviktor.${testVersion}.nupkg`);

  try {
    run(
      "dotnet",
      [
        "pack",
        "Hviktor.csproj",
        "-c",
        configuration,
        `-p:PackageVersion=${testVersion}`,
        "--output",
        nugetOutput,
        "-v",
        "quiet",
      ],
      { label: "dotnet pack" },
    );
  } catch {
    process.exit(1);
  }
  log.success(`Pack complete (${elapsed(t4)}s)`);

  if (!existsSync(packagePath)) {
    log.error(`Package not found at expected path: ${packagePath}`);
    log.warning(
      "dotnet pack may have used a different filename. Check ./nupkg/ manually.",
    );
    process.exit(1);
  }
  log.info(`Package: ${packagePath}`);

  // Inspect package contents
  const t5 = Date.now();
  log.section("Step 5: Inspecting package contents");

  const tempDir = join(__dirname, "temp-extract-test");
  if (existsSync(tempDir)) {
    rmSync(tempDir, { recursive: true, force: true });
  }
  mkdirSync(tempDir, { recursive: true });

  let exitCode = 0;

  try {
    try {
      extractZip(packagePath, tempDir);
    } catch (err) {
      log.error(`Failed to extract package: ${err.message}`);
      exitCode = 1;
      return; // jumps to finally
    }

    log.debug("Package extracted");

    const staticAssetsDir = join(tempDir, "staticwebassets");

    if (!existsSync(staticAssetsDir)) {
      log.error("FAILED: staticwebassets folder does not exist in package!");
      log.warning("Available top-level entries in package:");
      const entries = /** @type {Dirent[]} */ (
        readdirSync(tempDir, { withFileTypes: true })
      );
      for (const entry of entries) {
        log.debug(
          `  ${entry.isDirectory() ? "[dir] " : "      "}${entry.name}`,
        );
      }
      log.warning("The IncludeViteAssets MSBuild target may not be running.");
      exitCode = 1;
      return; // jumps to finally
    }

    const packagedFiles = collectFiles(staticAssetsDir);
    log.debug(
      `Package contains ${packagedFiles.length} file(s) in staticwebassets:`,
    );
    for (const f of packagedFiles
      .slice()
      .sort((a, b) => basename(a).localeCompare(basename(b)))) {
      log.debug(`  - ${relative(tempDir, f).replaceAll("\\", "/")}`);
    }

    if (packagedFiles.length === 0) {
      log.error("FAILED: No files found in staticwebassets!");
      log.error(
        "The IncludeViteAssets target did not add files to the package.",
      );
      log.error(
        "Diagnose with: dotnet pack -c Debug -v detailed > pack-log.txt",
      );
      exitCode = 1;
    } else if (packagedFiles.length < distFiles.length) {
      log.warning("WARNING: File count mismatch!");
      log.warning(`  Vite produced: ${distFiles.length} file(s)`);
      log.warning(`  Package has:   ${packagedFiles.length} file(s)`);
      log.warning(
        "Some files were not packaged. Check glob patterns in IncludeViteAssets.",
      );
      exitCode = 1;
    } else {
      log.success(
        `All ${packagedFiles.length} file(s) packaged correctly! (${elapsed(t5)}s)`,
      );
      console.log("");
      log.info("Top 5 packaged files by size:");
      const top5 = packagedFiles
        .map((f) => ({ name: basename(f), sizeKB: statSync(f).size / 1024 }))
        .sort((a, b) => b.sizeKB - a.sizeKB)
        .slice(0, 5);
      for (const { name, sizeKB } of top5) {
        log.debug(`  ${name}: ${sizeKB.toFixed(2)} KB`);
      }
    }
  } finally {
    if (existsSync(tempDir)) {
      rmSync(tempDir, { recursive: true, force: true });
    }

    if (exitCode !== 0) {
      process.exit(exitCode);
    }
  }

  // Cleanup test package (unless --keep-package)
  if (!keepPackage) {
    rmSync(packagePath, { force: true });
    log.debug("Test package removed (pass --keep-package to retain it)");
  }

  // Summary
  log.success(`Test complete - total ${elapsed(totalStart)}s`);
  if (keepPackage) {
    log.info(`Package location: ${packagePath}`);
  }
  console.log("");
}

main();
