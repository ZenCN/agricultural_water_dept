(function () {
    'use strict';

    angular
        .module('app.widget')
        .controller('modal_ctrl', modal_ctrl)
        .controller('modal_instance_ctrl', modal_instance_ctrl);

    modal_ctrl.$inject = ['$scope', '$modal'];

    function modal_ctrl(vm, $modal) {

        vm.open = function (type) {
            type = type == 'add' ? '新增' : '修改';

            vm.modal = {
                title: type + '验收资料',
                city_name: 'city_name',
                county_name: 'county_name',
                station_name: undefined,
                remark: undefined,
                error: undefined
            };

            $modal.open({
                templateUrl: 'client/app/controller/modal.html',
                controller: 'modal_instance_ctrl',
                resolve: {
                    modal: function () {
                        return vm.modal;
                    }
                }
            });
        };
    }

    modal_instance_ctrl.$inject = ['$scope', '$modalInstance', 'modal'];

    function modal_instance_ctrl(vm, $modalInstance, modal) {
        vm.modal = modal;

        vm.save = function () {
            $modalInstance.close();
            msg('保存成功！');
        };

        vm.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
})();