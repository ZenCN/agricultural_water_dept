(function () {
    'use strict';

    angular.module('app.service')
        .factory('normative_svr', normative_svr);

    normative_svr.$inject = ['svr'];

    function normative_svr(svr) {

        return {
            search: {
                file_name: get_file_name,
                file: get_file
            },
            remove_file: remove_file
        };

        function get_file_name(year, name, callback) {
            return svr.http('dt01/search?year=' + year + '&name=' + name, callback);
        }

        function get_file(year, name, callback) {
            name = name ? name : '';
            return svr.http('dt01/index?year=' + year + '&name=' + name, callback);
        }

        function remove_file(index, callback) {
            return svr.http('dt01/delete?index=' + index, callback);
        }
    }
})();