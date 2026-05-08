// Ref: https://github.com/digdir/designsystemet/blob/main/packages/react/src/components/search/search-clear.tsx

interface DotNetObjectReference {
  invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
}

const eventHandlers = new Map<
  string,
  {
    onclick?: (e: MouseEvent) => void;
    onkeydown?: (e: KeyboardEvent) => void;
  }
>();

export async function initializeSearch(
  elementId: string,
  dotNetRef: DotNetObjectReference,
): Promise<void> {
  // Remove existing handlers if any
  disposeSearch(elementId);

  const onclick = async (e: MouseEvent) => {
    const target = e.target;
    let input: HTMLElement | null | undefined = null;

    if (target instanceof HTMLElement) {
      input = target.closest(".ds-search")?.querySelector("input");
    }

    if (!input) {
      throw new Error("Input is missing");
    }

    /* narrow type to make TS happy */
    if (!(input instanceof HTMLInputElement)) {
      throw new TypeError("Input is not an input element");
    }

    e.preventDefault();
    input.value = "";
    input.focus();
  };

  // Escape key clears the input — mirrors native <input type="search"> behaviour
  // and ensures consistent cross-browser clearing (WCAG: predictable interaction).
  const onkeydown = (e: KeyboardEvent) => {
    if (e.code !== "Escape") {
      return;
    }
    const searchRoot = (e.target as HTMLElement | null)?.closest(".ds-search");
    const input = searchRoot?.querySelector("input");
    if (input instanceof HTMLInputElement && input.value !== "") {
      e.preventDefault();
      e.stopPropagation();
      input.value = "";
      input.focus();
    }
  };

  const clearElement = document.getElementById(elementId + "-clear");
  if (clearElement) {
    clearElement.addEventListener("click", onclick);
  }

  const searchElement = document.getElementById(elementId);
  if (searchElement) {
    searchElement.addEventListener("keydown", onkeydown);
  }

  eventHandlers.set(elementId, { onclick, onkeydown });
}

export function disposeSearch(elementId: string): void {
  const handlers = eventHandlers.get(elementId);

  if (handlers) {
    const clearElement = document.getElementById(elementId + "-clear");
    if (clearElement && handlers.onclick) {
      clearElement.removeEventListener("click", handlers.onclick);
    }

    const searchElement = document.getElementById(elementId);
    if (searchElement && handlers.onkeydown) {
      searchElement.removeEventListener("keydown", handlers.onkeydown);
    }
  }

  eventHandlers.delete(elementId);
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Search = globalThis.Hviktor.Search || {};
globalThis.Hviktor.Search.initializeSearch = initializeSearch;
globalThis.Hviktor.Search.disposeSearch = disposeSearch;
