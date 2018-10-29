(function (angular) {

    'use-strict'


    function notificationCtrl(requestService, $state) {

        var ctrl = this;

        
        
        ctrl.onClick = function (onlyRead)
        {
            ctrl.clickOnNotification({ n: ctrl.notification, onlyRead : onlyRead });
        }

        ctrl.repsondOnRequest = function (response) {
            console.log(ctrl.notification);
            var obj = {
                groupId: ctrl.notification.GroupId,
                userId: ctrl.notification.UserId,
                response: response
            };
            requestService.respondOnRequest(obj).then(
                function (response) {
                    $state.go('myGroups', { page : 1 , size : 10});
                },
                function (response) {
                    $state.go('myGroups', { page: 1, size: 10 });
                });
        }
    }

    angular.module('WebSurveying2017').component('notificationComponent', {

        templateUrl: 'AngularClient/Notification/notification.html',
        bindings: {
            notification: "=",
            clickOnNotification : "&"
        },
        controller : notificationCtrl
    });
})(angular);