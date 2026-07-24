#!/usr/bin/env node

import { execFileSync } from "node:child_process";
import { existsSync } from "node:fs";
import { basename, dirname, join, relative } from "node:path";
import { log } from "./log.mjs";
import { findFiles, findFileWithContent, normalizeSlashes } from "./fs.mjs";

/**
 * Runs a command, restricted to an allow-list of permitted executables.
 * @param {string} cmd - Command name (must be in `allowedCommands`).
 * @param {string[]} cmdArgs - Arguments.
 * @param {string} description - Human-readable description for error messages.
 * @param {string} cwd - Working directory.
 * @param {string[]} [allowedCommands] - Allowed command names.
 * @returns {{success: boolean, exitCode: number}} Result.
 */
export function runCommand(
  cmd,
  cmdArgs,
  description,
  cwd,
  allowedCommands = ["npx"],
) {
  if (!allowedCommands.includes(cmd)) {
    throw new Error(`Disallowed command: ${cmd}`);
  }

  const isWindows = process.platform === "win32";
  const safeArgs = cmdArgs
    .map(normalizeSlashes)
    .map((arg) => (isWindows && arg.includes(" ") ? `"${arg}"` : arg));

  log.debug(`Command: ${cmd} ${safeArgs.join(" ")}`);
  try {
    execFileSync(cmd, safeArgs, { stdio: "pipe", cwd, shell: isWindows });
    return { success: true, exitCode: 0 };
  } catch (e) {
    const stderr = e.stderr?.toString().trim();
    log.error(`${description} failed: ${stderr || e.message}`);
    return { success: false, exitCode: e.status || 1 };
  }
}

/**
 * Runs a CSS build step (PostCSS or Sass) and tracks timing and results.
 * @param {object} options - Build step options.
 * @param {string} options.name - Step name (e.g., "PostCSS (Tailwind)").
 * @param {() => string|null} options.findInput - Function returning the input file path.
 * @param {(input: string, output: string) => string[]} options.getArgs - Function returning npx arguments.
 * @param {(input: string) => string} options.getOutput - Function returning the output file path.
 * @param {string} options.rootPath - Repository root for relative paths.
 * @param {string} options.cwd - Working directory for the command.
 * @param {object} options.fileCache - File cache instance from `createFileCache`.
 * @returns {object} Build step result with timing and paths.
 */
export function runBuildStep({
  name,
  findInput,
  getArgs,
  getOutput,
  rootPath,
  cwd,
  fileCache,
}) {
  log.section(`Compiling ${name}`);
  const start = new Date();

  const inputFile = findInput();
  if (!inputFile) {
    log.error(`Could not find ${name} entry file`);
    process.exit(1);
  }

  const outputFile = getOutput(inputFile);
  const inputRelative = normalizeSlashes(relative(rootPath, inputFile));
  const outputRelative = normalizeSlashes(relative(rootPath, outputFile));

  log.info(`Input:  ${inputRelative}`);
  log.info(`Output: ${outputRelative}`);

  const result = runCommand("npx", getArgs(inputFile, outputFile), name, cwd);
  if (!result.success) {
    process.exit(1);
  }

  fileCache.clear(outputFile);

  const end = new Date();
  const duration = (end - start) / 1000;
  log.success(`${name} completed in ${duration.toFixed(2)}s`);

  return {
    Command: name.toLowerCase(),
    Step: `Compile ${name}`,
    InputFile: inputRelative,
    OutputFile: outputRelative,
    StartTime: start.toISOString(),
    EndTime: end.toISOString(),
    DurationSeconds: Number(duration.toFixed(2)),
    Success: true,
    ExitCode: 0,
    outputPath: outputFile,
  };
}

/**
 * Standard PostCSS build step configuration.
 * @param {string} stylesDir - Styles directory.
 * @param {string} rootPath - Repository root.
 * @param {string} cwd - Working directory.
 * @param {object} fileCache - File cache instance.
 * @param {string} [explicitInput] - Optional explicit input file, bypassing content-based detection.
 * @returns {object} Options for `runBuildStep`.
 */
