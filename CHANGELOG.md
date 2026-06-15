## [v1.1.4] - 2026-06-15

### Bug Fixes

- _(ci)_ Use organization-level variable for NuGet user

### Miscellaneous

- _(config)_ Remove HelseVestIKT package source from nuget.config
- _(docs)_ Bump Hviktor packages to 1.1.3
- _(release)_ Update CHANGELOG.md for v1.1.4
- _(release)_ Update CHANGELOG.md for v1.1.4
- Update package version to 1.1.4

## [v1.1.4] - 2026-06-15

### Bug Fixes

- _(ci)_ Use organization-level variable for NuGet user

### Miscellaneous

- _(config)_ Remove HelseVestIKT package source from nuget.config
- _(docs)_ Bump Hviktor packages to 1.1.3

## [v1.1.3] - 2026-06-15

### Miscellaneous

- Update npm dependencies
- _(docs)_ Bump Hviktor packages to 1.1.2
- _(docs)_ Update CHANGELOG.md
- _(workflows)_ Update permissions in deploy-docs and publish-packages workflows

## [v1.1.2] - 2026-06-12

### Features

- _(deploy)_ Configure dynamic BasePath for sub-path deployments
- _(deploy)_ Configure dynamic BasePath for sub-path deployments
- _(link)_ Improve active-state handling for non-root base URIs

### Bug Fixes

- _(workflows)_ Ensure base href rewrite in entry.template.html with validation steps
- _(workflows)_ Always use BasePath as is
- _(workflows)_ Update BasePath handling and conditional href replacement logic
- _(components)_ Remove leading slashes from href paths for consistency and rel base path support
- _(docs)_ Adjust href paths to use relative links for BasePath compatibility
- _(docs)_ Correct relative link in README for ComponentPage route
- _(vite)_ Handle CSS asset URLs as relative in experimental renderBuiltUrl config

### Miscellaneous

- _(docs)_ Bump Hviktor packages to 1.1.1
- _(workflows)_ Update GitHub Actions to latest versions and commit SHAs

## [v1.1.1] - 2026-05-19

### Features

- _(scripts)_ Add `pack:dev` script to package Hviktor projects with versioning support

### Bug Fixes

- _(styles)_ Update font URLs for vite auto detection and correct typography layer selectors
- _(styles)_ Unify monospace font variable for consistency across themes
- _(scripts)_ Update output paths and npm scripts for test-pack
- _(config)_ Reorder message group for `revert` type in cliff configuration

### Miscellaneous

- _(styles)_ Remove duplicate custom spacing utilities in favor of standard Tailwind utilities
- _(icons)_ Rename loader.svg to Loader.svg
- _(workflows)_ Add signing commits to the "Update Directory.Packages.props" step
- _(cliff)_ Update grouping rules for commit message types

## [v1.1.0] - 2026-05-15

### Breaking Changes

- BREAKING CHANGE: `EventCallback<bool> CheckedChanged` parameter renamed to `EventCallback OnChange`
  - Affected components: _(chip.radio)_ & _(chip.checbox)_

### Bug Fixes

- _(chip.radio)_
  - Ensure `checked` is `true` on change event fire
  - Update `IsChecked` parsing to handle empty string correctly
- _(componentpage)_ Replace `div` with `form` to ensure names are scoped to their owning forms

### Refactor

- _(chip.checkbox)_ Move `CheckedChanged` callback for state change handling from `Chip.Radio`

### Documentation

- Update getting started guide with corrected setup instructions

## [v1.0.0] - 2026-05-13

### Features

