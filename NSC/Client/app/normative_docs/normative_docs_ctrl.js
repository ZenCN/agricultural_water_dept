(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('normative_docs_ctrl', normative_docs_ctrl);

    normative_docs_ctrl.$inject = ['$scope', 'normative_svr'];

    function normative_docs_ctrl(vm, normative_svr) {
        vm.cur_dt = undefined;
        switch (vm.state.current.value) {
            case 'normative_docs':
                vm.cur_dt = 'dt01';
                break;
            case 'application_materials':
                vm.cur_dt = 'dt02';
                break;
            case 'demonstration_org':
                vm.cur_dt = 'dt03';
                break;
        }

        vm.year = {
            data: function () {
                var years = [], start_year = 2016, cur_year = new Date().getFullYear() + 1; //start_year 项目启动年份
                for (start_year; start_year <= cur_year; start_year++) {
                    years.push(start_year);
                }

                return years;
            },
            selected: new Date().getFullYear()
        };

        vm.no_results = false;
        vm.file_name = undefined;
        vm.file_name_selecting = function (item) {
            vm.file_name = item;
        };
        vm.get_file_names = function(name) {
            return normative_svr.search.file_name(vm.cur_dt, vm.year.selected, name, function (response) {
                vm.no_results = response.data.length == 0;

                return response.data;
            });
        };

        vm.search = {
            result: [],
            from_svr: function() {
                normative_svr.search.file(vm.cur_dt, vm.year.selected, vm.file_name, function (response) {
                    vm.search.result = response.data;

                    if (vm.search.result.length > 0) {
                        $.each(vm.search.result, function () {
                            this.D03 = Date.convert(this.D03);
                        });
                    } else {
                        msg('没有搜到任何文件');
                    }
                });
            }
        };
        vm.search.from_svr();

        vm.remove = function(id) {
            if (confirm('确定要删除吗？')) {
                normative_svr.remove_file(vm.cur_dt, id, function (response) {
                    if (response.data > 0) {
                        vm.search.result.seek('D01', id, 'del');
                        msg('删除成功');
                    }
                });
            }
        };

        vm.download = function(id) {
            window.location.href = vm.cur_dt + '/download?list=[' + id + ']';
        };

        vm.preview = function(id) {
            normative_svr.preview_file(id, vm.cur_dt, function(response) {
                if (isString(response.data)) {
                    window.preview_file_url = response.data;
                    window.open('client/bower_component/pdf-viewer/web/viewer.html');
                }
            });
        };
    }
})();