window.timezoneInterop = {
    getBrowserLocalNow: function () {
        const now = new Date();
        return now.getFullYear() + '-' +
            String(now.getMonth() + 1).padStart(2, '0') + '-' +
            String(now.getDate()).padStart(2, '0') + 'T' +
            String(now.getHours()).padStart(2, '0') + ':' +
            String(now.getMinutes()).padStart(2, '0');
    },
    getBrowserUtcOffsetMinutes: function () {
        return -new Date().getTimezoneOffset();
    }
};
