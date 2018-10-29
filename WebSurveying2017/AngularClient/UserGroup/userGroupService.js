(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('userGroupService', ugService);

    ugService.$inject = ['$http'];

    function ugService($http) {

        var url = "api/UsersGroups";
        var service = {
            postGroup: postGroup,
            editMembers : editMembers
        };

        function postGroup(obj) {

            return $http({
                'method': 'POST',
                'data': obj,
                'url': url + '/addgroup'
            });
        }


        function editMembers(groupId, data) {

            return $http({
                'method': 'PUT',
                'data': data,
                'url': url + '/' + groupId
            });
        }
        return service;

    };
})(angular);