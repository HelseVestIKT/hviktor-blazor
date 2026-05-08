/**
 * Initializes positioning for static see-ref popovers emitted by ComponentMetadataService.
 * Finds all popover elements with IDs starting with "see-ref-" that haven't been
 * initialized yet and calls the shared Hviktor.Popover.initialize() on each.
 */
// noinspection JSUnusedGlobalSymbols
export function initializeSeeRefPopovers() {
  const popovers = document.querySelectorAll('.ds-popover[id^="see-ref-"]');
  for (const popover of popovers) {
    if (
      !popover.__seeRefInitialized &&
      globalThis.Hviktor?.Popover?.initialize
    ) {
      globalThis.Hviktor.Popover.initialize(popover);
      popover.__seeRefInitialized = true;
    }
  }
}
