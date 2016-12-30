(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('main_ctrl', main_ctrl);

    main_ctrl.$inject = ['$scope', '$state'];

    function main_ctrl(vm, $state) {
        vm.state = {
            current: {
                name: undefined,
                value: $state.current.name.split('.').the_last()
            },
            go: function (state, params) {
                if (state != this.current.value) {
                    $state.go('head.main.' + state, params);
                    this.current.value = state;
                    this.current.name =  get_state_name(this.current.value);
                }
            }
        };

        vm.state.current.name = get_state_name(vm.state.current.value);

        function get_state_name(state) {
            switch (state) {
                case 'normative_docs':
                    return '标准化建设任务完成情况 >> 标准化建设规范性文件';
                case 'acceptance_material':
                    return '标准化建设任务完成情况 >> 验收资料';
                case 'application_materials':
                    return '农民用水合作组织示范创建 >> 申报资料';
                case 'demonstration_org':
                    return '农民用水合作组织示范创建 >> 国家示范组织名单及批复文件';
                default:
                    return '未知';
            };
        };
    }
})();