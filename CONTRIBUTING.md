# Contributing to Hviktor

Thank you for your interest in contributing to **Hviktor**, your contribution is welcome and
appreciated.

---

## Code of Conduct

This project is governed by the [Hviktor Code of Conduct](./CODE_OF_CONDUCT.md).
By participating, you are expected to uphold this code.

---

## How to Get Involved

### Report Bugs and Suggest Features

Use our [GitHub Issue Templates](https://github.com/HelseVestIKT/hviktor-blazor/issues/new/choose)
to report bugs or request new features.

### Fix a Bug

Spotted something that needs fixing? Fork this repository, make your changes, and submit
a [pull request](#pull-requests).
A team member will review it from there.

### Propose a New Feature

Have an idea for a new component or feature?

1. Submit
   a [feature request](https://github.com/HelseVestIKT/hviktor-blazor/issues/new?template=feature-rapport.yml)
   describing your requirements and your interest in contributing.
2. The team will review the request and assess its fit within the library.
3. If approved, we'll collaborate with you on design and implementation.

> **Note:** New components require careful attention to accessibility (WCAG 2.1 AA minimum), theming via Designsystemet
> tokens, and compatibility with both Blazor Server and WebAssembly. For larger contributions, we may invite you to
> participate in daily check-ins to ensure alignment with project standards.

---

## Getting Started with Development

### Prerequisites

- [.NET 10 SDK](https://dot.net)
- [Node.js](https://nodejs.org/) (for CSS/JS tooling)
- A code editor (Visual Studio, Rider, or VS Code)

### Setup

Clone the repository and check out the `main` branch:

```bash
git clone https://github.com/HelseVestIKT/hviktor-blazor.git
cd hviktor-blazor
```

Build the solution:

```bash
dotnet build
```

Build CSS and JS assets (from `Hviktor/`):

```bash
pnpm install
pnpm build:dev
```

Run the documentation site (from `Documentation/`):

```bash
dotnet run
```

### Running Tests

```bash
dotnet test                              # All tests
dotnet test Tests/Tests.Unit             # Unit tests (bUnit)
dotnet test Tests/Tests.Playwright       # Playwright tests (WCAG, behavior, composition)
```

---

## Project Conventions

- **Accessibility first:** WCAG 2.1 AA is the minimum. Never ship code that degrades a11y.
- **Separate markup and code-behind:** `.razor` for markup, `.razor.cs` for logic.
- **Use design tokens:** Colors, spacing, and theming come from Designsystemet. Never hardcode values.
- **XML doc comments:** Required on all public members.
- **Strong typing:** No `object` or untyped collections. Use generics.
- **Testing:** Every public component needs both unit tests (bUnit) and WCAG compliance tests (Playwright + axe-core).

---

## Pull Requests

When creating a pull request:

1. **Start with a draft:** Mark your PR as a [draft](https://github.blog/2019-02-14-introducing-draft-pull-requests/)
   while development is in progress.
2. **Target `main`:** All PRs should point to the `main` branch.
3. **Follow conventional commits:** PR titles must follow
   the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification.
4. **Pass CI checks:** Automated checks (build, lint, tests) must pass before merge.
5. **Mark as ready:** When complete, remove the draft status. A team member will review your code and provide feedback.

---

## Commit Messages

This project uses [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
and [Semantic Versioning 2.0](https://semver.org/).

- `fix:` indicates a patch release (0.0.x)
- `feat:` indicates a minor release (0.x.0)
- `!BREAKING:` indicates a major release (x.0.0)

### Scoping

Add a scope in parentheses to indicate the area of change:

- `fix(button): correct padding when icon-only`
- `feat(table): add sortable column headers`
- `docs(alert): add usage examples`

---

## Changelogs

We use [git-cliff](https://github.com/orhun/git-cliff) to generate changelogs.
Review previous releases for tone and wording conventions.

---

## Questions?

Open a [discussion](https://github.com/HelseVestIKT/hviktor-blazor/discussions) or reach out to the team.
We're happy to help you get started.

---

<div align="center">
  <sub>Helse Vest IKT</sub>
</div>
