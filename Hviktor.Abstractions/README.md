# **Hviktor.Abstractions**

Interfaces, base types, and abstractions for Hviktor

- [Overview](#overview)
- [Installation](#installation)
- [Usage](#usage)
- [Documentation](#documentation)
- [Changelog](#changelog)
- [License](#license)

## Overview

`Hviktor.Abstractions` contains interfaces, base classes, and enums used across the Hviktor
component library. This package is the foundation for creating custom services and models that integrate with
Hviktor components.

## Installation

```bash
dotnet add package Hviktor.Abstractions
```

## Usage

### Interfaces

The package contains the following interfaces for extending and customizing Hviktor:

#### Attribute Services

| Interface              | Description                              |
| ---------------------- | ---------------------------------------- |
| `IColorService`        | Service for handling colors              |
| `ISizeService`         | Service for handling sizes               |
| `IVariantService`      | Service for handling component variants  |
| `IPlacementService`    | Service for handling placement           |
| `IPositionService`     | Service for handling positioning         |
| `IWeightService`       | Service for handling font weight         |
| `IWidthService`        | Service for handling width               |
| `IInputTypeService`    | Service for handling input types         |
| `IParameterService<T>` | Generic service for parameter conversion |
| `IComparisonService`   | Service for comparing values             |

#### Localization and JS Interop

| Interface                    | Description                                                  |
| ---------------------------- | ------------------------------------------------------------ |
| `ILocalizationService`       | Service for handling language and localization               |
| `IStringLocalizerService<T>` | Service for localized strings with resource overrides        |
| `IJsRuntimeService`          | Service for dynamic import of JavaScript modules             |
| `IJsObjectReferenceService`  | Service for type-safe JS method calls via IJSObjectReference |

#### Component Interfaces

| Interface                  | Description                              |
| -------------------------- | ---------------------------------------- |
| `ICascadingComponent`      | Interface for cascading component values |
| `IAsyncCascadingComponent` | Async variant of cascading               |

### Implementing IParameterService

Create custom parameter services to convert values to HTML attributes:

```csharp
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Services;

/// <summary>
/// A custom service that provides additional functionality for working with the <see cref="Color"/> enum.
/// </summary>
public class ColorService : IParameterService<Color>
{
    /// <inheritdoc />
    public string GetDataAttribute(EnumValue<Color> value)
        => GetDataAttribute(value, Color.Accent);

    /// <summary>
    /// Returns a custom string value for the specified <see cref="Color"/> values.
    /// </summary>
    public string GetDataAttribute(EnumValue<Color> value, Color defaultValue)
    {
        var resolved = value.EnumValueOrNull ?? defaultValue;
        return resolved switch
        {
            Color.Accent => "accent",
            Color.Success => "positive",
            Color.Brand1 => "corp",
            Color.Brand2 => "corp-secondary",
            Color.Danger => "negative",
            _ => resolved.ToString().ToLowerInvariant()
        };
    }

    /// <inheritdoc />
    public Color GetFromString(string value) => GetFromString(value, default);

    /// <inheritdoc />
    public Color GetFromString(string value, Color defaultValue) =>
        Enum.TryParse<Color>(value, true, out var result)
            ? result
            : defaultValue;
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
