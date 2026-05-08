/**
 * CodeBlock component script for syntax highlighting using highlight.js
 * This script provides Blazor interop for the CodeBlock component.
 *
 * Usage per highlight.js documentation:
 * const highlightedCode = hljs.highlight(code, { language: 'xml' }).value
 */

declare const hljs: {
  highlightElement: (element: HTMLElement) => void;
  highlight: (
    code: string,
    options: { language: string; ignoreIllegals?: boolean },
  ) => { value: string };
  highlightAuto: (code: string) => { value: string; language?: string };
  getLanguage: (name: string) => object | undefined;
  listLanguages: () => string[];
};

export class CodeBlock {
  /**
   * Highlights code and returns the HTML string.
   * This is the primary method for Blazor interop.
   *
   * @param code The source code to highlight
   * @param language The programming language (e.g., 'csharp', 'javascript', 'xml')
   * @returns The highlighted HTML string
   *
   * @example
   * const html = CodeBlock.highlight('<div>Hello</div>', 'xml');
   */
  // TOOD: return : {value: string, language?: string} instead of just value:string
  static highlight(code: string, language: string): string {
    if (typeof hljs === "undefined") {
      console.error("CodeBlock: highlight.js is not loaded");
      return escapeHtml(code);
    }

    if (!code) {
      return "";
    }

    try {
      // Check if language is registered and not plaintext
      if (language && language !== "plaintext" && hljs.getLanguage(language)) {
        // Use hljs.highlight() per documentation
        const result = hljs.highlight(code, {
          language: language,
          ignoreIllegals: true,
        });
        return result.value;
      } else if (language === "plaintext" || !language) {
        // For plaintext, just escape HTML
        return escapeHtml(code);
      } else {
        // Language not found, try auto-detection
        console.warn(
          `CodeBlock: Language '${language}' not registered. Trying auto-detection.`,
        );
        const result = hljs.highlightAuto(code);
        return result.value;
      }
    } catch (error) {
      console.error("CodeBlock.highlight: Failed to highlight code", error);
      return escapeHtml(code);
    }
  }

  /**
   * Highlights a code element in place using highlight.js.
   * Use this for DOM-based highlighting after render.
   *
   * @param element The code element to highlight
   */
  static highlightElement(element: HTMLElement): void {
    if (!element) {
      console.warn("CodeBlock.highlightElement: No element provided");
      return;
    }

    if (typeof hljs === "undefined") {
      console.error("CodeBlock: highlight.js is not loaded");
      return;
    }

    try {
      hljs.highlightElement(element);
    } catch (error) {
      console.error(
        "CodeBlock.highlightElement: Failed to highlight element",
        error,
      );
    }
  }

  /**
   * Highlights all code blocks on the page that haven't been highlighted yet.
   */
  static highlightAll(): void {
    if (typeof hljs === "undefined") {
      console.error("CodeBlock: highlight.js is not loaded");
      return;
    }

    const codeBlocks = document.querySelectorAll<HTMLElement>(
      "pre code:not(.hljs)",
    );
    codeBlocks.forEach((block) => {
      hljs.highlightElement(block);
    });
  }

  /**
   * Checks if a language is supported by highlight.js.
   * @param language The language to check
   * @returns True if the language is supported
   */
  static isLanguageSupported(language: string): boolean {
    if (typeof hljs === "undefined") {
      return false;
    }
    return hljs.getLanguage(language) !== undefined;
  }

  /**
   * Gets a list of all registered languages.
   * @returns Array of language names
   */
  static getRegisteredLanguages(): string[] {
    if (typeof hljs === "undefined") {
      return [];
    }
    return hljs.listLanguages();
  }

  /**
   * Gets the appropriate theme class based on the nearest parent's data-color-scheme attribute.
   * Returns 'theme-github-dark' for dark scheme, 'theme-github' for light scheme.
   *
   * @param element The element to start searching from
   * @returns The theme class name ('theme-github' or 'theme-github-dark')
   */
  static getThemeClass(element: HTMLElement): string {
    if (!element) {
      console.warn(
        "CodeBlock.getThemeClass: No element provided, defaulting to light theme",
      );
      return "theme-github";
    }

    // Find the nearest parent with data-color-scheme attribute
    const schemeElement = element.closest(
      "[data-color-scheme]",
    ) as HTMLElement | null;

    if (schemeElement) {
      const scheme = schemeElement.getAttribute("data-color-scheme");

      if (scheme === "dark") {
        return "theme-github-dark";
      } else if (scheme === "light") {
        return "theme-github";
      }
      // For 'auto' or other values, check media query
      if (
        scheme === "auto" &&
        window.matchMedia &&
        window.matchMedia("(prefers-color-scheme: dark)").matches
      ) {
        return "theme-github-dark";
      }
    } else {
      console.debug(
        "CodeBlock.getThemeClass: No data-color-scheme found on ancestors",
      );
    }

    // Default to light theme (no automatic system preference detection unless data-color-scheme="auto")
    return "theme-github";
  }

  /**
   * Applies the correct theme class to a codeblock element based on color scheme.
   * Removes any existing theme class and adds the appropriate one.
   *
   * @param element The codeblock container element
   */
  static applyTheme(element: HTMLElement): void {
    if (!element) {
      console.warn("CodeBlock.applyTheme: No element provided");
      return;
    }

    // Remove existing theme classes
    element.classList.remove("theme-github", "theme-github-dark");

    // Add the appropriate theme class
    const themeClass = CodeBlock.getThemeClass(element);
    element.classList.add(themeClass);
  }

  /**
   * Initializes a codeblock element - applies theme and sets up observers.
   * Call this from Blazor after the component renders.
   *
   * @param element The codeblock container element
   */
  static initialize(element: HTMLElement): void {
    if (!element) {
      console.warn("CodeBlock.initialize: No element provided");
      return;
    }

    // Apply initial theme
    CodeBlock.applyTheme(element);

    // Set up a MutationObserver to watch for color-scheme changes on ancestors
    const observer = new MutationObserver(() => {
      CodeBlock.applyTheme(element);
    });

    // Find the nearest ancestor with data-color-scheme and observe it
    const schemeElement = element.closest(
      "[data-color-scheme]",
    ) as HTMLElement | null;
    if (schemeElement) {
      observer.observe(schemeElement, {
        attributes: true,
        attributeFilter: ["data-color-scheme"],
      });
    }

    // Also observe the document body for scheme changes
    observer.observe(document.body, {
      attributes: true,
      attributeFilter: ["data-color-scheme"],
    });

    // Listen for system color scheme changes
    if (window.matchMedia) {
      window
        .matchMedia("(prefers-color-scheme: dark)")
        .addEventListener("change", () => {
          CodeBlock.applyTheme(element);
        });
    }
  }
}

/**
 * Escapes HTML special characters for safe display.
 */
function escapeHtml(text: string): string {
  const div = document.createElement("div");
  div.textContent = text;
  return div.innerHTML;
}

// Attach to global Hviktor namespace for Blazor interop
(globalThis as any).Hviktor = (globalThis as any).Hviktor || {};
(globalThis as any).Hviktor.CodeBlock = CodeBlock;
