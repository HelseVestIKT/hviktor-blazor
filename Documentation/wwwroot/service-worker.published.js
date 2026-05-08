// Blazor WASM service worker for production builds.
// Caches _framework/ assets (content-hashed) with a cache-first strategy
// for fast repeat loads. Other assets use network-first.

const CACHE_NAME = "hviktor-docs-cache";
const FRAMEWORK_PREFIX = "_framework/";

self.addEventListener("install", (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) =>
      fetch("service-worker-assets.js")
        .then((response) => response.text())
        .then((text) => {
          // The assets manifest assigns to self.assetsManifest
          const fn = new Function(text);
          fn();
          const assets = self.assetsManifest.assets
            .filter((a) => a.url.startsWith(FRAMEWORK_PREFIX))
            .map(
              (a) =>
                new Request(a.url, { integrity: a.hash, cache: "no-cache" }),
            );
          return Promise.all(
            assets.map((req) =>
              fetch(req)
                .then((res) => {
                  if (res.ok) {
                    return cache.put(req, res);
                  }
                })
                .catch(() => {
                  /* Non-critical: will fetch on demand */
                }),
            ),
          );
        }),
    ),
  );
  self.skipWaiting();
});

self.addEventListener("activate", (event) => {
  event.waitUntil(
    caches
      .keys()
      .then((keys) =>
        Promise.all(
          keys
            .filter((key) => key !== CACHE_NAME)
            .map((key) => caches.delete(key)),
        ),
      ),
  );
  self.clients.claim();
});

self.addEventListener("fetch", (event) => {
  const url = new URL(event.request.url);

  // Only apply cache-first to same-origin _framework/ requests
  if (
    url.origin === self.location.origin &&
    url.pathname.includes(FRAMEWORK_PREFIX)
  ) {
    event.respondWith(
      caches
        .match(event.request)
        .then((cached) => cached || fetch(event.request)),
    );
  }

  // All other requests go to network (no offline support needed for docs)
});
