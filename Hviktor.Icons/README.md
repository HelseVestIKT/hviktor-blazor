# **Hviktor.Icons**

Typede ikonkonstanter for @helsevestikt/hviktor-icons webkomponentbiblioteket

- [Oversikt](#oversikt)
- [Installasjon](#installasjon)
- [Bruk](#bruk)
- [Tilgjengelige ikoner](#tilgjengelige-ikoner)
- [Egendefinerte ikoner](#egendefinerte-ikoner)
- [Oppdatering av ikoner](#oppdatering-av-ikoner)
- [Versjonsinformasjon](#versjonsinformasjon)
- [Dokumentasjon](#dokumentasjon)
- [Seneste endringer](#seneste-endringer)
- [Lisens](#lisens)

## Oversikt

`Hviktor.Icons` er et typet ikonbibliotek som gir C#-konstanter for alle ikoner
fra [`@helsevestikt/hviktor-icons`](https://www.npmjs.com/package/@helsevestikt/hviktor-icons).
Ikonene rendres som `<hvi-icon-* />` web components og er optimalisert for bruk med Hviktor
komponentbiblioteket.

### Hvorfor Hviktor.Icons?

- **900+ Ikoner** - Komplett samling fra hviktor-icons
- **Web Components** - Rendres som `<hvi-icon-* />` custom elements
- **Type-sikker** - IntelliSense og kompileringsfeil ved feil ikonnavn
- **Tilgjengelig** - Designet med WCAG-tilgjengelighet i tankene
- **Konsistent** - Alle ikoner følger samme designspråk
- **Enkel oppdatering** - Kjør `node generate-iconset.mjs` for å synkronisere

## Funksjoner

| Funksjon            | Beskrivelse                                    |
| ------------------- | ---------------------------------------------- |
| Ferdigbygde ikoner  | 900+ ikoner klare til bruk                     |
| Kategorisert        | Ikoner organisert i logiske kategorier         |
| IntelliSense-støtte | Full autokomplettering i IDE                   |
| Web Components      | Rendres som `<hvi-icon-*>` custom elements     |
| Temakompatibel      | Arver `currentColor` for enkel fargetilpasning |

## Installasjon

Installer via NuGet Package Manager:

```bash
dotnet add package Hviktor.Icons
```

Eller via Package Manager Console:

```powershell
Install-Package Hviktor.Icons
```

## Bruk

### Grunnleggende bruk

```razor
@using Hviktor.Components.Icon
@using Hviktor.Icons.Types

<Icon Definition="IconSet.Home" />
<Icon Definition="IconSet.MagnifyingGlass" />
<Icon Definition="IconSet.Cog" />
```

### Med størrelse

```razor
@using Hviktor.Abstractions.Enums.Attributes

<Icon Definition="IconSet.Heart" Size="Size.Small" />
<Icon Definition="IconSet.Star" Size="Size.Medium" />
<Icon Definition="IconSet.Globe" Size="Size.Large" />
```

### Dekorative ikoner (skjult for skjermlesere)

```razor
<Button>
    <Icon Definition="IconSet.Plus" aria-hidden />
    Legg til
</Button>
```

### Ikoner med tilgjengelig beskrivelse

```razor
<Icon Definition="IconSet.Phone" aria-label="Ring oss" />
```

## Tilgjengelige ikoner

Ikonene er organisert i følgende kategorier:

| Kategori              | Eksempler                                      |
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

### Finne ikoner

I kode kan du bruke IntelliSense ved å skrive `IconSet.` for å se alle tilgjengelige ikoner.

## Egendefinerte ikoner

Trenger du ikoner som ikke finnes i hviktor-icons?
Se [Hviktor.Icons.Abstractions](../Hviktor.Icons.Abstractions/README.md)
for veiledning om hvordan du lager egendefinerte ikondefinisjoner.

```csharp
using Hviktor.Icons.Abstractions.Types;

public static class MyIcons
{
    /// <summary>My custom icon rendered as a web component.</summary>
    public static readonly IconDefinition MySpecialIcon = new("hvi-icon-my-special");
}
```

## Oppdatering av ikoner

For å regenerere `IconSet.cs` fra siste versjon av `@helsevestikt/hviktor-icons`:

```bash
cd Hviktor.Icons/Hviktor.Icons
pnpm install
pnpm generate
```

> [!NOTE]
> Kjør `pnpm install` først for å oppdatere til siste versjon av `@helsevestikt/hviktor-icons`.

## Versjonsinformasjon

Gjeldende versjon og tidspunkt for siste oppdatering finnes
i `icons-meta.json`:

> [!CAUTION] > `icons-meta.json` inneholder informasjon om den gjeldende versjonen og tidspunktet for siste oppdatering
> av
> ikonbiblioteket, og bør derfor ikke redigeres manuelt.

Eksempel innhold:

```json
{
  "generatedAt": "2026-01-19T11:15:31Z",
  "version": "1.0.0",
  "source": "@helsevestikt/hviktor-icons",
  "iconCount": 800
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
