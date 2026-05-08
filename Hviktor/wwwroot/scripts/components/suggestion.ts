import {
  preparePopoverForPositioning,
  resetPopoverPlacement,
} from "../utils/popover-positioning";

/** Minimum gap between the listbox edge and the viewport edge. */
const VIEWPORT_PADDING = 8;

/** Gap between the anchor and the listbox. */
const LISTBOX_GAP = 8;

/** Debounce delay (ms) for filtering when JS-side filter is enabled. */
const FILTER_DEBOUNCE_MS = 80;

/**
 * Positions the listbox popover relative to its anchor element.
 * Prioritizes placing below (bottom-start), falling back to above (top-start).
 * When neither direction has enough room for the full list, the listbox
 * max-height is constrained so it fits within the available viewport space.
 */
function positionListbox(anchor: HTMLElement, listbox: HTMLElement): void {
  const rect = anchor.getBoundingClientRect();
  const spaceBelow =
    globalThis.innerHeight - rect.bottom - LISTBOX_GAP - VIEWPORT_PADDING;
  const spaceAbove = rect.top - LISTBOX_GAP - VIEWPORT_PADDING;

  // Reset max-height so offsetHeight reflects natural size
  listbox.style.removeProperty("max-height");

  const naturalHeight = listbox.offsetHeight;
  const left = rect.left;
  const width = rect.width;

  listbox.style.setProperty("width", `${width}px`);

  if (naturalHeight <= spaceBelow) {
    // Fits below: place at bottom-start, no constraint
    const top = rect.bottom + LISTBOX_GAP;
    listbox.style.setProperty("translate", `${left}px ${top}px`);
    listbox.dataset.placement = "bottom-start";
  } else if (naturalHeight <= spaceAbove) {
    // Fits above: place at top-start, no constraint
    const top = rect.top - LISTBOX_GAP - naturalHeight;
    listbox.style.setProperty("translate", `${left}px ${top}px`);
    listbox.dataset.placement = "top-start";
  } else if (spaceBelow >= spaceAbove) {
    // More room below: constrain height to fit
    const top = rect.bottom + LISTBOX_GAP;
    listbox.style.setProperty("max-height", `${spaceBelow}px`);
    listbox.style.setProperty("translate", `${left}px ${top}px`);
    listbox.dataset.placement = "bottom-start";
  } else {
    // More room above: constrain height to fit
    listbox.style.setProperty("max-height", `${spaceAbove}px`);
    const constrainedHeight = Math.min(naturalHeight, spaceAbove);
    const top = rect.top - LISTBOX_GAP - constrainedHeight;
    listbox.style.setProperty("translate", `${left}px ${top}px`);
    listbox.dataset.placement = "top-start";
  }
}

interface DotNetObjectReference {
  invokeMethodAsync(methodName: string, ...args: unknown[]): Promise<unknown>;
}

/**
 * Pre-indexed option entry. Built once per option element and cached
 * so filtering and keyboard navigation avoid repeated DOM queries.
 */
interface OptionEntry {
  el: HTMLElement;
  value: string;
  labelLower: string;
  disabled: boolean;
}

interface ComboboxState {
  dotNetRef: DotNetObjectReference;
  input: HTMLInputElement;
  listbox: HTMLElement | null;
  selected: string[];
  selectedSet: Set<string>;
  activeIndex: number;
  isOpen: boolean;
  isMultiple: boolean;
  isCreatable: boolean;
  filterEnabled: boolean;
  /** All option entries, indexed once on init and refreshed on DOM mutations. */
  optionIndex: OptionEntry[];
  /** Cached subset of visible (filtered, non-disabled) entries. */
  visibleCache: OptionEntry[];
  /** The last query string used for filtering, to skip redundant work. */
  lastFilterQuery: string;
  /** Timer id for debounced filtering. */
  filterDebounceId: ReturnType<typeof setTimeout> | null;
  /** MutationObserver watching the listbox for added/removed options. */
  listboxObserver: MutationObserver | null;
  /** Cached reference to the empty placeholder element inside the listbox. */
  emptyEl: HTMLElement | null;
  onInput: (e: Event) => void;
  onKeydown: (e: KeyboardEvent) => void;
  onFocus: (e: FocusEvent) => void;
  onBlur: (e: FocusEvent) => void;
  onListboxClick: (e: MouseEvent) => void;
  onChipClick: (e: MouseEvent) => void;
  onChipKeydown: (e: KeyboardEvent) => void;
  onClearClick: (e: MouseEvent) => void;
  onClearKeydown: (e: KeyboardEvent) => void;
  onReposition: (e?: Event) => void;
}

