(function () {
    'use strict';

    angular.module('app.service')
        .factory('acceptance_material_svr', acceptance_material_svr);

    acceptance_material_svr.$inject = ['svr'];

    function acceptance_material_svr(svr) {

        return {
            station: {
                get_names: get_names,
                search: search
            }
        };

        function get_names(callback) {
            
        };

        function search(code, callback) {
            
        };
    }
})();