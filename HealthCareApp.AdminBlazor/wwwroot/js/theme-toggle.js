export function getTheme() {
    try {
        return localStorage.getItem('hc-theme') || null;
    }
    catch (e) {
        return null;
    }
}

export function applyTheme(theme) {
    try {
        if (!theme) return;
        document.documentElement.classList.remove('theme-neon');
        document.documentElement.classList.remove('theme-legacy');
        document.documentElement.classList.add(theme);
        // also apply a data-theme attribute for CSS selectors if needed
        document.documentElement.setAttribute('data-hc-theme', theme);
    }
    catch (e) { }
}

export function toggleTheme() {
    try {
        const current = getTheme() || (document.documentElement.classList.contains('theme-neon') ? 'theme-neon' : 'theme-legacy');
        const next = current === 'theme-neon' ? 'theme-legacy' : 'theme-neon';
        localStorage.setItem('hc-theme', next);
        applyTheme(next);
        return next;
    }
    catch (e) {
        return null;
    }
}

export function initApplySavedTheme() {
    try {
        const saved = getTheme();
        if (saved) {
            document.documentElement.classList.add(saved);
        }
    }
    catch (e) { }
}
