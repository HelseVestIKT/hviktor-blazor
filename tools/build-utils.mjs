#!/usr/bin/env node
import { execFileSync } from "node:child_process";
import { createHash } from "node:crypto";
import {
  existsSync,
  readFileSync,
  readdirSync,
  writeFileSync,
  statSync,
} from "node:fs";
import { basename, dirname, extname, join, relative } from "node:path";
import { glob } from "glob";

/**
 * Creates a file cache to avoid duplicate stat/hash calls.
 * @returns {object} Cache with get, clear, hashFile, and getStats methods.
 */
export function createFileCache() {
  const cache = new Map();

  const normalizeSlashes = (p) => p.replaceAll("\\", "/");

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

// Path helpers
export const normalizeSlashes = (p) => p.replaceAll("\\", "/");
const toGlob = (p) => normalizeSlashes(join(...[p].flat()));

/**
 * Finds files matching a glob pattern.
 * @param {string|string[]} pattern - Glob pattern or path segments.
 * @returns {string[]} Normalized file paths.
 */
export const findFiles = (pattern) =>
  glob.sync(toGlob(pattern), { nodir: true }).map(normalizeSlashes);

// Console logging with colors
const color = (c, msg) => `\x1b[${c}m${msg}\x1b[0m`;

/** @type {object} Structured logger with colored output. */
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

/**
 * Parses --configuration from CLI arguments.
 * @param {string[]} argv - Process arguments (process.argv.slice(2)).
 * @returns {string} "Debug" or "Release".
 */
export function parseConfiguration(argv) {
  const configIndex = argv.findIndex(
    (a) => a === "--configuration" || a === "-c",
  );
  const configuration = configIndex >= 0 ? argv[configIndex + 1] : "Debug";

  if (!["Debug", "Release"].includes(configuration)) {
    console.error("Invalid configuration. Use Debug or Release.");
    process.exit(1);
  }

  return configuration;
}

/**
 * Scans for source files matching the given patterns, excluding specified paths.
 * @param {Array<{pattern: string[], type: string}>} sourcePatterns - Patterns to match.
 * @param {string[]} [excludePaths] - Paths to exclude.
 * @returns {Map<string, {path: string, name: string, type: string}>} Source file map.
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
 * Runs a command, restricted to an allow-list.
 * @param {string} cmd - Command name (must be in allowedCommands).
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
 * Runs a CSS build step (PostCSS or Sass) and tracks timing/results.
 * @param {object} options - Build step options.
 * @param {string} options.name - Step name (e.g., "PostCSS (Tailwind)").
 * @param {() => string|null} options.findInput - Function returning the input file path.
 * @param {(input: string, output: string) => string[]} options.getArgs - Function returning npx arguments.
 * @param {(input: string) => string} options.getOutput - Function returning the output file path.
 * @param {string} options.rootPath - Repository root for relative paths.
 * @param {string} options.cwd - Working directory for the command.
 * @param {object} options.fileCache - File cache instance from createFileCache.
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
 * Checks whether output files listed in the manifest have changed.
 * @param {object} manifest - Parsed build manifest.
 * @param {object} fileCache - File cache instance.
 * @returns {{needed: boolean, reason?: string}} Whether rebuild is needed.
 */
export function checkOutputFiles(manifest, fileCache) {
  for (const output of manifest.OutputFiles) {
    if (!existsSync(output.Path)) {
      return { needed: true, reason: `Output missing: ${output.FileName}` };
    }
    if (fileCache.hashFile(output.Path) !== output.Hash) {
      return { needed: true, reason: `Output changed: ${output.FileName}` };
    }
  }
  return { needed: false };
}

/**
 * Checks whether source files listed in the manifest have changed or new files appeared.
 * @param {object} manifest - Parsed build manifest.
 * @param {Array<{pattern: string[], type: string}>} sourcePatterns - Source patterns.
 * @param {object} fileCache - File cache instance.
 * @param {Map} [extraSourceFiles] - Additional source files to check (e.g., Vite sources).
 * @returns {{needed: boolean, reason?: string}} Whether rebuild is needed.
 */
export function checkSourceFiles(
  manifest,
  sourcePatterns,
  fileCache,
  extraSourceFiles = new Map(),
) {
  const outputPaths = manifest.OutputFiles.map((f) => f.Path);
  const manifestFiles = new Map(
    manifest.SourceFiles.map((f) => [normalizeSlashes(f.Path), f]),
  );

  for (const source of manifest.SourceFiles) {
    if (!existsSync(source.Path)) {
      return { needed: true, reason: `Source removed: ${source.FileName}` };
    }
    const stats = fileCache.getStats(source.Path);
    if (stats.mtime.toISOString() !== source.LastModified) {
      if (fileCache.hashFile(source.Path) !== source.Hash) {
        return { needed: true, reason: `Source changed: ${source.FileName}` };
      }
    }
  }

  const currentFiles = getSourceFiles(sourcePatterns, outputPaths);
  for (const [path] of extraSourceFiles) {
    if (!currentFiles.has(path)) {
      currentFiles.set(path, extraSourceFiles.get(path));
    }
  }

  for (const [path] of currentFiles) {
    if (!manifestFiles.has(path)) {
      return { needed: true, reason: `Source added: ${basename(path)}` };
    }
  }

  return { needed: false };
}

/**
 * Full change detection: checks manifest, config, outputs, and sources.
 * @param {object} options - Options.
 * @param {string} options.manifestPath - Path to build-manifest.json.
 * @param {string} options.configuration - Current build configuration.
 * @param {Array} options.sourcePatterns - Source file patterns.
 * @param {object} options.fileCache - File cache instance.
 * @param {Map} [options.extraSourceFiles] - Additional source files (e.g., Vite).
 * @returns {{needed: boolean, reason?: string}} Whether rebuild is needed.
 */
export function checkIfBuildNeeded({
  manifestPath,
  configuration,
  sourcePatterns,
  fileCache,
  extraSourceFiles = new Map(),
}) {
  if (!existsSync(manifestPath)) {
    return { needed: true, reason: "No manifest found" };
  }

  try {
    const manifest = JSON.parse(readFileSync(manifestPath, "utf8"));
    if (manifest.BuildInfo.Config !== configuration) {
      return {
        needed: true,
        reason: `Configuration changed: ${manifest.BuildInfo.Config} -> ${configuration}`,
      };
    }

    const outputResult = checkOutputFiles(manifest, fileCache);
    if (outputResult.needed) {
      return outputResult;
    }

    const sourceResult = checkSourceFiles(
      manifest,
      sourcePatterns,
      fileCache,
      extraSourceFiles,
    );
    if (sourceResult.needed) {
      return sourceResult;
    }

    log.success("Build not needed - all files are up to date");
    log.info(`Last build: ${manifest.BuildInfo.EndTime}`);
    manifest.OutputFiles.forEach((o) =>
      log.info(`Output: ${o.FileName} (${o.SizeKB} KB)`),
    );
    return { needed: false };
  } catch (e) {
    return { needed: true, reason: `Manifest error: ${e.message}` };
  }
}

/**
 * Collects output file metadata for CSS build outputs.
 * @param {string[]} outputPaths - Absolute paths to output files.
 * @param {string} rootPath - Repository root for relative paths.
 * @param {object} fileCache - File cache instance.
 * @returns {object[]} Output file metadata array.
 */
export function collectOutputFiles(outputPaths, rootPath, fileCache) {
  return outputPaths.filter(existsSync).map((filePath) => {
    const stats = fileCache.getStats(filePath);
    const sizeKB = Number((stats.size / 1024).toFixed(2));
    log.info(`Found: ${basename(filePath)} (${sizeKB} KB)`);
    return {
      Path: normalizeSlashes(filePath),
      RelativePath: normalizeSlashes(relative(rootPath, filePath)),
      FileName: basename(filePath),
      SizeBytes: stats.size,
      SizeKB: sizeKB,
      LastModified: stats.mtime.toISOString(),
      Hash: fileCache.hashFile(filePath),
      Type: filePath.endsWith(".min.css") ? "PostCSS Output" : "Sass Output",
    };
  });
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

/**
 * Returns a file type string for script files based on extension.
 * @param {string} ext - File extension (e.g., ".ts").
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
 * Generates the build manifest object.
 * @param {object} options: Manifest options.
 * @param {Map} options.sourceFileMap: Source file map.
 * @param {string} options.entryFile: PostCSS entry file path.
 * @param {string} options.sassFile: Sass entry file path.
 * @param {object[]} options.commandResults: Build step results.
 * @param {object[]} options.outputFiles: Output file metadata.
 * @param {string} options.configuration: Build configuration.
 * @param {string} options.rootPath: Repository root for relative paths.
 * @param {string} options.scriptPath: Path to the calling script (__filename).
 * @param {object} options.fileCache: File cache instance.
 * @param {string} [options.version]: Manifest version string.
 * @param {(file: string, type: string) => string} [options.getFileType]: Custom file type resolver.
 * @returns {object} Manifest object.
 */
export function generateManifest({
  sourceFileMap,
  entryFile,
  sassFile,
  commandResults,
  outputFiles,
  configuration,
  rootPath,
  scriptPath,
  fileCache,
  version = "1.0",
  getFileType: customGetFileType,
}) {
  const isTailwindEntry =
    entryFile != null &&
    (() => {
      const name = basename(entryFile);
      if (name === "tailwind.css" || name.includes("tailwind")) return true;
      try {
        const content = readFileSync(entryFile, "utf8");
        return /@import\s+['"]tailwindcss['"]|tailwind\.css/.test(content);
      } catch {
        return false;
      }
    })();

  const defaultGetFileType = (file, type) => {
    if (type === "razor") {
      return "Razor";
    }
    if (type === "config") {
      return "Vite Configuration";
    }
    if (type === "script") {
      return getScriptFileType(extname(file));
    }
    if (file === entryFile) {
      return isTailwindEntry ? "Tailwind CSS (Entry)" : "CSS (Entry)";
    }
    if (file === sassFile) {
      return "Sass (Entry)";
    }
    if (file.endsWith(".scss")) {
      return "Sass Partial/Import";
    }
    return "CSS";
  };

  const resolveFileType = customGetFileType || defaultGetFileType;

  const sourceFiles = Array.from(sourceFileMap.values()).map(
    ({ path, type }) => ({
      Type: resolveFileType(path, type),
      Path: path,
      RelativePath: normalizeSlashes(relative(rootPath, path)),
      FileName: basename(path),
      Hash: fileCache.hashFile(path),
      LastModified: fileCache.getStats(path).mtime.toISOString(),
    }),
  );

  log.debug(`Tracked ${sourceFiles.length} source files`);

  return {
    BuildInfo: {
      Config: configuration,
      Success: true,
    },
    SourceFiles: sourceFiles,
    Commands: commandResults.map(({ outputPath, ...rest }) => rest),
    Environment: {
      MachineName:
        process.env.COMPUTERNAME || process.env.HOSTNAME || "unknown",
      UserName: process.env.USERNAME || process.env.USER || "unknown",
      ScriptPath: scriptPath,
      WorkingDirectory: rootPath,
      NodeVersion: process.version,
    },
    OutputFiles: outputFiles,
    Metadata: {
      GeneratedBy: "build.mjs",
      Version: version,
      ManifestVersion: version,
    },
  };
}

/**
 * Writes the manifest to disk and logs a summary.
 * @param {object} manifest - Manifest object.
 * @param {string} manifestPath - Path to write.
 * @param {Date} startTime - Build start time.
 * @param {string} buildLabel - Label for log messages.
 */
export function finalizeManifest(
  manifest,
  manifestPath,
  startTime,
  buildLabel,
) {
  const endTime = new Date();
  const duration = (endTime - startTime) / 1000;

  manifest.BuildInfo.StartTime = startTime.toISOString();
  manifest.BuildInfo.EndTime = endTime.toISOString();
  manifest.BuildInfo.DurationSeconds = Number(duration.toFixed(2));

  writeFileSync(manifestPath, JSON.stringify(manifest, null, 2), "utf8");

  const cssCount = manifest.OutputFiles.filter(
    (f) =>
      f.Type.includes("CSS") ||
      f.Type === "Sass Output" ||
      f.Type === "PostCSS Output",
  ).length;
  const jsCount = manifest.OutputFiles.filter((f) =>
    f.Type.includes("JavaScript"),
  ).length;
  const totalSizeKB = manifest.OutputFiles.reduce(
    (sum, f) => sum + f.SizeKB,
    0,
  ).toFixed(2);

  log.info("Build Summary:");
  log.info(`  CSS files: ${cssCount}`);
  if (jsCount > 0) {
    log.info(`  JS files: ${jsCount}`);
  }
  log.info(
    `  Total output: ${manifest.OutputFiles.length} files (${totalSizeKB} KB)`,
  );
  log.success("Manifest saved to: build-manifest.json");
  log.success(`${buildLabel} completed in ${duration.toFixed(2)}s`);
  console.log("\n" + "=".repeat(60) + "\n");
}

/**
 * Standard PostCSS build step configuration.
 * @param {string} stylesDir - Styles directory.
 * @param {string} rootPath - Repository root.
 * @param {string} cwd - Working directory.
 * @param {object} fileCache - File cache instance.
 * @param {string} [explicitInput] - Optional explicit input file path, bypassing content detection.
 * @returns {object} Options for runBuildStep.
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
 * @returns {object} Options for runBuildStep.
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
 * Scans directories for Vite-related source files (TS/JS) and config files.
 * @param {string} projectDir - Project directory (__dirname of the calling script).
 * @param {string[]} sourceDirs - Relative directory names to scan for scripts.
 * @param {string[]} configFiles - Config filenames to track.
 * @returns {Map<string, {path: string, name: string, type: string}>} Vite source file map.
 */
export function getViteSourceFiles(projectDir, sourceDirs, configFiles) {
  const files = new Map();

  for (const dir of sourceDirs) {
    const absDir = join(projectDir, dir);
    if (!existsSync(absDir)) continue;
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

/**
 * Ensures pnpm dependencies are installed. Runs pnpm install if node_modules is missing.
 * @param {string} projectDir - Project directory.
 * @returns {object|null} Command result for the manifest, or null if already installed.
 */
export function ensurePnpmDependencies(projectDir) {
  log.section("Checking pnpm dependencies");
  const nodeModulesPath = join(projectDir, "node_modules");

  if (existsSync(nodeModulesPath)) {
    log.info("node_modules exists, skipping pnpm install");
    return null;
  }

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
 * Builds Vite assets (JS/TS bundling).
 * @param {string} projectDir - Project directory.
 * @param {string} configuration - "Debug" or "Release".
 * @param {string} distDir - Expected output directory for verification.
 * @returns {object|null} Command result for the manifest, or null if no vite.config.js.
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

/**
 * Collects Vite dist output files with metadata for the manifest.
 * @param {string} distDir - Vite output directory.
 * @param {string} rootPath - Repository root for relative paths.
 * @param {object} fileCache - File cache instance.
 * @returns {object[]} Output file metadata array.
 */
export function collectViteOutputFiles(distDir, rootPath, fileCache) {
  const viteOutputs = [];
  if (!existsSync(distDir)) {
    return viteOutputs;
  }

  for (const filePath of collectDistFiles(distDir)) {
    fileCache.clear(filePath);
    const stats = fileCache.getStats(filePath);
    const sizeKB = Number((stats.size / 1024).toFixed(2));
    const ext = extname(filePath);
    let fileType = "Vite Asset";
    if (ext === ".js") {
      fileType = "Vite JavaScript";
    } else if (ext === ".css") {
      fileType = "Vite CSS Asset";
    } else if (ext === ".json") {
      fileType = "Vite Manifest";
    }

    log.info(`Found: ${basename(filePath)} (${sizeKB} KB) [${fileType}]`);
    viteOutputs.push({
      Path: normalizeSlashes(filePath),
      RelativePath: normalizeSlashes(relative(rootPath, filePath)),
      FileName: basename(filePath),
      SizeBytes: stats.size,
      SizeKB: sizeKB,
      LastModified: stats.mtime.toISOString(),
      Hash: fileCache.hashFile(filePath),
      Type: fileType,
    });
  }

  return viteOutputs;
}
