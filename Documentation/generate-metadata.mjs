#!/usr/bin/env node

/**
 * MCP Metadata Generator for Hviktor
 *
 * Generates a structured JSON metadata file from ComponentRegistry.cs + XML docs
 * for consumption by MCP servers and other LLM tooling.
 *
 * Usage:
 *   node generate-metadata.mjs
 *   node generate-metadata.mjs --output ../my-output.json
 *   node generate-metadata.mjs --notes ./notes.json
 *
 * Prerequisites:
 *   dotnet build (so XML doc files exist in bin/)
 */

import { readFileSync, writeFileSync, existsSync, readdirSync } from "node:fs";
import { resolve, dirname, join, relative, basename } from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = dirname(fileURLToPath(import.meta.url));
const rootDir = dirname(__dirname);

// Parse CLI arguments
const args = process.argv.slice(2);

const color = (c, msg) => `\x1b[${c}m${msg}\x1b[0m`;
const log = {
  header: (msg) =>
    console.log(
      `\n${color(36, "=".repeat(60))}\n${color(36, msg)}\n${color(36, "=".repeat(60))}`,
    ),
  section: (msg) => console.log(`\n${color(33, "► " + msg)}`),
  info: (msg) => console.log(`  ${msg}`),
  success: (msg) => console.log(color(32, "✓ " + msg)),
  warning: (msg) => console.warn(color(33, "⚠ " + msg)),
  error: (msg) => console.error(color(31, "✗ " + msg)),
  debug: (msg) => console.log(color(2, "  " + msg)),
};

if (args.includes("--help") || args.includes("-h")) {
  log.info(`Usage: node generate-metadata.mjs [options]

Options:
  --output, -o <path>          Output JSON file path (default: ../metadata.json)
  --notes, -p <path>   Private notes JSON file (default: ./notes.json)
  --help, -h                   Show this help message

Prerequisites:
  Run 'dotnet build' first so XML doc files exist in bin/
`);
  process.exit(0);
}

const getArg = (long, short, fallback) => {
  const i = args.findIndex((a) => a === long || a === short);
  return i >= 0 && i + 1 < args.length ? args[i + 1] : fallback;
};

const outputPath = resolve(
  getArg("--output", "-o", join(rootDir, "metadata.json")),
);
const privateNotesPath = resolve(
  getArg("--notes", "-p", join(__dirname, "notes.json")),
);
const registryPath = join(
  __dirname,
  "Components",
  "Services",
  "ComponentRegistry.cs",
);
const demosDir = join(__dirname, "Components", "Demos");

// XML doc file locations (try Debug first, then Release)
const xmlDocRoots = [
  join(rootDir, "Hviktor", "bin", "Debug"),
  join(rootDir, "Hviktor", "bin", "Release"),
  join(rootDir, "Hviktor.Abstractions", "bin", "Debug"),
  join(rootDir, "Hviktor.Abstractions", "bin", "Release"),
  join(rootDir, "Hviktor.Extensions", "bin", "Debug"),
  join(rootDir, "Hviktor.Extensions", "bin", "Release"),
  join(rootDir, "Hviktor.Icons", "bin", "Debug"),
  join(rootDir, "Hviktor.Icons", "bin", "Release"),
  join(rootDir, "Hviktor.Icons.Abstractions", "bin", "Debug"),
  join(
    rootDir,
    "Hviktor.Icons",
    "Hviktor.Icons.Abstractions",
    "bin",
    "Release",
  ),
];

log.header("Hviktor MCP Metadata Generator");
log.debug(`Registry: ${relative(rootDir, registryPath)}`);
log.debug(`Output:   ${relative(rootDir, outputPath)}`);

// XML Doc Parser

/** @typedef {{ summary: string|null, remarks: string|null, rawParameters: string|null, use: string|null, avoid: string|null, guidelines: string|null }} MemberDoc */

/** Loads all XML doc files and indexes them by member name. */
function loadXmlDocs() {
  log.section("Loading XML docs");

  /** @type {Map<string, MemberDoc>} */
  const docs = new Map();

  for (const root of xmlDocRoots) {
    const xmlFiles = findFiles(root, ".xml");
    for (const xmlPath of xmlFiles) {
      try {
        const content = readFileSync(xmlPath, "utf-8");
        parseXmlDoc(content, docs);
      } catch {
        // Skip unreadable files
      }
    }
  }

  log.debug(`XML docs: ${docs.size} members indexed`);
  return docs;
}

/** Finds files with given extension recursively. */
function findFiles(dir, ext) {
  const results = [];
  if (!existsSync(dir)) return results;
  try {
    for (const entry of readdirSync(dir, { withFileTypes: true })) {
      const full = join(dir, entry.name);
      if (entry.isDirectory()) {
        results.push(...findFiles(full, ext));
      } else if (entry.name.endsWith(ext)) {
        results.push(full);
      }
    }
  } catch {
    // Permission errors, etc.
  }
  return results;
}

/**
 * Parses a .NET XML doc file and adds members to the map.
 * This is a lightweight regex-based parser, not a full XML parser,
 * because the XML doc format is simple and predictable.
 */
