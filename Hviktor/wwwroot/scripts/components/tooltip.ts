import {
  preparePopoverForPositioning,
  resetPopoverPlacement,
  setArrowOffset,
  updatePopoverPosition,
} from "../utils/popover-positioning";

const ARROW_OFFSET = 6;
const HIDE_DELAY_MS = 16;
const SINGLETON_TOOLTIP_ID = "ds-tooltip";
const TOOLTIP_ATTR = "data-tooltip";
const PLACEMENT_ATTR = "data-placement";
const TYPE_ATTR = "data-tooltip-type";
const MARKER_ATTR = "data-tooltip-marker";
const MARKER_SELECTOR = `template[${MARKER_ATTR}]`;
const ANCHOR_EVENTS = [
  "mouseenter",
  "mouseleave",
  "focusin",
  "focusout",
  "pointerdown",
] as const;

interface AnchorState {
  content: string;
  placement: string;
}

const tracked = new WeakMap<HTMLElement, AnchorState>();
const trackedElements = new Set<HTMLElement>();

let tooltipEl: HTMLElement | null = null;
let activeAnchor: HTMLElement | null = null;
let hideTimeout: ReturnType<typeof setTimeout> | null = null;

let pointerTriggered = false;
let pendingPointerFocus = false;

const resizeObserver = new ResizeObserver((entries) => {
  if (!activeAnchor || !isOpen()) {
    return;
  }
  for (const entry of entries) {
    if (entry.target === activeAnchor) {
      updatePopoverPosition(activeAnchor, tooltipEl!);
      break;
    }
  }
});

const isOpen = (): boolean => {
  return tooltipEl?.matches(":popover-open") ?? false;
};

const cancelHide = () => {
  if (hideTimeout) {
    clearTimeout(hideTimeout);
    hideTimeout = null;
  }
};

const resetState = () => {
  activeAnchor = null;
  pointerTriggered = false;
  pendingPointerFocus = false;
};

const shouldKeepVisible = (anchorEl: HTMLElement): boolean => {
  if (anchorEl.matches(":hover") || tooltipEl?.matches(":hover")) {
    return true;
  }

  return !pointerTriggered && anchorEl.matches(":focus-within");
};

// Create tooltip element lazily on body
const ensureTooltipEl = (): HTMLElement => {
  if (tooltipEl) {
    return tooltipEl;
  }

  tooltipEl = document.createElement("div");
  tooltipEl.id = SINGLETON_TOOLTIP_ID;
  tooltipEl.className = "ds-tooltip";
  tooltipEl.setAttribute("role", "tooltip");
  tooltipEl.setAttribute("popover", "manual");
  document.body.appendChild(tooltipEl);

  preparePopoverForPositioning(tooltipEl);
  setArrowOffset(ARROW_OFFSET);

  tooltipEl.addEventListener("mouseleave", () => {
    if (!activeAnchor) {
      return;
    }

    if (!shouldKeepVisible(activeAnchor)) {
      hide();
    }
  });

  globalThis.addEventListener("keydown", (event: Event) => {
    if ((event as KeyboardEvent).key !== "Escape" || !isOpen()) {
      return;
    }

    event.preventDefault();
    hideImmediately();
  });

  const repositionIfOpen = () => {
    if (!isOpen() || !activeAnchor) {
      return;
    }

    updatePopoverPosition(activeAnchor, tooltipEl!);
  };

  document.addEventListener("scroll", repositionIfOpen, {
    capture: true,
    passive: true,
  });
  globalThis.addEventListener("resize", repositionIfOpen);

  return tooltipEl;
};

const show = (anchorEl: HTMLElement) => {
  const state = tracked.get(anchorEl);
  if (!state) {
    return;
  }

  cancelHide();

  const el = ensureTooltipEl();

  if (activeAnchor !== anchorEl) {
    el.textContent = state.content;
    el.dataset.placement = state.placement;
    delete el.dataset.placementOriginal;
    resetPopoverPlacement(el);
    preparePopoverForPositioning(el);
    activeAnchor = anchorEl;
  }

  if (!isOpen()) {
    el.showPopover();
  }

  requestAnimationFrame(() => updatePopoverPosition(anchorEl, el));
};

const hideImmediately = () => {
  cancelHide();
  if (isOpen()) {
    tooltipEl!.hidePopover();
  }
  resetState();
};

const hide = () => {
  if (hideTimeout) {
    return;
  }

  hideTimeout = setTimeout(() => {
    hideTimeout = null;
    if (isOpen()) {
      tooltipEl!.hidePopover();
    }

    resetState();
  }, HIDE_DELAY_MS);
};

/**
 * Shared event handler for all anchor elements. Dispatches by event type.
 * Uses currentTarget (the element the listener was registered on) to reliably
 * resolve the anchor, since focusin/focusout bubble from child elements.
 */