const instances = new Map<string, ComboboxState>();

/**
 * Safely invokes a .NET method via the object reference.
 * Silently catches errors from disposed references or disconnected circuits.
 */
async function invokeDotNet(
  dotNetRef: DotNetObjectReference,
  methodName: string,
  ...args: unknown[]
): Promise<void> {
  try {
    await dotNetRef.invokeMethodAsync(methodName, ...args);
  } catch {
    // .NET object reference may be disposed or the circuit disconnected
  }
}

/**
 * Reads the value from an option element.
 * Falls back to trimmed text content if no value attribute is present.
 */
function optionValue(option: HTMLElement): string {
  return option.getAttribute("value") || option.textContent?.trim() || "";
}

/**
 * Reads the display label from an option element.
 */
function optionLabel(option: HTMLElement): string {
  return option.textContent?.trim() || optionValue(option);
}

/**
 * Builds the option index from the current DOM state of the listbox.
 * Called once on initialization and again when the MutationObserver
 * detects added/removed option elements.
 */
function buildOptionIndex(state: ComboboxState): void {
  if (!state.listbox) {
    state.optionIndex = [];
    state.visibleCache = [];
    return;
  }

  const elements = state.listbox.querySelectorAll<HTMLElement>(
    '[role="option"]:not([data-empty])',
  );
  const index: OptionEntry[] = new Array(elements.length);

  for (let i = 0; i < elements.length; i++) {
    const el = elements[i];
    index[i] = {
      el,
      value: optionValue(el),
      labelLower: optionLabel(el).toLowerCase(),
      disabled: el.hasAttribute("disabled"),
    };
  }

  state.optionIndex = index;
  state.emptyEl = state.listbox.querySelector<HTMLElement>("[data-empty]");
  // Invalidate filter cache so the next filter pass rebuilds it
  state.lastFilterQuery = "\x00";
  state.visibleCache = [];
}

/**
 * Filters options based on the current input value using the pre-built index.
 * Caches the visible result set and skips work when the query hasn't changed.
 */
function filterOptions(state: ComboboxState): void {
  const { input, listbox, filterEnabled, optionIndex } = state;
  if (!listbox || !filterEnabled) {
    return;
  }

  const query = input.value.trim().toLowerCase();

  // Skip if the query hasn't changed since the last filter pass
  if (query === state.lastFilterQuery) {
    return;
  }
  state.lastFilterQuery = query;

  const visible: OptionEntry[] = [];

  for (let i = 0; i < optionIndex.length; i++) {
    const entry = optionIndex[i];
    const matches = !query || entry.labelLower.includes(query);
    entry.el.hidden = !matches;
    if (matches && !entry.disabled) {
      visible.push(entry);
    }
  }

  state.visibleCache = visible;

  // Show or hide the empty placeholder using cached reference
  if (state.emptyEl) {
    state.emptyEl.hidden = visible.length > 0;
  }
}

/**
 * Invalidates the filter cache so the next filterOptions call re-evaluates.
 * Use after external changes (e.g., Blazor re-renders the option list).
 */
function invalidateFilterCache(state: ComboboxState): void {
  state.lastFilterQuery = "\x00";
  state.visibleCache = [];
}

/**
 * Returns the cached visible options. If the cache is empty and filter is
 * disabled (external filtering), rebuilds from the index.
 */
function getVisibleFromCache(state: ComboboxState): OptionEntry[] {
  if (state.visibleCache.length > 0) {
    return state.visibleCache;
  }

  // When JS-side filtering is disabled, the consumer controls visibility.
  // Build the visible cache from the current DOM hidden state.
  if (!state.filterEnabled) {
    const visible: OptionEntry[] = [];
    for (const entry of state.optionIndex) {
      if (!entry.el.hidden && !entry.disabled) {
        visible.push(entry);
      }
    }
    state.visibleCache = visible;
    return visible;
  }

  // Fallback: re-run the filter to populate the cache
  filterOptions(state);
  return state.visibleCache;
}

