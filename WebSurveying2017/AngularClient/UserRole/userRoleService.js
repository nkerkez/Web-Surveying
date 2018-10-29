(function (angular) {

    angular.module('WebSurveying2017').factory('userRoleService', urs);

    urs.$inject = ['$http'];

    function urs($http) {

        var url = '/api/usersroles';
        var service = {
            updateUserRole: updateUserRole
        };

        function updateUserRole(userId, roleName) {
            return $http(
                {
                    method: 'PUT',
                    url: url + '/' + userId + '/' + roleName
                });
        };

        return service;
    }

}

)(angular);