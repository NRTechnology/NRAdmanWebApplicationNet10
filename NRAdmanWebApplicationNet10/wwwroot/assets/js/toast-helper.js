/**
 * Toast Helper - Dynamic Toast Notification
 * Provides functions to show toast notifications from JavaScript
 */

'use strict';

const ToastHelper = {
    showToast: function(message, type = 'info', delay = 3000) {
        const bgClass = this.getBgClass(type);
        const { title, icon } = this.getToastConfig(type);

        const toastHTML = `
            <div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 9999;">
                <div class="bs-toast toast toast-ex animate__animated animate__fadeInRight animate__faster my-2 ${bgClass}"
                     role="alert"
                     aria-live="assertive"
                     aria-atomic="true"
                     data-bs-delay="${delay}">
                    <div class="toast-header ${bgClass} border-0">
                        <i class="icon-base ti ${icon} icon-xs me-2"></i>
                        <div class="me-auto fw-medium">${title}</div>
                        <small class="text-muted d-flex align-items-center gap-1">
                            <i class="icon-base ti tabler-clock icon-xs"></i>
                            sekarang
                        </small>
                        <button type="button" class="btn-close ${type === 'warning' ? '' : 'btn-close-white'}" 
                                data-bs-dismiss="toast" aria-label="Close"></button>
                    </div>
                    <div class="toast-body">
                        ${message}
                    </div>
                </div>
            </div>
        `;

        document.body.insertAdjacentHTML('beforeend', toastHTML);

        // Get the newly created toast element
        const toastElement = document.querySelector('.toast-container:last-child .bs-toast');

        if (toastElement && typeof bootstrap !== 'undefined') {
            const toast = new bootstrap.Toast(toastElement, {
                delay: delay,
                autohide: true
            });

            toast.show();

            // Handle cleanup and animation
            toastElement.addEventListener('hide.bs.toast', function() {
                toastElement.classList.remove('animate__fadeInRight');
                toastElement.classList.add('animate__fadeOutRight');
            });

            // Remove toast container after it's hidden
            toastElement.addEventListener('hidden.bs.toast', function() {
                toastElement.closest('.toast-container').remove();
            });
        }
    },

    success: function(message, delay = 3000) {
        this.showToast(message, 'success', delay);
    },

    error: function(message, delay = 3000) {
        this.showToast(message, 'error', delay);
    },

    warning: function(message, delay = 3000) {
        this.showToast(message, 'warning', delay);
    },

    info: function(message, delay = 3000) {
        this.showToast(message, 'info', delay);
    },

    getBgClass: function(type) {
        switch (type.toLowerCase()) {
            case 'success':
                return 'bg-success text-white';
            case 'error':
                return 'bg-danger text-white';
            case 'warning':
                return 'bg-warning text-dark';
            default:
                return 'bg-info text-white';
        }
    },

    getToastConfig: function(type) {
        const configs = {
            success: { title: 'Berhasil', icon: 'tabler-check' },
            error: { title: 'Error', icon: 'tabler-x' },
            warning: { title: 'Peringatan', icon: 'tabler-alert-triangle' },
            info: { title: 'Informasi', icon: 'tabler-info-circle' }
        };

        return configs[type.toLowerCase()] || configs['info'];
    }
};
