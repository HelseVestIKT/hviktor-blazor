# **Hviktor.Icons**

Typed icon constants with embedded SVG data from @helsevestikt/hviktor-icons

- [Overview](#overview)
- [Installation](#installation)
- [Usage](#usage)
- [Available Icons](#available-icons)
- [Custom Icons](#custom-icons)
- [Updating Icons](#updating-icons)
- [Version Information](#version-information)
- [Documentation](#documentation)
- [Changelog](#changelog)
- [License](#license)

## Overview

`Hviktor.Icons` is a typed icon library that provides C# constants for all icons
from [`@helsevestikt/hviktor-icons`](https://www.npmjs.com/package/@helsevestikt/hviktor-icons).
Each constant contains SVG path data that is rendered as inline `<svg>` elements via the `<Icon>` component.

### Why Hviktor.Icons?

- **940+ Icons** - Complete collection from hviktor-icons
- **Inline SVG** - Rendered as `<svg>` with embedded path data, no JS loading required
- **Type-safe** - IntelliSense and compile-time errors for incorrect icon names
- **Accessible** - Designed with WCAG accessibility in mind
- **Consistent** - All icons follow the same design language
- **Easy updates** - Run `node generate-iconset.mjs` to sync

## Features

| Feature              | Description                                    |
| -------------------- | ---------------------------------------------- |
| Pre-built icons      | 940+ icons ready to use                        |
| Categorized          | Icons organized in logical categories          |
| IntelliSense support | Full autocompletion in IDE                     |
| Inline SVG           | Rendered as `<svg>` with embedded path data    |
| Theme compatible     | Inherits `currentColor` for easy color styling |

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Hviktor.Icons
```

Or via Package Manager Console:

```powershell
Install-Package Hviktor.Icons
```

## Usage

### Basic Usage

```razor
@using Hviktor.Components.Icon
@using Hviktor.Icons.Types

<Icon Definition="IconSet.Home" />
<Icon Definition="IconSet.MagnifyingGlass" />
<Icon Definition="IconSet.Cog" />
```

### With Size

```razor
@using Hviktor.Abstractions.Enums.Attributes

<Icon Definition="IconSet.Heart" Size="Size.Small" />
<Icon Definition="IconSet.Star" Size="Size.Medium" />
<Icon Definition="IconSet.Globe" Size="Size.Large" />
```

### Decorative Icons (hidden from screen readers)

```razor
<Button>
    <Icon Definition="IconSet.Plus" aria-hidden />
    Add
</Button>
```

### Icons with Accessible Description

```razor
<Icon Definition="IconSet.Phone" aria-label="Call us" />
```

## Available Icons

The icons are organized into the following categories:

| Category              | Examples                                       |
| --------------------- | ---------------------------------------------- |
| Accessibility         | `Braille`, `SignLanguage`, `UniversalAccess`   |
| Arrows                | `ArrowLeft`, `ArrowRight`, `ChevronDown`       |
| Development           | `Bug`, `Code`, `Terminal`                      |
| Files and application | `File`, `Folder`, `Download`, `Upload`         |
| Home                  | `Home`, `House`, `Building`                    |
| Interface             | `Menu`, `Search`, `Settings`, `Filter`         |
| Law and security      | `Lock`, `Shield`, `Gavel`                      |
| Media                 | `Play`, `Pause`, `Camera`, `Microphone`        |
| Money                 | `CreditCard`, `Wallet`, `Receipt`              |
| Nature and animals    | `Tree`, `Flower`, `Dog`, `Cat`                 |
| People                | `Person`, `People`, `Hand`                     |
| Statistics and math   | `Calculator`, `Chart`, `Percent`               |
| Status                | `Checkmark`, `XMark`, `Information`, `Warning` |
| Transportation        | `Car`, `Airplane`, `Bus`, `Bicycle`            |
| Wellness              | `Heart`, `Hospital`, `Stethoscope`             |
| Workplace             | `Briefcase`, `Calendar`, `Email`, `Phone`      |

### Finding Icons

In code, you can use IntelliSense by typing `IconSet.` to see all available icons.

## Custom Icons

Need icons that are not available in hviktor-icons?
See [Hviktor.Icons.Abstractions](../Hviktor.Icons.Abstractions/README.md)
for guidance on how to create custom icon definitions.

```csharp
using Hviktor.Icons.Abstractions.Types;

public static class MyIcons
{
    /// <summary>My custom icon with inline SVG path data.</summary>
    public static readonly IconDefinition MySpecialIcon = new("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10...");
}
```

## Updating Icons

To regenerate `IconSet.cs` from the latest version of `@helsevestikt/hviktor-icons`:

```bash
cd Hviktor.Icons
pnpm install
pnpm generate
```

> [!NOTE]
> Run `pnpm install` first to update to the latest version of `@helsevestikt/hviktor-icons`.

## Version Information

The current version and timestamp of the last update can be found in `icons-meta.json`:

> [!CAUTION]
> `icons-meta.json` contains information about the current version and the timestamp of the last update
> of the icon library, and should therefore not be edited manually.

Example contents:

```json
{
  "generatedAt": "2026-04-13T07:41:02.723Z",
  "version": "0.0.48",
  "source": "@helsevestikt/hviktor-icons",
  "iconCount": 947
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
