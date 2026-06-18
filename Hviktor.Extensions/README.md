# **Hviktor.Extensions**

Extension methods and services for Hviktor

- [Overview](#overview)
- [Installation](#installation)
- [Configuration](#configuration)
- [Contents](#contents)
- [Usage](#usage)
- [Localization](#localization)
- [Documentation](#documentation)
- [Changelog](#changelog)
- [License](#license)

## Overview

`Hviktor.Extensions` contains extension methods, helper functions, and default implementations of
Hviktor services. This package is essential for configuring and using Hviktor in your Blazor application.

## Installation

```bash
dotnet add package Hviktor.Extensions
```

## Configuration

### Registering Services

Add Hviktor services in `Program.cs`:

```csharp
using Hviktor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Default configuration
builder.Services.AddHviktor();

// With custom configuration
builder.Services.AddHviktor(
    configure: services =>
    {
        // Register own implementation of localization service (overrides default)
        services.AddScoped<ILocalizationService, Hviktor.Services.LocalizationService>();
        services.AddSingleton(typeof(IStringLocalizerService<>), typeof(StringLocalizerService<>));

        // Set the license for EPPlus to use non-commercial features
        License.SetLicense(LicenseType.NonCommercialOrganization, "Helse Vest IKT");
        services.AddScoped<FileService>();
    },
    requestLocalization: options =>
    {
        var supportedCultures = new[] { "nb-NO", "nn-NO", "en" }.Select(c => new CultureInfo(c)).ToList();
        options.DefaultRequestCulture = new RequestCulture("nb-NO");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
    },
    resources: extension =>
    {
        // Register override resources using the generic overload (preferred):
        extension.RegisterOverride<Resources>();

        // Alternatively, use the instance method:
        extension.RegisterOverride("Resources", typeof(Resources).Assembly);

        // Or register override resources using the type:
        extension.RegisterOverride(typeof(Resources));

        // You can chain multiple registrations:
        extension.RegisterOverride("Resource", typeof(Resource).Assembly)
            .RegisterOverride(typeof(Resource));
            .RegisterOverride<Resource>();
    }
);
```

### Adding Middleware

Add Hviktor middleware in `Program.cs`:

```csharp
var app = builder.Build();

// Default middleware
app.UseHviktor();

// With component injection (e.g. ReconnectModal)
app.UseHviktor(typeof(ReconnectModal));

// With multiple components
app.UseHviktor(
    typeof(ReconnectModal),
    typeof(BlazorErrorUI),
    typeof(DotNetErrorUI)
);
```

## Contents

### Extension Methods

| Class                          | Description                                    |
| ------------------------------ | ---------------------------------------------- |
| `ServiceCollectionExtensions`  | Registers Hviktor services in the DI container |
| `ApplicationBuilderExtensions` | Configures Hviktor middleware                  |
| `EnumExtensions`               | Helper methods for enum types                  |
| `Converter<T>`                 | Type conversion with fallback values           |

### Services

The package contains default implementations of the following services:

| Service                     | Interface                    | Description                                      |
| --------------------------- | ---------------------------- | ------------------------------------------------ |
| `ColorService`              | `IColorService`              | Handles color conversion                         |
| `SizeService`               | `ISizeService`               | Handles size conversion                          |
| `VariantService`            | `IVariantService`            | Handles variant conversion                       |
| `WeightService`             | `IWeightService`             | Handles font weight conversion                   |
| `WidthService`              | `IWidthService`              | Handles width conversion                         |
| `PositionService`           | `IPositionService`           | Handles positioning conversion                   |
| `PlacementService`          | `IPlacementService`          | Handles placement conversion                     |
| `InputTypeService`          | `IInputTypeService`          | Handles input type conversion                    |
| `ComparisonService`         | `IComparisonService`         | Compares values                                  |
| `JsRuntimeService`          | `IJsRuntimeService`          | JavaScript interop service                       |
| `JsObjectReferenceService`  | `IJsObjectReferenceService`  | Type-safe JS method calls via IJSObjectReference |
| `StringLocalizerService<T>` | `IStringLocalizerService<T>` | Localized strings with resource overrides        |

For more information,
see [Hviktor.Abstractions (github.com)](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/Hviktor.Abstractions/README.md),
or go directly to the [Wiki (github.com)](https://github.com/HelseVestIKT/hviktor-blazor/wiki).

## Usage

### EnumExtensions

Get descriptions from enum values:

```csharp
using Hviktor.Extensions.Reflection;
using System.ComponentModel;

public enum States
{
    [Description("In Progress")]
    InProgress,

    [Description("Approved")]
    Approved,

    [Description("Rejected")]
    Rejected
}

// Get description from enum value
var state = States.InProgress;
var description = state.GetDescription();
// Returns: "In Progress"

// Get enum value from string
var enumValue = "Approved".GetEnumValue<States>();
// Returns: States.Approved
```

### Converter

Safe type conversion with fallback values:

> [!WARNING]  
> `Converter` is under active development and should not be used in production.

```csharp
using Hviktor.Extensions;

// Convert to int
int number = Converter<int>.ToInt("42");        // 42
int fallback = Converter<int>.ToInt(null, 0);   // 0

// Convert to double
double decimal_ = Converter<double>.ToDouble("3.14");

// Convert to decimal
decimal amount = Converter<decimal>.ToDecimal("1234.56");

// Convert to string
string text = Converter<string>.ToString(value);
```

### Overriding Services

You can override default services with your own implementations:

```csharp
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Abstractions.Enums.Attributes;

public class MyColorService : IColorService
{
    public string GetDataAttribute(EnumValue<Color> value)
    {
        return GetDataAttribute(value, Color.Accent);
    }

    public string GetDataAttribute(EnumValue<Color> value, Color defaultValue)
    {
        var resolved = value.HasValue ? value.Value : defaultValue;
        return resolved switch
        {
            Color.Accent => "accent",
            Color.Success => "positive",
            Color.Danger => "negative",
            _ => resolved.ToString().ToLowerInvariant()
        };
    }

    public Color GetFromString(string value)
    {
        return GetFromString(value, default);
    }

    public Color GetFromString(string value, Color defaultValue)
    {
        return Enum.TryParse<Color>(value, true, out var result)
            ? result
            : defaultValue;
    }
}

// Register before AddHviktor() to override default services
builder.Services.AddSingleton<IColorService, MyColorService>();
builder.Services.AddHviktor();
```

## Localization

The package includes localization support:

```csharp
using Hviktor.Abstractions.Interfaces.Localization;

public class MyComponent
{
    private readonly ILocalizationService _localization;

    public MyComponent(ILocalizationService localization)
    {
        _localization = localization;
    }

    public string GetLocalizedText()
    {
        return _localization.GetString("MyKey");
    }
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
