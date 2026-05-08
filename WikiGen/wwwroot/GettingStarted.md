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

**2. Import stylesheets in `App.razor`:**

```html
<link rel="stylesheet" href="_content/Hviktor/dist/index.css" />
<link rel="stylesheet" href="_content/Hviktor/dist/tailwind.css" />
```

**3. Append namespaces in `_Imports.razor`:**

```razor
@using Hviktor.Components
@using Hviktor.Abstractions.Enums.Attributes
```

### First Component

```razor
@using Hviktor.Components.Button

<Button variant="@Variant.Primary" size="@Size.Medium" @onclick="@HandleClick">
    Click me!
</Button>

@code {
    private void HandleClick()
    {
        Console.WriteLine("Button clicked!");
    }
}
```
