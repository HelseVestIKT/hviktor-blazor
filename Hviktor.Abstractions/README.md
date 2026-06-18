# **Hviktor.Abstractions**

Grensesnitt, basistyper og abstraksjoner for Hviktor

- [Oversikt](#oversikt)
- [Installasjon](#installasjon)
- [Bruk](#bruk)
- [Dokumentasjon](#dokumentasjon)
- [Seneste endringer](#seneste-endringer)
- [Lisens](#lisens)

## Oversikt

`Hviktor.Abstractions` inneholder grensesnitt, basisklasser og enumer som brukes på tvers av Hviktor
komponentbiblioteket. Denne pakken er grunnlaget for å lage egendefinerte tjenester og modeller som integreres med
Hviktor-komponenter.

## Installasjon

```bash
dotnet add package Hviktor.Abstractions
```

## Bruk

### Grensesnitt (Interfaces)

Pakken inneholder følgende grensesnitt for å utvide og tilpasse Hviktor:

| Grensesnitt            | Beskrivelse                                   |
| ---------------------- | --------------------------------------------- |
| `IColorService`        | Tjeneste for håndtering av farger             |
| `ISizeService`         | Tjeneste for håndtering av størrelser         |
| `IVariantService`      | Tjeneste for håndtering av komponentvarianter |
| `ISeverityService`     | Tjeneste for håndtering av alvorlighetsnivåer |
| `IPlacementService`    | Tjeneste for håndtering av plassering         |
| `IPositionService`     | Tjeneste for håndtering av posisjonering      |
| `IWeightService`       | Tjeneste for håndtering av skriftvekt         |
| `IWidthService`        | Tjeneste for håndtering av bredde             |
| `IInputTypeService`    | Tjeneste for håndtering av inputtyper         |
| `IParameterService<T>` | Generisk tjeneste for parameterkonvertering   |
| `IComparisonService`   | Tjeneste for sammenligning av verdier         |

### Implementere IParameterService

Lag egne parametertjenester for å konvertere verdier til HTML-attributter:

```csharp
using Hviktor.Abstractions.Interfaces;
using Hviktor.Abstractions.Types;

public class CustomColorService : IParameterService<Color>
{
    public string GetDataAttribute(Color value)
    {
        return value switch
        {
            Color.Accent => "brand",
            Color.Success => "positive",
            Color.Danger => "negative",
            _ => value.ToString().ToLowerInvariant()
        };
    }

    public Color GetFromString(string value, Color defaultValue = default)
    {
        return Enum.TryParse<Color>(value, true, out var result)
            ? result
            : defaultValue;
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
