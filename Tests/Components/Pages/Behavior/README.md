# Behavior Test Pages

Test pages for keyboard navigation, focus management, and interaction pattern validation via Playwright.

Pages are routed at `/component/behavior/{component}/{scenario}`.

## Conventions

- Each page isolates a single interaction pattern (keyboard open/close, focus trapping, etc.).
- All interactive elements must have `data-testid` attributes.
- Keep pages minimal: only the component under test with the required trigger/context.
- Do not rely on mouse interaction; pages must be fully operable via keyboard.
