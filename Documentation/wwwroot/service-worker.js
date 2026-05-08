// Development service worker.
//
// Intentionally registers no `fetch` listener: a no-op fetch handler still
// forces every navigation through the SW pipeline, and Chrome warns about it
// ("The service worker has a no-op fetch handler that has been bypassed").
// Without a fetch handler the SW is effectively pass-through, and the browser
// can skip it for navigation entirely.
//
// `install` + `activate` ensure quick takeover so updates propagate without
// requiring a hard reload during development.

self.addEventListener("install", () => self.skipWaiting());
self.addEventListener("activate", (event) =>
  event.waitUntil(self.clients.claim()),
);
