(function (angular) {


    'use-strict'

    function userInfoController($stateParams, $rootScope)
    {
        var ctrl = this;

        ctrl.Birthday = new Date(2004, 1, 1);

        ctrl.minDate = new Date(
            ctrl.Birthday.getFullYear() - 100,
            ctrl.Birthday.getMonth(),
            ctrl.Birthday.getDate()
        );

        ctrl.maxDate = new Date(
            ctrl.Birthday.getFullYear(),
            ctrl.Birthday.getMonth() + 10,
            ctrl.Birthday.getDate()
        );

     //   ctrl.isMyProfile = $stateParams.id == $rootScope.loggedInUser.id;
        ctrl.addUserToGroup = function () {
            ctrl.addUser(ctrl.user);
        }

        ctrl.updateProfile = function () {
            console.log("nikola")
            ctrl.update();
        }

        ctrl.resetPasswordClick = function () {
            console.log(ctrl.resetPass);
            ctrl.resetPassword({ obj: ctrl.resetPass });
        }

        ctrl.removeUserFromGroup = function () {
            console.log('anja')
            ctrl.removeFromGroup({ user : ctrl.user});
        }

        ctrl.contains = function () {
            var flag = false;
            
            angular.forEach(ctrl.usersInGroup, function (_user) {
                if (_user.Id == ctrl.user.Id) {
                    flag = true;
                    return;
                }
            });
            return flag;
        }

    }

    angular.module('WebSurveying2017').component('userInfoComponent',
        {
            templateUrl: 'AngularClient/User/userInfo.html',
            bindings: {
                user: "=",
                addUser : "&",
                state: "<",
                update: "&",
                resetPassword: "&",
                usersInGroup: "=",
                removeFromGroup : "&"
            },
            controller : userInfoController
        }
    );
})(angular);