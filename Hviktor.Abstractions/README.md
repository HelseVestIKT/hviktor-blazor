<div align="center">
  <h1>Hviktor.Abstractions</h1>
  <p><strong>Grensesnitt, basistyper og abstraksjoner for Hviktor</strong></p>
</div>

---

## Oversikt

`Hviktor.Abstractions` inneholder grensesnitt, basisklasser og enumer som brukes på tvers av Hviktor
komponentbiblioteket. Denne pakken er grunnlaget for å lage egendefinerte tjenester og modeller som integreres med
Hviktor-komponenter.

## Installasjon

```bash
dotnet add package Hviktor.Abstractions
```

## Innhold

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

## Bruk

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

## Komponentgrensesnitt

Pakken inneholder også grensesnitt for komponentegenskaper:

| Grensesnitt      | Beskrivelse                          |
| ---------------- | ------------------------------------ |
| `IAccessibility` | Tilgjengelighetsegenskaper (aria-\*) |
| `ICommon`        | Felles komponentegenskaper           |
| `IEvent`         | Hendelseshåndtering                  |
| `IStyle`         | Stilegenskaper                       |

## Se også

- [Hviktor](../Hviktor/README.md) - Hovedkomponentbiblioteket
- [Hviktor.Attributes](../Hviktor.Attributes/README.md) - Attributter for skjemavalidering
- [Hviktor.Extensions](../Hviktor.Extensions/README.md) - Utvidelsesmetoder
- [Hviktor Wiki](https://github.com/HelseVestIKT/hviktor-blazor/wiki)

---

<div align="center">
  <sub>En del av Hviktor komponentbiblioteket</sub><br/>
  <sub>Helse Vest IKT</sub>
</div>