/**
 * Updates the [selected] attribute on option elements using the Set for O(1) lookups.
 * Only writes to the DOM when the attribute state actually differs.
 */
function syncSelectedAttributes(state: ComboboxState): void {
  for (const entry of state.optionIndex) {
    const shouldBeSelected = state.selectedSet.has(entry.value);
    const isSelected = entry.el.hasAttribute("selected");
    if (shouldBeSelected && !isSelected) {
      entry.el.setAttribute("selected", "");
    } else if (!shouldBeSelected && isSelected) {
      entry.el.removeAttribute("selected");
    }
  }
}

/**
 * Updates the active descendant highlight for keyboard navigation.
 * Uses the cached visible list instead of querying the DOM.
 * Only touches the previously active and newly active element for minimal DOM writes.
 */
function setActiveDescendant(state: ComboboxState, index: number): void {
  const { input } = state;
  const visible = getVisibleFromCache(state);

  // Remove previous active state from only the previously active entry
  if (state.activeIndex >= 0 && state.activeIndex < visible.length) {
    visible[state.activeIndex].el.removeAttribute("data-active");
  }

  if (index < 0 || index >= visible.length) {
    state.activeIndex = -1;
    input.removeAttribute("aria-activedescendant");
    return;
  }

  state.activeIndex = index;
  const active = visible[index];

  // Ensure the option has an id for aria-activedescendant
  if (!active.el.id) {
    active.el.id = `${input.id || "suggestion"}-opt-${index}`;
  }

  active.el.setAttribute("data-active", "");
  input.setAttribute("aria-activedescendant", active.el.id);

  // Scroll the active option into view
  active.el.scrollIntoView({ block: "nearest" });
}

/**
 * Attaches scroll and resize listeners to keep the listbox positioned
 * while it is open. Listeners are passive and use capture for scroll
 * so we catch scrolls on any ancestor.
 */
function startRepositioning(state: ComboboxState): void {
  document.addEventListener("scroll", state.onReposition, {
    capture: true,
    passive: true,
  });
  globalThis.addEventListener("resize", state.onReposition);
}

/**
 * Removes the scroll and resize listeners attached by startRepositioning.
 */
function stopRepositioning(state: ComboboxState): void {
  document.removeEventListener("scroll", state.onReposition, true);
  globalThis.removeEventListener("resize", state.onReposition);
}

/**
 * Used to immediately closes all open combobox instances except the one targeted one.
 * Ensures snappier user experience by closing other open instances without waiting for blur timeout.
 */
function closeAllExcept(exceptId: string): void {
  for (const [id, other] of instances) {
    if (id !== exceptId && other.isOpen) {
      closeListbox(other);
    }
  }
}

/**
 * Opens the dropdown listbox.
 */
function openListbox(state: ComboboxState): void {
  const { input, listbox } = state;
  if (!listbox || state.isOpen) {
    return;
  }

  // Close any other open instance immediately
  const elementId = input.closest(".ds-suggestion")?.id;
  if (elementId) {
    closeAllExcept(elementId);
  }

  state.isOpen = true;
  input.setAttribute("aria-expanded", "true");

  // Use the Popover API when the element has a popover attribute,
  // otherwise fall back to toggling the hidden attribute.
  if (listbox.hasAttribute("popover")) {
    try {
      listbox.showPopover();
    } catch {
      // Popover may already be showing
    }

    const anchor = input.closest(".ds-suggestion") as HTMLElement;
    if (anchor) {
      requestAnimationFrame(() => positionListbox(anchor, listbox));
    }

    startRepositioning(state);
  } else {
    listbox.hidden = false;
  }

  filterOptions(state);

  // When JS-side filtering is disabled, sync the empty placeholder
  // based on whether any options exist (Blazor controls the option list).
  if (!state.filterEnabled && state.emptyEl) {
    state.emptyEl.hidden = state.optionIndex.length > 0;
  }
}

/**
 * Closes the dropdown listbox.
 */
function closeListbox(state: ComboboxState): void {
  const { input, listbox } = state;
  if (!listbox || !state.isOpen) {
    return;
  }

  state.isOpen = false;
  input.setAttribute("aria-expanded", "false");
  input.removeAttribute("aria-activedescendant");

  // Clear active state from only the previously active option
  if (state.activeIndex >= 0) {
    const visible = getVisibleFromCache(state);
    if (state.activeIndex < visible.length) {
      visible[state.activeIndex].el.removeAttribute("data-active");
    }
  }
  state.activeIndex = -1;

  stopRepositioning(state);

  if (listbox.hasAttribute("popover")) {
    try {
      listbox.hidePopover();
    } catch {
      // Popover may already be hidden
    }
    resetPopoverPlacement(listbox);
  } else {
    listbox.hidden = true;
  }
}