const handleAnchorEvent = (event: Event) => {
  const anchorEl = event.currentTarget as HTMLElement | null;
  if (!anchorEl || !tracked.has(anchorEl)) {
    return;
  }

  switch (event.type) {
    case "mouseenter":
      pointerTriggered = true;
      show(anchorEl);
      break;

    case "mouseleave":
      requestAnimationFrame(() => {
        if (activeAnchor !== anchorEl) {
          return;
        }

        if (!shouldKeepVisible(anchorEl)) {
          hide();
        }
      });
      break;

    case "focusin":
      if (pendingPointerFocus) {
        pendingPointerFocus = false;
        return;
      }
      pointerTriggered = false;
      show(anchorEl);
      break;

    case "focusout": {
      const related = (event as FocusEvent).relatedTarget as Node | null;
      if (related && anchorEl.contains(related)) {
        return;
      }

      if (activeAnchor !== anchorEl) {
        return;
      }

      if (!shouldKeepVisible(anchorEl)) {
        hide();
      }
      break;
    }

    case "pointerdown":
      pointerTriggered = true;
      pendingPointerFocus = true;
      break;
  }
};

// Applies the appropriate ARIA association based on data-tooltip-type
const applyAria = (el: HTMLElement) => {
  const type = el.getAttribute(TYPE_ATTR);
  if (type === "labelledby") {
    el.removeAttribute("aria-labelledby");
    el.setAttribute("aria-describedby", SINGLETON_TOOLTIP_ID);
  } else {
    el.removeAttribute("aria-describedby");
    el.setAttribute("aria-labelledby", SINGLETON_TOOLTIP_ID);
  }
};

const removeAria = (el: HTMLElement) => {
  if (el.getAttribute("aria-labelledby") === SINGLETON_TOOLTIP_ID) {
    el.removeAttribute("aria-labelledby");
  }
  if (el.getAttribute("aria-describedby") === SINGLETON_TOOLTIP_ID) {
    el.removeAttribute("aria-describedby");
  }
};

const attach = (el: HTMLElement) => {
  if (tracked.has(el)) {
    return;
  }

  const content = el.getAttribute(TOOLTIP_ATTR) ?? "";
  const placement = el.getAttribute(PLACEMENT_ATTR) ?? "top";

  tracked.set(el, { content, placement });
  trackedElements.add(el);

  for (const eventType of ANCHOR_EVENTS) {
    el.addEventListener(eventType, handleAnchorEvent);
  }

  resizeObserver.observe(el);
  applyAria(el);
};

const detach = (el: HTMLElement) => {
  if (!tracked.has(el)) {
    return;
  }

  for (const eventType of ANCHOR_EVENTS) {
    el.removeEventListener(eventType, handleAnchorEvent);
  }

  resizeObserver.unobserve(el);
  removeAria(el);
  tracked.delete(el);
  trackedElements.delete(el);

  if (activeAnchor === el) {
    hideImmediately();
  }
};

const updateState = (el: HTMLElement) => {
  const state = tracked.get(el);
  if (!state) {
    return;
  }

  const content = el.getAttribute(TOOLTIP_ATTR) ?? "";
  const placement = el.getAttribute(PLACEMENT_ATTR) ?? "top";

  state.content = content;
  state.placement = placement;
  applyAria(el);

  if (activeAnchor === el && isOpen()) {
    // Content or placement changed while visible: force re-show
    activeAnchor = null;
    show(el);
  }
};

// Scan DOM for all elements with data-tooltip and attach
const scanAndAttach = (root: Node) => {
  // Process template markers first so they set data-tooltip on siblings before attach runs
  processMarkers(root);

  if (root instanceof HTMLElement && root.hasAttribute(TOOLTIP_ATTR)) {
    attach(root);
  }

  if (root instanceof HTMLElement || root instanceof DocumentFragment) {
    const elements = (
      root as HTMLElement | DocumentFragment
    ).querySelectorAll<HTMLElement>(`[${TOOLTIP_ATTR}]`);
    for (const el of elements) {
      attach(el);
    }
  }
};

/**
 * Applies tooltip data attributes to a target element from marker config.
 */
const applyMarkerConfig = (
  target: HTMLElement,
  content: string,
  placement: string,
  type: string | null,
  autoplacement: string | null,
) => {
  target.setAttribute(TOOLTIP_ATTR, content);
  target.setAttribute(PLACEMENT_ATTR, placement);

  if (type) {
    target.setAttribute(TYPE_ATTR, type);
  } else {
    target.removeAttribute(TYPE_ATTR);
  }

  if (autoplacement) {
    target.setAttribute("data-autoplacement", autoplacement);
  }
};

