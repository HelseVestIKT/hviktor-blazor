import type { RuntimeAPI } from "./dotnet.d.ts";
import type {
  DomFunctions,
  InputFile,
  Virtualize,
  PageTitle,
  NavigationLock,
  AttachWebRendererInterop,
  INavigationManager,
  NavigationOptions,
  AddEventListener,
  RemoveEventListener,
  EventTypeOptions,
  IHubConnection,
  CircuitStartOptions,
  DefaultReconnectionHandler,
  WebStartOptions,
  WebAssemblyStartOptions,
  Platform,
  Pointer,
  RootComponentsFunctions,
  IReconnectStateChangedEvent,
} from "./web-js.d.ts";

interface IBlazor {
  navigateTo: (uri: string, options: NavigationOptions) => void;
  registerCustomEventType: (
    eventName: string,
    options: EventTypeOptions,
  ) => void;

  addEventListener?: AddEventListener;
  removeEventListener?: RemoveEventListener;
  disconnect?: () => void;
  reconnect?: (existingConnection?: IHubConnection) => Promise<boolean>;
  pauseCircuit?: () => Promise<boolean>;
  resumeCircuit?: () => Promise<boolean>;
  defaultReconnectionHandler?: DefaultReconnectionHandler;
  start?:
    | ((userOptions?: Partial<CircuitStartOptions>) => Promise<void>)
    | ((options?: Partial<WebAssemblyStartOptions>) => Promise<void>)
    | ((options?: Partial<WebStartOptions>) => Promise<void>);
  platform?: Platform;
  rootComponents: RootComponentsFunctions;
  runtime: RuntimeAPI;

  _internal: {
    navigationManager: INavigationManager | any;
    domWrapper: DomFunctions;
    Virtualize: Virtualize;
    PageTitle: PageTitle;
    forceCloseConnection?: () => Promise<void>;
    InputFile?: InputFile;
    NavigationLock: NavigationLock;
    invokeJSJson?: (
      identifier: string,
      targetInstanceId: number,
      resultType: number,
      argsJson: string,
      asyncHandle: number,
      callType: number,
    ) => string | null;
    endInvokeDotNetFromJS?: (
      callId: string,
      success: boolean,
      resultJsonOrErrorMessage: string,
    ) => void;
    receiveByteArray?: (id: number, data: Uint8Array) => void;
    getPersistedState?: () => string;
    getInitialComponentsUpdate?: () => Promise<string>;
    updateRootComponents?: (
      operations: string,
      webAssemblyState: string,
    ) => void;
    endUpdateRootComponents?: (batchId: number) => void;
    attachRootComponentToElement?: (
      arg0: any,
      arg1: any,
      arg2: any,
      arg3: any,
    ) => void;
    registeredComponents?: {
      getRegisteredComponentsCount: () => number;
      getAssembly: (id) => string;
      getTypeName: (id) => string;
      getParameterDefinitions: (id) => string;
      getParameterValues: (id) => any;
    };
    renderBatch?: (browserRendererId: number, batchAddress: Pointer) => void;
    getConfig?: (fileName: string) => Uint8Array | undefined;
    getApplicationEnvironment?: () => string;
    dotNetCriticalError?: any;
    loadLazyAssembly?: any;
    loadSatelliteAssemblies?: any;
    sendJSDataStream?: (data: any, streamId: number, chunkSize: number) => void;
    getJSDataStreamChunk?: (
      data: any,
      position: number,
      chunkSize: number,
    ) => Promise<Uint8Array>;
    receiveWebAssemblyDotNetDataStream?: (
      streamId: number,
      data: any,
      bytesRead: number,
      errorMessage: string,
    ) => void;
    receiveWebViewDotNetDataStream?: (
      streamId: number,
      data: any,
      bytesRead: number,
      errorMessage: string,
    ) => void;
    attachWebRendererInterop?: AttachWebRendererInterop;

    // JSExport APIs
    dotNetExports?: {
      InvokeDotNet: (
        assemblyName: string | null,
        methodIdentifier: string,
        dotNetObjectId: number,
        argsJson: string,
      ) => string | null;
      EndInvokeJS: (argsJson: string) => void;
      BeginInvokeDotNet: (
        callId: string | null,
        assemblyNameOrDotNetObjectId: string,
        methodIdentifier: string,
        argsJson: string,
      ) => void;
      ReceiveByteArrayFromJS: (id: number, data: Uint8Array) => void;
      UpdateRootComponentsCore: (
        operationsJson: string,
        appState: string,
      ) => void;
    };

    // APIs invoked by hot reload
    hotReloadApplied?: () => void;
  };
}

export { type IBlazor, type IReconnectStateChangedEvent };
