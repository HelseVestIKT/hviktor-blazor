import "globalthis/polyfill"; // Polyfill for globalThis

// ---------------------------------------------------------------------------
// Tailwind
// ---------------------------------------------------------------------------
// @ts-ignore
import "./wwwroot/styles/tailwind.scss";

// ---------------------------------------------------------------------------
// Designsystemet
// ---------------------------------------------------------------------------
// @ts-ignore
import "@digdir/designsystemet-css";
// @ts-ignore
import "@digdir/designsystemet-css/theme";

// ---------------------------------------------------------------------------
// Hviktor
// ---------------------------------------------------------------------------
import "./wwwroot/scripts/index"; // Main initialization script
// @ts-ignore
import "./wwwroot/styles/index.scss"; // Styles

// Utilities (import to ensure they're bundled and attach to window.Hviktor)
import "./wwwroot/scripts/cookie-storage";

// Components (import to ensure they're bundled and attach to window.Hviktor)
// Only load when needed (e.g., on specific pages)
import "./wwwroot/scripts/components/search";
import "./wwwroot/scripts/components/suggestion";
import "./wwwroot/scripts/components/tooltip";
import "./wwwroot/scripts/components/trigger";
import "./wwwroot/scripts/components/popover"; // Standalone popover component (depends on trigger)
