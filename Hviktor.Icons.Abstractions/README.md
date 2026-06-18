# **Hviktor.Icons.Abstractions**

Abstractions for creating custom icon definitions in Hviktor

- [Overview](#overview)
- [Installation](#installation)
- [Creating Custom Icons](#creating-custom-icons)
- [API Reference](#api-reference)
- [Best Practices](#best-practices)
- [Documentation](#documentation)
- [Changelog](#changelog)
- [License](#license)

## Overview

`Hviktor.Icons.Abstractions` provides foundational types for creating custom icon definitions for use with the Hviktor
component library. This package lets you define your own icons when the built-in icons
from [@helsevestikt/hviktor-icons (npmjs.com)](https://www.npmjs.com/package/@helsevestikt/hviktor-icons) in
`Hviktor.Icons` do not cover your needs.

## Installation

```bash
dotnet add package Hviktor.Icons.Abstractions
```

## Creating Custom Icons

### Define Your Icon

Create a static class with `IconDefinition` constants. Each definition contains
SVG path data (the `d` attribute) for the icon:

```csharp
using Hviktor.Icons.Abstractions.Types;

namespace MyApp.Icons;

public static class CustomIcons
{
    /// <summary>
    /// Blood bag icon for medical applications.
    /// </summary>
    public static readonly IconDefinition BloodBag = new("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10...");

    /// <summary>
    /// Syringe icon for medical applications.
    /// </summary>
    public static readonly IconDefinition Syringe = new("M19.5 4.5l-1-1L17 5l-2.5-2.5-1 1L16 6...");
}
```

### Use the Custom Icon

Use the custom icon with the `<Icon>` component:

```razor
@using Hviktor.Components.Icon
@using Hviktor.Abstractions.Enums.Attributes
@using MyApp.Icons

<Icon Definition="CustomIcons.BloodBag"/>

@* With size *@
<Icon Definition="CustomIcons.BloodBag" Size="Size.Large"/>

@* With additional attributes *@
<Icon Definition="CustomIcons.BloodBag" class="text-red-500" aria-hidden/>
```

## API Reference

### IconDefinition

The `IconDefinition` class encapsulates SVG path data for an icon.

| Property   | Type     | Description                                          |
| ---------- | -------- | ---------------------------------------------------- |
| `PathData` | `string` | SVG path `d` attribute data for the icon             |
| `HasValue` | `bool`   | Returns `true` if the path data is valid (not empty) |

**Constructor:**

```csharp
public IconDefinition(string pathData)
```

## Best Practices

1. **Organize icons by category** - Group related icons in separate static classes
2. **Add XML documentation** - Document each icon for IntelliSense support
3. **Use semantic names** - Name icons descriptively (e.g., `BloodBag`, not `Icon1`)
4. **Test accessibility** - Ensure icons have the correct `aria-hidden` when decorative
5. **Use standard SVG path data** - Extract the `d` attribute from SVG files or design tools

## Example: Complete Custom Icon Library

```csharp
using Hviktor.Icons.Abstractions.Types;

namespace MyApp.Icons;

/// <summary>
/// Custom medical icons for healthcare applications.
/// </summary>
public static class MedicalIcons
{
    /// <summary>Blood bag icon.</summary>
    public static readonly IconDefinition BloodBag = new("M12 2C6.48 2 2 6.48 2 12...");

    /// <summary>Syringe icon.</summary>
    public static readonly IconDefinition Syringe = new("M19.5 4.5l-1-1L17 5l-2.5-2.5...");

    /// <summary>Stethoscope icon.</summary>
    public static readonly IconDefinition Stethoscope = new("M9 2v1H6a2 2 0 00-2 2v6...");
}
```

## Documentation

The documentation for Hviktor is written in English and is available
at [Website (helsevestikt.github.io)](https://helsevestikt.github.io/hviktor-blazor/), or
via [Wiki](https://github.com/HelseVestIKT/hviktor-blazor/wiki).

Wiki documentation shortcuts:

- [Home](https://github.com/HelseVestIKT/hviktor-blazor/wiki)
- [Getting started](https://github.com/HelseVestIKT/hviktor-blazor/wiki/GettingStarted)
- [Contributing](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/CONTRIBUTING.md)
- [Publish](https://github.com/HelseVestIKT/hviktor-blazor/wiki/Publish)

## Changelog

See [releases](https://github.com/HelseVestIKT/hviktor-blazor/releases/latest) for a complete overview of
changes and new features.

## License

Hviktor is licensed under the MIT License.
See [LICENSE](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/LICENSE) for details.
