window.themeManager = {
    init: function () {
        const saved = localStorage.getItem('bead-theme') || 'light';
        document.documentElement.setAttribute('data-theme', saved);
        return saved;
    },
    set: function (theme) {
        document.documentElement.setAttribute('data-theme', theme);
        localStorage.setItem('bead-theme', theme);
    },
    get: function () {
        return localStorage.getItem('bead-theme') || 'light';
    }
};
