import {
  createDescription,
  createStatusIcon,
  createTitle,
  ModalLocalizer,
} from "../../models/modal-utils";

function getLocalizer(): ModalLocalizer {
  return (
    globalThis?.Hviktor?.Localizer?.DotnetModal ?? {
      title: "An error occurred during the hot-reload project build.",
      description: "Please refer to the IDE for more details.",
    }
  );
}

export class DotnetModal {
  readonly dialogId = "dotnet-compile-error-ui";

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
    dialog.dataset.placement = "top";
    dialog.closedBy = "none";
    dialog.setAttribute("aria-labelledby", `${this.dialogId}-title`);

    const container = document.createElement("div");
    container.className = "flex flex-row items-center gap-6";

    container.appendChild(createStatusIcon(`${this.dialogId}-failed`));

    const content = document.createElement("div");
    content.className = `${this.dialogId}-content flex flex-col gap-1`;

    content.appendChild(createTitle(`${this.dialogId}-title`, localizer.title));
    content.appendChild(createDescription(localizer.description));

    container.appendChild(content);
    dialog.appendChild(container);
    return dialog;
  }
}