/**
 * Selects an option value. In single mode, replaces the selection.
 * In multiple mode, toggles the value.
 */
async function selectValue(state: ComboboxState, value: string): Promise<void> {
  await invokeDotNet(state.dotNetRef, "OnBeforeSelectCallback", value);

  const element = state.input.closest(".ds-suggestion");
  if (!element) {
    return;
  }

  if (state.isMultiple) {
    const idx = state.selected.indexOf(value);
    if (idx >= 0) {
      state.selected.splice(idx, 1);
      state.selectedSet.delete(value);
    } else {
      state.selected.push(value);
      state.selectedSet.add(value);
    }
  } else {
    state.selected = [value];
    state.selectedSet = new Set([value]);
    state.input.value = value;
    state.input.placeholder = "";
    closeListbox(state);
  }

  renderChips(element as HTMLElement, state);
  syncSelectedAttributes(state);
  updateHiddenSelect(element as HTMLElement, state);

  // Reset active descendant after selection so keyboard navigation restarts
  if (state.isMultiple) {
    setActiveDescendant(state, -1);
  }

  // Reposition the listbox after chip changes may have shifted the container
  if (state.isOpen) {
    state.onReposition();
  }

  await invokeDotNet(state.dotNetRef, "OnAfterSelectCallback", [
    ...state.selected,
  ]);
}

/**
 * Renders the <data> chip elements for selected values inside the combobox container.
 * Uses a DocumentFragment for batch insertion to minimize layout thrashing.
 */
function renderChips(container: HTMLElement, state: ComboboxState): void {
  // Remove all existing chips
  const existing = container.querySelectorAll(":scope > data.ds-chip");
  existing.forEach((chip) => chip.remove());

  const input = state.input;
  const count = state.selected.length;

  if (count > 0) {
    const fragment = document.createDocumentFragment();
    for (let i = 0; i < count; i++) {
      const val = state.selected[i];
      const data = document.createElement("data");
      data.className = "ds-chip";
      data.setAttribute("data-removable", "true");
      data.value = val;
      data.textContent = val;
      data.setAttribute("role", "button");
      data.setAttribute("tabindex", "-1");
      data.setAttribute(
        "aria-label",
        `${val}, Press to remove, ${i + 1} of ${count}`,
      );
      fragment.appendChild(data);
    }
    // Single DOM insertion for all chips
    input.parentNode?.insertBefore(fragment, input);
  }

  // Reset placeholder to empty so the CSS chevron indicator works
  input.placeholder = "";
}

/**
 * Updates the hidden <select> element used for form submission.
 */
function updateHiddenSelect(
  container: HTMLElement,
  state: ComboboxState,
): void {
  const select = container.querySelector<HTMLSelectElement>(
    `#${CSS.escape(container.id)}-select`,
  );
  if (!select) {
    return;
  }

  select.innerHTML = "";
  for (const val of state.selected) {
    const option = document.createElement("option");
    option.value = val;
    option.textContent = val;
    option.selected = true;
    select.appendChild(option);
  }
}

/**
 * Removes a chip by value and updates the selection state.
 * Moves focus to the next chip, previous chip, or the input.
 */
function removeChip(chip: HTMLDataElement, state: ComboboxState): void {
  const value = chip.value || chip.textContent?.trim() || "";
  if (!value) {
    return;
  }

  const idx = state.selected.indexOf(value);
  if (idx >= 0) {
    state.selected.splice(idx, 1);
    state.selectedSet.delete(value);
  }

  const container = state.input.closest(".ds-suggestion") as HTMLElement;
  if (container) {
    renderChips(container, state);
    syncSelectedAttributes(state);
    updateHiddenSelect(container, state);
  }

  if (state.isOpen) {
    state.onReposition();
  }

  // Move focus to the next chip at the same position, or previous, or the input
  if (container) {
    const remainingChips = Array.from(
      container.querySelectorAll<HTMLElement>(":scope > data.ds-chip"),
    );
    const focusTarget =
      remainingChips[idx] ?? remainingChips[idx - 1] ?? state.input;
    focusTarget.focus();
  } else {
    state.input.focus();
  }

  invokeDotNet(state.dotNetRef, "OnAfterSelectCallback", [...state.selected]);
}

