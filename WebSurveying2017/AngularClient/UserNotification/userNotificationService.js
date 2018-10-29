(function(angular)
{
    'use-strict'

    angular.module('WebSurveying2017').factory('userNotificationService',  uNService);

    uNService.$inject = ['$http'];

    function uNService($http)
    {
        var url = 'api/usersnotifications';
        var service = {
            getAllForUser: getAllForUser,
            getPersonalForUser: getPersonalForUser,
            getModeratorForUser: getModeratorForUser,
            readNotification: readNotification
        };

        function getAllForUser() {
            return $http(
                {
                    method: 'GET',
                    url: url + '/all'
                });
        };

        function getPersonalForUser() {
            return $http(
                {
                    method: 'GET',
                    url: url + '/personal'
                });
        };

        function getModeratorForUser() {
            return $http(
                {
                    method: 'GET',
                    url: url + '/moderator'
                });
        };

        function readNotification(obj) {
            return $http({
                method: 'PUT',
                url: url + '/' + obj.NotificationId,
                data : obj
            });
        }
        return service;
    }

})(angular);