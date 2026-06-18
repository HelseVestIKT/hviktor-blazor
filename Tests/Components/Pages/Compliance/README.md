# Compliance Test Pages

Test pages for WCAG accessibility validation using [Deque axe-core](https://github.com/dequelabs/axe-core) via
Playwright. Each component has a subfolder with pages rendering specific parameter combinations (variant, size, color,
etc.) in isolation.

Each component folder contains one `.razor` page per parameter dimension:  
Pages are routed at `/component/compliance/{component}/{variant}`.

## Conventions

- One page per parameter dimension to isolate contrast and accessibility issues.
- All test target elements must have `data-testid` attributes.
- Keep pages minimal: only the component under test, no extra layout or decoration.
- Render all allowed values for the parameter being tested.

## Reference

- [WCAG 2.1 (w3.org)](https://www.w3.org/WAI/standards-guidelines/wcag/)
- [Deque axe-core rules (github.com)](https://github.com/dequelabs/axe-core/blob/develop/doc/rule-descriptions.md)