- _(init)_ Initial release of the Blazor component library
- _(tokens)_ Integrate design tokens from Designsystemet
- _(a11y)_ WCAG 2.1 AA compliance and keyboard navigation across all interactive components
- _(core)_ Blazor Server and WebAssembly render mode support
- _(docs)_ XML documentation on all public APIs
- _(components)_ Initial release of 45 components:
  - _(alert)_ with `color` and `size`
  - _(avatar)_ with `variant`, `size`, `color`, and `asChild`
  - _(avatarstack)_ with `color`, `size`, `overlap`, `gap`, `avatar-size`, `suffix`, and `expandable`
  - _(badge)_ with `variant`, `color`, `count`, and `maxCount`
  - _(breadcrumbs)_ with `size` and `color`
  - _(button)_ with `variant`, `size`, `color`, `icon`, `loading`, and `command`
  - _(card)_ with `variant`, `size`, `color`, and `asChild`
  - _(checkbox)_ with `checked`, `disabled`, `readonly`, `allowIndeterminate`, `label`, and `description`
  - _(chip)_ with `size` and `color`, including Button, Checkbox, Radio, and Removable sub-types
  - _(codeblock)_ with `language`, `size`, `color`, `lineNumbers`, `copyable`, `overflow`, and `noHighlight`
  - _(details)_ with `variant`, `size`, `color`, `open`, and `defaultOpen`
  - _(dialog)_ with `open`, `modal`, `placement`, `closeButton`, and `closedby`
  - _(divider)_
  - _(dropdown)_ with `open`, `placement`, and `autoPlacement`
  - _(errorsummary)_ with heading and linked error items
  - _(field)_ with `position` for toggle input alignment
  - _(fieldset)_ with Legend and Description sub-components
  - _(heading)_ with `level` and `size`
  - _(icon)_ with `definition`, `size`, `color`, `width`, and `height`
  - _(input)_ with `type`, `size` (character width), `disabled`, and `readonly`
  - _(label)_ with `weight`
  - _(link)_ with `size`, `color`, `href`, and `target`, including HyperLink, ActionLink, NavigationLink, and SkipLink
    sub-types
  - _(list)_ with Ordered and Unordered sub-components
  - _(loader)_ with `size`, `color`, `modal`, and `position`
  - _(logo)_ with `company` and `size`
  - _(markdown)_ with trusted Markdown-to-HTML rendering
  - _(pagination)_ with `data-current`, `data-total`, and page navigation
  - _(paragraph)_ with `variant` and `size`
  - _(popover)_ with `placement` and `autoPlacement`
  - _(radio)_ with `value`, `label`, `description`, and `disabled`
  - _(requiredtag)_ with `required` and `mode`
  - _(search)_ with search field and button composition
  - _(select)_ with `width`
  - _(skeleton)_ with `variant`, `width`, and `height`
  - _(skiplink)_ with `href` for skip navigation
  - _(spinner)_ with `size`, `color`, and `title`
  - _(suggestion)_ with `filter` and multi-select
  - _(switch)_ with `value`, `position`, `label`, and `description`
  - _(table)_ with `size`, `color`, `sticky-header`, and sorting
  - _(tabs)_ with `value`, `defaultValue`, and `@onchange`
  - _(tag)_ with `variant`, `size`, and `color`
  - _(textarea)_ with `size`
  - _(textfield)_ with `label`, `description`, and validation
  - _(togglegroup)_ with `size`
  - _(tooltip)_ with `content`, `placement`, and `type`
  - _(validationMessage)_ with `size` and `color`
- _(rendering)_ `HtmlAttributeBuilder` utility for constructing and manipulating HTML attributes
- _(models)_ Component base classes:
  - Base classes for cascading, nested, async, and popover component patterns
  - `PopoverBase` with JS interop, positioning, and controlled state
  - `CssLength`, `CssBoolean`, and `EnumValue` model types
- _(security)_ `Cryptography.GenerateId` for unpredictable DOM element IDs
- _(localization)_ Localization infrastructure:
  - `LocalizationService` for cookie-based culture management
  - `ResourceOverrideManager` for thread-safe resource overrides
  - `LocalizationExtensions` and `ResourceExtensions` helpers
- _(globalization)_ `Culture` utility with date and time format constants
- _(comparers)_ Generic `PropertyComparer<T>` for reflection-based property sorting
- _(extensions)_ Service and extension methods:
  - `ServiceCollectionExtensions` for Hviktor DI registration
  - `ApplicationBuilderExtensions` for Hviktor middleware setup
  - Attribute services for Color, Size, Variant, Placement, Position, Weight, and Width
  - `JsRuntimeService` and `JsObjectReferenceService` for JS interop
  - `StringLocalizerService` for typed resource localization
  - `ConverterExtensions`, `StringExtensions`, and `EnumExtensions` helpers
- _(abstractions)_ Shared types and interfaces:
  - Attribute enums for Color, Size, Variant, Placement, Position, Weight, and Width
  - `LinkTarget` and `InteractionType` enums
  - Table-specific enums for sorting and selection
  - Component and service interfaces
- _(icons)_ Icon system:
  - `IconDefinition` type and `IconSet` constants with `generate-iconset.mjs` codegen
  - `Hviktor.Icons.Abstractions` package with shared icon types
- _(i18n)_ Resource files with Norwegian Bokmål, Nynorsk, and English translations
- _(build)_ Build tooling:
  - `vite.config.js` with CSS code-splitting and icon chunk output
  - `Directory.Build.props` and `Directory.Packages.props` for centralized build configuration
  - `packages.lock.json` for reproducible NuGet builds
