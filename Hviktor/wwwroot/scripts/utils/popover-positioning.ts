/**
 * Shared popover positioning utilities.
 * Used by both trigger.ts (TriggerContext pattern) and popover.ts (standalone pattern).
 *
 * Positioning strategy:
 * - Popover elements live in the top-layer, so `position: fixed` is relative to the viewport.
 * - We use `inset: unset; margin: 0` to opt out of browser default centering, then apply
 *   `translate` with viewport-relative coordinates from `getBoundingClientRect()`.
 * - The popover must be visible (in the top-layer) for `offsetWidth`/`offsetHeight` to be
 *   non-zero, so we only compute position after `toggle` to `open` (not before `showPopover`).
 */

let arrowOffset = 8;

export const setArrowOffset = (offset: number) => {
  arrowOffset = offset;
};

export const getArrowOffset = () => arrowOffset;

/**
 * Prepares a popover element for manual positioning by overriding browser default styles
 * that would otherwise center it on screen. Must be called once per popover element.
 */
export const preparePopoverForPositioning = (popoverRef: HTMLElement): void => {
  popoverRef.style.setProperty("position", "fixed");
  popoverRef.style.setProperty("inset", "unset");
  popoverRef.style.setProperty("margin", "0");
};

// Position Functions

const calculateArrowPositionX = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  offsetX: number,
  offsetY: string,
) => {
  let arrowX = "50%";
  if (popoverRef.offsetWidth > boundingRect.width) {
    arrowX = `${offsetX + boundingRect.width / 2}px`;
  }
  popoverRef.style.setProperty("--_ds-floating-arrow-x", arrowX);
  popoverRef.style.setProperty("--_ds-floating-arrow-y", offsetY);
};

const calculateArrowPositionY = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  offsetX: string,
  offsetY: number,
) => {
  let arrowY = "50%";
  if (popoverRef.offsetHeight > boundingRect.height) {
    arrowY = `${offsetY + boundingRect.height / 2}px`;
  }
  popoverRef.style.setProperty("--_ds-floating-arrow-x", offsetX);
  popoverRef.style.setProperty("--_ds-floating-arrow-y", arrowY);
};

const calculateArrowPositionBottom = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  offsetX: number,
) => {
  calculateArrowPositionX(boundingRect, popoverRef, offsetX, "0");
};

const calculateArrowPositionTop = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  offsetX: number,
) => {
  calculateArrowPositionX(boundingRect, popoverRef, offsetX, "100%");
};

const calculateArrowPositionLeft = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  offsetY: number,
) => {
  calculateArrowPositionY(boundingRect, popoverRef, "100%", offsetY);
};

const calculateArrowPositionRight = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  offsetY: number,
) => {
  calculateArrowPositionY(boundingRect, popoverRef, "0", offsetY);
};

