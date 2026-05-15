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
using Hviktor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Hviktor services
builder.Services.AddHviktor();

var app = builder.Build();

// Configure Hviktor
app.UseHviktor();

app.Run();
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

By default `entry.js` also injects all required CSS at runtime, so no additional stylesheet tags are needed.

**Alternative: compile-time CSS (recommended for apps with their own SCSS build)**

If your host app has an SCSS build pipeline you can import Hviktor's pre-built CSS directly into your own stylesheet.
This moves CSS out of the JS runtime and into a single compiled bundle, avoiding a flash of unstyled content on load.
`entry.js` is still required for the JS runtime, only the CSS injection is replaced.

```scss
// styles/imports.scss
@import "../_content/Hviktor/dist/assets/entry.css";

// Optional: Blazor UI overlays
@import "../_content/Hviktor/dist/assets/blazor-error-ui.css";
@import "../_content/Hviktor/dist/assets/dot-net-error-ui.css";
@import "../_content/Hviktor/dist/assets/reconnect-modal.css";
```

Then reference the compiled stylesheet alongside `entry.js`:

```html
<link rel="stylesheet" href="styles/index.css" />
<script type="module" src="_content/Hviktor/dist/entry.js" async></script>
```

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
