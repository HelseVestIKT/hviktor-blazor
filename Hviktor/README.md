<div align="center">
<h1>
  <a href="https://github.com/HelseVestIKT/hviktor-blazor/" align="center">
    <img src="https://github.com/HelseVestIKT/hviktor-blazor/blob/main/Hviktor/logo.svg" width="24"/>
  </a>
  <strong>HVIKTOR</strong>
</h1><!-- omit in toc -->

  <p><strong>Blazor komponentbibliotek for Helse Vest IKT</strong></p>

  <p>
    <a href="#funksjoner">Funksjoner</a> •
    <a href="#installasjon">Installasjon</a> •
    <a href="#hurtigstart">Hurtigstart</a> •
    <a href="#slik-fungerer-komponenter">Slik fungerer komponenter</a> •
    <a href="#dokumentasjon">Dokumentasjon</a>
  </p>
</div>

---

## Hva er Hviktor?

Hviktor er et moderne Blazor komponentbibliotek utviklet av Helse Vest IKT. Biblioteket er basert på designsystemet
fra [Designsystemet](https://designsystemet.no/) og [Aksel](https://aksel.nav.no/), og er spesielt tilpasset for
utvikling av helseapplikasjoner.

Hviktor bygger videre på arbeidet fra [Hviktor](https://github.com/HelseVestIKT/Hviktor) (Angular) og tilbyr
tilsvarende funksjonalitet for Blazor-utviklere.

## Funksjoner

| Funksjon               | Beskrivelse                                                |
| ---------------------- | ---------------------------------------------------------- |
| 35+ komponenter        | Ferdigbygde UI-komponenter for rask utvikling              |
| Tilgjengelighet (WCAG) | Alle komponenter er designet med universell utforming      |
| Responsivt design      | Fungerer på alle skjermstørrelser                          |
| Blazor Server & WASM   | Støtter både Blazor Server og WebAssembly                  |
| Tailwind CSS           | Integrert med Tailwind for enkel styling                   |
| Ikonbibliotek          | 900+ ikoner via @helsevestikt/hviktor-icons web components |
| Lokalisering           | Innebygd støtte for flerspråklighet                        |

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
@using Hviktor.Components.Button
@using Hviktor.Components.Alert
@using Hviktor.Components.Card

<Card>
    <Card.Block>
        <Alert color="Color.Info">
            Velkommen til Hviktor!
        </Alert>

        <Button color="@Color.Primary" @onclick="HandleClick">
            Klikk meg
        </Button>
    </Card.Block>
</Card>

@code {
    private void HandleClick()
    {
        Console.WriteLine("Knappen ble klikket!");
    }
}
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
{{/* Råstreng */}}
<Skeleton variant="text" width="200" />

{{/* Typet enum fra C# */}}
<Skeleton variant="@Variant.Circle" width="48" height="48" />
```

## Dokumentasjon

Wiki:

- [Hjem](https://github.com/HelseVestIKT/hviktor-blazor/wiki)
- [Kom i gang](https://github.com/HelseVestIKT/hviktor-blazor/wiki/Get%20started)
- [Bidra of publiser](https://github.com/HelseVestIKT/hviktor-blazor/wiki/Contributing)

## Seneste endringer

Se [releases](https://github.com/HelseVestIKT/hviktor-blazor/releases/latest) for en fullstendig oversikt over
endringer og nye funksjoner.

## Lisens

Dette prosjektet er utviklet av Helse Vest IKT for intern bruk i helseregionen.

---

<div align="center">
  <sub>Helse Vest IKT</sub>
</div>
