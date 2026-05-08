/** @type {import('Microsoft.JSInterop').DotNetObjectReference|null} */
let dotNetRef = null;

/**
 * Handles Ctrl+K / Cmd+K to open the search dialog.
 * @param {KeyboardEvent} e
 */
function onKeyDown(e) {
  if ((e.ctrlKey || e.metaKey) && e.key === "k") {
    e.preventDefault();
    dotNetRef?.invokeMethodAsync("OpenSearchDialog");
  }
}

/**
 * Registers the Ctrl+K search shortcut.
 * @param {import('Microsoft.JSInterop').DotNetObjectReference} ref
 */
// noinspection JSUnusedGlobalSymbols
export function registerSearchShortcut(ref) {
  dotNetRef = ref;
  document.addEventListener("keydown", onKeyDown);
}

/** Unregisters the Ctrl+K search shortcut. */
// noinspection JSUnusedGlobalSymbols
export function unregisterSearchShortcut() {
  document.removeEventListener("keydown", onKeyDown);
  dotNetRef = null;
}