export function postcssStep(
  stylesDir,
  rootPath,
  cwd,
  fileCache,
  explicitInput,
) {
  return {
    name: "PostCSS",
    findInput: () =>
      explicitInput && existsSync(explicitInput)
        ? normalizeSlashes(explicitInput)
        : findFileWithContent(
            stylesDir,
            "**/*.{css,scss}",
            /@import\s+['"]tailwindcss['"]/,
          ),
    getArgs: (input, output) => ["postcss", input, "-o", output],
    getOutput: (input) =>
      normalizeSlashes(
        join(
          dirname(input),
          basename(input).replace(/\.scss$|\.css$/, ".min.css"),
        ),
      ),
    rootPath,
    cwd,
    fileCache,
  };
}

/**
 * Standard Sass build step configuration.
 * @param {string} stylesDir - Styles directory.
 * @param {string} rootPath - Repository root.
 * @param {string} cwd - Working directory.
 * @param {object} fileCache - File cache instance.
 * @returns {object} Options for `runBuildStep`.
 */
export function sassStep(stylesDir, rootPath, cwd, fileCache) {
  return {
    name: "Sass",
    findInput: () =>
      findFiles([stylesDir, "**/index.scss"])[0] ||
      findFiles([stylesDir, "*.scss"]).find(
        (f) => !basename(f).startsWith("_"),
      ),
    getArgs: (input, output) => [
      "sass",
      "--load-path=node_modules",
      input,
      output,
      "--no-source-map",
      "--style=compressed",
    ],
    getOutput: (input) =>
      normalizeSlashes(join(dirname(input), basename(input, ".scss") + ".css")),
    rootPath,
    cwd,
    fileCache,
  };
}

/**
 * Ensures pnpm dependencies are installed. Skips if `node_modules` already exists.
 * Use in development builds only. For production, use `cleanInstallPnpmDependencies`.
 * @param {string} projectDir - Project directory.
 * @returns {object|null} Command result for the manifest, or null if skipped.
 */
export function ensurePnpmDependencies(projectDir) {
  const nodeModulesPath = join(projectDir, "node_modules");

  if (existsSync(nodeModulesPath)) {
    log.debug("node_modules exists, skipping pnpm install");
    return null;
  }

  log.section("Installing pnpm dependencies");
  log.warning("node_modules not found. Running pnpm install...");
  const start = new Date();

  const result = runCommand("pnpm", ["install"], "pnpm install", projectDir, [
    "npx",
    "pnpm",
    "npm",
  ]);
  if (!result.success) {
    log.error("pnpm install failed");
    process.exit(1);
  }

  const end = new Date();
  const duration = (end - start) / 1000;
  log.success(`pnpm install completed in ${duration.toFixed(2)}s`);

  return {
    Command: "pnpm install",
    Step: "Install pnpm dependencies",
    StartTime: start.toISOString(),
    EndTime: end.toISOString(),
    DurationSeconds: Number(duration.toFixed(2)),
    Success: true,
    ExitCode: 0,
  };
}

/**
 * Runs a clean pnpm install using `--frozen-lockfile` (equivalent to `npm ci`).
 * Always installs fresh, ignoring any existing `node_modules`. Use in production builds.
 * @param {string} projectDir - Project directory.
 * @returns {object} Command result for the manifest.
 */
export function cleanInstallPnpmDependencies(projectDir) {
  log.section("Installing pnpm dependencies (clean)");
  const start = new Date();

  const result = runCommand(
    "pnpm",
    ["install", "--frozen-lockfile"],
    "pnpm install --frozen-lockfile",
    projectDir,
    ["npx", "pnpm", "npm"],
  );
  if (!result.success) {
    log.error("pnpm clean install failed");
    process.exit(1);
  }

  const end = new Date();
  const duration = (end - start) / 1000;
  log.success(`pnpm clean install completed in ${duration.toFixed(2)}s`);

  return {
    Command: "pnpm install --frozen-lockfile",
    Step: "Clean install pnpm dependencies",
    StartTime: start.toISOString(),
    EndTime: end.toISOString(),
    DurationSeconds: Number(duration.toFixed(2)),
    Success: true,
    ExitCode: 0,
  };
}

/**
 * Builds Vite assets (JS/TS bundling).
 * @param {string} projectDir - Project directory.
 * @param {string} configuration - "Debug" or "Release".
 * @param {string} distDir - Expected output directory for verification.
 * @returns {object|null} Command result for the manifest, or null if no `vite.config.js` found.
 */
export function buildViteAssets(projectDir, configuration, distDir) {
  if (!existsSync(join(projectDir, "vite.config.js"))) {
    return null;
  }

  log.section("Building Vite assets");
  const start = new Date();
  const viteMode = configuration === "Release" ? "production" : "development";

  log.info(`Mode: ${viteMode}`);

  const result = runCommand(
    "npx",
    ["vite", "build", "--mode", viteMode],
    "Vite build",
    projectDir,
  );
  if (!result.success) {
    log.error("Vite build failed");
    process.exit(1);
  }

  if (!existsSync(distDir)) {
    log.error("Vite build verification failed: dist directory not created");
    process.exit(1);
  }

  const end = new Date();
  const duration = (end - start) / 1000;
  log.success(`Vite build completed in ${duration.toFixed(2)}s`);

  return {
    Command: `npx vite build --mode ${viteMode}`,
    Step: "Build Vite assets",
    Mode: viteMode,
    StartTime: start.toISOString(),
    EndTime: end.toISOString(),
    DurationSeconds: Number(duration.toFixed(2)),
    Success: true,
    ExitCode: 0,
  };
}
