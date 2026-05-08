export class Dialog {
  /** @type {WeakMap<HTMLDialogElement, HTMLElement|null>} */
  static #triggerMap = new WeakMap();

  static open(ref) {
    // Remember the element that had focus before opening so we can restore it on close.
    Dialog.#triggerMap.set(ref, document.activeElement);
    ref.showModal();
  }

  static close(ref) {
    const trigger = Dialog.#triggerMap.get(ref);
    ref.close();

    // Restore focus to the element that opened the dialog (WCAG 2.4.3 Focus Order).
    if (trigger && typeof trigger.focus === "function") {
      // Use requestAnimationFrame to let the browser finish closing the dialog first.
      requestAnimationFrame(() => trigger.focus());
    }
  }
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Dialog = globalThis.Hviktor.Dialog || {};
globalThis.Hviktor.Dialog.open = Dialog.open;
globalThis.Hviktor.Dialog.close = Dialog.close;
