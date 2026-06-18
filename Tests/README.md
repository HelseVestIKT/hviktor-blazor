# **Tests**

Shared Blazor Server test host application used by `Tests.Playwright` for running browser-based tests against real
rendered components.

## Table of Contents

- [Purpose](#purpose)
- [Running](#running)
- [Page setup](#page-setup)
- [Conventions](#conventions)

## Purpose

This project is **not** a test project itself. It is an ASP.NET Core Blazor Server app that hosts test pages consumed by
Playwright tests. It provides:

- Compliance test pages (one per component/variant) for WCAG axe-core validation.
- Behavior test pages for keyboard navigation and interaction testing.
- Composition test pages for multi-component integration scenarios.

## Running

```bash
dotnet run --project Tests
```

The app starts at `https://localhost:7139`. In Playwright tests, the `TestsFixture` starts this app automatically on a
random port.

## Page setup

| Folder                          | Purpose                                           |
| ------------------------------- | ------------------------------------------------- |
| `Components/Pages/Compliance/`  | One page per component variant for axe-core tests |
| `Components/Pages/Behavior/`    | Pages for keyboard and interaction testing        |
| `Components/Pages/Composition/` | Multi-component integration scenarios             |

## Conventions

- All test target elements must have `data-testid` attributes.
- Pages use the route pattern `/component/{category}/{component}/{variant}`.
- Keep test pages minimal: only the component under test, no extra layout or decoration.
