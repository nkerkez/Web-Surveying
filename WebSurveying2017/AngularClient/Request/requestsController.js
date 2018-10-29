(function (angular) {

    angular.module('WebSurveying2017').controller('requestsController', rCtrl);

    function rCtrl(requestService, $stateParams) {

        var ctrl = this;
        ctrl.requests = {};
        (function () {
            requestService.getRequests($stateParams.groupId).then(
                function (response) {
                    ctrl.requests = response.data;
                    console.log(response.data);
                },
                function (response) {
                    alert(' error ')
                }
            );
        })();
    };

})(angular);