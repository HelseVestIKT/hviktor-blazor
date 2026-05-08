/**
 * Per-instance state for a ToggleGroup element.
 * Using a WeakMap avoids statically shared state between multiple instances on the same page.
 */
const instances = new WeakMap();

/** @type {string} */
const ITEM_SELECTOR = "[data-roving-tabindex-item]";

/**
 * Updates roving tabindex so only the target item is tabbable.
 *
 * @param {NodeListOf<HTMLElement>} items - all items in the group
 * @param {HTMLElement} target - the item that should become tabbable
 */
function setRovingTabindex(items, target) {
  items.forEach((item) => {
    item.setAttribute("tabindex", item === target ? "0" : "-1");
  });
}

/**
 * Handles keyboard navigation within a single ToggleGroup instance.
 * Implements the roving tabindex pattern scoped to this group.
 *
 * Arrow keys move focus without selecting. Tab leaves the group.
 * Home/End move to first/last item.
 *
 * @param {KeyboardEvent} event
 * @param {HTMLElement} element - the fieldset container
 */
function handleKeydown(event, element) {
  const items = element.querySelectorAll(ITEM_SELECTOR);
  const count = items.length;
  if (count === 0) {
    return;
  }

  const index = Array.prototype.indexOf.call(items, event.target);
  if (index === -1) {
    return;
  }

  let nextIndex;

  switch (event.key) {
    case "ArrowRight":
    case "ArrowDown":
      nextIndex = (index + 1) % count;
      break;

    case "ArrowLeft":
    case "ArrowUp":
      nextIndex = (index - 1 + count) % count;
      break;

    case "Home":
      nextIndex = 0;
      break;

    case "End":
      nextIndex = count - 1;
      break;

    case "Enter":
      event.preventDefault();
      event.target.click();
      return;

    default:
      return;
  }

  event.preventDefault();
  const target = items[nextIndex];
  setRovingTabindex(items, target);
  target.focus();
}

export class ToggleGroup {
  /**
   * Initializes keyboard navigation for the given ToggleGroup element.
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

    ToggleGroup.dispose(elementRef);

    const items = element.querySelectorAll(ITEM_SELECTOR);

    // Set initial roving tabindex: the checked item or the first item is tabbable
    const checked = element.querySelector(
      `${ITEM_SELECTOR}[aria-checked="true"]`,
    );
    setRovingTabindex(items, checked || items[0]);

    const handler = (event) => handleKeydown(event, element);
    element.addEventListener("keydown", handler);

    instances.set(element, { handler });
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

    element.removeEventListener("keydown", instance.handler);
    instances.delete(element);
  }
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.ToggleGroup = globalThis.Hviktor.ToggleGroup || {};
globalThis.Hviktor.ToggleGroup.initialize = (elementRef) =>
  ToggleGroup.initialize(elementRef);
globalThis.Hviktor.ToggleGroup.dispose = (elementRef) =>
  ToggleGroup.dispose(elementRef);
