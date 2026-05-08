export class Checkbox {
  static setIndeterminateState(ref, state) {
    ref.indeterminate = state;
  }
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Checkbox = globalThis.Hviktor.Checkbox || {};
globalThis.Hviktor.Checkbox.setIndeterminateState =
  Checkbox.setIndeterminateState;
