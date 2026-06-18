# **Hviktor**

Blazor component library for Helse Vest IKT

- [What is Hviktor?](#what-is-hviktor)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [How Components Work](#how-components-work)
- [Documentation](#documentation)
- [Changelog](#changelog)
- [License](#license)

## What is Hviktor?

Hviktor is a modern Blazor component library developed by Helse Vest IKT. The library is based on the design system
from [Designsystemet (designsystemet.no)](https://designsystemet.no/) and [Aksel (aksel.nav.no)](https://aksel.nav.no/),
and is specifically designed for
developing healthcare applications.

Hviktor builds on the work from [Designsystemet (designsystemet.no)](https://designsystemet.no/) and aims to offer
equivalent functionality for Blazor developers.

## Installation

### 1. Install the NuGet package

```bash
dotnet add package Hviktor
```

### 2. Register services in `Program.cs`

```csharp
using Hviktor.Extensions;

builder.Services.AddHviktor();
```

### 3. Add middleware in `Program.cs`

```csharp
app.UseHviktor();
```

### 4. Import components in `_Imports.razor`

```razor
@using Hviktor.Components
```

### 5. Add stylesheets in `App.razor` or `_Host.cshtml`

```html
<link rel="stylesheet" href="_content/Hviktor/dist/assets/entry.css" />
```

## Quick Start

```razor
@using Hviktor.Components.Button @using Hviktor.Components.Alert @using
Hviktor.Components.Card

<Card>
  <Card.Block>
    <Alert color="Color.Info"> Welcome to Hviktor! </Alert>

    <button color="@Color.Primary" @onclick="HandleClick">Click me</button>
  </Card.Block>
</Card>

@code { private void HandleClick() { Console.WriteLine("The button was clicked!");
} }
```

## How Components Work

Some components in Hviktor use a pattern where certain HTML attributes, such as `width`, `height`, and `variant`, are
**not** declared as typed `[Parameter]` properties. Instead, they are captured by
`[Parameter(CaptureUnmatchedValues = true)]` and processed internally in `ComputeAttributes()` before the remaining
attributes are forwarded to the DOM element.

This provides a natural HTML attribute syntax in markup while maintaining full type safety in code.

### Length Values: `CssLength` (`string | number`)

Attributes such as `width` and `height` are automatically converted to a `CssLength` value:

| Input                             | Result                                   |
| --------------------------------- | ---------------------------------------- |
| `width="200"`                     | `style="width: 200px"`                   |
| `width="1.5"`                     | `style="width: 1.5px"`                   |
| `width="10rem"`                   | `style="width: 10rem"`                   |
| `width="50%"`                     | `style="width: 50%"`                     |
| `width="clamp(4rem, 50%, 20rem)"` | `style="width: clamp(4rem, 50%, 20rem)"` |
| _(omitted)_                       | no `style` attribute                     |

Only numbers (integers or decimals without a unit) automatically receive `px` as a suffix. All other CSS length
expressions are passed through unchanged. Values of `0` or `null` produce an empty instance: no style is set.

```razor
<Skeleton width="200" height="20" />
<Skeleton width="10rem" height="2em" />
<Skeleton width="50%" />
```

### Enum Values: `EnumValue<T>` (`string | enum`)

Attributes such as `variant` accept both a typed enum and a raw string:

| Input                       | Path                                                         | Output                     |
| --------------------------- | ------------------------------------------------------------ | -------------------------- |
| `variant="rectangle"`       | string -> `EnumValue` raw                                    | `data-variant="rectangle"` |
| `variant="@Variant.Circle"` | boxed enum -> `.ToString()` -> `"Circle"` -> `EnumValue` raw | `data-variant="circle"`    |
| _(omitted)_                 | empty `EnumValue` -> default value                           | `data-variant="rectangle"` |

When a typed enum is passed from C# (e.g., `variant="@Variant.Text"`), Blazor boxes the value as `object` in
`AdditionalAttributes`. The component calls `.ToString()` on the boxed value, which yields `"Text"`, and further
processes this as a string. `GetFromString` performs a case-insensitive `Enum.TryParse` and returns the canonical
lowercase data attribute value.

```razor
@* Raw string *@
<Skeleton variant="text" width="200" />

@* Typed enum from C# *@
<Skeleton variant="@Variant.Circle" width="48" height="48" />
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
