(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').controller('groupUsersController', groupUsersCtrl);

    function groupUsersCtrl($state, $stateParams, userService, categoryService, userCategoryService, authService) {

        var ctrl = this;


        (function () {

            userService.getUsersForGroup($stateParams.id).then(
                function (response) {
                    ctrl.users = response.data;
                    console.log(ctrl.user);
                },
                function () {

                }
            );

        })();

    }
})(angular);