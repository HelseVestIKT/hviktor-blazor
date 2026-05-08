import { DotNet } from "@microsoft/dotnet-js-interop";
import type { IBlazor } from "./blazor.d.ts";
import type { MonoObject, MonoArray } from "./dotnet-legacy.d.ts";
import type {
  MonoConfig,
  DotnetHostBuilder,
  AssetBehaviors,
} from "./dotnet.d.ts";

interface IStatefulReconnectOptions {
  bufferSize: number;
}

declare enum HttpTransportType {
  None = 0,
  WebSockets = 1,
  ServerSentEvents = 2,
  LongPolling = 4,
}

interface HttpRequest {
  method?: string;
  url?: string;
  content?: string | ArrayBuffer;
  headers?: MessageHeaders;
  responseType?: XMLHttpRequestResponseType;
  abortSignal?: AbortSignal;
  timeout?: number;
  withCredentials?: boolean;
}

type HttpResponse = {
  new (): HttpResponse;
  statusCode: number;
  statusText?: string;
  content?: string | ArrayBuffer;
};

type HttpClient = {
  get(url: string): Promise<HttpResponse>;
  get(url: string, options: HttpRequest): Promise<HttpResponse>;
  get(url: string, options?: HttpRequest): Promise<HttpResponse>;
  post(url: string): Promise<HttpResponse>;
  post(url: string, options: HttpRequest): Promise<HttpResponse>;
  post(url: string, options?: HttpRequest): Promise<HttpResponse>;
  delete(url: string): Promise<HttpResponse>;
  delete(url: string, options: HttpRequest): Promise<HttpResponse>;
  delete(url: string, options?: HttpRequest): Promise<HttpResponse>;
  send(request: HttpRequest): Promise<HttpResponse>;
  getCookieString(url: string): string;
};

type EventSourceConstructor = new (
  url: string,
  eventSourceInitDict?: EventSourceInit,
) => EventSource;

interface WebSocketConstructor {
  new (url: string, protocols?: string | string[], options?: any): WebSocket;

  readonly CLOSED: number;
  readonly CLOSING: number;
  readonly CONNECTING: number;
  readonly OPEN: number;
}

interface MessageHeaders {
  [key: string]: string;
}

interface IHttpConnectionOptions {
  headers?: MessageHeaders;
  httpClient?: HttpClient;
  transport?: HttpTransportType | ITransport;
  logger?: ILogger | LogLevel;

  accessTokenFactory?(): string | Promise<string>;

  logMessageContent?: boolean;
  skipNegotiation?: boolean;
  WebSocket?: WebSocketConstructor;
  EventSource?: EventSourceConstructor;
  withCredentials?: boolean;
  timeout?: number;
  _useStatefulReconnect?: boolean;
}

interface InvocationMessage {
  readonly type: unknown;
  readonly target: string;
  readonly arguments: any[];
  readonly streamIds?: string[];
}

/** Union type of all known Hub messages. */
type HubMessage = unknown | InvocationMessage;

interface ILogger {
  log(logLevel: LogLevel, message: string, ...args: any[]): void;

  log(logLevel: LogLevel, message: string): void;

  log(logLevel: LogLevel, message: string, error: Error): void;

  log(logLevel: LogLevel, message: string, error?: Error, ...args: any[]): void;

  log(logLevel: LogLevel, error: Error): void;

  log(logLevel: LogLevel, error: Error, ...args: any[]): void;

  log(logLevel: LogLevel, message: string, error?: Error | undefined): void;
}

interface IRetryPolicy {
  nextRetryDelayInMilliseconds(
    previousRetryCount: number,
    elapsedMilliseconds: number,
  ): number | undefined | null;
}

declare enum TransferFormat {
  Text = 1,
  Binary = 2,
}

interface ITransport {
  connect(url: string, transferFormat: TransferFormat): Promise<void>;

  send(data: any): Promise<void>;

  stop(): Promise<void>;

  onreceive: ((data: string | ArrayBuffer) => void) | null;
  onclose: ((error?: Error) => void) | null;
}

interface IHubProtocol {
  readonly name: string;
  readonly version: number;
  readonly transferFormat: TransferFormat;

  parseMessages(input: string | ArrayBuffer, logger: ILogger): HubMessage[];

  writeMessage(message: HubMessage): string | ArrayBuffer;
}

