window.notificationInterop = {
    requestPermission: async function () {
        if (!('Notification' in window)) return 'unsupported';
        const permission = await Notification.requestPermission();
        return permission;
    },

    getPermission: function () {
        if (!('Notification' in window)) return 'unsupported';
        return Notification.permission;
    },

    scheduleReminder: function (id, hour, minute, label) {
        // Clear existing interval for this reminder
        const key = 'reminder_' + id;
        if (window[key]) {
            clearInterval(window[key]);
        }

        window[key] = setInterval(function () {
            const now = new Date();
            if (now.getHours() === hour && now.getMinutes() === minute) {
                if (Notification.permission === 'granted') {
                    new Notification('💓 ' + label, {
                        body: 'Es ist Zeit, deinen Blutdruck zu messen!',
                        icon: '/icons/icon-192.png',
                        badge: '/icons/icon-192.png',
                        vibrate: [200, 100, 200],
                        tag: 'bp-reminder-' + id
                    });
                }
            }
        }, 60000); // Check every minute
    },

    cancelReminder: function (id) {
        const key = 'reminder_' + id;
        if (window[key]) {
            clearInterval(window[key]);
            delete window[key];
        }
    }
};