/**
 * Handles click events on <data> chip elements to remove selections.
 */
function handleChipClick(e: MouseEvent, state: ComboboxState): void {
  const chip = (e.target as HTMLElement).closest<HTMLDataElement>(
    "data.ds-chip",
  );
  if (!chip) {
    return;
  }

  removeChip(chip, state);
}

/**
 * Handles keydown events on <data> chip elements for keyboard removal.
 * Enter and Space remove the chip. Backspace/Delete also remove it.
 * Arrow keys move focus between chips and the input.
 */
function handleChipKeydown(e: KeyboardEvent, state: ComboboxState): void {
  const chip = (e.target as HTMLElement).closest<HTMLDataElement>(
    "data.ds-chip",
  );
  if (!chip) {
    return;
  }

  switch (e.key) {
    case "Enter":
    case " ":
    case "Delete":
    case "Backspace": {
      e.preventDefault();
      removeChip(chip, state);
      break;
    }

    case "ArrowRight":
    case "ArrowDown": {
      e.preventDefault();
      const container = state.input.closest(".ds-suggestion") as HTMLElement;
      if (!container) {
        break;
      }
      const chips = Array.from(
        container.querySelectorAll<HTMLElement>(":scope > data.ds-chip"),
      );
      const currentIdx = chips.indexOf(chip);
      if (currentIdx < chips.length - 1) {
        chips[currentIdx + 1].focus();
      } else {
        state.input.focus();
      }
      break;
    }

    case "ArrowLeft":
    case "ArrowUp": {
      e.preventDefault();
      const container = state.input.closest(".ds-suggestion") as HTMLElement;
      if (!container) {
        break;
      }
      const chips = Array.from(
        container.querySelectorAll<HTMLElement>(":scope > data.ds-chip"),
      );
      const currentIdx = chips.indexOf(chip);
      if (currentIdx > 0) {
        chips[currentIdx - 1].focus();
      }
      break;
    }
  }
}

/**
 * Executes the clear action for the suggestion component.
 * Shared by both click and keyboard handlers.
 */
function executeClear(state: ComboboxState): void {
  if (state.isMultiple) {
    // Multi-select: only clear the input filter text, keep chip selections
    state.input.value = "";
    state.input.placeholder = "";
    state.input.focus();

    if (state.isOpen) {
      invalidateFilterCache(state);
      filterOptions(state);
      state.onReposition();
    }
  } else {
    // Single-select: clear the selection entirely
    state.selected = [];
    state.selectedSet.clear();

    const container = state.input.closest(".ds-suggestion") as HTMLElement;
    if (container) {
      renderChips(container, state);
      syncSelectedAttributes(state);
      updateHiddenSelect(container, state);
    }

    state.input.value = "";
    state.input.placeholder = "";
    state.input.focus();

    if (state.isOpen) {
      state.onReposition();
    }

    invokeDotNet(state.dotNetRef, "OnAfterSelectCallback", []);
  }
}

/**
 * Handles click events on the <del> clear button.
 */
function handleClearClick(e: MouseEvent, state: ComboboxState): void {
  const del = (e.target as HTMLElement).closest<HTMLElement>("del");
  if (!del) {
    return;
  }

  e.preventDefault();
  executeClear(state);
}

/**
 * Handles keydown events on the <del> clear button for keyboard activation.
 * Enter and Space activate the clear action per WAI-ARIA button pattern.
 */
function handleClearKeydown(e: KeyboardEvent, state: ComboboxState): void {
  const del = (e.target as HTMLElement).closest<HTMLElement>("del");
  if (!del) {
    return;
  }

  if (e.key === "Enter" || e.key === " ") {
    e.preventDefault();
    executeClear(state);
  }
}

/**
 * Initializes the native combobox behavior for a suggestion component.
 */