function parseXmlDoc(content, docs) {
  const memberRegex = /<member\s+name="([^"]+)">([\s\S]*?)<\/member>/g;
  let match;
  while ((match = memberRegex.exec(content)) !== null) {
    const name = match[1];
    const body = match[2];
    const summary = extractTag(body, "summary");
    const remarks = extractTag(body, "remarks");
    const rawParameters = extractRawTag(body, "parameters");
    const use = extractTag(body, "use");
    const avoid = extractTag(body, "avoid");
    const guidelines = extractTag(body, "guidelines");
    docs.set(name, { summary, remarks, rawParameters, use, avoid, guidelines });
  }
}

/** Extracts text content from an XML tag, stripping inner XML elements to plain text. */
function extractTag(body, tag) {
  const regex = new RegExp(`<${tag}>(.*?)</${tag}>`, "s");
  const m = body.match(regex);
  if (!m) return null;
  return cleanXmlText(m[1]);
}

/** Extracts raw XML content from a tag without cleaning. */
function extractRawTag(body, tag) {
  const regex = new RegExp(`<${tag}>(.*?)</${tag}>`, "s");
  const m = body.match(regex);
  return m ? m[1] : null;
}

/**
 * Parses implicit parameters from a raw `<parameters>` XML tag body.
 * Supports two XML doc formats:
 *
 * Format A (Alert/Avatar style):
 *   <item>
 *     <term><b>name</b>: <see cref="T:...Type"/>?<br/><i>(optional)</i></term>
 *     <description>
 *       <b>Default</b>: <see cref="F:...Value"/><br/>
 *       <b>Allowed</b>: <see cref="F:...A"/> | <see cref="F:...B"/><br/>
 *       <b>Description</b>: Some text.
 *     </description>
 *   </item>
 *
 * Format B (Badge style):
 *   <item>
 *     <term><c>name</c> (<see cref="T:...Type"/>)</term>
 *     <description>Description. Default: <see cref="F:...Value"/>. Values: A, B, C.</description>
 *   </item>
 */
function parseImplicitParametersFromXml(rawParameters) {
  if (!rawParameters) return [];

  const params = [];
  const itemRegex = /<item>([\s\S]*?)<\/item>/g;
  let m;
  while ((m = itemRegex.exec(rawParameters)) !== null) {
    const itemBody = m[1];
    const param = parseXmlParameterItem(itemBody);
    if (param) params.push(param);
  }
  return params;
}

/** Extracts the short name from a cref like "T:Hviktor.Abstractions.Enums.Attributes.Color" -> "Color". */
function crefShortName(cref) {
  if (!cref) return null;
  // Strip prefix (T:, F:, P:, N:, etc.)
  const stripped = cref.replace(/^[A-Z]:/, "");
  const parts = stripped.split(".");
  return parts[parts.length - 1];
}

/** Parses a single <item> from the parameters list into an implicit parameter object. */
function parseXmlParameterItem(itemBody) {
  const termMatch = itemBody.match(/<term>([\s\S]*?)<\/term>/);
  const descMatch = itemBody.match(/<description>([\s\S]*?)<\/description>/);
  if (!termMatch) return null;

  const termXml = termMatch[1];
  const descXml = descMatch ? descMatch[1] : "";

  // Extract parameter name from term.
  // Format A: <b>name</b>: <see cref="..."/>
  // Format B: <c>name</c> (<see cref="..."/>)
  const nameFromBold = termXml.match(/<b>([^<]+)<\/b>/);
  const nameFromCode = termXml.match(/<c>([^<]+)<\/c>/);
  const name = nameFromBold?.[1]?.trim() || nameFromCode?.[1]?.trim();
  if (!name) return null;

  // Extract type from term via <see cref="T:..."/>
  const typeCref = termXml.match(/<see\s+cref="([^"]+)"\s*\/>/);
  let type = typeCref ? crefShortName(typeCref[1]) : null;

  // Check if type is nullable (trailing ? after the see cref or in parens)
  const nullable = typeCref && termXml.match(/<see\s+cref="[^"]+"\s*\/>\s*\?/);
  if (nullable && type) type += "?";

  // If no cref, try to find a plain type name like "boolean", "string?"
  if (!type) {
    const plainType = termXml.match(/:\s*([\w?]+)/);
    if (plainType) type = plainType[1].trim();
  }

  // Parse description fields
  let defaultVal = undefined;
  let allowedValues = undefined;
  let description = undefined;

  // Format A: structured <b>Default</b>:, <b>Allowed</b>:, <b>Description</b>:
  const defaultMatch = descXml.match(
    /<b>Default<\/b>\s*:\s*([\s\S]*?)(?:<br|<b>|$)/,
  );
  if (defaultMatch) {
    const defText = defaultMatch[1].trim();
    // Extract from cref or plain text
    const defCref = defText.match(/<see\s+cref="([^"]+)"\s*\/>/);
    defaultVal = defCref
      ? crefShortName(defCref[1])
      : cleanXmlText(defText).trim() || undefined;
  }

  const allowedMatch = descXml.match(
    /<b>Allowed<\/b>\s*:\s*([\s\S]*?)(?:<br|<b>|$)/,
  );
  if (allowedMatch) {
    const allowedText = allowedMatch[1];
    // Extract all cref values
    const crefValues = [];
    const crefRegex = /<see\s+cref="([^"]+)"\s*\/>/g;
    let cm;
    while ((cm = crefRegex.exec(allowedText)) !== null) {
      const val = crefShortName(cm[1]);
      if (val) crefValues.push(val);
    }
    if (crefValues.length > 0) {
      allowedValues = crefValues;
    } else {
      // Plain text values separated by |
      const plainValues = cleanXmlText(allowedText)
        .split("|")
        .map((v) => v.trim())
        .filter(Boolean);
      if (plainValues.length > 0) allowedValues = plainValues;
    }
  }

  const descriptionMatch = descXml.match(
    /<b>Description<\/b>\s*:\s*([\s\S]*?)$/,
  );
  if (descriptionMatch) {
    description = cleanXmlText(descriptionMatch[1]).trim() || undefined;
  }

  // Format B fallback: plain description text with "Default:", "Values:" inline
  if (!description && !defaultMatch && !allowedMatch) {
    const plainDesc = cleanXmlText(descXml).trim();
    if (plainDesc) {
      const defInline = plainDesc.match(/Default:\s*`?(\w+)`?[.]?/);
      if (defInline) defaultVal = defInline[1];

      const valuesInline = plainDesc.match(/Values?:\s*([\w, ]+)/);
      if (valuesInline) {
        allowedValues = valuesInline[1]
          .split(",")
          .map((v) => v.trim())
          .filter(Boolean);
      }

      // Strip Default/Values suffixes for the description
      description =
        plainDesc
          .replace(/\s*Default:\s*`?\w+`?[.]?\s*/g, "")
          .replace(/\s*Values?:\s*[\w, ]+\.?\s*/g, "")
          .trim() || undefined;
    }
  }

  return {
    name,
    type: type || "string?",
    default: defaultVal || undefined,
    allowedValues: allowedValues?.length > 0 ? allowedValues : undefined,
    description: description || undefined,
    isImplicit: true,
  };
}

