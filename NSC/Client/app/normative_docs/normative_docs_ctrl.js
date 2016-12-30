(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('normative_docs_ctrl', normative_docs_ctrl);

    normative_docs_ctrl.$inject = ['$scope', 'normative_svr'];

    function normative_docs_ctrl(vm, normative_svr) {

        vm.year = {
            data: function () {
                var years = [], start_year = 2015, cur_year = new Date().getFullYear() + 1; //start_year 项目启动年份
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
            return normative_svr.search.file_name(vm.year.selected, name, function (response) {
                vm.no_results = response.data.length == 0;

                return response.data;
            });
        };

        vm.search = {
            result: [],
            from_svr: function() {
                normative_svr.search.file(vm.year.selected, vm.file_name, function(response) {
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

        vm.remove = function(id) {
            if (confirm('确定要删除吗？')) {
                normative_svr.remove_file(id, function(response) {
                    if (response.data > 0) {
                        vm.search.result.seek('D01', id, 'del');
                        msg('删除成功');
                    }
                });
            }
        };

        vm.download = function(id) {
            window.location.href = 'dt01/download?list=[' + id + ']';
        };

        vm.preview = function() {
            
        };
    }
})();