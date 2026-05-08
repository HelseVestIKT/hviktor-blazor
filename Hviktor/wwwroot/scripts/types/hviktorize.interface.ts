export interface IHviktor {
  Storage: {
    getCookie(key: string): string | null;
    setCookie(key: string, value: string): void;
  };
  Search: {
    initializeSearch(elementId: string, dotNetRef: any): void;
    disposeSearch(elementId: string): void;
  };
  Suggestion: {
    initializeCombobox(elementId: string, dotNetRef: any): void;
    disposeCombobox(elementId: string): void;
  };
  Trigger: {
    initialize(buttonRef: HTMLButtonElement, arrowOffset: number): void;
    dispose(buttonRef: HTMLButtonElement): void;
  };
  Field: {
    Counter: {
      initialize(fieldElementRef: HTMLDivElement, dotNetRef: any): void;
      dispose(fieldElementRef: HTMLDivElement): void;
    };
  };
  Dialog: {
    open(ref: HTMLElement): void;
    close(ref: HTMLElement): void;
  };
  ToggleGroup: {
    initialize(elementRef: HTMLElement): void;
    dispose(): void;
  };
  Tooltip: {
    register(anchorId: string, content: string, placement: string): void;
    unregister(anchorId: string): void;
  };
  Checkbox: {
    setIndeterminateState(ref: HTMLElement, state: boolean): void;
  };
}
