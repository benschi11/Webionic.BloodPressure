// BlutdruckTracker JS interop

// Capture PWA install prompt
window.deferredPrompt = null;
window.addEventListener('beforeinstallprompt', (e) => {
    e.preventDefault();
    window.deferredPrompt = e;
    const btn = document.getElementById('installBtn');
    if (btn) btn.style.display = 'inline-block';
});

window.addEventListener('appinstalled', () => {
    window.deferredPrompt = null;
    const btn = document.getElementById('installBtn');
    if (btn) btn.style.display = 'none';
});

window.blazorInterop = {
    // Register service worker for PWA
    registerServiceWorker: async function () {
        if ('serviceWorker' in navigator) {
            try {
                await navigator.serviceWorker.register('/service-worker.js');
                return true;
            } catch (e) {
                console.warn('Service worker registration failed:', e);
                return false;
            }
        }
        return false;
    },

    // Request notification permission
    requestNotificationPermission: async function () {
        if (!('Notification' in window)) return 'unsupported';
        if (Notification.permission === 'granted') return 'granted';
        if (Notification.permission === 'denied') return 'denied';
        const result = await Notification.requestPermission();
        return result;
    },

    getNotificationPermission: function () {
        if (!('Notification' in window)) return 'unsupported';
        return Notification.permission;
    },

    // Show a local notification
    showNotification: function (title, body) {
        if (Notification.permission === 'granted') {
            new Notification(title, {
                body: body,
                icon: '/icons/icon-192.png',
                badge: '/icons/icon-192.png'
            });
            return true;
        }
        return false;
    },

    // Schedule a reminder using setTimeout (works while page is open)
    scheduledReminders: {},

    scheduleReminder: function (id, delayMs, title, body, dotNetRef) {
        if (window.blazorInterop.scheduledReminders[id]) {
            clearTimeout(window.blazorInterop.scheduledReminders[id]);
        }
        window.blazorInterop.scheduledReminders[id] = setTimeout(() => {
            window.blazorInterop.showNotification(title, body);
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('OnReminderFired');
            }
        }, delayMs);
    },

    cancelReminder: function (id) {
        if (window.blazorInterop.scheduledReminders[id]) {
            clearTimeout(window.blazorInterop.scheduledReminders[id]);
            delete window.blazorInterop.scheduledReminders[id];
        }
    },

    // Chart.js rendering
    renderChart: function (canvasId, type, labels, datasets, options) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        const existingChart = Chart.getChart(canvasId);
        if (existingChart) existingChart.destroy();

        new Chart(canvas, {
            type: type,
            data: { labels, datasets },
            options: options || {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'top' }
                },
                scales: {
                    y: {
                        beginAtZero: false,
                        suggestedMin: 40,
                        suggestedMax: 200
                    }
                }
            }
        });
    },

    destroyChart: function (canvasId) {
        const existingChart = Chart.getChart(canvasId);
        if (existingChart) existingChart.destroy();
    }
};