export function initializeCombobox(
  elementId: string,
  dotNetRef: DotNetObjectReference,
): void {
  const element = document.getElementById(elementId);
  if (!element) {
    return;
  }

  // Remove existing instance if any
  disposeCombobox(elementId);

  const input = element.querySelector<HTMLInputElement>("input");
  if (!input) {
    return;
  }

  const listbox = element.querySelector<HTMLElement>('[role="listbox"]');
  const isMultiple = element.hasAttribute("data-multiple");
  const isCreatable = element.hasAttribute("data-creatable");
  const filterEnabled = !element.hasAttribute("data-nofilter");

  // Set ARIA attributes on the input
  input.setAttribute("role", "combobox");
  input.setAttribute("aria-expanded", "false");
  input.setAttribute("aria-haspopup", "listbox");
  if (listbox?.id) {
    input.setAttribute("aria-controls", listbox.id);
  }

  // Ensure the listbox starts hidden and is prepared for popover positioning
  if (listbox) {
    if (listbox.hasAttribute("popover")) {
      preparePopoverForPositioning(listbox);
    } else {
      listbox.hidden = true;
    }
  }

  const state: ComboboxState = {
    dotNetRef,
    input,
    listbox,
    selected: [],
    selectedSet: new Set(),
    activeIndex: -1,
    isOpen: false,
    isMultiple,
    isCreatable,
    filterEnabled,
    optionIndex: [],
    visibleCache: [],
    lastFilterQuery: "\x00",
    filterDebounceId: null,
    listboxObserver: null,
    emptyEl: null,

    onInput: (e: Event) => {
      const value = (e.target as HTMLInputElement).value;
      invokeDotNet(dotNetRef, "OnBeforeMatchCallback", value);
      openListbox(state);

      // Debounce JS-side filtering for large option sets
      if (state.filterDebounceId !== null) {
        clearTimeout(state.filterDebounceId);
      }

      if (filterEnabled) {
        state.filterDebounceId = setTimeout(() => {
          state.filterDebounceId = null;
          filterOptions(state);
          setActiveDescendant(state, -1);
          if (state.isOpen) {
            state.onReposition();
          }
        }, FILTER_DEBOUNCE_MS);
      } else {
        // External filtering: just reset active descendant
        invalidateFilterCache(state);
        setActiveDescendant(state, -1);
        if (state.isOpen) {
          requestAnimationFrame(() => state.onReposition());
        }
      }
    },

    onKeydown: (e: KeyboardEvent) => {
      if (!listbox) {
        return;
      }

      const visible = getVisibleFromCache(state);

      switch (e.key) {
        case "ArrowDown": {
          e.preventDefault();
          if (!state.isOpen) {
            openListbox(state);
          }
          const nextIndex =
            state.activeIndex < visible.length - 1 ? state.activeIndex + 1 : 0;
          setActiveDescendant(state, nextIndex);
          break;
        }

        case "ArrowUp": {
          e.preventDefault();
          if (!state.isOpen) {
            openListbox(state);
          }
          const prevIndex =
            state.activeIndex > 0 ? state.activeIndex - 1 : visible.length - 1;
          setActiveDescendant(state, prevIndex);
          break;
        }

        case "Home": {
          if (state.isOpen && visible.length > 0) {
            e.preventDefault();
            setActiveDescendant(state, 0);
          }
          break;
        }

        case "End": {
          if (state.isOpen && visible.length > 0) {
            e.preventDefault();
            setActiveDescendant(state, visible.length - 1);
          }
          break;
        }

        case "Enter": {
          e.preventDefault();
          if (
            state.isOpen &&
            state.activeIndex >= 0 &&
            state.activeIndex < visible.length
          ) {
            const active = visible[state.activeIndex];
            selectValue(state, active.value);
          } else if (state.isOpen && isCreatable && input.value.trim()) {
            // Create a new value from the input text
            selectValue(state, input.value.trim());
          } else if (!state.isOpen) {
            openListbox(state);
          }
          break;
        }

        case " ": {
          // Space selects the active item when the listbox is open
          if (
            state.isOpen &&
            state.activeIndex >= 0 &&
            state.activeIndex < visible.length
          ) {
            e.preventDefault();
            const active = visible[state.activeIndex];
            selectValue(state, active.value);
          }
          break;
        }

        case "Escape": {
          if (state.isOpen) {
            e.preventDefault();
            closeListbox(state);
          }
          break;
        }

        case "Tab": {
          // Close the listbox on Tab, allowing natural focus movement
          if (state.isOpen) {
            closeListbox(state);
          }
          break;
        }

        case "Backspace": {
          // In single-select mode, clear the selection when input is emptied
          if (
            !isMultiple &&
            state.selected.length > 0 &&
            input.value.length <= 1
          ) {
            state.selected = [];
            state.selectedSet.clear();
            const container = input.closest(".ds-suggestion") as HTMLElement;
            if (container) {
              syncSelectedAttributes(state);
              updateHiddenSelect(container, state);
            }
            invokeDotNet(dotNetRef, "OnAfterSelectCallback", []);
          }
          break;
        }

        case "ArrowLeft": {
          // In multi-select mode, move focus to the last chip when the
          // cursor is at the start of the input (position 0).
          if (
            isMultiple &&
            input.selectionStart === 0 &&
            input.selectionEnd === 0
          ) {
            const container = input.closest(".ds-suggestion") as HTMLElement;
            if (container) {
              const chips = container.querySelectorAll<HTMLElement>(
                ":scope > data.ds-chip",
              );
              if (chips.length > 0) {
                e.preventDefault();
                chips[chips.length - 1].focus();
              }
            }
          }
          break;
        }
      }
    },

    onFocus: () => {
      openListbox(state);
    },

    onBlur: (e: FocusEvent) => {
      // Only close if focus moves outside the combobox container and the listbox.
      // The listbox is a popover in the top-layer, so it's outside the DOM
      // container and needs a separate contains() check.
      const relatedTarget = e.relatedTarget as HTMLElement | null;
      if (
        relatedTarget &&
        (element.contains(relatedTarget) || listbox?.contains(relatedTarget))
      ) {
        return;
      }

      // Delay close to allow click events on options to fire first
      setTimeout(() => {
        const active = document.activeElement as HTMLElement | null;
        if (active && (element.contains(active) || listbox?.contains(active))) {
          return;
        }
        closeListbox(state);
      }, 150);
    },

    onListboxClick: (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      const option = target.closest<HTMLElement>(
        '[role="option"]:not([data-empty])',
      );
      if (!option || option.hasAttribute("disabled")) {
        return;
      }

      e.preventDefault();
      selectValue(state, optionValue(option));
      input.focus();
    },

    onChipClick: (e: MouseEvent) => {
      handleChipClick(e, state);
    },

    onChipKeydown: (e: KeyboardEvent) => {
      handleChipKeydown(e, state);
    },

    onClearClick: (e: MouseEvent) => {
      handleClearClick(e, state);
    },

    onClearKeydown: (e: KeyboardEvent) => {
      handleClearKeydown(e, state);
    },

    onReposition: (e?: Event) => {
      if (!state.isOpen || !listbox) {
        return;
      }

      // Ignore scroll events that originate from inside the listbox itself
      // to prevent resetting the scroll position when the user scrolls the options.
      if (
        e?.type === "scroll" &&
        e.target &&
        listbox.contains(e.target as Node)
      ) {
        return;
      }

      const anchor = input.closest(".ds-suggestion") as HTMLElement;
      if (anchor) {
        positionListbox(anchor, listbox);
      }
    },
  };

  // Build the initial option index
  buildOptionIndex(state);

  // Watch for Blazor re-renders that add/remove option elements in the listbox.
  // Only watches direct childList changes (not attribute mutations from our own code).
  // Debounces rapid consecutive mutations from Blazor batch re-renders.
  if (listbox) {
    let mutationDebounce: ReturnType<typeof setTimeout> | null = null;
    const observer = new MutationObserver(() => {
      if (mutationDebounce !== null) {
        clearTimeout(mutationDebounce);
      }
      mutationDebounce = setTimeout(() => {
        mutationDebounce = null;
        buildOptionIndex(state);
        syncSelectedAttributes(state);
        if (state.isOpen) {
          filterOptions(state);
        }
        // When JS-side filtering is disabled, update the empty placeholder
        // based on whether any non-empty options exist after the Blazor re-render.
        if (!state.filterEnabled && state.emptyEl) {
          state.emptyEl.hidden = state.optionIndex.length > 0;
        }
        invalidateFilterCache(state);
        // Reposition after the option list changes,
        // so max-height and placement reflect the new content size.
        if (state.isOpen) {
          requestAnimationFrame(() => state.onReposition());
        }
      }, 16);
    });
    observer.observe(listbox, {
      childList: true,
      subtree: true,
      attributes: false,
      characterData: false,
    });
    state.listboxObserver = observer;
  }

  // Attach event listeners
  input.addEventListener("input", state.onInput);
  input.addEventListener("keydown", state.onKeydown);
  input.addEventListener("focus", state.onFocus);
  input.addEventListener("blur", state.onBlur);

  if (listbox) {
    listbox.addEventListener("click", state.onListboxClick);

    // Prevent mousedown inside the listbox from moving focus away from the input.
    // This keeps the input focused while the user clicks options or drags the scrollbar.
    listbox.addEventListener("mousedown", (e: MouseEvent) => {
      e.preventDefault();
    });
  }

  // Use event delegation on the container for chip and clear interactions
  element.addEventListener("click", state.onChipClick);
  element.addEventListener("keydown", state.onChipKeydown);
  element.addEventListener("click", state.onClearClick);
  element.addEventListener("keydown", state.onClearKeydown);

  instances.set(elementId, state);
}

