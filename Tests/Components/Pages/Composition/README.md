# Composition Test Pages

Test pages that render multiple components together in realistic scenarios.

Used by `Tests.Playwright` composition tests to validate cross-component interaction,
data flow, and accessibility of composed layouts.

## Conventions

- Each page represents a realistic usage scenario with multiple interacting components.
- All interactive elements must have `data-testid` attributes for Playwright locators.
- Keep pages focused on one integration scenario each.
