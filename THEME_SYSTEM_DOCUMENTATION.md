# Light and Dark Theme System Documentation

## Overview

This website now includes a comprehensive light and dark theme system that works across both the public-facing pages and the admin area. The theme system is built using CSS custom properties (CSS variables) and JavaScript for dynamic theme switching.

## Features

### 🎨 **Automatic Theme Detection**
- Detects the user's system preference (light/dark mode)
- Respects the user's choice and saves it in localStorage
- Applies the preferred theme on subsequent visits

### 🔄 **Multiple Ways to Switch Themes**
1. **Floating Toggle Button**: A circular button with sun/moon icons (right side of screen)
2. **Keyboard Shortcut**: `Ctrl/Cmd + Shift + D`
3. **Navigation Menu**: Theme toggle added to main navigation
4. **Admin Panel**: Integrated with existing admin settings panel

### 📱 **Responsive Design**
- Theme toggle button adapts to different screen sizes
- Mobile-optimized positioning and sizing
- Touch-friendly interface elements

### ♿ **Accessibility Features**
- Screen reader announcements for theme changes
- Proper ARIA labels and keyboard navigation
- High contrast ratios in both themes
- Respects `prefers-reduced-motion` setting

## How to Use

### For End Users

#### Method 1: Floating Toggle Button
- Look for the circular button on the right side of your screen
- Click the 🌙 (moon) icon to switch to dark mode
- Click the ☀️ (sun) icon to switch to light mode

#### Method 2: Keyboard Shortcut
- Press `Ctrl + Shift + D` (Windows/Linux) or `Cmd + Shift + D` (Mac)
- This will toggle between light and dark themes

#### Method 3: Navigation Menu
- In the main navigation menu, look for the theme toggle option
- Click "Dark Mode" to switch to dark theme
- Click "Light Mode" to switch to light theme

#### Method 4: Admin Panel (Admin Area Only)
- Click the palette icon in the admin sidebar
- In the settings panel, find the "THEME MODE" section
- Click "Light" or "Dark" to switch themes

### Theme Persistence
- Your theme choice is automatically saved
- When you return to the site, your preferred theme will be applied
- Works across all pages (public and admin areas)

## Technical Implementation

### CSS Architecture
The theme system uses CSS custom properties for maintainable theming:

```css
:root {
  /* Light theme colors */
  --primary-color: #eb0028;
  --bg-primary: #ffffff;
  --text-primary: #444444;
  /* ... more variables */
}

[data-theme="dark"] {
  /* Dark theme colors */
  --bg-primary: #1a1a1a;
  --text-primary: #e0e0e0;
  /* ... overridden variables */
}
```

### JavaScript API
The theme system exposes several global functions:

```javascript
// Toggle between light and dark
toggleTheme();

// Set specific theme
setTheme('dark');   // or 'light'

// Get current theme
getCurrentTheme();  // returns 'light' or 'dark'

// Check if dark mode is active
window.themeManager.isDarkMode();
```

### Theme Change Events
Listen for theme changes in your custom code:

```javascript
window.addEventListener('themechange', function(event) {
  console.log('Theme changed to:', event.detail.theme);
  // Your custom logic here
});
```

## Customization

### Adding Custom Theme Support
To add theme support to new components:

1. **Define CSS variables for colors**:
```css
.your-component {
  background-color: var(--card-bg);
  color: var(--text-primary);
  border-color: var(--border-light);
}
```

2. **Add dark theme overrides if needed**:
```css
[data-theme="dark"] .your-component {
  /* Override specific properties if the default variables aren't sufficient */
}
```

### Customizing Colors
Edit the CSS custom properties in `themes.css` to change the color scheme:

```css
:root {
  --primary-color: #your-brand-color;
  --bg-primary: #your-background-color;
  /* ... other customizations */
}
```

## File Structure

```
Ui/wwwroot/
├── css/
│   ├── themes.css           # Main theme variables and base styles
│   └── theme-extensions.css # Extended theme support for all components
└── js/
    └── theme-manager.js     # Theme management JavaScript
```

## Browser Support

- **Modern Browsers**: Full support (Chrome 49+, Firefox 31+, Safari 9.1+, Edge 16+)
- **Legacy Browsers**: Graceful fallback to light theme
- **Mobile Browsers**: Full support with responsive design

## Performance

- **Initial Load**: No additional HTTP requests (CSS/JS bundled)
- **Theme Switching**: Instant with CSS transitions
- **Storage**: Minimal localStorage usage (< 50 bytes)
- **Memory**: Lightweight JavaScript implementation

## Troubleshooting

### Theme Not Switching
1. Check browser console for JavaScript errors
2. Ensure both CSS files are loaded correctly
3. Verify localStorage is enabled

### Colors Not Changing
1. Check if CSS custom properties are supported
2. Verify the theme CSS files are loaded after main styles
3. Look for CSS specificity conflicts

### Mobile Issues
1. Check viewport meta tag is present
2. Verify touch targets are at least 44px
3. Test on actual devices, not just browser dev tools

## Future Enhancements

Potential future improvements:
- **System Theme Auto-Switch**: Automatically switch when system theme changes
- **Scheduled Themes**: Auto-switch based on time of day
- **More Color Schemes**: Additional theme variants (blue, green, etc.)
- **Custom Theme Builder**: Allow users to create custom color schemes
- **Animation Options**: More theme transition effects

## Support

For technical issues or questions about the theme system:
1. Check this documentation first
2. Look for similar issues in the project repository
3. Contact the development team with specific details about the issue

---

**Note**: This theme system is designed to be accessible, performant, and easy to maintain. All color combinations have been tested for WCAG 2.1 AA compliance