export const positionBottom = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverWidth = popoverRef.offsetWidth;
  const buttonCenterX = boundingRect.left + boundingRect.width / 2;
  const top = boundingRect.top + boundingRect.height + getArrowOffset();
  const left = buttonCenterX - popoverWidth / 2;

  popoverRef.style.setProperty("--_ds-floating-arrow-x", "50%");
  popoverRef.style.setProperty("--_ds-floating-arrow-y", "0");
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionBottomEnd = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverWidth = popoverRef.offsetWidth;
  const top = boundingRect.top + boundingRect.height + getArrowOffset();
  const left = boundingRect.left + boundingRect.width - popoverWidth;

  calculateArrowPositionBottom(
    boundingRect,
    popoverRef,
    popoverRef.offsetWidth - boundingRect.width,
  );
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionBottomStart = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const top = boundingRect.top + boundingRect.height + getArrowOffset();
  const left = boundingRect.left;

  calculateArrowPositionBottom(boundingRect, popoverRef, 0);
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionTop = (boundingRect: DOMRect, popoverRef: HTMLElement) => {
  const popoverWidth = popoverRef.offsetWidth;
  const buttonCenterX = boundingRect.left + boundingRect.width / 2;
  const top = boundingRect.top - popoverRef.offsetHeight - getArrowOffset();
  const left = buttonCenterX - popoverWidth / 2;

  popoverRef.style.setProperty("--_ds-floating-arrow-x", "50%");
  popoverRef.style.setProperty("--_ds-floating-arrow-y", "100%");
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionTopStart = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const top = boundingRect.top - popoverRef.offsetHeight - getArrowOffset();
  const left = boundingRect.left;

  calculateArrowPositionTop(boundingRect, popoverRef, 0);
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionTopEnd = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverWidth = popoverRef.offsetWidth;
  const top = boundingRect.top - popoverRef.offsetHeight - getArrowOffset();
  const left = boundingRect.left + boundingRect.width - popoverWidth;

  calculateArrowPositionTop(
    boundingRect,
    popoverRef,
    popoverRef.offsetWidth - boundingRect.width,
  );
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionLeft = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverHeight = popoverRef.offsetHeight;
  const buttonCenterY = boundingRect.top + boundingRect.height / 2;
  const top = buttonCenterY - popoverHeight / 2;
  const left = boundingRect.left - popoverRef.offsetWidth - getArrowOffset();

  popoverRef.style.setProperty("--_ds-floating-arrow-x", "100%");
  popoverRef.style.setProperty("--_ds-floating-arrow-y", "50%");
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionLeftStart = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const top = boundingRect.top;
  const left = boundingRect.left - popoverRef.offsetWidth - getArrowOffset();

  calculateArrowPositionLeft(boundingRect, popoverRef, 0);
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionLeftEnd = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverHeight = popoverRef.offsetHeight;
  const top = boundingRect.top + boundingRect.height - popoverHeight;
  const left = boundingRect.left - popoverRef.offsetWidth - getArrowOffset();

  calculateArrowPositionLeft(
    boundingRect,
    popoverRef,
    popoverRef.offsetHeight - boundingRect.height,
  );
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionRight = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverHeight = popoverRef.offsetHeight;
  const buttonCenterY = boundingRect.top + boundingRect.height / 2;
  const top = buttonCenterY - popoverHeight / 2;
  const left = boundingRect.left + boundingRect.width + getArrowOffset();

  popoverRef.style.setProperty("--_ds-floating-arrow-x", "0");
  popoverRef.style.setProperty("--_ds-floating-arrow-y", "50%");
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionRightStart = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const top = boundingRect.top;
  const left = boundingRect.left + boundingRect.width + getArrowOffset();

  calculateArrowPositionRight(boundingRect, popoverRef, 0);
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

export const positionRightEnd = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
) => {
  const popoverHeight = popoverRef.offsetHeight;
  const top = boundingRect.top + boundingRect.height - popoverHeight;
  const left = boundingRect.left + boundingRect.width + getArrowOffset();

  calculateArrowPositionRight(
    boundingRect,
    popoverRef,
    popoverRef.offsetHeight - boundingRect.height,
  );
  popoverRef.style.setProperty("translate", `${left}px ${top}px`);
};

// Viewport Fitting

