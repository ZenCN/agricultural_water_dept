(function() {
    'use strict';

    angular
        .module('app.page')
        .controller('acceptance_material_ctrl', acceptance_material_ctrl);

    acceptance_material_ctrl.$inject = ['$scope', 'acceptance_material_svr'];

    function acceptance_material_ctrl(vm, acceptance_material_svr) {

        vm.city = {
            code: undefined,
            data: []
        };

        vm.county = {
            code: undefined,
            data: []
        };

        vm.no_results = false;
        vm.conservancy_station = undefined;
        vm.station_selecting = function(item) {
            vm.conservancy_station = item;
        };
        vm.get_stations = function(name) {
            return acceptance_material_svr.search.station.get_names(name, function (response) {
                vm.no_results = response.data.length == 0;

                return response.data;
            });
        };

        vm.search = {
            result: [],
            from_svr: function() {
                return acceptance_material_svr.search.station.search(function (response) {

                    vm.search = response.data;
                });
            }
        };
    }
})();