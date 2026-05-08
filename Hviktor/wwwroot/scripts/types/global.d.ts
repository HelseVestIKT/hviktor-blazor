import type { IBlazor } from "./blazor";
import type { IHviktor } from "./hviktor.interface";

declare global {
  interface Window {
    Blazor: IBlazor;
    Hviktor: IHviktor;
  }
}
