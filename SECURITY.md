# Security Policy

## Reporting a Vulnerability

If you discover a security vulnerability in Hviktor, please report it responsibly
through [GitHub private vulnerability reporting](https://github.com/HelseVestIKT/hviktor-blazor/security/advisories/new).

**Do not** open a public issue or pull request describing the vulnerability.

We will acknowledge your report promptly and work with you to understand and address the issue before any public
disclosure.

## Supported Versions

Only the latest released version of each Hviktor NuGet package receives security updates. We recommend always
updating to the latest version.

## Security Practices

### Dependency Management

- **Renovate** automates dependency updates across both NuGet and pnpm packages. It is configured to only propose
  updates
  for packages published for at least 3 days, allowing time for the ecosystem to catch problematic releases.
- **`dotnet list package --vulnerable`** is run regularly to detect known NuGet vulnerabilities.
- **`pnpm audit`** is run regularly for JS/CSS tooling dependencies.
- **`packages.lock.json`** is used for reproducible builds (enabled via `Directory.Build.props`).

### Automated Scanning

- **GitHub code scanning** for static analysis.
- **SonarQube Cloud** for continuous code quality and security analysis.
- **SonarCloud** for continuous code quality and security analysis.

### Secure Coding

- All dynamic content rendered in Razor markup is sanitized. `MarkupString` is never used with untrusted content.
- External links with `target="_blank"` always include `rel="noopener noreferrer"`.
- DOM element IDs use cryptographically generated values to avoid predictable identifiers.
- All `[Parameter]` inputs are validated with `[AllowedValues]`, `[Range]`, or custom attributes.
- No secrets, API keys, or sensitive data are stored client-side or in component parameters.
- `ILogger<T>` is used for logging, and sensitive user data is never logged.

## Disclosure Policy

We follow coordinated disclosure. Once a fix is available, we will:

1. Release a patched version of the affected package(s).
2. Publish a GitHub security advisory with details and remediation steps.
3. Credit the reporter (unless anonymity is requested).
