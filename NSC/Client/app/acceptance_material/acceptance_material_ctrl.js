(function() {
    'use strict';

    angular
        .module('app.page')
        .controller('acceptance_material_ctrl', acceptance_material_ctrl);

    acceptance_material_ctrl.$inject = ['$scope', 'acceptance_material_svr', 'login_svr'];

    function acceptance_material_ctrl(vm, acceptance_material_svr, login_svr) {

        vm.city = {
            code: undefined,
            data: []
        };

        vm.county = {
            code: undefined,
            data: []
        };

        switch (parseInt(vm.user.level)) {
        case 2:
            login_svr.load_units(vm.user.code, function(response) {
                vm.city.data = response.data;
            });
            break;
        case 3:
            login_svr.load_units(vm.user.code, function(response) {
                vm.county.data = response.data;
            });
            break;
        case 4:
            vm.county.code = vm.user.code;
            break;
        }

        vm.city_change = function() {
            if (isString(vm.city.code)) {
                login_svr.load_units(vm.city.code, function(response) {
                    vm.county.data = response.data;
                });
            } else {
                vm.county.data = [];
            }
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
                });
            }
        };

        vm.download = function(file_name, file_url) {
            window.location.href = 'dt04/download?file_name=' + file_name + '&file_url=' + file_url;
        };

        vm.state_name = function(state) {
            switch (state) {
            case 1:
                return '[县]未报送';
            case 2:
                return '[市]已退回';
            case 3:
                return '[县]已报送';
            case 4:
                return '[市]已备案';
            }
        };

        vm.show = function(btn_name, _this) {
            switch (btn_name) {
            case 'send':
            case 'modify':
            case 'remove':
                if (vm.user.level == 4 && [1, 2].exist(_this.D02)) {
                    return true;
                } else {
                    return false;
                }
            case 'record':
            case 'untread':
                if (vm.user.level == 3 && _this.D02 == 3) {
                    return true;
                } else {
                    return false;
                }
            }
        };

        vm.remove = function(_this) {
            if (confirm('删除后不可恢复，确定删除？')) {
                return acceptance_material_svr.operate(_this.D01, 'remove', function(response) {
                    if (response.data > 0) {
                        vm.search.result.seek('D01', _this.D01, 'del');
                        msg('删除成功！');
                    } else {
                        throw msg(response.data);
                    }
                });
            }
        };

        vm.send = function(_this) {
            if (confirm('报送后不能修改，确定报送？')) {
                return acceptance_material_svr.operate(_this.D01, 'send', function(response) {
                    if (response.data > 0) {
                        _this.D02 = response.data;
                        msg('报送成功！');
                    } else {
                        throw msg(response.data);
                    }
                });
            }
        };

        vm.record = function(_this) {
            if (confirm('备案后不能修改，确定备案？')) {
                return acceptance_material_svr.operate(_this.D01, 'record', function(response) {
                    if (response.data > 0) {
                        _this.D02 = response.data;
                        msg('备案成功！');
                    } else {
                        throw msg(response.data);
                    }
                });
            }
        };

        vm.untread = function(_this) {
            if (confirm('确定退回？')) {
                return acceptance_material_svr.operate(_this.D01, 'untread', function(response) {
                    if (response.data > 0) {
                        _this.D02 = response.data;
                        msg('退回成功！');
                    } else {
                        throw msg(response.data);
                    }
                });
            }
        };
    }
})();