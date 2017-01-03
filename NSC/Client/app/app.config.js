(function () {
    'use strict';

    angular
        .module('app')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider'];

    function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {
        $ocLazyLoadProvider.config({
            debug: false,
            events: true
        });

        $urlRouterProvider.when('', '/login');
        $urlRouterProvider.otherwise('/login');

        var resolve_dep = function (config) {
            return {
                load: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load(config);
                    }
                ]
            };
        };

        $stateProvider
            .state('login', {
                url: '/login',
                controller: 'login_ctrl',
                templateUrl: 'client/app/login/login.html',
                resolve: resolve_dep([
                    'client/css/login.css',
                    'client/app/login/login_ctrl.js',
                    'client/app/service/login_svr.js'
                ])
            })
            .state('head', {
                abstract: true,
                controller: 'head_ctrl',
                templateUrl: 'client/app/head/head.html',
                resolve: resolve_dep([
                    'client/css/head.css',
                    'client/app/head/head_ctrl.js',
                    'client/app/directive/update_pwd.js'
                ])
            })
            .state('head.main', {
                url: '/main',
                controller: 'main_ctrl',
                templateUrl: 'client/app/main/main.html',
                resolve: resolve_dep([
                    'client/css/common/animate.css',
                    'client/css/common/sidebar.css',
                    'client/bower_component/jquery-metis-menu/metisMenu.css',
                    'client/bower_component/jquery-metis-menu/metisMenu.min.js',
                    'client/css/main.css',
                    'client/app/main/main_ctrl.js',
                    'client/app/directive/metis_menu.js'
                ])
            })
            /*------------------------标准化建设任务完成情况------------------------*/
            .state('head.main.normative_docs', {    //规范性文件
                url: '/normative_docs',
                controller: 'normative_docs_ctrl',
                templateUrl: 'client/app/normative_docs/normative_docs.html',
                resolve: resolve_dep([
                    'client/css/common/query.css',
                    'client/bower_component/bootstrap-fileinput/css/fileinput.min.css',
                    'client/app/normative_docs/normative_docs_ctrl.js',
                    'client/bower_component/bootstrap-fileinput/js/fileinput.min.js',
                    'client/bower_component/jquery-tooltipster/css/tooltipster.bundle.min.css',
                    'client/bower_component/jquery-tooltipster/css/plugins/tooltipster/sideTip/themes/tooltipster-sideTip-shadow.min.css',
                    'client/bower_component/jquery-tooltipster/js/tooltipster.bundle.js',
                    'client/bower_component/md5.min.js',
                    'client/app/directive/multiple_upload.js',
                    'client/app/service/normative_svr.js'
                ])
            })
            .state('head.main.acceptance_material', {   //验收资料
                url: '/acceptance_material',
                controller: 'acceptance_material_ctrl',
                templateUrl: 'client/app/acceptance_material/acceptance_material.html',
                resolve: resolve_dep([
                    'client/bower_component/bootstrap-fileinput/css/fileinput.min.css',
                    'client/app/acceptance_material/acceptance_material_ctrl.js',
                    'client/app/service/acceptance_material_svr.js',
                    'client/app/controller/modal_ctrl.js',
                    'client/app/directive/sing_upload.js',
                    'client/bower_component/bootstrap-fileinput/js/fileinput.min.js'
                ])
            })
            /*------------------------农民用水合作组织示范创建------------------------*/
            .state('head.main.application_materials', {   //申报资料
                url: '/application_materials',
                controller: 'normative_docs_ctrl',
                templateUrl: 'client/app/normative_docs/normative_docs.html',
                resolve: resolve_dep([
                    'client/css/common/query.css',
                    'client/bower_component/bootstrap-fileinput/css/fileinput.min.css',
                    'client/app/normative_docs/normative_docs_ctrl.js',
                    'client/bower_component/bootstrap-fileinput/js/fileinput.min.js',
                    'client/bower_component/jquery-tooltipster/css/tooltipster.bundle.min.css',
                    'client/bower_component/jquery-tooltipster/css/plugins/tooltipster/sideTip/themes/tooltipster-sideTip-shadow.min.css',
                    'client/bower_component/jquery-tooltipster/js/tooltipster.bundle.js',
                    'client/bower_component/md5.min.js',
                    'client/app/directive/upload_file.js',
                    'client/app/service/normative_svr.js'
                ])
            })
            .state('head.main.demonstration_org', {   //示范组织名单及批复文件
                url: '/demonstration_org',
                controller: 'normative_docs_ctrl',
                templateUrl: 'client/app/normative_docs/normative_docs.html',
                resolve: resolve_dep([
                    'client/css/common/query.css',
                    'client/bower_component/bootstrap-fileinput/css/fileinput.min.css',
                    'client/app/normative_docs/normative_docs_ctrl.js',
                    'client/bower_component/bootstrap-fileinput/js/fileinput.min.js',
                    'client/bower_component/jquery-tooltipster/css/tooltipster.bundle.min.css',
                    'client/bower_component/jquery-tooltipster/css/plugins/tooltipster/sideTip/themes/tooltipster-sideTip-shadow.min.css',
                    'client/bower_component/jquery-tooltipster/js/tooltipster.bundle.js',
                    'client/bower_component/md5.min.js',
                    'client/app/directive/upload_file.js',
                    'client/app/service/normative_svr.js'
                ])
            });
    }
})();