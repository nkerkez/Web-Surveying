(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('userService', ['$http', '$q', function ($http, $q) {

        var url = 'api/users'

        var userFactory = {};

        var getUsers = function (page, size) {
            return $http(
                {
                    method: 'GET',
                    url: url  + '/' + page + '/' + size
                });
        }

        var getUsersForGroup = function (id) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/group/' + id
                });
        }

        var searchUsers = function (queryString, page, size) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/search/' + page + '/' + size + queryString
                });
        }
        var getUser = function (id) {

            return $http(
                {
                    method: 'GET',
                    url: url + '/' + id
                });
        };

        var putUser = function (obj) {

            return $http(
                {
                    method: 'PUT',
                    url: url + '/' + obj.Id,
                    data : obj
                });
        };

        var getModeratorsForCategory = function (categoryId) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/moderators/' + categoryId
                });
        };

        var getByUsername = function (username) {

            return $http({
                method: 'GET',
                url: url + '/username/' + username
            });
        };
        

        userFactory.getModeratorsForCategory = getModeratorsForCategory;
        userFactory.getUser = getUser;
        userFactory.putUser = putUser;
        userFactory.getUsers = getUsers;
        userFactory.searchUsers = searchUsers;
        userFactory.getUsersForGroup = getUsersForGroup;
        return userFactory;

    }]);

})(angular);