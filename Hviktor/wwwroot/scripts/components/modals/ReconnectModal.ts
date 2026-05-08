interface ReconnectStateChangedDetail {
  state: "show" | "hide" | "failed" | "rejected";
}

class ReconnectModal {
  private static readonly StateChangedEvent =
    "components-reconnect-state-changed";
  private static readonly ModalId = "components-reconnect-modal";
  private static readonly ReconnectButtonId = "components-reconnect-button";
  private static readonly ResumeButtonId = "components-resume-button";
  private static readonly SecondsSpanId = "components-seconds-to-next-attempt";

  static maxRetries: number = 5;
  static readonly defaultRetryIntervalMs: number[] = [0, 1000, 2000, 5000];
  static readonly retriesBeforeFailure: number = 10;

  // @ts-ignore
  private static retryTimerRef: NodeJS.Timeout | null = null;
  private remainingSeconds: number = 0;

  private dialogElement: HTMLDialogElement | null = null;
  private secondsElement: HTMLSpanElement | null = null;

  constructor() {
    this.bindDomElements();
  }

  private bindDomElements(): void {
    const elementBindings: { id: string; bind: (el: HTMLElement) => void }[] = [
      {
        id: ReconnectModal.ModalId,
        bind: (el) =>
          el.addEventListener(
            ReconnectModal.StateChangedEvent,
            this.handleReconnectStateChanged,
          ),
      },
      {
        id: ReconnectModal.ReconnectButtonId,
        bind: (el) => el.addEventListener("click", () => this.retry()),
      },
      {
        id: ReconnectModal.ResumeButtonId,
        bind: (el) => el.addEventListener("click", () => this.resume()),
      },
    ];

    const pending = elementBindings.filter(({ id, bind }) => {
      const el = document.getElementById(id);
      if (el) {
        bind(el);
        return false;
      }
      return true;
    });

    if (pending.length === 0) return;

    const observer = new MutationObserver(() => {
      let allFound = true;
      for (const item of pending) {
        if (!item.id) continue;
        const el = document.getElementById(item.id);
        if (el) {
          item.bind(el);
          item.id = "";
        } else {
          allFound = false;
        }
      }
      if (allFound) observer.disconnect();
    });

    observer.observe(document.body, { childList: true, subtree: true });
  }

  private getDialog(): HTMLDialogElement {
    this.dialogElement ??= document.getElementById(
      ReconnectModal.ModalId,
    ) as HTMLDialogElement;
    return this.dialogElement;
  }

  private getSecondsSpan(): HTMLSpanElement {
    this.secondsElement ??= document.getElementById(
      ReconnectModal.SecondsSpanId,
    ) as HTMLSpanElement;
    return this.secondsElement;
  }

  private setDialogState(cssClass: string): void {
    const el = this.getDialog();
    if (el) el.classList.value = `ds-dialog ${cssClass}`;
  }

  private showModal(): void {
    this.setDialogState("components-reconnect-show");
    this.getDialog()?.showModal();
  }

  private hideModal(): void {
    this.setDialogState("components-reconnect-hide");
    this.getDialog()?.close();
  }

  private showSuccess(): void {
    this.setDialogState("components-reconnect-success");
  }

  private showRejected(): void {
    this.setDialogState("components-reconnect-rejected");
  }

  private showFailed(): void {
    this.setDialogState("components-reconnect-failed");
  }

  private showResumeFailed(): void {
    this.setDialogState("components-reconnect-resume-failed");
  }

  // Event listener now receives only the CustomEvent
  public handleReconnectStateChanged = (evt: Event): void => {
    const custom = evt as CustomEvent<ReconnectStateChangedDetail>;
    const modalEl = evt.currentTarget as HTMLDialogElement;
    const state = custom.detail?.state;
    console.debug("Reconnect state changed:", state);
    switch (state) {
      case "show":
        modalEl.showModal();
        break;
      case "hide":
        modalEl.close();
        break;
      case "failed":
        document.addEventListener(
          "visibilitychange",
          this.retryWhenDocumentBecomesVisible,
        );
        break;
      case "rejected":
        location.reload();
        break;
    }
  };

  public async onConnectionUp(): Promise<void> {
    await this.resume();
  }

  public async onConnectionDown(): Promise<void> {
    await this.retry();
  }

