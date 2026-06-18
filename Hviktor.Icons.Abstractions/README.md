# **Hviktor.Icons.Abstractions**

Abstraksjoner for å lage egendefinerte ikondefinisjoner i Hviktor

- [Oversikt](#oversikt)
- [Installasjon](#installasjon)
- [Lage egendefinerte ikoner](#lage-egendefinerte-ikoner)
- [API-referanse](#api-referanse)
- [Beste praksis](#beste-praksis)
- [Dokumentasjon](#dokumentasjon)
- [Seneste endringer](#seneste-endringer)
- [Lisens](#lisens)

## Oversikt

`Hviktor.Icons.Abstractions` tilbyr grunnleggende typer for å lage egendefinerte ikondefinisjoner til bruk med
Hviktor komponentbiblioteket. Denne pakken lar deg definere egne ikoner når de
innebygde ikonene
fra [@helsevestikt/hviktor-icons (npmjs.com)](https://www.npmjs.com/package/@helsevestikt/hviktor-icons)
i `Hviktor.Icons` ikke dekker dine behov.

## Installasjon

```bash
dotnet add package Hviktor.Icons.Abstractions
```

## Lage egendefinerte ikoner

### Steg 1: Definer ikonet ditt

Opprett en statisk klasse med `IconDefinition`-konstanter. Hver definisjon inneholder
navnet på det tilhørende `<hvi-icon-*>` web component custom element:

```csharp
using Hviktor.Icons.Abstractions.Types;

namespace MyApp.Icons;

public static class CustomIcons
{
    /// <summary>
    /// Blood bag icon for medical applications.
    /// Renders as <c>&lt;hvi-icon-blood-bag /&gt;</c>.
    /// </summary>
    public static readonly IconDefinition BloodBag = new("hvi-icon-blood-bag");

    /// <summary>
    /// Syringe icon for medical applications.
    /// Renders as <c>&lt;hvi-icon-syringe /&gt;</c>.
    /// </summary>
    public static readonly IconDefinition Syringe = new("hvi-icon-syringe");
}
```

### Steg 2: Registrer web component

Det tilhørende web component custom element må være registrert i nettleseren.
Hvis ikonet finnes i `@helsevestikt/hviktor-icons`, registreres det automatisk
via `entry.ts`-importen. For egendefinerte ikoner må du sørge for at
custom element er registrert separat.

### Steg 3: Bruk det egendefinerte ikonet

Bruk det egendefinerte ikonet med `<Icon>`-komponenten:

```razor
@using Hviktor.Components.Icon
@using Hviktor.Abstractions.Enums.Attributes
@using MyApp.Icons

<Icon Definition="CustomIcons.BloodBag"/>

@* Med størrelse *@
<Icon Definition="CustomIcons.BloodBag" Size="Size.Large"/>

@* Med tilleggsattributter *@
<Icon Definition="CustomIcons.BloodBag" class="text-red-500" aria-hidden/>
```

## API-referanse

### IconDefinition

`IconDefinition`-klassen innkapsler elementnavnet for et web component-ikon.

| Egenskap      | Type     | Beskrivelse                                                 |
| ------------- | -------- | ----------------------------------------------------------- |
| `ElementName` | `string` | Web component-elementnavnet (f.eks. `hvi-icon-arrow-right`) |
| `HasValue`    | `bool`   | Returnerer `true` hvis elementnavnet er gyldig (ikke tomt)  |

**Konstruktør:**

```csharp
public IconDefinition(string elementName)
```

## Beste praksis

1. **Organiser ikoner etter kategori** — Grupper relaterte ikoner i separate statiske klasser
2. **Legg til XML-dokumentasjon** — Dokumenter hvert ikon for IntelliSense-støtte
3. **Bruk semantiske navn** — Navngi ikoner beskrivende (f.eks. `BloodBag`, ikke `Icon1`)
4. **Test tilgjengelighet** — Sørg for at ikoner har riktig `aria-hidden` når de er dekorative
5. **Bruk kebab-case for elementnavn** — Følg mønsteret `hvi-icon-{kebab-case-name}`

## Eksempel: Komplett egendefinert ikonbibliotek

```csharp
using Hviktor.Icons.Abstractions.Types;

namespace MyApp.Icons;

/// <summary>
/// Custom medical icons for healthcare applications.
/// </summary>
public static class MedicalIcons
{
    /// <summary>Blood bag icon.</summary>
    public static readonly IconDefinition BloodBag = new("hvi-icon-blood-bag");

    /// <summary>Syringe icon.</summary>
    public static readonly IconDefinition Syringe = new("hvi-icon-syringe");

    /// <summary>Stethoscope icon.</summary>
    public static readonly IconDefinition Stethoscope = new("hvi-icon-stethoscope");
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
