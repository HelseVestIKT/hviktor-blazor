# **Hviktor.Extensions**

Utvidelsesmetoder og tjenester for Hviktor

- [Oversikt](#oversikt)
- [Installasjon](#installasjon)
- [Konfigurasjon](#konfigurasjon)
- [Innhold](#innhold)
- [Bruk](#bruk)
- [Lokalisering](#lokalisering)
- [Dokumentasjon](#dokumentasjon)
- [Seneste endringer](#seneste-endringer)
- [Lisens](#lisens)

## Oversikt

`Hviktor.Extensions` inneholder utvidelsesmetoder, hjelpefunksjoner og standardimplementasjoner av
Hviktor-tjenester. Denne pakken er essensiell for å konfigurere og bruke Hviktor i din Blazor-applikasjon.

## Installasjon

```bash
dotnet add package Hviktor.Extensions
```

## Konfigurasjon

### Registrere tjenester

Legg til Hviktor-tjenester i `Program.cs`:

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

### Legge til middleware

Legg til Hviktor-middleware i `Program.cs`:

```csharp
var app = builder.Build();

// Default moddleware
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

## Innhold

### Utvidelsesmetoder

| Klasse                         | Beskrivelse                                    |
| ------------------------------ | ---------------------------------------------- |
| `ServiceCollectionExtensions`  | Registrerer Hviktor-tjenester i DI-containeren |
| `ApplicationBuilderExtensions` | Konfigurerer Hviktor-middleware                |
| `EnumExtensions`               | Hjelpemetoder for enum-typer                   |
| `Converter<T>`                 | Typekonvertering med fallback-verdier          |

### Tjenester

Pakken inneholder standardimplementasjoner av følgende tjenester:

| Tjeneste            | Grensesnitt          | Beskrivelse                          |
| ------------------- | -------------------- | ------------------------------------ |
| `ColorService`      | `IColorService`      | Håndterer fargekonvertering          |
| `SizeService`       | `ISizeService`       | Håndterer størrelseskonvertering     |
| `VariantService`    | `IVariantService`    | Håndterer variantkonvertering        |
| `WeightService`     | `IWeightService`     | Håndterer skriftvektkonvertering     |
| `WidthService`      | `IWidthService`      | Håndterer breddekonvertering         |
| `PositionService`   | `IPositionService`   | Håndterer posisjoneringskonvertering |
| `PlacementService`  | `IPlacementService`  | Håndterer plasseringskonvertering    |
| `InputTypeService`  | `IInputTypeService`  | Håndterer input-type konvertering    |
| `ComparisonService` | `IComparisonService` | Sammenligning av verdier             |
| `JsRuntimeService`  | `IJsRuntimeService`  | JavaScript interop-tjeneste          |

For mer informasjon,
se [Hviktor.Abstractions (github.com)](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/Hviktor.Abstractions/README.md),
eller gå direkte til [Wiki (github.com)](https://github.com/HelseVestIKT/hviktor-blazor/wiki).

## Bruk

### EnumExtensions

Hent beskrivelser fra enum-verdier:

```csharp
using Hviktor.Extensions;
using System.ComponentModel;

public enum States
{
    [Description("Under behandling")]
    UnderBehandling,

    [Description("Godkjent")]
    Godkjent,

    [Description("Avvist")]
    Avvist
}

// Get description from enum value
var state = States.UnderBehandling;
var beskrivelse = state.GetDescription();
// Returns: "Under behandling"

// Get enum value from string
var enumVerdi = "Godkjent".GetEnumValue<States>();
// Returns: States.Godkjent
```

### Converter

Sikker typekonvertering med fallback-verdier:

> [!WARNING]  
> `Converter` er under aktiv utvikling og bør ikke brukes i produksjon.

```csharp
using Hviktor.Extensions;

// Convert to int
int tall = Converter<int>.ToInt("42");        // 42
int fallback = Converter<int>.ToInt(null, 0); // 0

// Convert to double
double desimal = Converter<double>.ToDouble("3.14");

// Convert to decimal
decimal belop = Converter<decimal>.ToDecimal("1234.56");

// Convert to string
string tekst = Converter<string>.ToString(verdi);
```

### Overstyre tjenester

Du kan overstyre standardtjenester med egne implementasjoner:

```csharp
using Hviktor.Abstractions.Interfaces;

public class MyColorService : IColorService
{
    public string GetDataAttribute(Color value)
    {
        return value switch
        {
            Color.Accent => "accent",
            Color.Success => "positive",
            Color.Danger => "negative",
            _ => value.ToString().ToLowerInvariant()
        };
    }

    public Color GetFromString(string value, Color defaultValue = default)
    {
        // My implementation
    }
}

// Register before AddHviktor() to override default services
builder.Services.AddSingleton<IColorService, MyColorService>();
builder.Services.AddHviktor();
```

## Lokalisering

Pakken inkluderer støtte for lokalisering:

```csharp
using Hviktor.Abstractions.Interfaces.Localization;

public class MyComponent
{
    private readonly ILocalizationService _localization;

    public MinKomponent(ILocalizationService localization)
    {
        _localization = localization;
    }

    public string GetLocalizedText()
    {
        return _localization.GetString("MyKey");
    }
}
```

## Dokumentasjon

Dokumentasjonen for Hviktor er skrevet på Engelsk og er tilgjengelig
på [Nettsted (helsevestikt.github.io)](https://helsevestikt.github.io/hviktor-blazor/), eller
via [Wiki](https://github.com/HelseVestIKT/hviktor-blazor/wiki).

Snarveier til Wiki dokumentasjon:

- [Home](https://github.com/HelseVestIKT/hviktor-blazor/wiki)
- [Getting started](https://github.com/HelseVestIKT/hviktor-blazor/wiki/GettingStarted)
- [Contributing](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/CONTRIBUTING.md)
- [Publish](https://github.com/HelseVestIKT/hviktor-blazor/wiki/Publish)

## Seneste endringer

Se [releases](https://github.com/HelseVestIKT/hviktor-blazor/releases/latest) for en fullstendig oversikt over
endringer og nye funksjoner.

## Lisens

Hviktor er lisensiert under MIT-lisensen. Se [LICENSE](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/LICENSE)
for detaljer.
