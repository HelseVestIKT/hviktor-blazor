export interface ModalLocalizer {
  title: string;
  description: string;
  backToSafety?: string;
  reload?: string;
}

/**
 * Creates an inline SVG xmark (close) icon.
 */
export function createXmarkSvg(): SVGSVGElement {
  const ns = "http://www.w3.org/2000/svg";
  const svg = document.createElementNS(ns, "svg");
  svg.setAttribute("width", "64");
  svg.setAttribute("height", "64");
  svg.setAttribute("viewBox", "0 0 24 24");
  svg.setAttribute("fill", "none");
  svg.setAttribute("aria-hidden", "true");

  const path = document.createElementNS(ns, "path");
  path.setAttribute("fill-rule", "evenodd");
  path.setAttribute("clip-rule", "evenodd");
  path.setAttribute("fill", "currentColor");
  path.setAttribute(
    "d",
    "M6.53033 5.46967C6.23744 5.17678 5.76256 5.17678 5.46967 5.46967C5.17678 5.76256 5.17678 6.23744 5.46967 6.53033L10.9393 12L5.46967 17.4697C5.17678 17.7626 5.17678 18.2374 5.46967 18.5303C5.76256 18.8232 6.23744 18.8232 6.53033 18.5303L12 13.0607L17.4697 18.5303C17.7626 18.8232 18.2374 18.8232 18.5303 18.5303C18.8232 18.2374 18.8232 17.7626 18.5303 17.4697L13.0607 12L18.5303 6.53033C18.8232 6.23744 18.8232 5.76256 18.5303 5.46967C18.2374 5.17678 17.7626 5.17678 17.4697 5.46967L12 10.9393L6.53033 5.46967Z",
  );
  svg.appendChild(path);

  return svg;
}

/**
 * Creates a styled button element for modal actions.
 */
export function createButton(
  id: string,
  text: string,
  onClick: () => void,
): HTMLButtonElement {
  const btn = document.createElement("button");
  btn.id = id;
  btn.className = "ds-button";
  btn.dataset.variant = "tertiary";
  btn.dataset.color = "accent";
  btn.textContent = text;
  btn.onclick = onClick;
  return btn;
}

/**
 * Creates a close button positioned in the top-right corner.
 */
export function createCloseButton(
  id: string,
  onClick: () => void,
): HTMLButtonElement {
  const btn = createButton(id, "", onClick);
  btn.style.position = "absolute";
  btn.style.top = "var(--ds-size-2)";
  btn.style.right = "var(--ds-size-2)";
  btn.setAttribute("aria-label", "Close");
  btn.appendChild(createXmarkSvg());
  return btn;
}

/**
 * Creates a status icon wrapper with the xmark SVG.
 */
export function createStatusIcon(className: string): HTMLDivElement {
  const wrapper = document.createElement("div");
  wrapper.className = className;
  wrapper.setAttribute("aria-hidden", "true");
  wrapper.appendChild(createXmarkSvg());
  return wrapper;
}

export function createTitle(id: string, text: string): HTMLHeadingElement {
  const title = document.createElement("p");
  title.id = id;
  title.textContent = text;
  return title;
}

export function createDescription(text: string): HTMLHeadingElement {
  const desc = document.createElement("p");
  desc.textContent = text;
  return desc;
}
