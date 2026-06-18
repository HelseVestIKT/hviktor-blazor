# **Tests.Playwright**

See [Playwright.dev (playwright.dev)](https://playwright.dev/dotnet/docs/intro) .NET documentation for detailed installation and usage
guides.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)

## Installation

1. From root, go to the `Tests.Playwright` directory:

   ```bash
    cd Tests.Playwright
   ```

2. Install or update .NET `Playwright` package:
   ```bash
   dotnet add package Microsoft.Playwright.Xunit.v3
   ```
3. Build the `Tests.Playwright` project:
   ```bash
   dotnet build
   ```
4. Install required browsers:
   ```bash
   ./bin/Debug/net10.0/playwright.ps1 install
   ```

## Usage

1. From root, go to the `Tests.Playwright` directory:
   ```bash
    cd Tests.Playwright
   ```
2. Run the tests:
   ```bash
   dotnet test
   ```

## Configuration

Playwright settings are loaded from `testconfig.json` (CI) or `testconfig.Development.json` (local override).
Use the development config for local debugging (e.g., `headless: false`, `slowMo: 500`).
The `testconfig.Development.json` file is gitignored.
