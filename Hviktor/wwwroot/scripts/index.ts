import { BlazorModal } from "./components/modals/BlazorModal";
import { DotnetModal } from "./components/modals/DotnetModal";

let initialized = false;

type BlazorHostingModel = "server" | "webassembly" | "web";

function detectHostingModel(): BlazorHostingModel {
  const scripts = document.querySelectorAll('script[src*="_framework/blazor"]');
  for (const script of scripts) {
    const src = script.getAttribute("src");
    if (src === null) {
      continue;
    }

    if (src.includes("blazor.server.js")) {
      return "server";
    }

    if (src.includes("blazor.webassembly.js")) {
      return "webassembly";
    }

    if (src.includes("blazor.web.js")) {
      return "web";
    }
  }

  return "server";
}

function isAutostartDisabled(): boolean {
  const scripts = document.querySelectorAll('script[src*="_framework/blazor"]');
  for (const script of scripts) {
    if (script.getAttribute("autostart") === "false") {
      return true;
    }
  }

  return false;
}

interface ErrorModal {
  dialogId: string;

  show(): void;

  close(): void;
}

function handleMutation(
  nodes: NodeList,
  dialogs: ErrorModal[],
  callback: (dialog: ErrorModal) => void,
): void {
  for (let i = 0; i < nodes.length; i++) {
    const node = nodes.item(i);
    if (!(node instanceof HTMLElement)) {
      continue;
    }
    const dialog = dialogs.find((d) => d.dialogId === node.id);
    if (dialog) {
      callback(dialog);
    }
  }
}

function initializeErrorModals(): void {
  if (initialized) {
    return;
  }

  const dialogs: ErrorModal[] = [new BlazorModal(), new DotnetModal()];

  const observer = new MutationObserver((mutationList) => {
    for (const mutation of mutationList) {
      if (mutation.type !== "childList") {
        continue;
      }
      handleMutation(mutation.addedNodes, dialogs, (dialog) => dialog.show());
      handleMutation(mutation.removedNodes, dialogs, (dialog) =>
        dialog.close(),
      );
    }
  });

  observer.observe(document.body, { childList: true, subtree: true });
  initialized = true;
}

async function startBlazorServer(): Promise<void> {
  const rmInstance = await import("./components/modals/ReconnectModal");
  const reconnect = new rmInstance.ReconnectModal();
  // noinspection JSUnusedGlobalSymbols (configureSignalR)
  await globalThis.Blazor.start?.({
    circuit: {
      configureSignalR: (_: any) => {
        // Default configuration
      },
      logLevel: 3,
      initializers: undefined!,
      circuitHandlers: [],
      reconnectionOptions: {
        dialogId: "components-reconnect-modal",
        maxRetries: rmInstance.ReconnectModal.maxRetries,
        retryIntervalMilliseconds: (
          previousAttempts: number,
          maxRetries?: number | undefined,
        ) => reconnect.retryIntervalMilliseconds(previousAttempts, maxRetries),
      },
      reconnectionHandler: {
        onConnectionUp: () => reconnect.onConnectionUp(),
        onConnectionDown: (evt: Partial<any>) => reconnect.onConnectionDown(),
      },
    },
  });
}

async function startBlazorWebAssembly(): Promise<void> {
  await globalThis.Blazor.start?.();
}

async function startBlazor(): Promise<void> {
  // When autostart is not disabled, Blazor starts itself before this
  // script runs. Calling start() again would throw, so skip it.
  if (!isAutostartDisabled()) {
    return;
  }

  const hostingModel = detectHostingModel();
  try {
    switch (hostingModel) {
      case "server":
      case "web":
        await startBlazorServer();
        break;
      case "webassembly":
        await startBlazorWebAssembly();
        break;
    }
  } catch (error: unknown) {
    if (error instanceof Error && error.message?.includes("already started")) {
      return;
    }
    throw error;
  }
}

function initialize(): void {
  globalThis.Hviktor = globalThis.Hviktor || {};
  globalThis.Hviktor.Localizer = globalThis.Hviktor.Localizer || {};

  // Error modals are initialized synchronously, so they work
  // even if Blazor never loads or crashes during boot.
  initializeErrorModals();

  // Blazor startup is async, but failures are non-fatal for error modals.
  const waitForBlazor = (): void => {
    if (globalThis.Blazor) {
      startBlazor().catch((error) => {
        console.error("Error starting Blazor:", error);
      });
    } else {
      setTimeout(waitForBlazor, 100);
    }
  };

  waitForBlazor();
}

// Initialize as early as possible
if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initialize);
} else {
  initialize();
}
