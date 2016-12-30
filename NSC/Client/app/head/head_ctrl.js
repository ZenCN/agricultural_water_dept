(function () {
    'use strict';

    angular
        .module('app.layout')
        .controller('head_ctrl', head_ctrl);

    head_ctrl.$inject = ['$scope', '$http', '$state'];

    function head_ctrl(vm, $http, $state) {

        if (!angular.isObject(vm.user)) {
            if ($.cookie('AreaCode')) {
                vm.user = {
                    name: $.cookie('AreaName'),
                    code: $.cookie('AreaCode'),
                    level: $.cookie('Level')
                };
            } else {
                $state.go('login');
            }
        }

        vm.sing_out = function () {
            if (confirm('确定要注销吗？')) {
                $http.get('login/logout').then(function(response) {
                    if (response.data.retVal) {
                        $state.go('login');
                    } else {
                        msg(response.data);
                        throw response.data;
                    }
                });
            }
        };
    }
})();