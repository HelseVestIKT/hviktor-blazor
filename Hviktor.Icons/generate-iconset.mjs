#!/usr/bin/env node

/**
 * Icon Set Generator for Hviktor.Icons
 *
 * Reads the webcomponent source files from @helsevestikt/hviktor-icons, extracts
 * SVG path data, and generates a C# `IconSet.cs` file containing typed
 * `IconDefinition` constants with embedded SVG paths.
 *
 * Usage:
 *   node generate-iconset.mjs
 *
 * Prerequisites:
 *   pnpm install (in this directory so @helsevestikt/hviktor-icons is available)
 *
 * The generated file is: Types/IconSet.cs
 */

import { readFileSync, writeFileSync, existsSync, readdirSync } from "node:fs";
import { resolve, dirname } from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = dirname(fileURLToPath(import.meta.url));

// Locate the package
const candidates = [
  resolve(__dirname, "node_modules", "@helsevestikt", "hviktor-icons"),
  resolve(
    __dirname,
    "..",
    "..",
    "Hviktor",
    "Hviktor",
    "node_modules",
    "@helsevestikt",
    "hviktor-icons",
  ),
  resolve(__dirname, "..", "node_modules", "@helsevestikt", "hviktor-icons"),
];

const pkgRoot = candidates.find((c) => existsSync(resolve(c, "package.json")));
if (!pkgRoot) {
  console.error(
    "❌ Could not find @helsevestikt/hviktor-icons. Run pnpm install first.",
  );
  process.exit(1);
}

console.log(`📦 Found package at: ${pkgRoot}`);
const pkgJson = JSON.parse(
  readFileSync(resolve(pkgRoot, "package.json"), "utf-8"),
);
const version = pkgJson.version || "unknown";

/**
 * Extracts the SVG path `d` attribute from a webcomponent JS source file.
 * @param {string} filePath
 * @returns {string | null}
 */
function extractPathData(filePath) {
  const content = readFileSync(filePath, "utf-8");
  const match = content.match(/return\s+'([^']+)'/);
  return match ? match[1] : null;
}

// Discover icons from webcomponent source files and extract SVG paths
/** @returns {{ tagName: string, className: string, pathData: string }[]} */
function discoverFromWebcomponentFiles() {
  const wcDir = resolve(pkgRoot, "dist", "lib", "webcomponents");
  if (!existsSync(wcDir)) {
    return [];
  }

  const files = readdirSync(wcDir).filter((f) =>
    f.endsWith(".webcomponent.js"),
  );
  const icons = [];

  for (const file of files) {
    const match = file.match(/^(icon-[\w-]+)\.webcomponent\.js$/);
    if (!match) continue;

    const fileBase = match[1];
    const tagName = `hvi-${fileBase}`;
    const filePath = resolve(wcDir, file);
    const pathData = extractPathData(filePath);

    if (!pathData) {
      console.warn(`   ⚠️  No path data found in ${file}, skipping`);
      continue;
    }

    // Derive class name from file content
    const content = readFileSync(filePath, "utf-8");
    const classMatch = content.match(/export class (\w+)/);
    const className = classMatch ? classMatch[1] : fileBase;

    icons.push({ tagName, className, pathData });
  }

  return icons;
}

// Fallback: discover from manifest (tag names only, no path data)
/** @returns {{ tagName: string, className: string, pathData: string }[]} */
function discoverFromCustomElementsManifest() {
  const manifestPath = resolve(pkgRoot, "dist", "custom-elements.json");
  if (!existsSync(manifestPath)) {
    return [];
  }

  const manifest = JSON.parse(readFileSync(manifestPath, "utf-8"));
  const wcDir = resolve(pkgRoot, "dist", "lib", "webcomponents");
  const icons = [];

  for (const mod of manifest.modules ?? []) {
    for (const decl of mod.declarations ?? []) {
      if (
        decl.customElement &&
        decl.tagName &&
        decl.tagName.startsWith("hvi-icon-")
      ) {
        const fileBase = decl.tagName.replace(/^hvi-/, "");
        const filePath = resolve(wcDir, `${fileBase}.webcomponent.js`);
        const pathData = existsSync(filePath)
          ? extractPathData(filePath)
          : null;

        if (!pathData) {
          console.warn(`   ⚠️  No path data for ${decl.tagName}, skipping`);
          continue;
        }

        icons.push({ tagName: decl.tagName, className: decl.name, pathData });
      }
    }
  }
  return icons;
}

// Run discovery: prefer direct file scanning, fallback to manifest
let icons = discoverFromWebcomponentFiles();
console.log(`   Webcomponent files: ${icons.length} icons`);

if (icons.length === 0) {
  console.log(
    "   ⚠️  No files found, falling back to Custom Elements Manifest...",
  );
  icons = discoverFromCustomElementsManifest();
  console.log(`   Custom Elements Manifest: ${icons.length} icons`);
}

if (icons.length === 0) {
  console.error("❌ No icons found. Check package structure.");
  process.exit(1);
}

// Sort by tag name for stable output
icons.sort((a, b) => a.tagName.localeCompare(b.tagName));

console.log(`✅ Found ${icons.length} icons`);
console.log(`   First: ${icons[0].tagName} (${icons[0].className})`);
console.log(
  `   Last:  ${icons[icons.length - 1].tagName} (${icons[icons.length - 1].className})`,
);

// Convert to C# property names
/**
 * Converts a tag name like "hvi-icon-align-center-fill" to "AlignCenterFill".
 * @param {string} tagName
 */
function toPascalCase(tagName) {
  return tagName
    .replace(/^hvi-icon-/, "")
    .split("-")
    .map((part) => part.charAt(0).toUpperCase() + part.slice(1))
    .join("");
}

// Generate C# source
const timestamp = new Date().toISOString();

const lines = icons.map(({ tagName, pathData }) => {
  const propName = toPascalCase(tagName);
  const summary = `    /// <summary>Represents the <c>${propName}</c> icon (<c>${tagName}</c>).</summary>`;
  const definition = ` static readonly IconDefinition ${propName} = new("${pathData}");`;

  return `${summary}
    public${propName === "Equals" ? " new" : ""}${definition}`;
});

const csContent = `// <auto-generated>
//     This code was generated by generate-iconset.mjs.
//     Source: @helsevestikt/hviktor-icons v${version}
//     Generated: ${timestamp}
//
//     Do not modify this file manually. Re-run the generator instead:
//       node generate-iconset.mjs
// </auto-generated>

using Hviktor.Icons.Abstractions.Types;

namespace Hviktor.Icons.Types;

/// <summary>
/// Typed icon constants for the <c>@helsevestikt/hviktor-icons</c> icon library.<br/>
/// Each constant is an <see cref="IconDefinition"/> containing the SVG path data
/// for the corresponding icon.
/// </summary>
/// <remarks>
/// Auto-generated from <c>@helsevestikt/hviktor-icons</c> v${version}.<br/>
/// </remarks>
public static class IconSet
{
${lines.join("\n")}
}
`;

// Write output and update metadata
const outputPath = resolve(__dirname, "Types", "IconSet.cs");
writeFileSync(outputPath, csContent, "utf-8");
console.log(`📝 Generated ${outputPath}`);
console.log(`   ${icons.length} icon definitions`);

const meta = {
  generatedAt: timestamp,
  version,
  source: "@helsevestikt/hviktor-icons",
  iconCount: icons.length,
};
writeFileSync(
  resolve(__dirname, "icons-meta.json"),
  JSON.stringify(meta, null, 2) + "\n",
  "utf-8",
);
console.log("📝 Updated icons-meta.json");
