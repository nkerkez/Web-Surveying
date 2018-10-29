(function (angular) {

    angular.module('WebSurveying2017').controller('questionResultController', qrCtrl);

    function qrCtrl(questionService, userSurveyService, $stateParams, $state) {
        var ctrl = this;
        ctrl.result = {};
        
        (function () {

            questionService.getQuestion($stateParams.id)
                .then(
                function (response) {
                    console.log(response.data)
                    ctrl.result = response.data;
                },
                function (response) {
                    $state.go('error', { 'code': response.status });
                });
            
        })();

        ctrl.getUsers = function (surveyId) {
            userSurveyService.getUsers(surveyId)
                .then(
                function (response) {
                    console.log(response.data)
                    ctrl.users = response.data.Users;
                },
                function (response) {
                    $state.go('error', { 'code' : response.status });
                });
        }

        ctrl.isUserAnswer = function (user, answerId) {

        }
    }
})(angular)