/** Converts XML doc inner content to plain text. */
function cleanXmlText(xml) {
  return (
    xml
      // <see cref="T:Namespace.Type"/> or <see cref="P:..." /> -> type name
      .replace(/<see\s+cref="[^:]*:([^"]*?)"\s*\/>/g, (_, cref) => {
        const parts = cref.split(".");
        return "`" + parts[parts.length - 1] + "`";
      })
      // <see langword="null"/> -> `null`
      .replace(/<see\s+langword="([^"]+)"\s*\/>/g, "`$1`")
      // <c>text</c> -> `text`
      .replace(/<c>(.*?)<\/c>/gs, "`$1`")
      // <para>text</para> -> \n\ntext\n\n
      .replace(/<para>(.*?)<\/para>/gs, "\n\n$1\n\n")
      // <br/> or <br /> -> \n
      .replace(/<br\s*\/?>/g, "\n")
      // <b>text</b> -> **text**
      .replace(/<b>(.*?)<\/b>/gs, "**$1**")
      // Strip any remaining XML tags
      .replace(/<[^>]+>/g, "")
      // Collapse whitespace per line
      .split("\n")
      .map((l) => l.trim())
      .join("\n")
      .replace(/\n{3,}/g, "\n\n")
      .trim()
  );
}

// ComponentRegistry Parser

/**
 * Parses ComponentRegistry.cs to extract component definitions.
 * This reads the structured C# source and extracts the data that would
 * normally only be available at runtime via reflection.
 */
function parseRegistry(source) {
  log.section("Parsing component blocks");
  const components = [];

  // Find all BuildXxx() methods that return List<ComponentInfo>
  const methodRegex =
    /private\s+static\s+List<ComponentInfo>\s+Build\w+\(\)\s*\{([\s\S]*?)^\s{4}\}/gm;
  let methodMatch;
  while ((methodMatch = methodRegex.exec(source)) !== null) {
    const methodBody = methodMatch[1];
    const componentBlocks = splitComponentBlocks(methodBody);
    for (const block of componentBlocks) {
      const component = parseComponentBlock(block);
      if (component) {
        components.push(component);
      }
    }
  }

  return components;
}

