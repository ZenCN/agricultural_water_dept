(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('login_ctrl', login_ctrl);

    login_ctrl.$inject = ['$rootScope', '$scope', 'login_svr', '$state'];

    function login_ctrl($r_scope, vm, login_svr, $state) {
        vm.province = {
            name: '湖南省',
            code: 43000000,
            level: 2
        };

        vm.city = {
            data: [],
            name: undefined,
            code: '',
            level: 3
        };

        vm.county = {
            data: [],
            name: undefined,
            code: '',
            level: 4
        };

        vm.password = undefined;

        vm.selected = {
            current: 'province',
            change: function (area) {
                if (area == 'city') {
                    if (typeof vm.city.code == 'string' && vm.city.code.length) { //choose a city unit
                        this.current = 'city';
                        login_svr.load_units(vm.city.code, function (response) {
                            vm.county.data = response.data
                        });
                    } else {  //clear
                        this.current = 'province';
                    }
                } else {
                    if (typeof vm.county.code == 'string' && vm.county.code.length) { //choose a county unit
                        this.current = 'county';
                    } else { //clear
                        this.current = 'city';
                    }
                }
            }
        };

        vm.validate = function () {
            if (isString(vm.password)) {
                $('#login').addClass('logining');

                login_svr.validate(vm[vm.selected.current].code, vm.password, function(response) {
                    if (response.data > 0) {

                        $r_scope.user = vm[vm.selected.current];

                        if ($r_scope.user.level > 2) {
                            $r_scope.user.name = $r_scope.user.data.seek('code', $r_scope.user.code, 'name');

                            if ($r_scope.user.level == 4) {
                                $.cookie('city_name', vm.city.data.seek('code', vm.city.code, 'name'));
                            }
                        }

                        delete $r_scope.user.data;

                        $state.go('head.main');
                    } else {
                        $('#login').removeClass('logining');
                        $('.error').html('密码错误');
                    }
                });
            } else {
                $('.error').html('请输入密码');
            }
        };

        login_svr.load_units(vm.province.code, function (response) {
            vm.city.data = response.data;
        });
    }
})();