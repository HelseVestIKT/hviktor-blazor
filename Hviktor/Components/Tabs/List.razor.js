/**
 * Per-instance state for a Tabs List element.
 * Using a WeakMap avoids statically shared state between multiple instances on the same page.
 */
const instances = new WeakMap();

/**
 * Handles keyboard navigation within a tablist per WAI-ARIA Tabs pattern.
 *
 * Supported keys:
 * - ArrowRight: move focus to next tab (wraps to first)
 * - ArrowLeft: move focus to previous tab (wraps to last)
 * - Home: move focus to first tab
 * - End: move focus to last tab
 *
 * @param {KeyboardEvent} event
 * @param {NodeListOf<HTMLElement>} tabs - all tab buttons in the list
 * @param {HTMLElement} tab - the tab that received the keydown event
 */
function handleKeydown(event, tabs, tab) {
  const count = tabs.length;
  if (count === 0) {
    return;
  }

  const index = Array.prototype.indexOf.call(tabs, tab);

  switch (event.key) {
    case "ArrowRight":
      event.preventDefault();
      tabs[(index + 1) % count].focus();
      break;

    case "ArrowLeft":
      event.preventDefault();
      tabs[(index - 1 + count) % count].focus();
      break;

    case "Home":
      event.preventDefault();
      tabs[0].focus();
      break;

    case "End":
      event.preventDefault();
      tabs[count - 1].focus();
      break;
  }
}

export class List {
  /**
   * Initializes keyboard navigation for the given tablist element.
   * Safe to call multiple times: disposes any previous instance first.
   *
   * @param {HTMLElement | { current: HTMLElement }} elementRef
   */
  static initialize(elementRef) {
    const element =
      elementRef instanceof HTMLElement ? elementRef : elementRef?.current;
    if (!element) {
      return;
    }

    // Dispose any existing instance before re-initializing (e.g., after a re-render)
    List.dispose(elementRef);

    /** @type {NodeListOf<HTMLElement>} */
    const tabs = element.querySelectorAll('button[role="tab"]');

    // Store a bound handler per tab so we can remove the exact same reference later
    const handlers = new Map();
    tabs.forEach((tab) => {
      const handler = (event) => handleKeydown(event, tabs, tab);
      handlers.set(tab, handler);
      tab.addEventListener("keydown", handler);
    });

    instances.set(element, { tabs, handlers });
  }

  /**
   * Removes all keyboard-navigation listeners added by {@link initialize}.
   *
   * @param {HTMLElement | { current: HTMLElement }} elementRef
   */
  static dispose(elementRef) {
    const element =
      elementRef instanceof HTMLElement ? elementRef : elementRef?.current;
    if (!element) return;

    const instance = instances.get(element);
    if (!instance) return;

    const { tabs, handlers } = instance;
    tabs.forEach((tab) => {
      const handler = handlers.get(tab);
      if (handler) {
        tab.removeEventListener("keydown", handler);
      }
    });

    instances.delete(element);
  }
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.List = globalThis.Hviktor.List || {};
globalThis.Hviktor.List.initialize = (elementRef) =>
  List.initialize(elementRef);
globalThis.Hviktor.List.dispose = (elementRef) => List.dispose(elementRef);