/** Splits the items block into individual component `new(...)` blocks. */
function splitComponentBlocks(block) {
  const blocks = [];
  // Find all `new ComponentInfo("slug"` starting points
  const regex = /new ComponentInfo\("([a-z][a-z0-9-]*)"/g;
  const starts = [];
  let m;
  while ((m = regex.exec(block)) !== null) {
    starts.push(m.index);
  }

  for (let i = 0; i < starts.length; i++) {
    const start = starts[i];
    const end = i + 1 < starts.length ? starts[i + 1] : block.length;
    blocks.push(block.substring(start, end));
  }

  return blocks;
}

/** Parses a single component `new(...)` block. */
function parseComponentBlock(block) {
  // Extract slug and title
  const header = block.match(/^new ComponentInfo\("([^"]+)",\s*"([^"]+)"/);
  if (!header) return null;

  const slug = header[1];
  const title = header[2];

  // Extract typeof(ComponentType) - the main component type (third positional arg)
  const typeofMatch = block.match(
    /new ComponentInfo\("[^"]+",\s*"[^"]+",\s*typeof\(([^)]+)\)/,
  );
  const componentType = typeofMatch ? typeofMatch[1] : null;

  // Extract Designsystemet link
  const dsLink = extractStringAfterDemos(block);

  // Extract implicit parameters
  const implicitParams = extractImplicitParameters(
    block,
    "ImplicitParameters:",
  );

  // Extract sub-components
  const subComponents = extractSubComponents(block);

  // Extract demo types
  const demos = extractDemos(block);

  return {
    slug,
    title,
    componentType,
    designSystemUrl: dsLink,
    implicitParameters: implicitParams.length > 0 ? implicitParams : undefined,
    subComponents: subComponents.length > 0 ? subComponents : undefined,
    demos,
  };
}

/** Extracts the Designsystemet URL from a component block. */
function extractStringAfterDemos(block) {
  // The URL is typically the first quoted string after the demos array
  const urlMatch = block.match(/"(https:\/\/[^"]+)"/);
  return urlMatch ? urlMatch[1] : undefined;
}

/** Extracts Implicit(...) calls from a section of the block. */
function extractImplicitParameters(block, marker) {
  const params = [];
  const markerIdx = block.indexOf(marker);
  if (markerIdx < 0) return params;

  // Find the array content after the marker
  const afterMarker = block.substring(markerIdx + marker.length);
  const bracketContent = extractBracketContent(afterMarker);
  if (!bracketContent) return params;

  // Parse each parameter call
  // Match Implicit(...), ColorParam(...), VariantParam(...), SizeParam(...), CommonSizeParam(...), AsChildParam()
  const paramCalls = splitParamCalls(bracketContent);

  for (const call of paramCalls) {
    const param = parseParamCall(call.trim());
    if (param) params.push(param);
  }

  return params;
}

/** Extracts content inside the first balanced [...] after given text. */
function extractBracketContent(text) {
  const start = text.indexOf("[");
  if (start < 0) return null;

  let depth = 0;
  for (let i = start; i < text.length; i++) {
    if (text[i] === "[") depth++;
    else if (text[i] === "]") {
      depth--;
      if (depth === 0) {
        return text.substring(start + 1, i);
      }
    }
  }
  return null;
}

/** Splits parameter calls at top-level commas (respecting nested parens/brackets). */
function splitParamCalls(content) {
  const calls = [];
  let depth = 0;
  let current = "";

  for (let i = 0; i < content.length; i++) {
    const ch = content[i];
    if (ch === "(" || ch === "[") depth++;
    else if (ch === ")" || ch === "]") depth--;
    else if (ch === "," && depth === 0) {
      const trimmed = current.trim();
      if (trimmed.length > 0) calls.push(trimmed);
      current = "";
      continue;
    }
    current += ch;
  }

  const trimmed = current.trim();
  if (trimmed.length > 0) calls.push(trimmed);
  return calls;
}

/** Parses a single parameter helper call like Implicit("name", "type", ...) */
function parseParamCall(call) {
  // AsChildParam()
  if (call.startsWith("AsChildParam")) {
    return {
      name: "asChild",
      type: "bool?",
      description:
        "Change the default rendered element for the one passed as a child, merging their props and behavior.",
      default: "false",
      allowedValues: ["true", "false"],
    };
  }

  // ColorParam(...)
  if (call.startsWith("ColorParam")) {
    return parseHelperCall(call, "ColorParam", {
      name: "color",
      type: "Color",
      nullableType: "Color?",
      description: "The color theme. Also accepted as data-color.",
      defaultAllowed: [
        "Accent",
        "Brand1",
        "Brand2",
        "Brand3",
        "Neutral",
        "Info",
        "Success",
        "Warning",
        "Danger",
      ],
    });
  }

  // VariantParam(...)
  if (call.startsWith("VariantParam")) {
    return parseHelperCall(call, "VariantParam", {
      name: "variant",
      type: "Variant",
      nullableType: "Variant?",
      description: "The visual variant. Also accepted as data-variant.",
    });
  }

  // SizeParam(...)
  if (call.startsWith("SizeParam")) {
    return parseHelperCall(call, "SizeParam", {
      name: "size",
      type: "Size",
      nullableType: "Size?",
      description: "The size. Also accepted as data-size.",
      defaultAllowed: ["ExtraSmall", "Small", "Medium", "Large", "ExtraLarge"],
    });
  }

  // CommonSizeParam(...)
  if (call.startsWith("CommonSizeParam")) {
    const argsStr = extractParenContent(call);
    const callArgs = splitArgs(argsStr);
    const defaultVal = parseStringArg(callArgs[0]) ?? "Medium";
    const desc =
      parseStringArg(callArgs[1]) ?? "The size. Also accepted as data-size.";
    return {
      name: "size",
      type: defaultVal === "null" || !defaultVal ? "Size?" : "Size",
      description: desc || undefined,
      default: defaultVal === "null" ? undefined : defaultVal || undefined,
      allowedValues: ["Small", "Medium", "Large"],
    };
  }

  // Implicit("name", "type", ...)
  if (call.startsWith("Implicit(") || call.startsWith("Implicit(\n")) {
    return parseImplicitCall(call);
  }

  return null;
}

/** Parses a helper like ColorParam("default", "description", [...]) */
function parseHelperCall(call, fnName, defaults) {
  const argsStr = extractParenContent(call);
  if (!argsStr || argsStr.trim() === "") {
    // No arguments: use defaults with nullable type
    return {
      name: defaults.name,
      type: defaults.nullableType || defaults.type,
      description: defaults.description,
      default: undefined,
      allowedValues: defaults.defaultAllowed,
    };
  }

  const callArgs = splitArgs(argsStr);
  const defaultVal = parseStringArg(callArgs[0]);
  const description =
    parseNamedArg(callArgs, "description") ??
    parseStringArg(callArgs[1]) ??
    defaults.description;
  const allowed =
    parseArrayArg(callArgs, "allowedValues") ?? defaults.defaultAllowed;

  return {
    name: defaults.name,
    type: defaultVal ? defaults.type : defaults.nullableType || defaults.type,
    description: description || undefined,
    default: defaultVal || undefined,
    allowedValues: allowed,
  };
}

/** Parses Implicit("name", "type", "desc", "default", [...]) */
function parseImplicitCall(call) {
  const argsStr = extractParenContent(call);
  if (!argsStr) return null;

  const callArgs = splitArgs(argsStr);
  const name = parseStringArg(callArgs[0]);
  const type = parseStringArg(callArgs[1]);
  if (!name || !type) return null;

  const description = parseStringArg(callArgs[2]);
  const defaultVal = parseStringArg(callArgs[3]);
  const allowed = parseInlineArray(callArgs[4]);

  return {
    name,
    type,
    description: description || undefined,
    default: defaultVal || undefined,
    allowedValues: allowed?.length > 0 ? allowed : undefined,
  };
}

/** Extracts SubComponentInfo blocks. */
function extractSubComponents(block) {
  const subs = [];
  const marker = "SubComponents:";
  const markerIdx = block.indexOf(marker);
  if (markerIdx < 0) return subs;

  const afterMarker = block.substring(markerIdx + marker.length);
  const bracketContent = extractBracketContent(afterMarker);
  if (!bracketContent) return subs;

  // Split on `new SubComponentInfo(`
  const subRegex = /new SubComponentInfo\(/g;
  const starts = [];
  let m;
  while ((m = subRegex.exec(bracketContent)) !== null) {
    starts.push(m.index);
  }

  for (let i = 0; i < starts.length; i++) {
    const start = starts[i];
    const end = i + 1 < starts.length ? starts[i + 1] : bracketContent.length;
    const subBlock = bracketContent.substring(start, end);

    const titleMatch = subBlock.match(/new SubComponentInfo\("([^"]+)"/);
    const typeMatch = subBlock.match(/typeof\(([^)]+)\)/);
    if (!titleMatch) continue;

    const implicitParams = extractImplicitParameters(
      subBlock,
      "ImplicitParameters:",
    );

    subs.push({
      name: titleMatch[1],
      componentType: typeMatch ? typeMatch[1] : undefined,
      implicitParameters:
        implicitParams.length > 0 ? implicitParams : undefined,
    });
  }

  return subs;
}

/** Extracts demo type names from the demos array. */
function extractDemos(block) {
  const demos = [];
  // Match new DemoInfo(typeof(Type)) with optional title and description strings
  // Use a more robust approach: find each DemoInfo( and parse arguments
  const demoStartRegex = /new DemoInfo\(typeof\(([^)]+)\)/g;
  let m;
  while ((m = demoStartRegex.exec(block)) !== null) {
    const type = m[1];
    // Extract the remaining arguments after the typeof(...)
    const afterType = block.substring(m.index + m[0].length);
    const restMatch = afterType.match(
      /^(?:\s*,\s*"((?:[^"\\]|\\.)*)"\s*)?(?:,\s*"((?:[^"\\]|\\.)*)"\s*)?\)/s,
    );
    const title =
      restMatch?.[1]?.replace(/\\n/g, "\n").replace(/\\"/g, '"') || undefined;
    const description =
      restMatch?.[2]?.replace(/\\n/g, "\n").replace(/\\"/g, '"') || undefined;
    demos.push({ type, title, description });
  }
  return demos;
}

// Argument parsing helpers

/** Extracts content inside parentheses. */
function extractParenContent(call) {
  const start = call.indexOf("(");
  if (start < 0) return null;
  let depth = 0;
  for (let i = start; i < call.length; i++) {
    if (call[i] === "(") depth++;
    else if (call[i] === ")") {
      depth--;
      if (depth === 0) return call.substring(start + 1, i);
    }
  }
  return null;
}

/** Splits arguments at top-level commas. */
function splitArgs(argsStr) {
  if (!argsStr) return [];
  const result = [];
  let depth = 0;
  let current = "";
  for (const ch of argsStr) {
    if (ch === "(" || ch === "[") depth++;
    else if (ch === ")" || ch === "]") depth--;
    else if (ch === "," && depth === 0) {
      result.push(current.trim());
      current = "";
      continue;
    }
    current += ch;
  }
  if (current.trim()) result.push(current.trim());
  return result;
}

/** Parses a C# string literal like `"hello"` or `null`. */
function parseStringArg(arg) {
  if (!arg) return null;
  arg = arg.trim();
  // Handle named arguments like `description: "..."`
  if (arg.includes(":")) {
    const colonIdx = arg.indexOf(":");
    const beforeColon = arg.substring(0, colonIdx).trim();
    // Only treat as named arg if the part before colon is a simple identifier
    if (/^[a-zA-Z@_][a-zA-Z0-9_]*$/.test(beforeColon)) {
      arg = arg.substring(colonIdx + 1).trim();
    }
  }
  if (arg === "null") return null;
  const m = arg.match(/^"((?:[^"\\]|\\.)*)"/s);
  return m ? m[1].replace(/\\n/g, "\n").replace(/\\"/g, '"') : null;
}

/** Finds a named argument and returns its string value. */
function parseNamedArg(args, name) {
  for (const arg of args) {
    const trimmed = arg.trim();
    if (trimmed.startsWith(name + ":") || trimmed.startsWith(name + " :")) {
      const val = trimmed.substring(trimmed.indexOf(":") + 1).trim();
      if (val === "null") return null;
      const m = val.match(/^"((?:[^"\\]|\\.)*)"/s);
      return m ? m[1].replace(/\\n/g, "\n").replace(/\\"/g, '"') : null;
    }
  }
  return undefined; // Not found (different from null)
}