/**
 * Programmatically sets the selected values on a combobox element.
 * Creates or removes <data> chip elements to match the provided values.
 */
export function setSelected(elementId: string, values: string[]): void {
  const element = document.getElementById(elementId);
  if (!element) {
    return;
  }

  const state = instances.get(elementId);
  const safeValues = values.filter((v) => v != null && v.length > 0);

  if (state) {
    state.selected = [...safeValues];
    state.selectedSet = new Set(safeValues);
    renderChips(element, state);
    syncSelectedAttributes(state);

    // In single-select mode, show the selected value in the input
    if (!state.isMultiple && safeValues.length === 1) {
      state.input.value = safeValues[0];
    }
  } else {
    // Fallback: set chips even without an initialized state
    // (for SSR prerender before JS interop is ready)
    const input = element.querySelector("input");
    if (!input) {
      return;
    }

    const existingChips = element.querySelectorAll(":scope > data.ds-chip");
    existingChips.forEach((chip) => chip.remove());

    for (let i = 0; i < safeValues.length; i++) {
      const data = document.createElement("data");
      data.className = "ds-chip";
      data.setAttribute("data-removable", "true");
      data.value = safeValues[i];
      data.textContent = safeValues[i];
      data.setAttribute("role", "button");
      data.setAttribute("tabindex", "-1");

      const position = i + 1;
      data.setAttribute(
        "aria-label",
        `${safeValues[i]}, Press to remove, ${position} of ${safeValues.length}`,
      );
      input.insertAdjacentElement("beforebegin", data);
    }

    if (input.value && safeValues.length > 0) {
      input.value = "";
      input.placeholder = "";
    }
  }

  updateHiddenSelect(
    element,
    state ||
      ({
        selected: safeValues,
        selectedSet: new Set(safeValues),
      } as unknown as ComboboxState),
  );
}