/**
 * Collects adjacent non-element sibling nodes following the marker, stopping at
 * the first element node. Returns only text nodes with non-whitespace content.
 * A non-empty result means the ChildContent starts with text (not an element).
 */
const collectTextNodes = (marker: HTMLTemplateElement): Text[] => {
  const textNodes: Text[] = [];
  let node = marker.nextSibling;

  while (node) {
    if (node.nodeType === Node.ELEMENT_NODE) {
      break;
    }
    if (node.nodeType === Node.TEXT_NODE && node.textContent?.trim()) {
      textNodes.push(node as Text);
    }
    node = node.nextSibling;
  }

  return textNodes;
};

/**
 * Processes a single template marker: reads tooltip config from the marker's data
 * attributes, applies them to the next element child, then removes the marker.
 * When the ChildContent is just plaintext, we must wrap the text nodes in a SPAN tag with tooltip attributes.
 */
const processMarker = (marker: HTMLTemplateElement) => {
  const content = marker.getAttribute("data-tooltip-content") ?? "";
  const placement = marker.getAttribute("data-tooltip-placement") ?? "top";
  const type = marker.getAttribute("data-tooltip-type");
  const autoplacement = marker.getAttribute("data-tooltip-autoplacement");

  // Collect text nodes between the marker and the first element sibling.
  // A non-empty result means ChildContent is plain text.
  const textNodes = collectTextNodes(marker);

  if (textNodes.length > 0) {
    // Text-only ChildContent: wrap in a focusable <span>
    const wrapper = document.createElement("span");
    wrapper.setAttribute("tabindex", "0");
    applyMarkerConfig(wrapper, content, placement, type, autoplacement);

    marker.parentNode?.insertBefore(wrapper, marker);
    for (const textNode of textNodes) {
      wrapper.appendChild(textNode);
    }

    marker.remove();
    return;
  }

  // Element ChildContent: apply attributes directly to the next element sibling
  const elementSibling = marker.nextElementSibling as HTMLElement | null;
  if (elementSibling) {
    applyMarkerConfig(elementSibling, content, placement, type, autoplacement);
  }

  marker.remove();
};

/**
 * Finds and processes all template tooltip markers within a subtree.
 */
const processMarkers = (root: Node) => {
  if (root instanceof HTMLTemplateElement && root.hasAttribute(MARKER_ATTR)) {
    processMarker(root);
    return;
  }

  if (root instanceof HTMLElement || root instanceof DocumentFragment) {
    const markers = (
      root as HTMLElement | DocumentFragment
    ).querySelectorAll<HTMLTemplateElement>(MARKER_SELECTOR);
    for (const marker of markers) {
      processMarker(marker);
    }
  }

  // Also check the parent in case the marker was added as a child (Blazor re-renders)
  if (root instanceof HTMLElement && root.parentElement) {
    const parentMarkers =
      root.parentElement.querySelectorAll<HTMLTemplateElement>(
        `:scope > ${MARKER_SELECTOR}`,
      );
    for (const marker of parentMarkers) {
      processMarker(marker);
    }
  }
};

// MutationObserver: watches for data-tooltip additions, removals, and changes
const observer = new MutationObserver((mutations) => {
  for (const mutation of mutations) {
    if (mutation.type === "childList") {
      for (const node of mutation.addedNodes) {
        // Process markers first so they set data-tooltip on siblings
        if (
          node instanceof HTMLTemplateElement &&
          node.hasAttribute(MARKER_ATTR)
        ) {
          processMarker(node);
        } else if (node instanceof HTMLElement) {
          processMarkers(node);
        }

        scanAndAttach(node);
      }

      for (const node of mutation.removedNodes) {
        if (node instanceof HTMLElement) {
          detach(node);
          const descendants = node.querySelectorAll<HTMLElement>(
            `[${TOOLTIP_ATTR}]`,
          );
          for (const el of descendants) {
            detach(el);
          }
        }
      }
    }

    if (
      mutation.type === "attributes" &&
      mutation.target instanceof HTMLElement
    ) {
      const el = mutation.target;

      if (mutation.attributeName === TOOLTIP_ATTR) {
        if (el.hasAttribute(TOOLTIP_ATTR)) {
          if (tracked.has(el)) {
            updateState(el);
          } else {
            attach(el);
          }
        } else {
          detach(el);
        }
      }

      if (
        (mutation.attributeName === PLACEMENT_ATTR ||
          mutation.attributeName === TYPE_ATTR) &&
        tracked.has(el)
      ) {
        updateState(el);
      }
    }
  }
});

// Initialize: scan existing DOM and start observing
processMarkers(document.body);
scanAndAttach(document.body);

observer.observe(document.body, {
  childList: true,
  subtree: true,
  attributes: true,
  attributeFilter: [TOOLTIP_ATTR, PLACEMENT_ATTR, TYPE_ATTR],
});