/** Finds a named array argument like `allowedValues: [...]`. */
function parseArrayArg(args, name) {
  for (const arg of args) {
    const trimmed = arg.trim();
    if (trimmed.startsWith(name + ":") || trimmed.startsWith(name + " :")) {
      return parseInlineArray(
        trimmed.substring(trimmed.indexOf(":") + 1).trim(),
      );
    }
  }
  return undefined;
}

/** Parses a C# inline array like `["a", "b"]` or `(IReadOnlyList<string>)["a", "b"]`. */
function parseInlineArray(arg) {
  if (!arg) return null;
  arg = arg.trim();
  // Strip cast like (IReadOnlyList<string>)
  arg = arg.replace(/^\([^)]*\)\s*/, "");
  if (!arg.startsWith("[")) return null;

  const values = [];
  const regex = /"([^"]+)"/g;
  let m;
  while ((m = regex.exec(arg)) !== null) {
    values.push(m[1]);
  }
  return values;
}

// Typed Parameter Extraction from .razor.cs files

/** Scans all .razor.cs files under Components/ and extracts [Parameter] properties. */
function extractTypedParameters(xmlDocs) {
  log.section("Extracting typed parameters");

  const componentsDir = join(rootDir, "Hviktor", "Components");
  const csFiles = findFiles(componentsDir, ".razor.cs");
  /** @type {Map<string, object[]>} Maps "Namespace.TypeName" to parameter list */
  const paramsByType = new Map();

  for (const file of csFiles) {
    const content = readFileSync(file, "utf-8");
    const ns = content.match(/namespace\s+([\w.]+)/)?.[1];
    // Match class declarations like: public partial class, public sealed partial class, etc.
    const cls = content.match(
      /public\s+(?:sealed\s+|abstract\s+)?partial\s+class\s+(\w+)/,
    )?.[1];
    if (!ns || !cls) {
      continue;
    }

    const fullType = `${ns}.${cls}`;
    const params = [];

    // Match [Parameter...] followed by property declaration
    // Handles: [Parameter], [Parameter, EditorRequired], [Parameter]\npublic, [Parameter] public
    const paramRegex =
      /\[Parameter[^\]]*]\s*(?:\r?\n\s*)?(?:public\s+)?(?:required\s+)?([\w<>?,\s]+?)\s+(\w+)\s*\{/g;
    let m;
    while ((m = paramRegex.exec(content)) !== null) {
      const type = m[1].trim();
      const name = m[2];

      // Skip CaptureUnmatchedValues (AdditionalAttributes) - check the attribute itself
      const attrRegion = content.substring(
        Math.max(0, m.index - 50),
        m.index + 50,
      );
      if (attrRegion.includes("CaptureUnmatchedValues")) continue;

      // Look up XML doc summary for this property
      const memberId = `P:${fullType}.${name}`;
      const doc = xmlDocs.get(memberId);

      // Check for [EditorRequired] in the attribute or `required` keyword before type
      const attrText = m[0]; // The matched attribute + property declaration
      const precedingContext = content.substring(
        Math.max(0, m.index - 200),
        m.index + m[0].length,
      );
      const isRequired =
        /EditorRequired/.test(precedingContext) ||
        /\brequired\s+/.test(attrText);

      // Check for [DefaultValue] on preceding lines
      const defaultMatch = precedingContext.match(/\[DefaultValue\(([^)]+)\)]/);
      let defaultVal = undefined;
      if (defaultMatch) {
        defaultVal = defaultMatch[1].replace(/^"(.*)"$/, "$1");
      }

      params.push({
        name,
        type,
        default: defaultVal,
        description: doc?.summary || undefined,
        isRequired: isRequired || undefined,
      });
    }

    if (params.length > 0) {
      paramsByType.set(fullType, params);
      // Also store by short name for fuzzy matching
      paramsByType.set(cls, params);
    }
  }

  log.debug(`Found ${paramsByType.size} typed parameters`);
  return paramsByType;
}

