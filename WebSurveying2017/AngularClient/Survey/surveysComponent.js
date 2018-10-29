(function (angular) {

    'use-strict'

    function surveysController($state) {
        var ctrl = this;

        ctrl.openSurvey = function (id) {
            $state.go('survey', { 'id': id });
        }
    }
    angular.module('WebSurveying2017').component('surveysComponent', {
        templateUrl: 'AngularClient/Survey/surveys.html',
        bindings: {
           surveys : '='
        },
        controller : surveysController
    });
})(angular);