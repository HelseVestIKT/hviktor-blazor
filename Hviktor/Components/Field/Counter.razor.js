export class Counter {
  static inputListeners = new WeakMap();

  static initialize(fieldElementRef, dotNetRef) {
    if (!fieldElementRef) {
      return null;
    }

    const inputElement = fieldElementRef.querySelector("input, textarea");
    if (!inputElement) {
      console.warn(
        "Counter: Could not find input or textarea element within field",
      );
      return null;
    }

    const inputId = inputElement.id;
    if (!inputId) {
      console.warn("Counter: Input element has no ID");
      return null;
    }

    const descriptionElement = fieldElementRef.querySelector(
      '[data-field="description"]',
    );
    if (descriptionElement && descriptionElement.id) {
      const existingDescribedBy = inputElement.getAttribute("aria-describedby");
      if (existingDescribedBy) {
        if (!existingDescribedBy.includes(descriptionElement.id)) {
          inputElement.setAttribute(
            "aria-describedby",
            `${existingDescribedBy} ${descriptionElement.id}`,
          );
        }
      } else {
        inputElement.setAttribute("aria-describedby", descriptionElement.id);
      }
    }

    const handleInput = () => {
      const length = inputElement.value?.length ?? 0;
      dotNetRef.invokeMethodAsync("OnInputValueChanged", length);
    };

    this.inputListeners.set(fieldElementRef, { inputElement, handleInput });
    inputElement.addEventListener("input", handleInput);
    handleInput();
    return inputId;
  }

  static dispose(fieldElementRef) {
    if (!fieldElementRef) {
      return;
    }

    const listenerData = this.inputListeners.get(fieldElementRef);
    if (listenerData) {
      const { inputElement, handleInput } = listenerData;
      inputElement.removeEventListener("input", handleInput);
      this.inputListeners.delete(fieldElementRef);
    }
  }
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Field = globalThis.Hviktor.Field || {};
globalThis.Hviktor.Field.Counter = globalThis.Hviktor.Field.Counter || {};
globalThis.Hviktor.Field.Counter.initialize = Counter.initialize;
globalThis.Hviktor.Field.Counter.dispose = Counter.dispose;