export const doesPopoverFitInViewport = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  placement: string,
): boolean => {
  const popoverWidth = popoverRef.offsetWidth + getArrowOffset();
  const popoverHeight = popoverRef.offsetHeight + getArrowOffset();

  switch (placement) {
    case "bottom":
      return (
        boundingRect.top + boundingRect.height + popoverHeight <=
          globalThis.innerHeight &&
        boundingRect.left + boundingRect.width / 2 - popoverWidth / 2 >= 0 &&
        boundingRect.left + boundingRect.width / 2 + popoverWidth / 2 <=
          globalThis.innerWidth
      );
    case "bottom-start":
      return (
        boundingRect.top + boundingRect.height + popoverHeight <=
          globalThis.innerHeight &&
        boundingRect.left >= 0 &&
        boundingRect.left + popoverWidth <= globalThis.innerWidth
      );
    case "bottom-end":
      return (
        boundingRect.top + boundingRect.height + popoverHeight <=
          globalThis.innerHeight &&
        boundingRect.left + boundingRect.width - popoverWidth >= 0 &&
        boundingRect.left + boundingRect.width <= globalThis.innerWidth
      );
    case "top":
      return (
        boundingRect.top - popoverHeight >= 0 &&
        boundingRect.left + boundingRect.width / 2 - popoverWidth / 2 >= 0 &&
        boundingRect.left + boundingRect.width / 2 + popoverWidth / 2 <=
          globalThis.innerWidth
      );
    case "top-start":
      return (
        boundingRect.top - popoverHeight >= 0 &&
        boundingRect.left >= 0 &&
        boundingRect.left + popoverWidth <= globalThis.innerWidth
      );
    case "top-end":
      return (
        boundingRect.top - popoverHeight >= 0 &&
        boundingRect.left + boundingRect.width - popoverWidth >= 0 &&
        boundingRect.left + boundingRect.width <= globalThis.innerWidth
      );
    case "left":
      return (
        boundingRect.left - popoverWidth >= 0 &&
        boundingRect.top + boundingRect.height / 2 - popoverHeight / 2 >= 0 &&
        boundingRect.top + boundingRect.height / 2 + popoverHeight / 2 <=
          globalThis.innerHeight
      );
    case "left-start":
      return (
        boundingRect.left - popoverWidth >= 0 &&
        boundingRect.top >= 0 &&
        boundingRect.top + popoverHeight <= globalThis.innerHeight
      );
    case "left-end":
      return (
        boundingRect.left - popoverWidth >= 0 &&
        boundingRect.top + boundingRect.height - popoverHeight >= 0 &&
        boundingRect.top + boundingRect.height <= globalThis.innerHeight
      );
    case "right":
      return (
        boundingRect.left + boundingRect.width + popoverWidth <=
          globalThis.innerWidth &&
        boundingRect.top + boundingRect.height / 2 - popoverHeight / 2 >= 0 &&
        boundingRect.top + boundingRect.height / 2 + popoverHeight / 2 <=
          globalThis.innerHeight
      );
    case "right-start":
      return (
        boundingRect.left + boundingRect.width + popoverWidth <=
          globalThis.innerWidth &&
        boundingRect.top >= 0 &&
        boundingRect.top + popoverHeight <= globalThis.innerHeight
      );
    case "right-end":
      return (
        boundingRect.left + boundingRect.width + popoverWidth <=
          globalThis.innerWidth &&
        boundingRect.top + boundingRect.height - popoverHeight >= 0 &&
        boundingRect.top + boundingRect.height <= globalThis.innerHeight
      );
    default:
      return false;
  }
};

// Placement Finding

const calculateVerticalScore = (
  p: string,
  typeY: string,
  pos: string,
  bestVerticalScore: number,
  bestVertical: string | null,
) => {
  let score = 0;
  if (p.startsWith(typeY)) score += 2;
  if (p.endsWith(pos)) score += 1;
  if (score > bestVerticalScore) {
    bestVerticalScore = score;
    bestVertical = p;
  }
  return { bestVerticalScore, bestVertical };
};

const calculateHorizontalScore = (
  p: string,
  typeX: string,
  pos: string,
  bestHorizontalScore: number,
  bestHorizontal: string | null,
) => {
  let score = 0;
  if (p.startsWith(typeX)) score += 2;
  if (p.endsWith(pos)) score += 1;
  if (score > bestHorizontalScore) {
    bestHorizontalScore = score;
    bestHorizontal = p;
  }
  return { bestHorizontalScore, bestHorizontal };
};

const calculatePositionScore = (
  availableSet: Set<string>,
  targetPlacement: string,
  pos: string,
): string => {
  let bestVertical: string | null = null;
  let bestHorizontal: string | null = null;
  let bestVerticalScore: number = -1;
  let bestHorizontalScore: number = -1;

  const typeY = targetPlacement.startsWith("top") ? "top" : "bottom";
  const typeX = targetPlacement.startsWith("left") ? "left" : "right";

  for (const p of availableSet) {
    if (p.includes("top") || p.includes("bottom")) {
      const v = calculateVerticalScore(
        p,
        typeY,
        pos,
        bestVerticalScore,
        bestVertical,
      );
      bestVerticalScore = v.bestVerticalScore;
      bestVertical = v.bestVertical;
    }
    if (p.includes("left") || p.includes("right")) {
      const h = calculateHorizontalScore(
        p,
        typeX,
        pos,
        bestHorizontalScore,
        bestHorizontal,
      );
      bestHorizontalScore = h.bestHorizontalScore;
      bestHorizontal = h.bestHorizontal;
    }
  }

  if (bestHorizontal) return bestHorizontal;
  if (bestVertical) return bestVertical;
  return availableSet.values().next().value || targetPlacement;
};