/**
 * Cleans up all event listeners and state for a combobox instance.
 */
export function disposeCombobox(elementId: string): void {
  const state = instances.get(elementId);
  if (!state) {
    instances.delete(elementId);
    return;
  }

  const element = document.getElementById(elementId);

  // Cancel any pending filter debounce
  if (state.filterDebounceId !== null) {
    clearTimeout(state.filterDebounceId);
  }

  // Disconnect the MutationObserver
  if (state.listboxObserver) {
    state.listboxObserver.disconnect();
    state.listboxObserver = null;
  }

  stopRepositioning(state);

  state.input.removeEventListener("input", state.onInput);
  state.input.removeEventListener("keydown", state.onKeydown);
  state.input.removeEventListener("focus", state.onFocus);
  state.input.removeEventListener("blur", state.onBlur);

  if (state.listbox) {
    state.listbox.removeEventListener("click", state.onListboxClick);
  }

  if (element) {
    element.removeEventListener("click", state.onChipClick);
    element.removeEventListener("keydown", state.onChipKeydown);
    element.removeEventListener("click", state.onClearClick);
    element.removeEventListener("keydown", state.onClearKeydown);
  }

  // Release references to help GC
  state.optionIndex = [];
  state.visibleCache = [];

  instances.delete(elementId);
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Suggestion = globalThis.Hviktor.Suggestion || {};
globalThis.Hviktor.Suggestion.initializeCombobox = initializeCombobox;
globalThis.Hviktor.Suggestion.setSelected = setSelected;
globalThis.Hviktor.Suggestion.disposeCombobox = disposeCombobox;
