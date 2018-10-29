(function (angular) {
    angular.module('WebSurveying2017').controller('errorController', errorCtrl);

    function errorCtrl($stateParams) {
        var ctrl = this;
        ctrl.message = "";
        if ($stateParams.code == '401') {
            ctrl.message = "Nemate mogućnost izvršavanje funkcije"
        }
    }
})(angular);