export const findBestPlacement = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  preferredPlacement: string,
): string => {
  if (doesPopoverFitInViewport(boundingRect, popoverRef, preferredPlacement)) {
    return preferredPlacement;
  }

  const placements = [
    "bottom",
    "bottom-start",
    "bottom-end",
    "top",
    "top-start",
    "top-end",
    "left",
    "left-start",
    "left-end",
    "right",
    "right-start",
    "right-end",
  ];

  const availableSet = new Set(
    placements.filter((p) =>
      doesPopoverFitInViewport(boundingRect, popoverRef, p),
    ),
  );

  if (availableSet.size === 0) return preferredPlacement;
  if (availableSet.has(preferredPlacement)) return preferredPlacement;

  let pos = "";
  const target = preferredPlacement.split("-");
  if (target.length > 1) {
    pos = target[1];
  }

  return calculatePositionScore(availableSet, preferredPlacement, pos);
};

// Main Positioning Function

export const applyPosition = (
  boundingRect: DOMRect,
  popoverRef: HTMLElement,
  placement: string,
): string => {
  const bestPlacement = findBestPlacement(boundingRect, popoverRef, placement);

  switch (bestPlacement) {
    case "bottom":
      positionBottom(boundingRect, popoverRef);
      break;
    case "bottom-start":
      positionBottomStart(boundingRect, popoverRef);
      break;
    case "bottom-end":
      positionBottomEnd(boundingRect, popoverRef);
      break;
    case "top":
      positionTop(boundingRect, popoverRef);
      break;
    case "top-start":
      positionTopStart(boundingRect, popoverRef);
      break;
    case "top-end":
      positionTopEnd(boundingRect, popoverRef);
      break;
    case "left":
      positionLeft(boundingRect, popoverRef);
      break;
    case "left-start":
      positionLeftStart(boundingRect, popoverRef);
      break;
    case "left-end":
      positionLeftEnd(boundingRect, popoverRef);
      break;
    case "right":
      positionRight(boundingRect, popoverRef);
      break;
    case "right-start":
      positionRightStart(boundingRect, popoverRef);
      break;
    case "right-end":
      positionRightEnd(boundingRect, popoverRef);
      break;
    default:
      positionBottomEnd(boundingRect, popoverRef);
      return "bottom-end";
  }

  return bestPlacement;
};

/**
 * Updates the position of a popover relative to its trigger element.
 * Uses data-placement-original to remember the originally requested placement,
 * so it can revert back when space becomes available again.
 *
 * NOTE: Only call this while the popover is open (in the top-layer) so that
 * `offsetWidth`/`offsetHeight` return non-zero values for accurate sizing.
 */
export const updatePopoverPosition = (
  triggerRef: HTMLElement,
  popoverRef: HTMLElement,
): void => {
  if (!triggerRef || !popoverRef) {
    return;
  }

  // Ensure fixed positioning is applied so translate coordinates map 1:1 to the viewport.
  preparePopoverForPositioning(popoverRef);

  const boundingRect = triggerRef.getBoundingClientRect();

  // Store the original placement on first call, use it for all subsequent calculations
  if (!popoverRef.dataset.placementOriginal) {
    popoverRef.dataset.placementOriginal =
      popoverRef.dataset.placement || "bottom-end";
  }

  // Always use the original placement as the preferred one
  const preferredPlacement = popoverRef.dataset.placementOriginal;
  popoverRef.dataset.placement = applyPosition(
    boundingRect,
    popoverRef,
    preferredPlacement,
  );
};

/**
 * Resets the stored original placement and clears inline positioning styles.
 * Call this when disposing a popover.
 */
export const resetPopoverPlacement = (popoverRef: HTMLElement): void => {
  if (!popoverRef) {
    return;
  }
  delete popoverRef.dataset.placementOriginal;
  popoverRef.style.removeProperty("position");
  popoverRef.style.removeProperty("inset");
  popoverRef.style.removeProperty("margin");
  popoverRef.style.removeProperty("translate");
  popoverRef.style.removeProperty("--_ds-floating-arrow-x");
  popoverRef.style.removeProperty("--_ds-floating-arrow-y");
};
