const AdminShippingPackagingManager = {
    // Initialize the page
    init: function() {
        this.bindEvents();
        this.initializeDataTable();
    },

    // Bind event handlers
    bindEvents: function() {
        // Add click handlers for action buttons
        $(document).on('click', '.btn-edit-packaging', this.editPackaging);
        $(document).on('click', '.btn-delete-packaging', this.deletePackaging);
        $(document).on('click', '.btn-add-packaging', this.addPackaging);
        
        // Form submission handler
        $('#packagingForm').on('submit', this.savePackaging);
    },

    // Initialize DataTable
    initializeDataTable: function() {
        if ($('#packagingTable').length > 0) {
            $('#packagingTable').DataTable({
                "responsive": true,
                "lengthChange": false,
                "autoWidth": false,
                "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
                "language": {
                    "search": "Search:",
                    "lengthMenu": "Show _MENU_ entries",
                    "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                    "infoEmpty": "Showing 0 to 0 of 0 entries",
                    "infoFiltered": "(filtered from _MAX_ total entries)",
                    "loadingRecords": "Loading...",
                    "processing": "Processing...",
                    "emptyTable": "No data available in table"
                }
            }).buttons().container().appendTo('#packagingTable_wrapper .col-md-6:eq(0)');
        }
    },

    // Load all packaging types via API
    loadPackagingTypes: function(callback) {
        ShippingPackgingService.GetAll(
            function(response) {
                if (response && response.success && response.data) {
                    if (callback) callback(response.data);
                } else {
                    AppHelper.showToast('Failed to load packaging types', 'error');
                }
            },
            function(error) {
                console.error('Error loading packaging types:', error);
                AppHelper.showToast('Error loading packaging types', 'error');
            }
        );
    },

    // Add new packaging
    addPackaging: function(e) {
        e.preventDefault();
        window.location.href = '/admin/ShippingPackaging/Edit';
    },

    // Edit packaging
    editPackaging: function(e) {
        e.preventDefault();
        var packagingId = $(this).data('id');
        window.location.href = '/admin/ShippingPackaging/Edit/' + packagingId;
    },

    // Delete packaging
    deletePackaging: function(e) {
        e.preventDefault();
        
        if (!confirm('Are you sure you want to delete this packaging type?')) {
            return;
        }

        var packagingId = $(this).data('id');
        var row = $(this).closest('tr');

        ShippingPackgingService.Delete(packagingId,
            function(response) {
                if (response && response.success) {
                    AppHelper.showToast('Packaging type deleted successfully', 'success');
                    row.fadeOut(300, function() {
                        $(this).remove();
                    });
                } else {
                    AppHelper.showToast('Failed to delete packaging type', 'error');
                }
            },
            function(error) {
                console.error('Error deleting packaging type:', error);
                AppHelper.showToast('Error deleting packaging type', 'error');
            }
        );
    },

    // Save packaging (Add/Update)
    savePackaging: function(e) {
        e.preventDefault();
        
        var formData = {
            id: $('#Id').val(),
            shipingPackgingAname: $('#ShipingPackgingAname').val(),
            shipingPackgingEname: $('#ShipingPackgingEname').val(),
            createdBy: $('#CreatedBy').val(),
            createdDate: $('#CreatedDate').val(),
            currentState: $('#CurrentState').val() || 1
        };

        // Validate form data
        if (!formData.shipingPackgingAname || !formData.shipingPackgingEname) {
            AppHelper.showToast('Please fill in all required fields', 'warning');
            return;
        }

        var isUpdate = formData.id && formData.id !== '00000000-0000-0000-0000-000000000000';
        
        var serviceMethod = isUpdate ? ShippingPackgingService.Update : ShippingPackgingService.Add;
        
        serviceMethod(formData,
            function(response) {
                if (response && response.success) {
                    AppHelper.showToast(
                        isUpdate ? 'Packaging type updated successfully' : 'Packaging type added successfully', 
                        'success'
                    );
                    setTimeout(function() {
                        window.location.href = '/admin/ShippingPackaging';
                    }, 1500);
                } else {
                    AppHelper.showToast('Failed to save packaging type', 'error');
                }
            },
            function(error) {
                console.error('Error saving packaging type:', error);
                AppHelper.showToast('Error saving packaging type', 'error');
            }
        );
    },

    // Populate select dropdown with packaging types
    populatePackagingSelect: function(selectElement, selectedValue = null) {
        this.loadPackagingTypes(function(packagingTypes) {
            $(selectElement).empty();
            $(selectElement).append('<option value="">Select Packaging Type</option>');
            
            packagingTypes.forEach(function(packaging) {
                var selected = selectedValue && selectedValue === packaging.id ? 'selected' : '';
                $(selectElement).append(
                    `<option value="${packaging.id}" ${selected}>${packaging.shipingPackgingEname}</option>`
                );
            });
        });
    }
};

// Initialize when document is ready
$(document).ready(function() {
    AdminShippingPackagingManager.init();
});