# Getting Started

## Installation

Add the GitHub Packages registry as a NuGet source (one-time setup):

```bash
dotnet nuget add source "https://nuget.pkg.github.com/HelseVestIKT/index.json" --name "github-hviktor" --username YOUR_USERNAME --password YOUR_GITHUB_TOKEN
```

Install Hviktor via the .NET CLI:

```bash
dotnet add package Hviktor
dotnet add package Hviktor.Abstractions
dotnet add package Hviktor.Extensions
```

### Setup

**1. Register services in `Program.cs`:**

```csharp
using Hviktor.Accessors.Layout;
using Hviktor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Hviktor services
builder.Services.AddHviktor();

var app = builder.Build();

// Configure Hviktor
app.UseHviktor(typeof(ReconnectModal), typeof(BlazorErrorUI), typeof(DotNetErrorUI));

await app.RunAsync();
```

**2. Load scripts and styles in `App.razor`:**

The simplest approach is to load `entry.js`, which injects all required CSS automatically at runtime. It provides:

- Blazor startup orchestration
- JS interop for interactive components (Search, Tooltip, Popover, and more)
- Lazy-loading of icon web components
- CSS injection (when not using a custom SCSS build)

```html
<script type="module" src="_content/Hviktor/dist/entry.js" async></script>
```

**Blazor WebAssembly: compile-time CSS (recommended)**

For Blazor WebAssembly apps, load `entry.css` as a separate `<link>` tag alongside your own compiled stylesheet.
This avoids a flash of unstyled content during the WASM boot phase and keeps CSS loading independent of JavaScript.

In `index.html`, preload both stylesheets and the entry script for parallel download, then apply them in order —
your app stylesheet first, `entry.css` second:

```html
<head>
  <!-- Preload for parallel download -->
  <link rel="preload" href="styles/index.css" as="style" />
  <link
    rel="preload"
    href="_content/Hviktor/dist/assets/entry.css"
    as="style"
  />
  <link rel="modulepreload" href="_framework/blazor.webassembly.js" />
  <link rel="modulepreload" href="_content/Hviktor/dist/entry.js" />

  <!-- Apply stylesheets: app styles first, then Hviktor -->
  <link rel="stylesheet" href="styles/index.css" />
  <link rel="stylesheet" href="_content/Hviktor/dist/assets/entry.css" />
</head>
<body>
  <script src="_framework/blazor.webassembly.js" autostart="false"></script>
  <script type="module" src="_content/Hviktor/dist/entry.js" async></script>
</body>
```

**Important: if you use Tailwind CSS, order matters**

Tailwind's base and reset styles must come before Hviktors design tokens. Place `@use "tailwind"` as the
very first import in your root SCSS file, and load `entry.css` as a separate `<link>` tag — not as an SCSS import.
Importing `entry.css` into SCSS would place Hviktors styles inside the same cascade layer as Tailwind's reset,
causing the design tokens to be overridden.

```scss
// Tailwind MUST be first
@use "tailwind"; // compiles @import "tailwindcss" via postcss

// Your app styles after Tailwind
@use "layout";
@use "navbar";
```

**Blazor WebAssembly only:** The UI overlay assets (`blazor-error-ui`, `dot-net-error-ui`, `reconnect-modal`) are
standalone and not order-sensitive, so they can be imported inside your SCSS:

```scss
@import "../_content/Hviktor/dist/assets/blazor-error-ui.css";
@import "../_content/Hviktor/dist/assets/dot-net-error-ui.css";
@import "../_content/Hviktor/dist/assets/reconnect-modal.css";
```

For Blazor Server, pass the component types to `UseHviktor()` instead. The middleware renders the components and
injects both their HTML and CSS `<link>` tags automatically into every HTML response.

**Blazor Server: automatic injection**

For Blazor Server apps, `UseHviktor()` automatically:

- Injects `entry.css` and `entry.js` into every HTML response.
- Renders and injects any component types passed as arguments (e.g., `ReconnectModal`), including their CSS.
- Calls `UseRequestLocalization()` internally, so you do not need to call it separately.

Preloading styles and scripts is still recommended for perceived performance.

**3. Append namespaces in `_Imports.razor`:**

```razor
@using Hviktor.Components
@using Hviktor.Abstractions.Enums.Attributes
```

### First Component

```razor
@using Hviktor.Components.Button
@inject ILogger<MyPage> Logger

<Button variant="@Variant.Primary" size="@Size.Medium" @onclick="@HandleClick">
    Click me!
</Button>

@code {
    private void HandleClick()
    {
        Logger.LogInformation("Button clicked!");
    }
}
```
