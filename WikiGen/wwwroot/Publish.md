# Publish

## Prerequisites

- .NET 10 SDK
- Node.js (for CSS/JS build tooling)
- Access to the GitHub Packages NuGet registry

## Build

### Build all projects

```shell
dotnet build
```

### Release build (generates XML documentation)

```shell
dotnet build -c Release
```

### Build CSS and JavaScript assets

From the `Hviktor/` directory:

```shell
pnpm install
pnpm build:prod
```

## Run Tests

Run the full test suite before publishing:

```shell
dotnet test
```

Run specific test projects:

```shell
dotnet test Tests/Tests.Unit
dotnet test Tests/Tests.Playwright
```

## Create NuGet Packages

Pack all projects in Release configuration:

```shell
dotnet pack -c Release
```

This produces `.nupkg` files for each project:

- `Hviktor`
- `Hviktor.Abstractions`
- `Hviktor.Extensions`
- `Hviktor.Icons`
- `Hviktor.Icons.Abstractions`

## Publish to GitHub Packages

### Configure NuGet source

Add the GitHub Packages registry as a NuGet source (one-time setup):

```shell
dotnet nuget add source "https://api.nuget.org/v3/index.json" --name "nuget.org"
```

### Push packages

```shell
dotnet nuget push "**/*.nupkg" --source "nuget.org"
```

## Automated Release Pipeline

The repository uses a multi-stage GitHub Actions pipeline for releases:

1. **Prepare Release** (`prepare-release.yml`): Triggered manually via Actions > "Prepare Release". Enter a version tag
   (e.g., `v5.1.0`) and optionally mark as pre-release. The workflow generates a changelog, bumps the version in
   `Directory.Build.props`, and opens a PR from branch `changelog/v5.1.0`.
2. **Review and merge**: The PR shows the changelog and version bump. Edit if needed, then merge to `main`.
3. **Create Release** (`create-release.yml`): When the changelog PR merges, this workflow detects the `changelog/v*`
   branch, creates an annotated git tag, generates the release body using git-cliff, and creates the GitHub release.
4. **Publish Packages** (`publish-packages.yml`): Triggered by the release creation. Builds and publishes all NuGet
   packages to GitHub Packages in dependency order:
   - `Hviktor.Icons.Abstractions`
   - `Hviktor.Abstractions`
   - `Hviktor.Extensions`
   - `Hviktor.Icons`
   - `Hviktor`
5. **Update Documentation**: After all packages are published, the documentation site version is updated automatically.

### Supporting workflows

- **Quality Gate Validation**: runs SonarCloud analysis on pull requests.
- **Unit Tests** and **Playwright WCAG/Composition Tests**: run on pull requests and pushes.

## Versioning

Package versions are defined centrally in `Directory.Build.props` under the `<Version>` property. The **Prepare
Release** workflow automatically updates this file when preparing a release, so manual version bumps are not required.

## Updating Icons

To regenerate the icon set from the latest `@helsevestikt/hviktor-icons` package:

```shell
cd Hviktor.Icons/Hviktor.Icons
pnpm install
pnpm generate
```
