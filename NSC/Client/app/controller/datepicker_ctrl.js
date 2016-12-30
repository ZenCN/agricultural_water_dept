(function () {
    'use strict';

    angular
        .module('app.widget')
        .controller('datepicker_ctrl', datepicker_ctrl);

    datepicker_ctrl.$inject = ['$scope', 'datepickerPopupConfig'];

    function datepicker_ctrl($scope, datepickerPopupConfig) {
        datepickerPopupConfig.appendToBody = true;  //append the body tag

        $scope.format = 'yyyyƒÍMM‘¬dd»’';

        $scope.today = function () {
            $scope.dt = new Date();
        };
        $scope.today();

        $scope.clear = function () {
            $scope.dt = null;
        };

        $scope.minDate = $scope.dt.getFullYear() - 1 + '-01-01';
        $scope.maxDate = '2025-12-31';

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        //Disable weekend selection
        $scope.disabled = function (date, mode) {
            return ( mode === 'day' && ( date.getDay() === 0 || date.getDay() === 6 ) );
        };

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope[$event.target.attributes['is-open'].value] = true;
        };
    }
})();