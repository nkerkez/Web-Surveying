(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').controller('likeOrDislikeSurveyController', ctrl);

    function ctrl(likeOrDislikeSurveyService, $stateParams) {

        var ctrl = this;

        ctrl.users = [];

        ctrl.viewLikes = function () {
            likeOrDislikeSurveyService.getLikesForSurvey($stateParams.id).then(
                function (response) {

                    ctrl.users = response.data;

                },
                function (error) {

                }
            );
        };

        ctrl.viewDislikes = function () {
            likeOrDislikeSurveyService.getDislikesForSurvey($stateParams.id).then(
                function (response) {

                    ctrl.users = response.data;

                },
                function (error) {

                }
            );
        };
        (function () {

            ctrl.viewLikes();

        })();
    }
})(angular);