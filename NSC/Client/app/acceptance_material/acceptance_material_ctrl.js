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

        if (vm.user.level == 4) {
            vm.county.code = vm.user.code;
        }

        vm.no_results = false;
        vm.conservancy_station = undefined;
        vm.station_selecting = function(item) {
            vm.conservancy_station = item;
        };
        vm.get_stations = function(name) {
            return acceptance_material_svr.get_station_names(name, function(response) {
                vm.no_results = response.data.length == 0;

                return response.data;
            });
        };

        vm.search = {
            result: [],
            from_svr: function() {
                return acceptance_material_svr.search({
                    city_code: vm.city.code,
                    county_code: vm.county.code,
                    station_name: vm.conservancy_station
                }, function(response) {
                    vm.search.result = response.data;
                    console.log(response.data);
                });
            }
        };

        vm.download = function (file_name, file_url) {
            window.location.href = 'dt04/download?file_name=' + file_name + '&file_url=' + file_url;
        };

        vm.show = function(btn_name) {
             
        };
    }
})();