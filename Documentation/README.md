<div align="center">
<h1>
  <a href="https://github.com/HelseVestIKT/hviktor-blazor/" align="center">
    <img src="https://github.com/HelseVestIKT/hviktor-blazor/blob/main/Documentation/wwwroot/assets/img/logo.svg" width="24"/>
  </a>
  <strong>Documentation</strong>
</h1><!-- omit in toc -->
</div>

---

> [!WARNING]
> This documentation is a work in progress.

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Install dependencies](#install-dependencies)
  - [Build CSS/JS assets](#build-cssjs-assets)
  - [Run the site](#run-the-site)
- [Services](#services)
- [Adding a New Component Page](#adding-a-new-component-page)
- [MCP Metadata](#mcp-metadata)
  - [Generating the metadata](#generating-the-metadata)
  - [Configuring the MCP server](#configuring-the-mcp-server)
  - [Private notes](#private-notes)

---

## Overview

The Documentation project is a **Blazor WebAssembly** site that serves as the interactive component library reference
for Hviktor. It renders live demos, parameter tables (including implicit HTML attributes), source code previews,
and accessibility links for every documented component.

The site also generates a **structured JSON metadata file** for consumption by
[Model Context Protocol](https://modelcontextprotocol.io/) servers and other LLM tooling, giving AI assistants full
knowledge of the Hviktor component API.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) 18+

### Install dependencies

```bash
cd Documentation
pnpm install
```

### Build CSS/JS assets

```bash
pnpm build:dev
```

This compiles SCSS via Sass and processes Tailwind CSS via PostCSS.

### Run the site

```bash
dotnet run
```

The site starts at `http://localhost:5237` (or `https://localhost:7237`).

> [!TIP]
> In Debug configuration, the project references `Hviktor`, `Hviktor.Extensions`, and `Hviktor.Icons` as local
> project references. In Release, it references the published NuGet packages.

---

## Services

| Service                    | Purpose                                                                           |
| -------------------------- | --------------------------------------------------------------------------------- |
| `ComponentRegistry`        | Maps URL slugs to component types, demos, implicit parameters, and sub-components |
| `ComponentMetadataService` | Reads `[Parameter]` properties via reflection and enriches them with XML docs     |
| `DemoSourceService`        | Reads demo `.razor` files from embedded resources for source code display         |
| `ComponentSourceService`   | Reads `.razor` and `.razor.cs` source files from disk for the "Source" tab        |
| `ThemeService`             | Manages light/dark theme preference                                               |

---

## Adding a New Component Page

- Add the component to `ComponentRegistry.cs` with its slug, title, type, demos, and implicit parameters.
- Create demo `.razor` files under `Components/Demos/` (flat or in a subfolder matching the component name).
- The `ComponentPage.razor` route (`./components/{slug}`) renders everything automatically from the registry entry.

> [!NOTE]
> Demo `.razor` files are included as embedded resources via the `.csproj` glob:
> `<EmbeddedResource Include="Components\Demos\**\*.razor"/>`.

---

## MCP Metadata

Hviktor exposes a structured JSON metadata file (`metadata.json`) for consumption by
[Model Context Protocol](https://modelcontextprotocol.io/) servers and other LLM tooling. The file contains every
documented component's parameters, implicit HTML attributes, sub-components, XML doc summaries, usage examples, and
optional private notes.

An MCP-aware client (Claude Desktop, GitHub Copilot in JetBrains/VS Code, Cursor, etc.) can read this file through a
filesystem MCP server, giving the LLM full knowledge of the Hviktor component API without needing to scan source
files.

### Generating the metadata

```bash
# Build the solution first (generates XML docs in bin/)
dotnet build

# Generate the metadata JSON
pnpm generate:metadata
```

This runs `generate-metadata.mjs`, which parses `ComponentRegistry.cs`, reads XML doc files from `bin/`, scans
`.razor.cs` files for typed `[Parameter]` properties, reads demo `.razor` source code, and optionally layers
`notes.json` for internal notes. The result is written to `metadata.json` at the repository root.

#### CLI options

```bash
node generate-metadata.mjs [options]

  --output, -o <path>          Output path (default: ../metadata.json)
  --notes, -p <path>           Private notes file (default: ./notes.json)
  --help, -h                   Show help
```

### Configuring the MCP server

A `.mcp.json` file in the workspace root registers a filesystem MCP server:

```json
{
  "servers": {
    "hviktor-metadata": {
      "type": "stdio",
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-filesystem",
        "metadata.json",
        "Documentation/notes.json",
        "AGENTS.md"
      ]
    }
  }
}
```

| File                       | Content                                                    |
| -------------------------- | ---------------------------------------------------------- |
| `metadata.json`            | Full component metadata (parameters, examples, XML docs)   |
| `Documentation/notes.json` | Internal team notes, known issues, migration notes         |
| `AGENTS.md`                | Project conventions, code rules, and architecture overview |

**JetBrains Rider / IntelliJ**: Detected automatically by GitHub Copilot.

**VS Code / Cursor**: Place `.mcp.json` in the workspace root, or configure via the MCP settings panel.

**Claude Desktop**: Add the server to `claude_desktop_config.json` instead.

### Private notes

The `notes.json` file is an optional build-time input for internal team information that should not appear
in public NuGet packages or XML docs but is valuable for developers and LLM context.

```json
{
  // Keys must match the component slug from ComponentRegistry
  "components": {
    "dialog": {
      "notes": "Free-form internal notes for the team.",
      "knownIssues": [
        "Safari < 17.5 does not support closedby attribute on <dialog>"
      ],
      "migrationNotes": "v5.0: Dialog now uses command/commandfor."
    }
  }
}
```

All fields are optional. JSON comments (`//`) are supported.

<div align="center">
  <sub>Helse Vest IKT</sub>
</div>
