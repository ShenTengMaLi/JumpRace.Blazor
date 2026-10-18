// In development, we just skip waiting and claim clients immediately.
// The published service worker (service-worker.published.js) does the real caching.

self.addEventListener('install', event => event.waitUntil(self.skipWaiting()));
self.addEventListener('activate', event => event.waitUntil(self.clients.claim()));

self.addEventListener('fetch', event => {
  // During development we let the browser handle everything normally.
});