type HubConnectionBuilder = {
  protocol: IHubProtocol;
  httpConnectionOptions: IHttpConnectionOptions;
  url: string;
  logger: ILogger;
  reconnectPolicy?: IRetryPolicy;
  configureLogging(logging: LogLevel | string | ILogger): HubConnectionBuilder;
  configureLogging(logger: ILogger): HubConnectionBuilder;
  configureLogging(logLevel: string): HubConnectionBuilder;
  configureLogging(logging: LogLevel | string | ILogger): HubConnectionBuilder;
  withUrl(url: string): HubConnectionBuilder;
  withUrl(url: string, transportType: HttpTransportType): HubConnectionBuilder;
  withUrl(url: string, options: IHttpConnectionOptions): HubConnectionBuilder;
  withHubProtocol(protocol: IHubProtocol): HubConnectionBuilder;
  withAutomaticReconnect(): HubConnectionBuilder;
  withAutomaticReconnect(retryDelays: number[]): HubConnectionBuilder;
  withAutomaticReconnect(reconnectPolicy: IRetryPolicy): HubConnectionBuilder;
  withServerTimeout(milliseconds: number): HubConnectionBuilder;
  withKeepAliveInterval(milliseconds: number): HubConnectionBuilder;
  withStatefulReconnect(
    options?: IStatefulReconnectOptions,
  ): HubConnectionBuilder;
  build(): IHubConnection;
};

type BeforeBlazorServerStartedCallback = (
  options: Partial<CircuitStartOptions>,
) => Promise<void>;
type AfterBlazorServerStartedCallback = (blazor: IBlazor) => Promise<void>;

type ServerInitializers = {
  beforeStart: BeforeBlazorServerStartedCallback[];
  afterStarted: AfterBlazorServerStartedCallback[];
};

interface CircuitStartOptions {
  configureSignalR: (builder: HubConnectionBuilder) => void;
  logLevel: LogLevel;
  reconnectionOptions: ReconnectionOptions;
  reconnectionHandler?: ReconnectionHandler;
  initializers: ServerInitializers;
  circuitHandlers: CircuitHandler[];
}

interface ReconnectionOptions {
  maxRetries?: number;
  retryIntervalMilliseconds:
    | number
    | ((
        previousAttempts: number,
        maxRetries?: number,
      ) => number | undefined | null);
  dialogId: string;
}

interface CircuitHandler {
  onCircuitOpened?: () => void;
  onCircuitClosed?: () => void;
}

interface ReconnectionHandler {
  onConnectionDown(
    options: ReconnectionOptions,
    error?: Error,
    isClientPause?: boolean,
    remotePause?: boolean,
  ): void;

  onConnectionUp(): void;
}

interface ReconnectionHandler {
  onConnectionDown(
    options: ReconnectionOptions,
    error?: Error,
    isClientPause?: boolean,
    remotePause?: boolean,
  ): void;

  onConnectionUp(): void;
}

interface ReconnectDisplay {
  show(): void;

  hide(): void;

  failed(
    currentAttempt: number,
    maxAttempts: number | undefined,
    error?: Error,
  ): void;

  rejected(error?: Error): void;

  update(
    currentAttempt: number,
    maxAttempts: number | undefined,
    secondsToNextAttempt: number,
  ): void;

  paused(remote?: boolean): void;

  resumeFailed(error?: Error): void;
}

declare class DefaultReconnectionHandler implements ReconnectionHandler {
  constructor();

  onConnectionDown(
    options: ReconnectionOptions,
    error?: Error,
    isClientPause?: boolean,
    remotePause?: boolean,
  ): void;

  onConnectionUp(): void;

  // Protected/internal methods
  readonly _reconnectDisplay: ReconnectDisplay;

  _connectionDown(
    options: ReconnectionOptions,
    error?: Error,
    isClientPause?: boolean,
    remotePause?: boolean,
  ): void;

  _connectionUp(): void;

  _getRetryDelayInMilliseconds(
    attempt: number,
    maxAttempts?: number,
  ): number | null;
}

interface IReconnectStateChangedEvent {
  state:
    | "show"
    | "hide"
    | "retrying"
    | "failed"
    | "resume-failed"
    | "paused"
    | "rejected";
  currentAttempt?: number;
  secondsToNextAttempt?: number;
  remote?: boolean;
}

interface EventTypeOptions {
  browserEventName?: string;
  createEventArgs?: (event: Event) => unknown;
}

interface IHubConnection {
  readonly connectionId?: string;
  readonly baseUrl: string;
  readonly state:
    | "Disconnected"
    | "Connecting"
    | "Connected"
    | "Disconnecting"
    | "Reconnecting";
  serverTimeoutInMilliseconds: number;
  keepAliveIntervalInMilliseconds: number;