// Demo Source Reader

/** Reads demo .razor files and strips directives. */
function readDemoSource(demoType) {
  // Strip fully qualified prefix if present
  let key = demoType;
  if (key.startsWith("Documentation.Components.Demos.")) {
    key = key.replace("Documentation.Components.Demos.", "");
  }

  // demoType is like "AlertDemo", "Chip.ChipRadioDemo", "Button.ButtonColorDemo"
  // Map to file path under Documentation/Components/Demos/
  const parts = key.split(".");
  let filePath;

  if (parts.length === 1) {
    // Flat: AlertDemo -> Components/Demos/AlertDemo.razor
    filePath = join(demosDir, `${parts[0]}.razor`);
    // Fallback: search subdirectories (e.g. ErrorSummaryDemo -> ErrorSummary/ErrorSummaryDemo.razor)
    if (!existsSync(filePath)) {
      const found = findFiles(demosDir, ".razor").find(
        (f) => basename(f) === `${parts[0]}.razor`,
      );
      if (found) filePath = found;
    }
  } else {
    // Nested: Button.ButtonColorDemo -> Components/Demos/Button/ButtonColorDemo.razor
    filePath = join(
      demosDir,
      ...parts.slice(0, -1),
      `${parts[parts.length - 1]}.razor`,
    );
  }

  if (!existsSync(filePath)) return undefined;

  const content = readFileSync(filePath, "utf-8");
  // Strip @using, @inject, @inherits directives from the top
  const lines = content.split("\n");
  let startIdx = 0;
  for (let i = 0; i < lines.length; i++) {
    const trimmed = lines[i].trim();
    if (
      trimmed.startsWith("@using") ||
      trimmed.startsWith("@inject") ||
      trimmed.startsWith("@inherits") ||
      trimmed === ""
    ) {
      startIdx = i + 1;
    } else {
      break;
    }
  }

  return lines.slice(startIdx).join("\n").trim();
}

