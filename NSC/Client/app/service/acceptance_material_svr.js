(function () {
    'use strict';

    angular.module('app.service')
        .factory('acceptance_material_svr', acceptance_material_svr);

    acceptance_material_svr.$inject = ['svr'];

    function acceptance_material_svr(svr) {

        return {
            station: {
                get_names: get_names,
                search: search
            },
            save: save
        };

        function get_names(callback) {
            
        };

        function save(params, callback) {
            return svr.http('dt04/save?json=' + angular.toJson({
                    DD1: params.county_code,
                    DD2: params.station_name,
                    DD3: params.city_name,
                    DD4: params.county_name,
                    D02: 1,
                    D04: md5.acceptance_report.file_name,
                    D05: md5.acceptance_data.file_name,
                    D06: md5.acceptance_card.file_name,
                    D07: md5.acceptance_report.file_path,
                    D08: md5.acceptance_data.file_path,
                    D09: md5.acceptance_card.file_path
                }), callback);
        };

        function search(code, callback) {
            
        };
    }
})();