  start(): Promise<void>;

  stop(): Promise<void>;

  on(methodName: string, newMethod: (...args: any[]) => void): void;

  off(methodName: string, method?: (...args: any[]) => any): void;

  invoke<T = any>(methodName: string, ...args: any[]): Promise<T>;

  send(methodName: string, ...args: any[]): Promise<void>;

  onclose(callback: (error?: Error) => void): void;

  onreconnecting(callback: (error?: Error) => void): void;

  onreconnected(callback: (connectionId?: string) => void): void;
}

interface BlazorEvent {
  type: keyof BlazorEventMap;
}

interface BlazorEventMap {
  enhancedload: BlazorEvent;
  enhancednavigationstart: BlazorEvent;
  enhancednavigationend: BlazorEvent;
}

type AddEventListener = <K extends keyof BlazorEventMap>(
  type: K,
  listener: (ev: BlazorEventMap[K]) => void,
) => void;
type RemoveEventListener = <K extends keyof BlazorEventMap>(
  type: K,
  listener: (ev: BlazorEventMap[K]) => void,
) => void;
type ComponentParameters = object | null | undefined;

type RootComponentsFunctions = {
  add: (
    toElement: Element,
    componentIdentifier: string,
    initialParameters: ComponentParameters,
  ) => Promise<DynamicRootComponent>;
};

type DynamicRootComponent = {
  setParameters: (parameters: ComponentParameters) => Promise<void>;
  dispose: () => Promise<void>;
};

interface NavigationOptions {
  forceLoad: boolean;
  replaceHistoryEntry: boolean;
  historyEntryState?: string;
}

type ListenForNavigationEvents = (
  rendererId: WebRendererId,
  locationChangedCallback: (
    uri: string,
    state: string | undefined,
    intercepted: boolean,
  ) => Promise<void>,
  locationChangingCallback: (
    callId: number,
    uri: string,
    state: string | undefined,
    intercepted: boolean,
  ) => Promise<void>,
) => void;

type SetHasInteractiveRouter = (rendererId: WebRendererId) => void;
type SetHasLocationChangingListeners = (
  rendererId: WebRendererId,
  hasListeners: boolean,
) => void;
type EndLocationChanging = (
  callId: number,
  shouldContinueNavigation: boolean,
) => void;
type NavigateToFromDotNet = (uri: string, options: NavigationOptions) => void;
type Refresh = (forceReload: boolean) => void;
type GetBaseURI = () => string;
type GetLocationHref = () => string;
type ScrollToElement = (identifier: string) => void;

interface INavigationManager {
  listenForNavigationEvents: ListenForNavigationEvents;
  enableNavigationInterception: SetHasInteractiveRouter;
  setHasLocationChangingListeners: SetHasLocationChangingListeners;
  endLocationChanging: EndLocationChanging;
  navigateTo: NavigateToFromDotNet;
  refresh: Refresh;
  getBaseURI: GetBaseURI;
  getLocationHref: GetLocationHref;
  scrollToElement: ScrollToElement;
}

interface Platform {
  load(
    options: Partial<WebAssemblyStartOptions>,
    onConfigLoaded?: (loadedConfig: MonoConfig) => void,
  ): Promise<void>;

  start(): Promise<PlatformApi>;

  callEntryPoint(): Promise<unknown>;

  getArrayEntryPtr<TPtr extends Pointer>(
    array: System_Array<TPtr>,
    index: number,
    itemSize: number,
  ): TPtr;

  getObjectFieldsBaseAddress(referenceTypedObject: System_Object): Pointer;

  readInt16Field(baseAddress: Pointer, fieldOffset?: number): number;

  readInt32Field(baseAddress: Pointer, fieldOffset?: number): number;

  readUint64Field(baseAddress: Pointer, fieldOffset?: number): number;

  readObjectField<T extends System_Object>(
    baseAddress: Pointer,
    fieldOffset?: number,
  ): T;

  readStringField(
    baseAddress: Pointer,
    fieldOffset?: number,
    readBoolValueAsString?: boolean,
  ): string | null;

  readStructField<T extends Pointer>(
    baseAddress: Pointer,
    fieldOffset?: number,
  ): T;

  beginHeapLock(): HeapLock;

  invokeWhenHeapUnlocked(callback: Function): void;
}

type PlatformApi = {
  invokeLibraryInitializers(
    functionName: string,
    args: unknown[],
  ): Promise<void>;
};

interface HeapLock {
  release();
}

type System_Object = MonoObject;

