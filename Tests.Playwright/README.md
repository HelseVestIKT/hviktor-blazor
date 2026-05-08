<div align="center">
<h1>
  <a href="https://github.com/HelseVestIKT/hviktor-blazor/" align="center">
    <img src="https://github.com/HelseVestIKT/hviktor-blazor/blob/main/Documentation/wwwroot/assets/img/logo.svg" width="24"/>
  </a>
  <strong>Tests.Playwright</strong>
</h1><!-- omit in toc -->
</div>

See [Playwright.dev](https://playwright.dev/dotnet/docs/intro) .NET documentation for detailed installation and usage
guides.

## Installation

1. From root (`C:\..\Hviktor\`), go to `Tests.Playwright`directory:

   ```bash
    cd .\Tests\Tests.Playwright
   ```

2. Install or update .NET `Playwright` package:
   ```bash
   dotnet add package Microsoft.Playwright.Xunit.v3
   ```
3. Build the `Tests.Playwright` project:
   ```bash
   dotnet build Tests.Playwright
   ```
4. Install required browsers:
   ```bash
   ./bin/Debug/net10.0/playwright.ps1 install
   ```

## Usage

1. From root (`C:\..\Hviktor\`), go to `Tests.Playwright`directory:

   ```bash
    cd .\Tests\Tests.Playwright
   ```

2. Run the tests:
   ```bash
   dotnet run Tests.Playwright
   ```

<h2></h2>
<div align="center">
  <sub>Helse Vest IKT</sub>
</div>
