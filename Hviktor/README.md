# **Hviktor**

Blazor komponentbibliotek for Helse Vest IKT

- [Hva er Hviktor?](#hva-er-hviktor)
- [Installasjon](#installasjon)
- [Hurtigstart](#hurtigstart)
- [Slik fungerer komponenter](#slik-fungerer-komponenter)
- [Dokumentasjon](#dokumentasjon)
- [Seneste endringer](#seneste-endringer)
- [Lisens](#lisens)

## Hva er Hviktor?

Hviktor er et moderne Blazor komponentbibliotek utviklet av Helse Vest IKT. Biblioteket er basert på designsystemet
fra [Designsystemet (designsystemet.no)](https://designsystemet.no/) og [Aksel (aksel.nav.no)](https://aksel.nav.no/), og er spesielt tilpasset for
utvikling av helseapplikasjoner.

Hviktor bygger videre på arbeidet fra [Hviktor (github.com)](https://github.com/HelseVestIKT/Hviktor) (Angular) og tilbyr
tilsvarende funksjonalitet for Blazor-utviklere.

## Installasjon

### 1. Installer NuGet-pakken

```bash
dotnet add package Hviktor
```

### 2. Registrer tjenester i `Program.cs`

```csharp
using Hviktor.Extensions;

builder.Services.AddHviktor();
```

### 3. Legg til middleware i `Program.cs`

```csharp
app.UseHviktor();
```

### 4. Importer komponenter i `_Imports.razor`

```razor
@using Hviktor.Components
```

### 5. Legg til stilark i `App.razor` eller `_Host.cshtml`

```html
<link rel="stylesheet" href="_content/Hviktor/dist/assets/entry.css" />
```

## Hurtigstart

```razor
@using Hviktor.Components.Button @using Hviktor.Components.Alert @using
Hviktor.Components.Card

<Card>
  <Card.Block>
    <Alert color="Color.Info"> Velkommen til Hviktor! </Alert>

    <button color="@Color.Primary" @onclick="HandleClick">Klikk meg</button>
  </Card.Block>
</Card>

@code { private void HandleClick() { Console.WriteLine("Knappen ble klikket!");
} }
```

## Slik fungerer komponenter

Noen komponenter i Hviktor bruker et mønster der visse HTML-attributter — som `width`, `height` og `variant` — **ikke
** er deklarert som typede `[Parameter]`-egenskaper. I stedet fanges de opp av
`[Parameter(CaptureUnmatchedValues = true)]` og behandles internt i `ComputeAttributes()` før resterende attributter
videresendes til DOM-elementet.

Dette gir en naturlig HTML-attributtsyntaks i markup, samtidig som full typesikkerhet ivaretas i kode.

### Lengdeverdier — `CssLength` (`string | number`)

Attributter som `width` og `height` konverteres automatisk til en `CssLength`-verdi:

| Input                             | Resultat                                 |
| --------------------------------- | ---------------------------------------- |
| `width="200"`                     | `style="width: 200px"`                   |
| `width="1.5"`                     | `style="width: 1.5px"`                   |
| `width="10rem"`                   | `style="width: 10rem"`                   |
| `width="50%"`                     | `style="width: 50%"`                     |
| `width="clamp(4rem, 50%, 20rem)"` | `style="width: clamp(4rem, 50%, 20rem)"` |
| _(utelatt)_                       | ingen `style`-attributt                  |

Bare tall (heltall eller desimaltall uten enhet) får automatisk `px` som suffiks. Alle andre CSS-lengdeuttrykk sendes
gjennom uendret. Verdier av `0` eller `null` produserer en tom instans — ingen stil settes.

```razor
<Skeleton width="200" height="20" />
<Skeleton width="10rem" height="2em" />
<Skeleton width="50%" />
```

### Enum-verdier — `EnumValue<T>` (`string | enum`)

Attributter som `variant` aksepterer både en typet enum og en råstreng:

| Input                       | Sti                                                       | Output                     |
| --------------------------- | --------------------------------------------------------- | -------------------------- |
| `variant="rectangle"`       | streng → `EnumValue` rå                                   | `data-variant="rectangle"` |
| `variant="@Variant.Circle"` | bokset enum → `.ToString()` → `"Circle"` → `EnumValue` rå | `data-variant="circle"`    |
| _(utelatt)_                 | tom `EnumValue` → standardverdi                           | `data-variant="rectangle"` |

Når en typet enum sendes fra C# (f.eks. `variant="@Variant.Text"`), bokser Blazor verdien som `object` i
`AdditionalAttributes`. Komponenten kaller `.ToString()` på den boksede verdien, som gir `"Text"`, og viderebehandler
dette som en streng. `GetFromString` gjør en case-insensitiv `Enum.TryParse` og returnerer den kanoniske lowercase
data-attributtverdien.

```razor
@* Råstreng *@
<Skeleton variant="text" width="200" />

@* Typet enum fra C# *@
<Skeleton variant="@Variant.Circle" width="48" height="48" />
```

## Dokumentasjon

Dokumentasjonen for Hviktor er skrevet på Engelsk og er tilgjengelig på [Nettsted (helsevestikt.github.io)](https://helsevestikt.github.io/hviktor-blazor/), eller via [Wiki](https://github.com/HelseVestIKT/hviktor-blazor/wiki).

Snarveier til Wiki dokumentasjon:

- [Home](https://github.com/HelseVestIKT/hviktor-blazor/wiki)
- [Getting started](https://github.com/HelseVestIKT/hviktor-blazor/wiki/GettingStarted)
- [Contributing](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/CONTRIBUTING.md)
- [Publish](https://github.com/HelseVestIKT/hviktor-blazor/wiki/Publish)

## Seneste endringer

Se [releases](https://github.com/HelseVestIKT/hviktor-blazor/releases/latest) for en fullstendig oversikt over
endringer og nye funksjoner.

## Lisens

Hviktor er lisensiert under MIT-lisensen. Se [LICENSE](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/LICENSE) for detaljer.
