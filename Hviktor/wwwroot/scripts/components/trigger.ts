/**
 * Trigger module for handling popover triggers within TriggerContext pattern.
 * Uses shared positioning utilities from popover-positioning.ts
 */

import {
  resetPopoverPlacement,
  setArrowOffset,
  updatePopoverPosition,
} from "../utils/popover-positioning";

interface ListenerConfig {
  type: string;
  listener: EventListenerOrEventListenerObject;
  options?: AddEventListenerOptions;
  bindTo: EventTarget;
}

interface TriggerListeners {
  popoverToggle: ListenerConfig;
  docClick: ListenerConfig;
  docKeyup: ListenerConfig;
  docScroll: ListenerConfig;
  winResize: ListenerConfig;
}

interface TriggerState {
  listenerConfigs: TriggerListeners;
  resizeObserver: ResizeObserver;
}

const triggerStates: Map<HTMLElement, TriggerState> = new Map();

const getPopoverTarget = (triggerRef: HTMLElement): HTMLElement | null => {
  if (!triggerRef) {
    return null;
  }

  const popovertargetId = triggerRef.getAttribute("popovertarget");
  if (!popovertargetId) {
    console.error("Popover target ID not found on trigger element.");
    return null;
  }

  return document.getElementById(popovertargetId);
};

const closePopover = (triggerRef: HTMLElement) => {
  const popoverRef = getPopoverTarget(triggerRef);
  popoverRef?.hidePopover();
};

const updatePosition = (triggerRef: HTMLElement) => {
  const popoverRef = getPopoverTarget(triggerRef);
  if (!popoverRef) {
    return;
  }
  // Only reposition when the popover is actually open and has layout dimensions.
  if (!popoverRef.matches(":popover-open")) {
    return;
  }
  updatePopoverPosition(triggerRef, popoverRef);
};

export const initializeTrigger = (triggerRef: HTMLElement, offsetInPx = 8) => {
  if (!triggerRef) {
    return;
  }

  setArrowOffset(offsetInPx);
  const popoverRef = getPopoverTarget(triggerRef);
  if (!popoverRef) {
    return;
  }

  // Check if already initialized
  if (triggerStates.has(triggerRef)) {
    return;
  }

  const onToggle = (event: Event) => {
    const toggleEvent = event as ToggleEvent;
    // Only update position when transitioning to open so that offsetWidth/offsetHeight
    // are non-zero (element is now in the top-layer and has a layout box).
    if (toggleEvent.newState === "open") {
      updatePopoverPosition(triggerRef, popoverRef);
    }
  };

  const onClickEvent = (event: Event) => {
    if (
      !triggerRef.contains(event.target as Node) &&
      !popoverRef.contains(event.target as Node)
    ) {
      // Check if this is a controlled popover - if so, invoke callback instead of auto-closing
      if (globalThis.Hviktor?.Popover?.isControlled?.(popoverRef)) {
        globalThis.Hviktor.Popover.invokeOnCloseIfControlled(popoverRef);
      } else {
        closePopover(triggerRef);
      }
    }
  };

  const onKeyUpEvent = (event: Event) => {
    const keyEvent = event as KeyboardEvent;
    if (keyEvent.code === "Escape") {
      // Only act if the popover is currently open and focus is within it.
      if (!popoverRef.matches(":popover-open")) {
        return;
      }
      // Check if this is a controlled popover - if so, invoke callback instead of auto-closing
      if (globalThis.Hviktor?.Popover?.isControlled?.(popoverRef)) {
        globalThis.Hviktor.Popover.invokeOnCloseIfControlled(popoverRef);
      } else {
        closePopover(triggerRef);
      }
      // Return focus to the trigger that opened this popover (WCAG 2.1 / ARIA pattern).
      triggerRef.focus();
    }
  };

  const onScrollOrResize = () => updatePosition(triggerRef);

  const listenerConfigs: TriggerListeners = {
    popoverToggle: { type: "toggle", listener: onToggle, bindTo: popoverRef },
    docClick: { type: "click", listener: onClickEvent, bindTo: document },
    docKeyup: { type: "keyup", listener: onKeyUpEvent, bindTo: document },
    // Use capture:true so scroll events from any nested scrollable container are caught.
    docScroll: {
      type: "scroll",
      listener: onScrollOrResize,
      options: { capture: true, passive: true },
      bindTo: document,
    },
    winResize: {
      type: "resize",
      listener: onScrollOrResize,
      bindTo: globalThis,
    },
  };

  for (const config of Object.values(listenerConfigs) as ListenerConfig[]) {
    config.bindTo.addEventListener(
      config.type,
      config.listener,
      config.options,
    );
  }

  // ResizeObserver keeps the popover anchored when the trigger element changes size
  // (e.g. due to content changes, font loading, or viewport-unit-based layouts).
  const resizeObserver = new ResizeObserver(() => updatePosition(triggerRef));
  resizeObserver.observe(triggerRef);

  triggerStates.set(triggerRef, { listenerConfigs, resizeObserver });
};

export const disposeTrigger = (triggerRef: HTMLElement) => {
  if (!triggerRef) {
    return;
  }

  const popoverRef = getPopoverTarget(triggerRef);
  if (popoverRef) {
    resetPopoverPlacement(popoverRef);
  }

  const state = triggerStates.get(triggerRef);
  if (state) {
    for (const config of Object.values(
      state.listenerConfigs,
    ) as ListenerConfig[]) {
      config.bindTo.removeEventListener(
        config.type,
        config.listener,
        config.options,
      );
    }
    state.resizeObserver.disconnect();
    triggerStates.delete(triggerRef);
  }
};

// Expose to global scope
globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Trigger = globalThis.Hviktor.Trigger || {};
globalThis.Hviktor.Trigger.initialize = initializeTrigger;
globalThis.Hviktor.Trigger.dispose = disposeTrigger;
