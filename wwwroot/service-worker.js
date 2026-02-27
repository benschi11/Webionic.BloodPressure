// Service Worker for BlutdruckTracker PWA
const CACHE_NAME = 'blutdruck-cache-v1';
const urlsToCache = [
    '/',
    '/manifest.json',
    '/app.css'
];

self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME).then(cache => cache.addAll(urlsToCache))
    );
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(keys =>
            Promise.all(keys.filter(k => k !== CACHE_NAME).map(k => caches.delete(k)))
        )
    );
    self.clients.claim();
});

self.addEventListener('fetch', event => {
    // Network first strategy for API calls, cache first for static assets
    if (event.request.method !== 'GET') return;

    event.respondWith(
        fetch(event.request)
            .catch(() => caches.match(event.request))
    );
});

// Handle push notifications
self.addEventListener('push', event => {
    const data = event.data ? event.data.json() : {};
    const title = data.title || 'Blutdruck messen';
    const options = {
        body: data.body || 'Zeit für deine Blutdruckmessung!',
        icon: '/icons/icon-192.png',
        badge: '/icons/icon-192.png',
        tag: 'blutdruck-reminder',
        renotify: true
    };
    event.waitUntil(self.registration.showNotification(title, options));
});

self.addEventListener('notificationclick', event => {
    event.notification.close();
    event.waitUntil(clients.openWindow('/'));
});
