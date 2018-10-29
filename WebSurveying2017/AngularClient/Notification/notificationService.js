(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('notificationService', notService);

    notService.$inject = ['$http'];
    function notService($http) {

        var url = "api/notifications";
        var service = {
            readNotification : readNotification
        };

        function readNotification(id) {
            return $http({
                method: 'PUT',
                url : url + '/read/' + id
            });
        }

        return service;
    }
})(angular);