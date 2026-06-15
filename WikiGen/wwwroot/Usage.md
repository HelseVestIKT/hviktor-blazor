# Usage

## Installation

Add the GitHub Packages registry as a NuGet source (one-time setup):

```shell
dotnet nuget add source "https://api.nuget.org/v3/index.json" --name "nuget.org"
```

Install the required NuGet packages:

```shell
dotnet add package Hviktor
dotnet add package Hviktor.Abstractions
dotnet add package Hviktor.Extensions
```

To use the icon library, also install:

```shell
dotnet add package Hviktor.Icons
dotnet add package Hviktor.Icons.Abstractions
```

## Register Services

In your `Program.cs`, register the Hviktor services and middleware:

```csharp
using Hviktor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Hviktor services
builder.Services.AddHviktor();

var app = builder.Build();

// Add Hviktor middleware (injects required scripts)
app.UseHviktor();

app.Run();
```

### Configuration Options

`AddHviktor` accepts optional parameters for customization:

```csharp
builder.Services.AddHviktor(
    configure: services =>
    {
        // Register or override additional services
    },
    requestLocalization: options =>
    {
        // Configure request localization
        options.SetDefaultCulture("nb-NO");
    },
    resources: manager =>
    {
        // Register resource overrides for localization
    }
);
```

## Add Imports

Add the following to your `_Imports.razor` to make Hviktor components available globally:

```razor
@using Hviktor.Components
@using Hviktor.Abstractions.Enums.Attributes
```

If using icons:

```razor
@using Hviktor.Icons.Types
```

## Include Styles

Reference the Hviktor stylesheet in your `App.razor` or layout:

```html
<link rel="stylesheet" href="_content/Hviktor/dist/index.css" />
<link rel="stylesheet" href="_content/Hviktor/dist/tailwind.css" />
```

## Using Components

Hviktor provides a wide range of Blazor components based on Designsystemet. Here are some examples:

### Button

```razor
<Button Variant="Variant.Primary" Size="Size.Medium">
    Click me
</Button>
```

### Alert

```razor
<Alert Color="Color.Info">
    This is an informational alert.
</Alert>
```

### Card

```razor
<Card>
    <CardHeader>Title</CardHeader>
    <CardContent>Card body content goes here.</CardContent>
</Card>
```

### Icons

Use typed icon constants from `IconSet` with the `Icon` component:

```razor
<Icon Definition="IconSet.Checkmark" />
```

For decorative icons, `aria-hidden="true"` is applied automatically.

## Available Components

For a complete list of available components, refer to
the [Component Catalog](https://literate-carnival-7p3vnlg.pages.github.io/) in the documentation.

## Design Tokens

Hviktor uses Designsystemet design tokens for colors, spacing, and typography.
Use the CSS custom properties provided by the theme.

Components support `Color`, `Variant`, and `Size` parameters to control their appearance.

## Accessibility

All components are built to meet WCAG 2.1 AA. Ensure your application maintains proper heading hierarchy, landmark
regions, and keyboard navigation when composing Hviktor components.
