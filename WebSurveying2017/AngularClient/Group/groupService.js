(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('groupService', gService);

    gService.$inject = ['$http'];

    function gService($http) {

        var url = "api/groups";
        var service = {
            getForUser: getForUser,
            getForAuthor: getForAuthor,
            getForMember: getForMember,
            getForAuthorAndMember : getForAuthorAndMember,
            updateGroup: updateGroup,
            getGroups: getGroups,
            _getForMember: _getForMember,
            deleteEntity: deleteEntity
        };



        function deleteEntity(id) {
            return $http(
                {
                    method: 'DELETE',
                    url: url + '/' + id

                });


        }
        function _getForMember() {
            return $http(
                {
                    method: 'GET',
                    url: url + '/member'
                });
        }

        function getForAuthorAndMember() {
            return $http(
                {
                    method: 'GET',
                    url: url + '/authorAndMember'
                });
        }

        function getGroups(search, page, size) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/' + page + '/' + size + search
                });
        }

        function updateGroup(group) {
            return $http({
                'method': 'PUT',
                'url': url + '/' + group.Id,
                'data': group
            });
        }

        function getForUser(search, page, size) {

            return $http({
                'method': 'GET',
                'url': url + '/user' + '/' + page + '/' + size + search
            });
        }
        function getForAuthor() {

            return $http({
                'method': 'GET',
                'url': url + '/author'
            });
        }
        function getForMember(search, page, size) {

            return $http({
                'method': 'GET',
                'url': url + '/member'  + '/' + page + '/' + size + search
            });
        }


        return service;

    };
})(angular);