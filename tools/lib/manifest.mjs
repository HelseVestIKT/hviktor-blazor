#!/usr/bin/env node

import { readFileSync, writeFileSync } from "node:fs";
import { basename, extname, relative } from "node:path";
import { log } from "./log.mjs";
import { normalizeSlashes } from "./fs.mjs";
import { getScriptFileType } from "./sources.mjs";

/**
 * Generates the build manifest object.
 * @param {object} options Manifest options.
 * @param {Map} options.sourceFileMap Source file map.
 * @param {string} [options.entryFile] PostCSS entry file path.
 * @param {string} [options.sassFile] Sass entry file path.
 * @param {object[]} options.commandResults Build step results.
 * @param {object[]} options.outputFiles Output file metadata.
 * @param {string} options.configuration Build configuration.
 * @param {string} options.rootPath Repository root for relative paths.
 * @param {string} options.scriptPath Path to the calling script (`__filename`).
 * @param {object} options.fileCache File cache instance.
 * @param {string} [options.version] Manifest version string.
 * @param {(file: string, type: string) => string} [options.getFileType] Custom file type resolver.
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
      if (name === "tailwind.css" || name.includes("tailwind")) {
        return true;
      }

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
 * Writes the manifest to disk and logs a build summary.
 * @param {object} manifest Manifest object.
 * @param {string} manifestPath Path to write.
 * @param {Date} startTime Build start time.
 * @param {string} buildLabel Label for log messages.
 * @param {object} [options]
 * @param {boolean} [options.writeFile=true] Whether to write the manifest to disk.
 *   Pass `false` for Release builds where the manifest must not be committed or included in publish output.
 */
export function finalizeManifest(
  manifest,
  manifestPath,
  startTime,
  buildLabel,
  { writeFile = true } = {},
) {
  const endTime = new Date();
  const duration = (endTime - startTime) / 1000;

  manifest.BuildInfo.StartTime = startTime.toISOString();
  manifest.BuildInfo.EndTime = endTime.toISOString();
  manifest.BuildInfo.DurationSeconds = Number(duration.toFixed(2));

  if (writeFile) {
    writeFileSync(manifestPath, JSON.stringify(manifest, null, 2), "utf8");
  }

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
  if (writeFile) {
    log.success("Manifest saved to: build-manifest.json");
  }
  log.success(`${buildLabel} completed in ${duration.toFixed(2)}s`);
  console.log("\n" + "=".repeat(60) + "\n");
}