interface System_Array<T> extends System_Object, MonoArray {}

interface Pointer {
  Pointer__DO_NOT_IMPLEMENT: unknown;
}

declare enum WebRendererId {
  Default = 0,
  Server = 1,
  WebAssembly = 2,
  WebView = 3,
}

export enum LogLevel {
  Trace = 0,
  Debug = 1,
  Information = 2,
  Warning = 3,
  Error = 4,
  Critical = 5,
  None = 6,
}

interface SsrStartOptions {
  disableDomPreservation?: boolean;
  circuitInactivityTimeoutMs?: number;
}

interface WebStartOptions {
  enableClassicInitializers?: boolean;
  circuit: CircuitStartOptions;
  webAssembly: WebAssemblyStartOptions;
  logLevel?: LogLevel;
  ssr: SsrStartOptions;
}

interface WebAssemblyStartOptions {
  loadBootResource(
    type: WebAssemblyBootResourceType,
    name: string,
    defaultUri: string,
    integrity: string,
    behavior: AssetBehaviors,
  ): string | Promise<Response> | null | undefined;

  environment?: string;
  applicationCulture?: string;
  initializers?: WebAssemblyInitializers;

  configureRuntime(builder: DotnetHostBuilder): void;
}

type WebAssemblyBootResourceType =
  | "assembly"
  | "pdb"
  | "dotnetjs"
  | "dotnetwasm"
  | "globalization"
  | "manifest"
  | "configuration";

type BeforeBlazorWebAssemblyStartedCallback = (
  options: Partial<WebAssemblyStartOptions>,
) => Promise<void>;
type AfterBlazorWebAssemblyStartedCallback = (blazor: IBlazor) => Promise<void>;

type WebAssemblyInitializers = {
  beforeStart: BeforeBlazorWebAssemblyStartedCallback[];
  afterStarted: AfterBlazorWebAssemblyStartedCallback[];
};

type DomFunctions = {
  focus: (element: HTMLOrSVGElement, preventScroll?: boolean) => void;
  focusBySelector: (selector: string) => void;
};

interface BrowserFile {
  id: number;
  lastModified: string;
  name: string;
  size: number;
  contentType: string;
  blob: Blob;
}

interface InputElement extends HTMLInputElement {
  _blazorInputFileNextFileId: number;
  _blazorFilesById: { [id: number]: BrowserFile };
}

type InputFile = {
  init: (callbackWrapper: any, elem: InputElement) => void;
  toImageFile: (
    elem: InputElement,
    fileId: number,
    format: string,
    maxWidth: number,
    maxHeight: number,
  ) => Promise<BrowserFile>;
  readFileData: (elem: InputElement, fileId: number) => Promise<Blob>;
};

type NavigationLock = {
  enableNavigationPrompt: (id: string) => void;
  disableNavigationPrompt: (id: string) => void;
};

type JSComponentParameterType =
  | "string"
  | "boolean"
  | "boolean?"
  | "number"
  | "number?"
  | "object"
  | "eventcallback";

interface JSComponentParameter {
  name: string;
  type: JSComponentParameterType;
}

type JSComponentParametersByIdentifier = {
  [identifier: string]: JSComponentParameter[];
};
type JSComponentIdentifiersByInitializer = { [initializer: string]: string[] };

type AttachWebRendererInterop = (
  rendererId: number,
  interopMethods: DotNet.DotNetObject,
  jsComponentParameters?: JSComponentParametersByIdentifier,
  jsComponentInitializers?: JSComponentIdentifiersByInitializer,
) => void;

type Virtualize = {
  init: (
    dotNetHelper: DotNet.DotNetObject,
    spacerBefore: HTMLElement,
    spacerAfter: HTMLElement,
    rootMargin?: number,
  ) => void;
  dispose: (dotNetHelper: DotNet.DotNetObject) => void;
};

type PageTitle = {
  getAndRemoveExistingTitle: () => string | null;
};

export {
  type DomFunctions,
  type InputFile,
  type InputElement,
  type NavigationLock,
  type AttachWebRendererInterop,
  type Virtualize,
  type PageTitle,
  type INavigationManager,
  type NavigationOptions,
  type AddEventListener,
  type RemoveEventListener,
  type EventTypeOptions,
  type IHubConnection,
  type CircuitStartOptions,
  type DefaultReconnectionHandler,
  type WebStartOptions,
  type WebAssemblyStartOptions,
  type Platform,
  type Pointer,
  type RootComponentsFunctions,
  type IReconnectStateChangedEvent,
};
