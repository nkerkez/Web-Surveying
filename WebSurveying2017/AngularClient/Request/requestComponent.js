(function (angular) {

    function requestController(requestService, $state, $stateParams) {
        var ctrl = this;

        ctrl.respondOnRequest = function (response) {
            var obj = {
                groupId: $stateParams.groupId,
                userId: ctrl.user.Id,
                response: response
            }

            requestService.respondOnRequest(obj).then(
                function (response) {
                    $state.go('myGroups', { page : 1, size : 10})
                },
                function (response) {
                    $state.go('myGroups', { page: 1, size: 10 });
                });
        }
    }


    angular.module('WebSurveying2017').component('requestComponent',
        {
            templateUrl: 'AngularClient/Request/request.html',
            bindings: {
                user: '<'
            },
            controller : requestController

        });

})(angular);