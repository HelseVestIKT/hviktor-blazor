/**
 * Popover module for handling standalone popovers (without TriggerContext).
 * Uses shared positioning utilities from popover-positioning.ts
 *
 * This module is used when a popover is triggered by a button with popovertarget
 * attribute but without a TriggerContext wrapper.
 *
 * Also provides controlled state management for popovers/dropdowns that need
 * to manage their open state via Blazor (Open/OnClose pattern).
 */

import {
  resetPopoverPlacement,
  setArrowOffset,
  updatePopoverPosition,
} from "../utils/popover-positioning";

interface PopoverListeners {
  toggle: EventListenerOrEventListenerObject;
  docClick: EventListenerOrEventListenerObject;
  docKeyup: EventListenerOrEventListenerObject;
  docScroll: EventListenerOrEventListenerObject;
  winResize: EventListenerOrEventListenerObject;
}

interface PopoverState {
  listeners: PopoverListeners;
  resizeObserver: ResizeObserver;
}

interface ControlledState {
  dotNetRef: DotNet.DotNetObject;
}

declare global {
  namespace DotNet {
    interface DotNetObject {
      invokeMethodAsync<T>(
        methodIdentifier: string,
        ...args: unknown[]
      ): Promise<T>;
    }
  }
}

const popoverStates: Map<HTMLElement, PopoverState> = new Map();
const controlledPopovers: Map<HTMLElement, ControlledState> = new Map();

/**
 * Finds the trigger element that targets this popover
 */
const findTriggerElement = (popoverId: string): HTMLElement | null => {
  return document.querySelector(`[popovertarget="${popoverId}"]`);
};

/**
 * Updates the position of the popover relative to its trigger, but only while open.
 */
const updatePosition = (popoverRef: HTMLElement) => {
  if (!popoverRef.matches(":popover-open")) {
    return;
  }
  const triggerRef = findTriggerElement(popoverRef.id);
  if (!triggerRef) {
    return;
  }
  updatePopoverPosition(triggerRef, popoverRef);
};

/**
 * Check if a popover is in controlled mode.
 */
export const isControlled = (popoverRef: HTMLElement): boolean => {
  return controlledPopovers.has(popoverRef);
};

/**
 * Invoke the onClose callback if the popover is controlled.
 * Returns true if controlled (callback invoked), false otherwise.
 */
export const invokeOnCloseIfControlled = (popoverRef: HTMLElement): boolean => {
  const state = controlledPopovers.get(popoverRef);
  if (state?.dotNetRef) {
    state.dotNetRef.invokeMethodAsync("OnCloseFromJs");
    return true;
  }
  return false;
};

/**
 * Register a popover as controlled with a .NET reference for callbacks.
 */
export const registerControlled = (
  popoverRef: HTMLElement,
  dotNetRef: DotNet.DotNetObject,
) => {
  if (!popoverRef) return;
  controlledPopovers.set(popoverRef, { dotNetRef });
};

/**
 * Unregister a controlled popover.
 */
export const unregisterControlled = (popoverRef: HTMLElement) => {
  if (!popoverRef) return;
  controlledPopovers.delete(popoverRef);
};

/**
 * Show the popover (for controlled mode).
 * Calls showPopover first so the element enters the top-layer and has layout dimensions,
 * then positions it in a requestAnimationFrame so offsetWidth/offsetHeight are non-zero.
 */
export const show = (popoverRef: HTMLElement) => {
  if (!popoverRef) {
    return;
  }
  popoverRef.showPopover();
  // Position after the browser has laid out the top-layer element.
  requestAnimationFrame(() => updatePosition(popoverRef));
};

/**
 * Hide the popover (for controlled mode).
 */
export const hide = (popoverRef: HTMLElement) => {
  if (!popoverRef) return;
  popoverRef.hidePopover();
};

/**
 * Initialize a popover element for standalone use (without TriggerContext).
 */
export const initializePopover = (popoverRef: HTMLElement, offsetInPx = 8) => {
  if (!popoverRef || !popoverRef.id) {
    console.error("Popover must have an id attribute");
    return;
  }

  // Check if already initialized
  if (popoverStates.has(popoverRef)) {
    return;
  }

  const triggerRef = findTriggerElement(popoverRef.id);
  if (!triggerRef) {
    console.warn(`No trigger found for popover: ${popoverRef.id}`);
    return;
  }

  setArrowOffset(offsetInPx);

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
      // Check if controlled - if so, invoke callback instead of auto-closing
      if (isControlled(popoverRef)) {
        invokeOnCloseIfControlled(popoverRef);
      } else {
        popoverRef.hidePopover();
      }
    }
  };

  const onKeyUpEvent = (event: Event) => {
    const keyEvent = event as KeyboardEvent;
    if (keyEvent.code === "Escape") {
      // Only act if the popover is currently open.
      if (!popoverRef.matches(":popover-open")) {
        return;
      }
      // Check if controlled - if so, invoke callback instead of auto-closing
      if (isControlled(popoverRef)) {
        invokeOnCloseIfControlled(popoverRef);
      } else {
        popoverRef.hidePopover();
      }
      // Return focus to the trigger that opened this popover (WCAG 2.1 / ARIA pattern).
      triggerRef?.focus();
    }
  };

  const onScrollOrResize = () => updatePosition(popoverRef);

  const listeners: PopoverListeners = {
    toggle: onToggle,
    docClick: onClickEvent,
    docKeyup: onKeyUpEvent,
    docScroll: onScrollOrResize,
    winResize: onScrollOrResize,
  };

  popoverRef.addEventListener("toggle", listeners.toggle);
  document.addEventListener("click", listeners.docClick);
  document.addEventListener("keyup", listeners.docKeyup);
  // Use capture:true so scroll events from nested scrollable containers are caught.
  document.addEventListener("scroll", listeners.docScroll, {
    capture: true,
    passive: true,
  });
  globalThis.addEventListener("resize", listeners.winResize);

  // ResizeObserver keeps the popover anchored when the trigger element changes size.
  const resizeObserver = new ResizeObserver(() => updatePosition(popoverRef));
  resizeObserver.observe(triggerRef);

  popoverStates.set(popoverRef, { listeners, resizeObserver });
};

/**
 * Dispose a popover element
 */
export const disposePopover = (popoverRef: HTMLElement) => {
  if (!popoverRef) return;

  resetPopoverPlacement(popoverRef);

  const state = popoverStates.get(popoverRef);
  if (state) {
    popoverRef.removeEventListener("toggle", state.listeners.toggle);
    document.removeEventListener("click", state.listeners.docClick);
    document.removeEventListener("keyup", state.listeners.docKeyup);
    document.removeEventListener("scroll", state.listeners.docScroll, {
      capture: true,
    });
    globalThis.removeEventListener("resize", state.listeners.winResize);
    state.resizeObserver.disconnect();
    popoverStates.delete(popoverRef);
  }
};

// Expose to global scope
globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Popover = globalThis.Hviktor.Popover || {};
globalThis.Hviktor.Popover.initialize = initializePopover;
globalThis.Hviktor.Popover.dispose = disposePopover;
globalThis.Hviktor.Popover.show = show;
globalThis.Hviktor.Popover.hide = hide;
globalThis.Hviktor.Popover.registerControlled = registerControlled;
globalThis.Hviktor.Popover.unregisterControlled = unregisterControlled;
globalThis.Hviktor.Popover.isControlled = isControlled;
globalThis.Hviktor.Popover.invokeOnCloseIfControlled =
  invokeOnCloseIfControlled;