// Private Notes

function loadPrivateNotes() {
  log.section("Loading private notes");

  if (!existsSync(privateNotesPath)) {
    log.warning("Private notes: (not found, skipping)");
    return null;
  }
  try {
    const raw = readFileSync(privateNotesPath, "utf-8");
    // Strip UTF-8 BOM and JSON comments (// style)
    const cleaned = raw.replace(/^\uFEFF/, "").replace(/^\s*\/\/.*$/gm, "");
    const notes = JSON.parse(cleaned);
    log.debug(
      `Private notes: ${Object.keys(notes.components || {}).length} components`,
    );
    return notes;
  } catch (e) {
    log.error(`Failed to parse private notes: ${e.message}`);
    return null;
  }
}

// Namespace Resolution

/**
 * Resolves a short component type name from ComponentRegistry to a full namespace.
 * Dynamically parses the `using` statements from the registry source file.
 */
function resolveNamespace(shortType, registrySource) {
  // Handle fully qualified types like Hviktor.Components.Badge.Badge
  if (shortType.startsWith("Hviktor.")) {
    return shortType;
  }

  // Handle nested sub-component types like Chip.Button, Dropdown.TriggerContext
  // These reference a parent type via its using alias
  if (shortType.includes(".")) {
    const parentName = shortType.split(".")[0];
    const resolved = resolveNamespace(parentName, registrySource);
    if (resolved !== parentName) {
      const parentNs = resolved.substring(0, resolved.lastIndexOf("."));
      return `${parentNs}.${shortType.replace(".", "+")}`;
    }
    return shortType;
  }

  // Build map from `using Hviktor.Components.Foo;` statements
  // Last segment of the namespace is the type alias (e.g. Alert from Hviktor.Components.Alert)
  if (!resolveNamespace._cache) {
    const map = {};
    const usingRegex = /^using\s+(Hviktor[\w.]+);/gm;
    let m;
    while ((m = usingRegex.exec(registrySource)) !== null) {
      const fullNs = m[1];
      const lastDot = fullNs.lastIndexOf(".");
      const alias = fullNs.substring(lastDot + 1);
      // The type is typically Namespace.TypeName where TypeName === alias
      map[alias] = `${fullNs}.${alias}`;
    }
    // Also handle the `using List;` alias pointing to Hviktor.Models.List
    const listUsing = registrySource.match(/^using\s+List;/m);
    if (listUsing) {
      // This is a namespace alias; types inside are Item, Unordered, Ordered, ListBase
      map["ListBase"] = "Hviktor.Models.List.ListBase";
      map["Item"] = "Hviktor.Models.List.Item";
      map["Unordered"] = "Hviktor.Models.List.Unordered";
      map["Ordered"] = "Hviktor.Models.List.Ordered";
    }
    resolveNamespace._cache = map;
  }

  return resolveNamespace._cache[shortType] || shortType;
}

// Main Assembly

