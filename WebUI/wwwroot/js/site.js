// BiCozum Belediye - Custom JavaScript

// Global variables
let currentUser = null;
let apiBaseUrl = '/api';

// Initialize when document is ready
$(document).ready(function() {
    initializeApp();
    setupEventListeners();
    initializeDataTables();
});

// Initialize application
function initializeApp() {
    // Check if user is authenticated
    checkAuthentication();
    
    // Add fade-in animation to cards
    $('.card').addClass('fade-in');
    
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Setup event listeners
function setupEventListeners() {
    // Form submissions
    $('form').on('submit', function(e) {
        showLoading();
    });
    
    // Delete confirmations
    $('.btn-delete').on('click', function(e) {
        e.preventDefault();
        const url = $(this).data('url');
        const itemName = $(this).data('item-name') || 'Bu öğeyi';
        
        Swal.fire({
            title: 'Emin misiniz?',
            text: `${itemName} silmek istediğinizden emin misiniz?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Evet, sil!',
            cancelButtonText: 'İptal'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = url;
            }
        });
    });
    
    // Status change confirmations
    $('.btn-status-change').on('click', function(e) {
        e.preventDefault();
        const url = $(this).data('url');
        const newStatus = $(this).data('status');
        const itemName = $(this).data('item-name') || 'Bu talebi';
        
        Swal.fire({
            title: 'Durum Güncelleme',
            text: `${itemName} ${newStatus} durumuna güncellemek istediğinizden emin misiniz?`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Evet, güncelle!',
            cancelButtonText: 'İptal'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = url;
            }
        });
    });
}

// Initialize DataTables
function initializeDataTables() {
    $('.datatable').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json'
        },
        responsive: true,
        pageLength: 10,
        order: [[0, 'desc']],
        columnDefs: [
            {
                targets: -1,
                orderable: false,
                searchable: false
            }
        ]
    });
}

// Show loading spinner
function showLoading() {
    const spinner = `
        <div class="spinner-overlay">
            <div class="spinner"></div>
        </div>
    `;
    $('body').append(spinner);
}

// Hide loading spinner
function hideLoading() {
    $('.spinner-overlay').remove();
}

// Show success message
function showSuccess(message) {
    Swal.fire({
        icon: 'success',
        title: 'Başarılı!',
        text: message,
        timer: 3000,
        showConfirmButton: false
    });
}

// Show error message
function showError(message) {
    Swal.fire({
        icon: 'error',
        title: 'Hata!',
        text: message
    });
}

// Show info message
function showInfo(message) {
    Swal.fire({
        icon: 'info',
        title: 'Bilgi',
        text: message
    });
}

// Check authentication
function checkAuthentication() {
    // This would typically check for JWT token
    const token = localStorage.getItem('jwt_token');
    if (!token && window.location.pathname !== '/Account/Login') {
        // Redirect to login if not authenticated
        // window.location.href = '/Account/Login';
    }
}

// API Helper Functions
const ApiHelper = {
    // GET request
    get: function(url, successCallback, errorCallback) {
        $.ajax({
            url: apiBaseUrl + url,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwt_token')
            },
            success: function(data) {
                if (successCallback) successCallback(data);
            },
            error: function(xhr, status, error) {
                if (errorCallback) errorCallback(xhr, status, error);
                else showError('Bir hata oluştu: ' + error);
            }
        });
    },
    
    // POST request
    post: function(url, data, successCallback, errorCallback) {
        $.ajax({
            url: apiBaseUrl + url,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwt_token')
            },
            success: function(data) {
                if (successCallback) successCallback(data);
            },
            error: function(xhr, status, error) {
                if (errorCallback) errorCallback(xhr, status, error);
                else showError('Bir hata oluştu: ' + error);
            }
        });
    },
    
    // PUT request
    put: function(url, data, successCallback, errorCallback) {
        $.ajax({
            url: apiBaseUrl + url,
            type: 'PUT',
            data: JSON.stringify(data),
            contentType: 'application/json',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwt_token')
            },
            success: function(data) {
                if (successCallback) successCallback(data);
            },
            error: function(xhr, status, error) {
                if (errorCallback) errorCallback(xhr, status, error);
                else showError('Bir hata oluştu: ' + error);
            }
        });
    },
    
    // DELETE request
    delete: function(url, successCallback, errorCallback) {
        $.ajax({
            url: apiBaseUrl + url,
            type: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwt_token')
            },
            success: function(data) {
                if (successCallback) successCallback(data);
            },
            error: function(xhr, status, error) {
                if (errorCallback) errorCallback(xhr, status, error);
                else showError('Bir hata oluştu: ' + error);
            }
        });
    }
};

// Dashboard Functions
const DashboardHelper = {
    // Load dashboard statistics
    loadStats: function() {
        ApiHelper.get('/dashboard/stats', function(data) {
            updateDashboardCards(data);
        });
    },
    
    // Update dashboard cards
    updateDashboardCards: function(stats) {
        $('#totalComplaints').text(stats.totalComplaints);
        $('#pendingComplaints').text(stats.pendingComplaints);
        $('#completedComplaints').text(stats.completedComplaints);
        $('#totalUsers').text(stats.totalUsers);
    },
    
    // Load chart data
    loadChartData: function() {
        ApiHelper.get('/dashboard/chart', function(data) {
            createComplaintsChart(data);
        });
    }
};

// Create complaints chart
function createComplaintsChart(data) {
    const ctx = document.getElementById('complaintsChart');
    if (!ctx) return;
    
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Talepler',
                data: data.values,
                borderColor: '#0d6efd',
                backgroundColor: 'rgba(13, 110, 253, 0.1)',
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Aylık Talep Sayısı'
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

// Form validation
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return true;
    
    let isValid = true;
    const requiredFields = form.querySelectorAll('[required]');
    
    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            field.classList.add('is-invalid');
            isValid = false;
        } else {
            field.classList.remove('is-invalid');
        }
    });
    
    return isValid;
}

// Format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Get status badge class
function getStatusBadgeClass(status) {
    switch(status.toLowerCase()) {
        case 'beklemede':
            return 'bg-warning';
        case 'atandı':
            return 'bg-info';
        case 'tamamlandı':
            return 'bg-success';
        case 'iptal':
            return 'bg-danger';
        default:
            return 'bg-secondary';
    }
}

// Get status text
function getStatusText(status) {
    switch(status.toLowerCase()) {
        case 'beklemede':
            return 'Beklemede';
        case 'atandı':
            return 'Atandı';
        case 'tamamlandı':
            return 'Tamamlandı';
        case 'iptal':
            return 'İptal';
        default:
            return status;
    }
}

// Export functions to global scope
window.ApiHelper = ApiHelper;
window.DashboardHelper = DashboardHelper;
window.showSuccess = showSuccess;
window.showError = showError;
window.showInfo = showInfo;
window.showLoading = showLoading;
window.hideLoading = hideLoading;
window.formatDate = formatDate;
window.getStatusBadgeClass = getStatusBadgeClass;
window.getStatusText = getStatusText;
window.validateForm = validateForm;
