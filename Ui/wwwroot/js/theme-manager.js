// Theme Management System
class ThemeManager {
    constructor() {
        this.currentTheme = this.getStoredTheme() || this.getSystemTheme();
        this.init();
    }

    init() {
        this.applyTheme(this.currentTheme);
        this.createThemeToggle();
        this.bindEvents();
        
        // Listen for system theme changes
        if (window.matchMedia) {
            window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
                if (!this.getStoredTheme()) {
                    this.setTheme(e.matches ? 'dark' : 'light', false);
                }
            });
        }
    }

    getSystemTheme() {
        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            return 'dark';
        }
        return 'light';
    }

    getStoredTheme() {
        return localStorage.getItem('theme-preference');
    }

    storeTheme(theme) {
        localStorage.setItem('theme-preference', theme);
    }

    applyTheme(theme) {
        document.documentElement.setAttribute('data-theme', theme);
        document.body.setAttribute('data-theme', theme);
        this.currentTheme = theme;
        this.updateThemeToggle();
        
        // Update meta theme-color for mobile browsers
        this.updateMetaThemeColor(theme);
        
        // Apply admin-specific theme changes
        this.applyAdminTheme(theme);
        
        // Dispatch custom event for theme change
        window.dispatchEvent(new CustomEvent('themechange', { 
            detail: { theme: theme } 
        }));
    }

    applyAdminTheme(theme) {
        // Apply theme to admin specific elements that might not be covered by CSS
        const body = document.body;
        
        // Handle admin sidebar theme synchronization
        if (theme === 'dark') {
            body.classList.add('sidebar-dark');
            body.classList.remove('sidebar-light');
        } else {
            body.classList.add('sidebar-light');
            body.classList.remove('sidebar-dark');
        }
        
        // Update admin setting panel state
        this.updateAdminSettingsState(theme);
    }

    updateAdminSettingsState(theme) {
        // Update sidebar theme options in admin panel
        const sidebarLightOption = document.getElementById('sidebar-light-theme');
        const sidebarDarkOption = document.getElementById('sidebar-dark-theme');
        
        if (sidebarLightOption && sidebarDarkOption) {
            if (theme === 'dark') {
                sidebarLightOption.classList.remove('selected');
                sidebarDarkOption.classList.add('selected');
            } else {
                sidebarDarkOption.classList.remove('selected');
                sidebarLightOption.classList.add('selected');
            }
        }
    }

    updateMetaThemeColor(theme) {
        let metaThemeColor = document.querySelector('meta[name="theme-color"]');
        if (!metaThemeColor) {
            metaThemeColor = document.createElement('meta');
            metaThemeColor.name = 'theme-color';
            document.head.appendChild(metaThemeColor);
        }
        
        const colors = {
            light: '#ffffff',
            dark: '#1a1a1a'
        };
        
        metaThemeColor.content = colors[theme] || colors.light;
    }

    setTheme(theme, store = true) {
        this.applyTheme(theme);
        if (store) {
            this.storeTheme(theme);
        }
    }

    toggleTheme() {
        const newTheme = this.currentTheme === 'dark' ? 'light' : 'dark';
        this.setTheme(newTheme);
        
        // Provide user feedback
        this.showThemeChangeNotification(newTheme);
    }

    showThemeChangeNotification(theme) {
        // Create or update notification
        let notification = document.getElementById('theme-notification');
        if (!notification) {
            notification = document.createElement('div');
            notification.id = 'theme-notification';
            notification.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: var(--primary-color);
                color: white;
                padding: 12px 20px;
                border-radius: 8px;
                z-index: 10000;
                font-size: 14px;
                box-shadow: 0 4px 12px var(--shadow-medium);
                transform: translateX(400px);
                transition: transform 0.3s ease;
            `;
            document.body.appendChild(notification);
        }

        const themeNames = {
            light: 'Light Mode',
            dark: 'Dark Mode'
        };

        notification.textContent = `Switched to ${themeNames[theme]}`;
        
        // Show notification
        setTimeout(() => {
            notification.style.transform = 'translateX(0)';
        }, 100);

        // Hide notification
        setTimeout(() => {
            notification.style.transform = 'translateX(400px)';
        }, 2000);
    }

    createThemeToggle() {
        // Check if toggle already exists
        if (document.getElementById('theme-toggle-btn')) {
            return;
        }

        const toggleButton = document.createElement('button');
        toggleButton.id = 'theme-toggle-btn';
        toggleButton.className = 'theme-toggle';
        toggleButton.setAttribute('aria-label', 'Toggle theme');
        toggleButton.setAttribute('title', 'Toggle between light and dark theme');
        
        this.updateThemeToggle(toggleButton);
        
        document.body.appendChild(toggleButton);
    }

    updateThemeToggle(button = null) {
        const toggleButton = button || document.getElementById('theme-toggle-btn');
        if (!toggleButton) return;

        const icons = {
            light: '??', // Moon for switching to dark
            dark: '??'   // Sun for switching to light
        };

        toggleButton.innerHTML = icons[this.currentTheme] || icons.light;
        toggleButton.setAttribute('title', 
            this.currentTheme === 'dark' ? 
            'Switch to light mode' : 
            'Switch to dark mode'
        );
    }

    bindEvents() {
        // Theme toggle button
        document.addEventListener('click', (e) => {
            if (e.target.id === 'theme-toggle-btn' || e.target.closest('#theme-toggle-btn')) {
                e.preventDefault();
                this.toggleTheme();
            }
        });

        // Keyboard shortcut (Ctrl/Cmd + Shift + D)
        document.addEventListener('keydown', (e) => {
            if ((e.ctrlKey || e.metaKey) && e.shiftKey && e.key === 'D') {
                e.preventDefault();
                this.toggleTheme();
            }
        });

        // Listen for theme change events from other parts of the application
        window.addEventListener('setTheme', (e) => {
            if (e.detail && e.detail.theme) {
                this.setTheme(e.detail.theme);
            }
        });

        // Admin-specific event bindings
        this.bindAdminEvents();
    }

    bindAdminEvents() {
        // Bind existing admin sidebar theme options
        document.addEventListener('click', (e) => {
            if (e.target.closest('#sidebar-light-theme')) {
                e.preventDefault();
                this.setTheme('light');
            } else if (e.target.closest('#sidebar-dark-theme')) {
                e.preventDefault();
                this.setTheme('dark');
            }
        });
    }

    // Method to add theme toggle to navigation menu
    addThemeToggleToMenu(menuSelector = '.mainmenu ul') {
        const menu = document.querySelector(menuSelector);
        if (!menu) return;

        const listItem = document.createElement('li');
        const link = document.createElement('a');
        link.href = '#';
        link.innerHTML = `<i class="fas fa-moon" id="menu-theme-icon"></i> <span id="menu-theme-text">Dark Mode</span>`;
        link.addEventListener('click', (e) => {
            e.preventDefault();
            this.toggleTheme();
        });

        listItem.appendChild(link);
        menu.appendChild(listItem);

        // Update menu item when theme changes
        window.addEventListener('themechange', (e) => {
            const icon = document.getElementById('menu-theme-icon');
            const text = document.getElementById('menu-theme-text');
            if (icon && text) {
                if (e.detail.theme === 'dark') {
                    icon.className = 'fas fa-sun';
                    text.textContent = 'Light Mode';
                } else {
                    icon.className = 'fas fa-moon';
                    text.textContent = 'Dark Mode';
                }
            }
        });
    }

    // Method to integrate with admin settings panel
    integrateWithAdminPanel() {
        const adminThemeSettings = document.getElementById('theme-settings');
        if (!adminThemeSettings) return;

        // Check if theme mode section already exists
        if (document.querySelector('.theme-mode-options')) return;

        const themeToggleSection = document.createElement('div');
        themeToggleSection.innerHTML = `
            <p class="settings-heading mt-3">THEME MODE</p>
            <div class="theme-mode-options">
                <div class="theme-option ${this.currentTheme === 'light' ? 'selected' : ''}" 
                     data-theme="light" id="light-theme-option">
                    <div class="img-ss rounded-circle bg-light border me-3"></div>Light
                </div>
                <div class="theme-option ${this.currentTheme === 'dark' ? 'selected' : ''}" 
                     data-theme="dark" id="dark-theme-option">
                    <div class="img-ss rounded-circle bg-dark border me-3"></div>Dark
                </div>
            </div>
        `;

        adminThemeSettings.appendChild(themeToggleSection);

        // Add click handlers for admin theme options
        themeToggleSection.addEventListener('click', (e) => {
            const themeOption = e.target.closest('.theme-option');
            if (themeOption) {
                const selectedTheme = themeOption.dataset.theme;
                this.setTheme(selectedTheme);
                
                // Update selected state
                themeToggleSection.querySelectorAll('.theme-option').forEach(option => {
                    option.classList.remove('selected');
                });
                themeOption.classList.add('selected');
            }
        });
    }

    // Method to handle print styles
    setupPrintStyles() {
        const style = document.createElement('style');
        style.textContent = `
            @media print {
                * {
                    background: white !important;
                    color: black !important;
                    box-shadow: none !important;
                }
                .theme-toggle {
                    display: none !important;
                }
            }
        `;
        document.head.appendChild(style);
    }

    // Accessibility improvements
    setupAccessibility() {
        // Announce theme changes to screen readers
        window.addEventListener('themechange', (e) => {
            const announcement = document.createElement('div');
            announcement.setAttribute('aria-live', 'polite');
            announcement.setAttribute('aria-atomic', 'true');
            announcement.className = 'sr-only';
            announcement.textContent = `Theme changed to ${e.detail.theme} mode`;
            
            document.body.appendChild(announcement);
            
            setTimeout(() => {
                document.body.removeChild(announcement);
            }, 1000);
        });
    }

    // Method to ensure theme consistency across page loads
    ensureThemeConsistency() {
        // Force reapplication of theme after page load
        setTimeout(() => {
            this.applyTheme(this.currentTheme);
        }, 100);
        
        // Check if we're in admin area and apply specific styles
        if (document.querySelector('.container-scroller') || document.querySelector('.admin-area')) {
            this.applyAdminTheme(this.currentTheme);
        }
    }

    // Method to fix any theme inconsistencies
    fixThemeInconsistencies() {
        // Remove any conflicting classes
        document.body.classList.remove('sidebar-light', 'sidebar-dark');
        
        // Reapply correct theme
        this.applyTheme(this.currentTheme);
        
        // Update all theme-related UI elements
        this.updateAllThemeElements();
    }

    updateAllThemeElements() {
        // Update navbar theme toggle
        const navbarThemeIcon = document.getElementById('navbar-theme-icon');
        if (navbarThemeIcon) {
            if (this.currentTheme === 'dark') {
                navbarThemeIcon.className = 'mdi mdi-weather-sunny';
            } else {
                navbarThemeIcon.className = 'mdi mdi-weather-night';
            }
        }

        // Update main theme toggle
        this.updateThemeToggle();
        
        // Update admin settings panel
        this.updateAdminSettingsState(this.currentTheme);
        
        // Update menu theme options if they exist
        const menuIcon = document.getElementById('menu-theme-icon');
        const menuText = document.getElementById('menu-theme-text');
        if (menuIcon && menuText) {
            if (this.currentTheme === 'dark') {
                menuIcon.className = 'fas fa-sun';
                menuText.textContent = 'Light Mode';
            } else {
                menuIcon.className = 'fas fa-moon';
                menuText.textContent = 'Dark Mode';
            }
        }
    }

    // Utility method to check if dark mode is active
    isDarkMode() {
        return this.currentTheme === 'dark';
    }

    // Method to get current theme
    getCurrentTheme() {
        return this.currentTheme;
    }

    // Method to force theme refresh
    refreshTheme() {
        this.fixThemeInconsistencies();
        this.ensureThemeConsistency();
    }
}

// Auto-initialize theme manager when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.themeManager = new ThemeManager();
    
    // Add additional setup
    window.themeManager.setupPrintStyles();
    window.themeManager.setupAccessibility();
    
    // Add to menu if main navigation exists
    if (document.querySelector('.mainmenu ul')) {
        window.themeManager.addThemeToggleToMenu();
    }
    
    // Integrate with admin panel if it exists
    setTimeout(() => {
        window.themeManager.integrateWithAdminPanel();
        window.themeManager.ensureThemeConsistency();
    }, 200);
    
    // Additional delay to ensure all elements are loaded
    setTimeout(() => {
        window.themeManager.updateAllThemeElements();
    }, 500);
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ThemeManager;
}

// Global utility functions
window.setTheme = function(theme) {
    if (window.themeManager) {
        window.themeManager.setTheme(theme);
    }
};

window.toggleTheme = function() {
    if (window.themeManager) {
        window.themeManager.toggleTheme();
    }
};

window.getCurrentTheme = function() {
    return window.themeManager ? window.themeManager.getCurrentTheme() : 'light';
};

window.refreshTheme = function() {
    if (window.themeManager) {
        window.themeManager.refreshTheme();
    }
};