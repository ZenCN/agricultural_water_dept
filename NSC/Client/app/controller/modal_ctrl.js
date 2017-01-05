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
                city_name: $.cookie('city_name'),
                county_name: vm.user.name,
                county_code: vm.user.code,
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

    modal_instance_ctrl.$inject = ['$scope', '$modalInstance', 'modal', 'acceptance_material_svr'];

    function modal_instance_ctrl(vm, $modalInstance, modal, acceptance_material_svr) {
        vm.modal = modal;

        vm.save = function () {
            var $acceptance_report = $('#acceptance_report'),
                $acceptance_data = $('#acceptance_data'),
                $acceptance_card = $('#acceptance_card');

            if (!isString(vm.modal.station_name)) {
                vm.modal.error = '水利站名称未填写';
            } else if ($acceptance_report.fileinput('getFilesCount') == 0) {
                vm.modal.error = '未选择验收报告';
            } else if ($acceptance_data.fileinput('getFilesCount') == 0) {
                vm.modal.error = '未选择申报资料';
            } else if ($acceptance_card.fileinput('getFilesCount') == 0) {
                vm.modal.error = '未选择验收卡';
            } else {
                $acceptance_report.fileinput('upload');
                $acceptance_data.fileinput('upload');
                $acceptance_card.fileinput('upload');

                (function all_uploaded() {
                    if (md5.acceptance_report.uploaded == md5.acceptance_data.uploaded == md5.acceptance_card.uploaded == true) {

                        acceptance_material_svr.save(vm.modal, function(response) {
                            if (response.data > 0) {
                                delete md5.acceptance_report;
                                delete md5.acceptance_data;
                                delete md5.acceptance_card;

                                msg('保存成功!');
                                $modalInstance.close();
                            } else {
                                throw msg(response.data);
                            }
                        });

                        return;
                    }
                    setTimeout(all_uploaded, 100);
                })();
            }
        };

        vm.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
})();