  private retryTimer(): void {
    if (this.remainingSeconds <= 0) {
      this.clearRetryTimer();
      return;
    }

    this.remainingSeconds--;
    this.getSecondsSpan().textContent = `${this.remainingSeconds}`;
  }

  private startTimer(seconds: number): void {
    this.clearRetryTimer();
    this.remainingSeconds = seconds;
    this.getSecondsSpan().textContent = `${this.remainingSeconds}`;
    ReconnectModal.retryTimerRef = globalThis.setInterval(
      () => this.retryTimer(),
      1000,
    );
  }

  private clearRetryTimer(): void {
    if (ReconnectModal.retryTimerRef) {
      clearInterval(ReconnectModal.retryTimerRef);
      ReconnectModal.retryTimerRef = null;
    }
  }

  public async retry(): Promise<void> {
    document.removeEventListener(
      "visibilitychange",
      this.retryWhenDocumentBecomesVisible,
    );

    console.log("Connection Down - Retrying connection...");
    for (let i = 0; i < ReconnectModal.retriesBeforeFailure; i++) {
      const delayMs: number =
        this.retryIntervalMilliseconds(
          i,
          ReconnectModal.retriesBeforeFailure,
        ) ?? 0;
      if (i > 0) {
        this.startTimer(delayMs / 1000);
      }

      console.log("Waiting for reconnect attempt #" + (i + 1) + " ...");
      await this.delay(delayMs);

      try {
        console.log("Starting Reconnect attempt #" + (i + 1) + " ...");
        this.showModal();

        const response = await this.reconnect();
        if (response === 1) {
          console.debug("Reconnected successfully.");
          await this.onSuccessfulConnection();
          return;
        } else if (response === 0) {
          console.error(
            "Server rejected the connection, could not reconnect circuit...",
          );
          this.showRejected();
          return;
        } else {
          console.error("Reconnect attempt failed...");
        }
      } catch (err: any) {
        this.clearRetryTimer();
        if (err.message === "404 Not Found") {
          await this.delay(10_000);
          this.showFailed();
        } else if (err.message === "407 Proxy Authentication Required") {
          await this.delay(2000);
          this.showResumeFailed();
        }
        document.addEventListener(
          "visibilitychange",
          this.retryWhenDocumentBecomesVisible,
        );
        console.error(err);
        return;
      }
    }

    this.showResumeFailed();
  }

  private async reconnect(): Promise<number> {
    try {
      const reconnectSuccessful = await globalThis.Blazor.reconnect?.();
      if (reconnectSuccessful) {
        return 1;
      } else {
        return 0;
      }
    } catch (err: any) {
      console.error("Reconnect failed:", err);
      if (
        err.errorType === "FailedToNegotiateWithServerError" &&
        err.message.includes("Status code '404'")
      ) {
        throw new Error("404 Not Found");
      }
      if (
        err.errorType === "FailedToNegotiateWithServerError" &&
        err.message.includes("Failed to complete negotiation with the server")
      ) {
        throw new Error("407 Proxy Authentication Required");
      }
      return -1;
    }
  }

  public async resume(): Promise<number> {
    console.debug("Attempting to resume circuit...");
    const response = await this.reconnect();
    if (response === 1) {
      console.debug("Circuit resumed successfully.");
      await this.onSuccessfulConnection();
    } else if (response === 0) {
      console.error(
        "Server rejected the connection, could not resume circuit...",
      );
      this.showRejected();
    } else {
      this.showResumeFailed();
      console.error("Resume failed...");
    }
    return response;
  }

  private retryWhenDocumentBecomesVisible = async (): Promise<void> => {
    if (document.visibilityState === "visible") {
      console.debug("Document became visible, retrying...");
      await this.retry();
    }
  };

  private delay(durationMilliseconds: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, durationMilliseconds));
  }

  private async onSuccessfulConnection(): Promise<void> {
    this.clearRetryTimer();
    console.debug("Connection is up");
    this.showSuccess();
    await this.delay(2000);
    this.hideModal();
  }

  public retryIntervalMilliseconds(
    previousAttempts: number,
    maxRetries?: number,
  ): number | null {
    if (maxRetries === undefined || previousAttempts >= maxRetries) {
      return null;
    }

    const index = Math.min(
      previousAttempts,
      ReconnectModal.defaultRetryIntervalMs.length - 1,
    );
    return ReconnectModal.defaultRetryIntervalMs[index] ?? 15000;
  }
}

export { ReconnectModal };
