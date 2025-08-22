const ShippingPackgingService = {
    GetAll: function (onSuccess, onError) {
        ApiClient.get('/api/ShippingPackging', onSuccess, onError, false);
    },

    GetById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/ShippingPackging/${id}`, onSuccess, onError, false);
    },

    Add: function (data, onSuccess, onError) {
        ApiClient.post('/api/ShippingPackging', data, onSuccess, onError, false);
    },

    Update: function (data, onSuccess, onError) {
        ApiClient.put(`/api/ShippingPackging/${data.id}`, data, onSuccess, onError, false);
    },

    Delete: function (id, onSuccess, onError) {
        ApiClient.delete(`/api/ShippingPackging/${id}`, onSuccess, onError, false);
    },

    ChangeStatus: function (id, status, onSuccess, onError) {
        ApiClient.put(`/api/ShippingPackging/${id}/status`, { status: status }, onSuccess, onError, false);
    }
};