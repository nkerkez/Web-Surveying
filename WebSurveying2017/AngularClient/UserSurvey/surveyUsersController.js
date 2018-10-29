(function (angular) {

    angular.module('WebSurveying2017').controller('surveyUsersController', suCtrl);

    function suCtrl(userSurveyService, $stateParams, $state) {
        var ctrl = this;
        (function () {
            userSurveyService.getUsers($stateParams.id).then(
                function (response) {
                    ctrl.obj = response.data;
                },
                function (response) {

                });

        })();

        ctrl.userProfile = function (userId) {
            $state.go('user', { id: userId });
        }

        ctrl.viewResult = function (userId) {
            console.log(userId);
            var res = userId.replace(/\//g, "*");
            res = res.replace(/\+/g, '=')
            console.log(res);
            console.log('aaa');
            $state.go("surveyresult", { id: ctrl.obj.SurveyId, userId: res });
        }
    }

})(angular)