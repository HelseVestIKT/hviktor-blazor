import {
  createButton,
  createCloseButton,
  createDescription,
  createStatusIcon,
  createTitle,
  ModalLocalizer,
} from "../../models/modal-utils";

function getLocalizer(): ModalLocalizer {
  return (
    globalThis?.Hviktor?.Localizer?.BlazorModal ?? {
      title: "An unhandled error has occurred",
      description: "",
      backToSafety: "Back to safety",
      reload: "Reload",
    }
  );
}

export class BlazorModal {
  readonly dialogId = "boot-error-ui";

  close(): void {
    const element = document.getElementById(
      this.dialogId,
    ) as HTMLDialogElement | null;
    if (element?.open) {
      element.close();
    }
    element?.remove();
  }

  show(): void {
    let element = document.getElementById(
      this.dialogId,
    ) as HTMLDialogElement | null;
    if (!element) {
      element = this.createDialog();
      document.body.appendChild(element);
    }
    if (!element.open) {
      element.showModal();
    }
  }

  private createDialog(): HTMLDialogElement {
    const localizer = getLocalizer();
    const dialog = document.createElement("dialog");
    dialog.id = this.dialogId;
    dialog.className = "ds-dialog";
    dialog.dataset.modal = "true";
    dialog.dataset.nosnippet = "true";
    dialog.closedBy = "none";
    dialog.setAttribute("aria-labelledby", `${this.dialogId}-title`);

    const container = document.createElement("div");
    container.className = "flex flex-col items-center text-center gap-6 mt-4";

    container.appendChild(createStatusIcon(`${this.dialogId}-failed`));
    container.appendChild(
      createCloseButton(`close-${this.dialogId}`, () => this.close()),
    );

    const content = document.createElement("div");
    content.className = `${this.dialogId}-content flex flex-col items-center text-center gap-2`;

    content.appendChild(createTitle(`${this.dialogId}-title`, localizer.title));
    content.appendChild(createDescription(localizer.description));

    const actions = document.createElement("div");
    actions.className = "flex items-center justify-center gap-2";
    actions.appendChild(
      createButton(
        `back-${this.dialogId}`,
        localizer.backToSafety ?? "Back to safety",
        () => {
          location.href = "/";
        },
      ),
    );
    actions.appendChild(
      createButton(
        `reload-${this.dialogId}`,
        localizer.reload ?? "Reload",
        () => {
          location.reload();
        },
      ),
    );
    content.appendChild(actions);

    container.appendChild(content);
    dialog.appendChild(container);
    return dialog;
  }
}