function buildMetadata() {
  const xmlDocs = loadXmlDocs();
  const privateNotes = loadPrivateNotes();
  const typedParamsMap = extractTypedParameters(xmlDocs);

  // Read and parse ComponentRegistry.cs
  if (!existsSync(registryPath)) {
    log.error(`ComponentRegistry.cs not found at ${registryPath}`);
    process.exit(1);
  }
  const registrySource = readFileSync(registryPath, "utf-8");
  const components = parseRegistry(registrySource);

  log.debug(`Components: ${components.length} parsed from registry`);

  log.section("Building metadata");

  // Build the final metadata
  const metadata = {
    $schema: "https://hviktor.dev/schemas/mcp-metadata-v1.json",
    generatedAt: new Date().toISOString(),
    generator: "generate-metadata.mjs",
    components: components.map((comp) => {
      const fullType = comp.componentType
        ? resolveNamespace(comp.componentType, registrySource)
        : null;
      const ns = fullType?.substring(0, fullType.lastIndexOf(".")) || undefined;
      const shortName = comp.componentType?.split(".").pop() || comp.title;

      // Get XML doc class summary
      const classMemberId = fullType ? `T:${fullType}` : null;
      const classDoc = classMemberId ? xmlDocs.get(classMemberId) : null;

      // Get typed [Parameter] properties
      const typedParams =
        typedParamsMap.get(fullType) || typedParamsMap.get(shortName) || [];

      // Build parameters list (typed first, then implicit)
      const parameters = [];
      for (const p of typedParams) {
        parameters.push({
          name: p.name,
          type: p.type,
          description: p.description || undefined,
          isRequired: p.isRequired || undefined,
          isImplicit: undefined, // explicitly typed parameter
        });
      }

      // Merge implicit parameters: XML docs (<parameters> tag) + registry
      // Registry-defined params take precedence over XML-sourced ones.
      const xmlImplicitParams = parseImplicitParametersFromXml(
        classDoc?.rawParameters,
      );
      const registryImplicitParams = comp.implicitParameters || [];
      const registryNames = new Set(registryImplicitParams.map((p) => p.name));

      for (const p of registryImplicitParams) {
        parameters.push({
          name: p.name,
          type: p.type,
          default: p.default || undefined,
          allowedValues: p.allowedValues || undefined,
          description: p.description || undefined,
          isImplicit: true,
        });
      }
      for (const p of xmlImplicitParams) {
        if (!registryNames.has(p.name)) {
          parameters.push({
            name: p.name,
            type: p.type,
            default: p.default || undefined,
            allowedValues: p.allowedValues || undefined,
            description: p.description || undefined,
            isImplicit: true,
          });
        }
      }

      // Build sub-components
      const subComponents = comp.subComponents?.map((sc) => {
        const scFullType = sc.componentType
          ? resolveNamespace(sc.componentType, registrySource)
          : undefined;
        const scShortName = sc.componentType?.split(".").pop();
        const scTypedParams =
          typedParamsMap.get(scFullType) ||
          typedParamsMap.get(scShortName) ||
          [];

        // Sub-component XML doc implicit params
        const scClassId = scFullType ? `T:${scFullType}` : null;
        const scClassDoc = scClassId ? xmlDocs.get(scClassId) : null;
        const scXmlImplicit = parseImplicitParametersFromXml(
          scClassDoc?.rawParameters,
        );
        const scRegistryImplicit = sc.implicitParameters || [];
        const scRegistryNames = new Set(scRegistryImplicit.map((p) => p.name));

        const scParams = [];
        for (const p of scTypedParams) {
          scParams.push({
            name: p.name,
            type: p.type,
            description: p.description || undefined,
            isRequired: p.isRequired || undefined,
          });
        }
        for (const p of scRegistryImplicit) {
          scParams.push({
            name: p.name,
            type: p.type,
            default: p.default || undefined,
            allowedValues: p.allowedValues || undefined,
            description: p.description || undefined,
            isImplicit: true,
          });
        }
        for (const p of scXmlImplicit) {
          if (!scRegistryNames.has(p.name)) {
            scParams.push({
              name: p.name,
              type: p.type,
              default: p.default || undefined,
              allowedValues: p.allowedValues || undefined,
              description: p.description || undefined,
              isImplicit: true,
            });
          }
        }

        return {
          name: sc.name,
          namespace: scFullType
            ? scFullType.substring(0, scFullType.lastIndexOf("."))
            : undefined,
          parameters: scParams.length > 0 ? scParams : undefined,
        };
      });

      // Build examples from demos
      const examples = comp.demos
        ?.map((demo) => {
          const code = readDemoSource(demo.type);
          return {
            title: demo.title || undefined,
            description: demo.description || undefined,
            code: code || undefined,
          };
        })
        .filter((e) => e.title || e.description || e.code);

      // Private notes
      const notes = privateNotes?.components?.[comp.slug];

      const result = {
        name: comp.title,
        slug: comp.slug,
        namespace: ns,
        summary: classDoc?.summary || undefined,
        remarks: classDoc?.remarks || undefined,
        use: classDoc?.use || undefined,
        avoid: classDoc?.avoid || undefined,
        guidelines: classDoc?.guidelines || undefined,
        designSystemUrl: comp.designSystemUrl || undefined,
        accessibilityUrl:
          comp.designSystemUrl?.replace("/overview", "/accessibility") ||
          undefined,
        lastUpdated: comp.lastUpdated || undefined,
        parameters: parameters.length > 0 ? parameters : undefined,
        subComponents: subComponents?.length > 0 ? subComponents : undefined,
        examples: examples?.length > 0 ? examples : undefined,
      };

      // Layer private notes if present
      if (notes) {
        result.internal = {
          notes: notes.notes || undefined,
          knownIssues:
            notes.knownIssues?.length > 0 ? notes.knownIssues : undefined,
          migrationNotes: notes.migrationNotes || undefined,
        };
      }

      return result;
    }),
  };

  log.debug(`Metadata: ${metadata.components.length} components`);

  log.section("Writing metadata");
  // Strip undefined values during serialization
  const json = JSON.stringify(
    metadata,
    (_, v) => (v === undefined ? undefined : v),
    2,
  );
  writeFileSync(outputPath, json, "utf-8");
  log.debug(`Metadata written to ${outputPath}`);
  log.debug(
    `Generated: ${relative(rootDir, outputPath)} (${components.length} components)`,
  );
}

try {
  buildMetadata();
} catch (e) {
  log.error(e.message);
} finally {
  console.log();
  log.success("Metadata generation complete.");
  log.debug(`Output file: ${relative(rootDir, outputPath)}`);
}
