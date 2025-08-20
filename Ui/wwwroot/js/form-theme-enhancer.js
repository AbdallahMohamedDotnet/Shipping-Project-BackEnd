// Enhanced Form Theme Support and Dynamic Styling
(function() {
    'use strict';

    // Form Theme Enhancement Class
    class FormThemeEnhancer {
        constructor() {
            this.init();
        }

        init() {
            this.bindEvents();
            this.enhanceExistingForms();
            this.setupDynamicFormElements();
        }

        bindEvents() {
            // Listen for theme changes
            window.addEventListener('themechange', (e) => {
                this.handleThemeChange(e.detail.theme);
            });

            // Listen for new form elements being added dynamically
            document.addEventListener('DOMContentLoaded', () => {
                this.enhanceAllForms();
            });

            // Handle form validation styling
            document.addEventListener('input', (e) => {
                if (e.target.matches('.form-control')) {
                    this.handleFormValidation(e.target);
                }
            });

            // Handle select2 dropdowns if they exist
            document.addEventListener('select2:open', () => {
                this.enhanceSelect2Dropdown();
            });
        }

        handleThemeChange(theme) {
            // Update any dynamically created elements
            this.updateDynamicElements(theme);
            
            // Refresh third-party components
            this.refreshThirdPartyComponents(theme);
            
            // Update validation messages
            this.updateValidationMessages();
        }

        enhanceExistingForms() {
            // Enhance all existing forms
            const forms = document.querySelectorAll('form');
            forms.forEach(form => this.enhanceForm(form));
        }

        enhanceAllForms() {
            // Wait for DOM to be ready then enhance all forms
            setTimeout(() => {
                this.enhanceExistingForms();
                this.setupFormValidation();
                this.enhanceCustomElements();
            }, 100);
        }

        enhanceForm(form) {
            if (!form) return;

            // Add theme-aware classes
            form.classList.add('theme-enhanced-form');

            // Enhance form controls
            const controls = form.querySelectorAll('.form-control, input, select, textarea');
            controls.forEach(control => this.enhanceFormControl(control));

            // Enhance buttons
            const buttons = form.querySelectorAll('.btn, button, input[type="submit"], input[type="button"]');
            buttons.forEach(button => this.enhanceButton(button));

            // Enhance labels
            const labels = form.querySelectorAll('label');
            labels.forEach(label => this.enhanceLabel(label));
        }

        enhanceFormControl(control) {
            if (!control) return;

            // Add focus enhancement
            control.addEventListener('focus', () => {
                control.classList.add('theme-focus');
                this.addFocusGlow(control);
            });

            control.addEventListener('blur', () => {
                control.classList.remove('theme-focus');
                this.removeFocusGlow(control);
            });

            // Add validation styling
            control.addEventListener('invalid', () => {
                this.addValidationError(control);
            });

            // Enhance select dropdowns
            if (control.tagName === 'SELECT') {
                this.enhanceSelect(control);
            }
        }

        enhanceButton(button) {
            if (!button) return;

            // Add loading state capability
            const originalClick = button.onclick;
            button.addEventListener('click', (e) => {
                if (button.dataset.loading === 'true') {
                    e.preventDefault();
                    return false;
                }
            });
        }

        enhanceLabel(label) {
            if (!label) return;

            // Enhance required field indicators
            const requiredIndicator = label.querySelector('sup');
            if (requiredIndicator) {
                requiredIndicator.classList.add('required-indicator');
            }
        }

        addFocusGlow(element) {
            const glow = document.createElement('div');
            glow.className = 'focus-glow';
            glow.style.cssText = `
                position: absolute;
                top: -2px;
                left: -2px;
                right: -2px;
                bottom: -2px;
                background: linear-gradient(45deg, var(--primary-color), var(--primary-light));
                border-radius: 6px;
                z-index: -1;
                opacity: 0.3;
                filter: blur(4px);
                pointer-events: none;
            `;

            const parent = element.parentNode;
            if (parent && !parent.querySelector('.focus-glow')) {
                parent.style.position = 'relative';
                parent.appendChild(glow);
            }
        }

        removeFocusGlow(element) {
            const parent = element.parentNode;
            if (parent) {
                const glow = parent.querySelector('.focus-glow');
                if (glow) {
                    glow.remove();
                }
            }
        }

        addValidationError(element) {
            element.classList.add('is-invalid');
            
            // Create error message if it doesn't exist
            if (!element.parentNode.querySelector('.invalid-feedback')) {
                const errorMsg = document.createElement('div');
                errorMsg.className = 'invalid-feedback';
                errorMsg.textContent = element.validationMessage || 'This field is required.';
                element.parentNode.appendChild(errorMsg);
            }
        }

        enhanceSelect(select) {
            // Add custom dropdown arrow if needed
            if (!select.parentNode.querySelector('.custom-dropdown-arrow')) {
                const arrow = document.createElement('div');
                arrow.className = 'custom-dropdown-arrow';
                arrow.innerHTML = '<i class="fa fa-chevron-down"></i>';
                arrow.style.cssText = `
                    position: absolute;
                    right: 12px;
                    top: 50%;
                    transform: translateY(-50%);
                    pointer-events: none;
                    color: var(--text-secondary);
                    font-size: 0.8rem;
                `;
                
                select.parentNode.style.position = 'relative';
                select.parentNode.appendChild(arrow);
                select.style.appearance = 'none';
                select.style.paddingRight = '2.5rem';
            }
        }

        setupDynamicFormElements() {
            // Observer for dynamically added elements
            const observer = new MutationObserver((mutations) => {
                mutations.forEach((mutation) => {
                    mutation.addedNodes.forEach((node) => {
                        if (node.nodeType === Node.ELEMENT_NODE) {
                            // Check if it's a form or contains forms
                            if (node.tagName === 'FORM') {
                                this.enhanceForm(node);
                            } else if (node.querySelector) {
                                const forms = node.querySelectorAll('form');
                                forms.forEach(form => this.enhanceForm(form));
                                
                                const controls = node.querySelectorAll('.form-control, input, select, textarea');
                                controls.forEach(control => this.enhanceFormControl(control));
                            }
                        }
                    });
                });
            });

            observer.observe(document.body, {
                childList: true,
                subtree: true
            });
        }

        setupFormValidation() {
            // Enhanced form validation
            const forms = document.querySelectorAll('form[data-validate="true"], .needs-validation');
            forms.forEach(form => {
                form.addEventListener('submit', (e) => {
                    if (!form.checkValidity()) {
                        e.preventDefault();
                        e.stopPropagation();
                        this.showValidationErrors(form);
                    }
                    form.classList.add('was-validated');
                });
            });
        }

        showValidationErrors(form) {
            const invalidControls = form.querySelectorAll(':invalid');
            invalidControls.forEach(control => {
                this.addValidationError(control);
                
                // Focus on first invalid control
                if (invalidControls[0] === control) {
                    control.focus();
                }
            });
        }

        enhanceCustomElements() {
            // Enhance progress bars
            this.enhanceProgressBars();
            
            // Enhance multi-step forms
            this.enhanceMultiStepForms();
            
            // Enhance date pickers
            this.enhanceDatePickers();
        }

        enhanceProgressBars() {
            const progressBars = document.querySelectorAll('#progressbar');
            progressBars.forEach(progressBar => {
                const steps = progressBar.querySelectorAll('li');
                steps.forEach((step, index) => {
                    step.style.zIndex = steps.length - index;
                    
                    // Add click handlers for navigation
                    step.addEventListener('click', () => {
                        if (!step.classList.contains('disabled')) {
                            this.navigateToStep(index);
                        }
                    });
                });
            });
        }

        enhanceMultiStepForms() {
            const stepForms = document.querySelectorAll('.steps');
            stepForms.forEach(form => {
                const fieldsets = form.querySelectorAll('fieldset');
                const nextButtons = form.querySelectorAll('.next');
                const prevButtons = form.querySelectorAll('.previous');

                nextButtons.forEach(button => {
                    button.addEventListener('click', (e) => {
                        e.preventDefault();
                        this.nextStep(form);
                    });
                });

                prevButtons.forEach(button => {
                    button.addEventListener('click', (e) => {
                        e.preventDefault();
                        this.prevStep(form);
                    });
                });
            });
        }

        enhanceDatePickers() {
            const datePickers = document.querySelectorAll('.datetimepicker1, [data-provide="datepicker"]');
            datePickers.forEach(picker => {
                // Add calendar icon click handler
                const icon = picker.parentNode.querySelector('.datepicker-icon');
                if (icon) {
                    icon.style.cursor = 'pointer';
                    icon.addEventListener('click', () => {
                        picker.focus();
                        if (picker.click) picker.click();
                    });
                }
            });
        }

        updateDynamicElements(theme) {
            // Update any dynamically created dropdowns
            const dropdowns = document.querySelectorAll('.dropdown-menu');
            dropdowns.forEach(dropdown => {
                dropdown.style.backgroundColor = getComputedStyle(document.documentElement).getPropertyValue('--card-bg');
                dropdown.style.borderColor = getComputedStyle(document.documentElement).getPropertyValue('--border-light');
            });

            // Update modals
            const modals = document.querySelectorAll('.modal-content');
            modals.forEach(modal => {
                modal.style.backgroundColor = getComputedStyle(document.documentElement).getPropertyValue('--card-bg');
                modal.style.borderColor = getComputedStyle(document.documentElement).getPropertyValue('--border-light');
            });
        }

        refreshThirdPartyComponents(theme) {
            // Refresh Select2 if it exists
            if (window.$ && $.fn.select2) {
                $('.select2-container').remove();
                $('select[data-select2]').select2();
            }

            // Refresh DataTables if it exists
            if (window.$ && $.fn.DataTable) {
                $('.dataTable').each(function() {
                    $(this).DataTable().draw();
                });
            }

            // Refresh Bootstrap components
            this.refreshBootstrapComponents();
        }

        refreshBootstrapComponents() {
            // Update tooltip colors
            const tooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
            tooltips.forEach(tooltip => {
                if (window.bootstrap && bootstrap.Tooltip) {
                    const bsTooltip = bootstrap.Tooltip.getInstance(tooltip);
                    if (bsTooltip) {
                        bsTooltip.dispose();
                        new bootstrap.Tooltip(tooltip);
                    }
                }
            });
        }

        updateValidationMessages() {
            const errorMessages = document.querySelectorAll('.invalid-feedback, .valid-feedback');
            errorMessages.forEach(message => {
                // Update colors based on current theme
                const currentTheme = document.documentElement.getAttribute('data-theme');
                if (currentTheme === 'dark') {
                    if (message.classList.contains('invalid-feedback')) {
                        message.style.color = '#f8d7da';
                    } else {
                        message.style.color = '#d4edda';
                    }
                }
            });
        }

        // Utility methods for multi-step forms
        nextStep(form) {
            const current = form.querySelector('fieldset:not([style*="display: none"])');
            const next = current.nextElementSibling;
            
            if (next && next.tagName === 'FIELDSET') {
                this.showStep(current, next, 'next');
                this.updateProgress(form, Array.from(form.querySelectorAll('fieldset')).indexOf(next));
            }
        }

        prevStep(form) {
            const current = form.querySelector('fieldset:not([style*="display: none"])');
            const prev = current.previousElementSibling;
            
            if (prev && prev.tagName === 'FIELDSET') {
                this.showStep(current, prev, 'prev');
                this.updateProgress(form, Array.from(form.querySelectorAll('fieldset')).indexOf(prev));
            }
        }

        showStep(current, target, direction) {
            current.style.display = 'none';
            target.style.display = 'block';
            
            // Add animation classes
            if (direction === 'next') {
                target.style.animation = 'slideInRight 0.3s ease-in-out';
            } else {
                target.style.animation = 'slideInLeft 0.3s ease-in-out';
            }
            
            setTimeout(() => {
                target.style.animation = '';
            }, 300);
        }

        updateProgress(form, stepIndex) {
            const progressItems = form.parentNode.querySelectorAll('#progressbar li');
            progressItems.forEach((item, index) => {
                if (index <= stepIndex) {
                    item.classList.add('active');
                } else {
                    item.classList.remove('active');
                }
            });
        }

        navigateToStep(stepIndex) {
            const form = document.querySelector('.steps');
            if (form) {
                const fieldsets = form.querySelectorAll('fieldset');
                fieldsets.forEach((fieldset, index) => {
                    fieldset.style.display = index === stepIndex ? 'block' : 'none';
                });
                this.updateProgress(form, stepIndex);
            }
        }

        // Public methods
        addButtonLoading(button) {
            if (!button) return;
            
            button.dataset.loading = 'true';
            button.disabled = true;
            button.classList.add('loading');
            
            const originalText = button.textContent;
            button.dataset.originalText = originalText;
            button.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Loading...';
        }

        removeButtonLoading(button) {
            if (!button) return;
            
            button.dataset.loading = 'false';
            button.disabled = false;
            button.classList.remove('loading');
            button.innerHTML = button.dataset.originalText || 'Submit';
        }

        showAlert(type, title, message, container = null) {
            const alert = document.createElement('div');
            alert.className = `alert alert-${type} alert-dismissible fade show`;
            alert.innerHTML = `
                <strong>${title}:</strong> ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            `;

            const targetContainer = container || document.querySelector('.container:first-child');
            if (targetContainer) {
                targetContainer.insertBefore(alert, targetContainer.firstChild);
                
                // Auto-dismiss after 5 seconds
                setTimeout(() => {
                    if (alert.parentNode) {
                        alert.remove();
                    }
                }, 5000);
            }
        }
    }

    // CSS animations for step transitions
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideInRight {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }
        
        @keyframes slideInLeft {
            from {
                transform: translateX(-100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }
        
        .theme-enhanced-form {
            transition: all 0.3s ease;
        }
        
        .required-indicator {
            color: var(--primary-color);
            font-weight: bold;
        }
        
        .is-invalid {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25) !important;
        }
        
        .spinner-border-sm {
            width: 1rem;
            height: 1rem;
            border-width: 0.15em;
        }
    `;
    document.head.appendChild(style);

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            window.formThemeEnhancer = new FormThemeEnhancer();
        });
    } else {
        window.formThemeEnhancer = new FormThemeEnhancer();
    }

    // Export for global use
    window.FormThemeEnhancer = FormThemeEnhancer;
})();