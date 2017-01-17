(function() {
    'use strict';

    angular
        .module('app.page')
        .controller('acceptance_material_ctrl', acceptance_material_ctrl);

    acceptance_material_ctrl.$inject = ['$scope', 'acceptance_material_svr', 'login_svr'];

    function acceptance_material_ctrl(vm, acceptance_material_svr, login_svr) {

        vm.city = {
            code: undefined,
            data: [],
            state: {
                data: [
                    { name: '所有', code: 0 },
                    { name: '未备', code: 3 },
                    { name: '已备', code: 4 },
                    { name: '已退', code: 2 }
                ],
                code: 3
            }
        };

        vm.county = {
            code: undefined,
            data: [],
            state: {
                data: [
                    { name: '所有', code: 0 },
                    { name: '未报', code: 1 },
                    { name: '已报', code: 3 },
                    { name: '已退', code: 2 }
                ],
                code: 1
            }
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
            return acceptance_material_svr.get_station_names(name, vm.user.level, function(response) {
                vm.no_results = response.data.length == 0;

                return response.data;
            });
        };

        vm.search = {
            result: [],
            from_svr: function () {
                var state = undefined;
                switch (parseInt(vm.user.level)) {
                    case 2:
                        state = 4;
                        break;
                    case 3:
                        state = vm.city.state.code;
                        break;
                    case 4:
                        state = vm.county.state.code;
                        break;
                }

                return acceptance_material_svr.search({
                    level: vm.user.level,
                    state: state,
                    city_code: vm.city.code,
                    county_code: vm.county.code,
                    station_name: vm.conservancy_station
                }, function(response) {
                    vm.search.result = response.data;
                });
            }
        };
        vm.search.from_svr();

        vm.download = function(file_name, file_url) {
            window.location.href = 'dt04/download?file_name=' + file_name + '&file_url=' + file_url;
        };

        vm.state_name = function(state) {
            switch (state) {
            case 1:
                return '未报';
            case 2:
                return '已退';
            case 3:
                return '已报';
            case 4:
                return '已备';
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

        vm.operate = function(type, _this) {
            var text = undefined;

            switch (type) {
            case 'remove':
                text = '删除后不可恢复，确定删除？';
                break;
            case 'send':
                text = '报送后不能修改，确定报送？';
                break;
            case 'record':
                text = '备案后不能修改，确定备案？';
                break;
            case 'untread':
                text = '确定退回？';
                break;
            }

            if (confirm(text)) {
                return acceptance_material_svr.operate(_this.D01, type, function (response) {
                    if (response.data > 0) {
                        if (type == 'remove') {
                            vm.search.result.seek('D01', _this.D01, 'del');
                        } else {
                            vm.search.from_svr();
                        }
                        msg(text.substr(text.length - 3, 2) + '成功！');
                    } else {
                        throw msg(response.data);
                    }
                });
            }
        };

        vm.preview = function (id, type) {
            acceptance_material_svr.preview_file(id, type, function (response) {
                if (isString(response.data)) {
                    window.preview_file_url = response.data;
                    window.open('client/bower_component/pdf-viewer/web/viewer.html');
                }
            });
        };
    }
})();