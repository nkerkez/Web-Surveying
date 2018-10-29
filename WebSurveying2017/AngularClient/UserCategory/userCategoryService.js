(function(angular){

    'use-strict'

    angular.module('WebSurveying2017').factory('userCategoryService', ucs);

    ucs.$inject = ['$http'];
    function ucs($http) {

        var url = '/api/userscategories';
        var service = {
            postCategoriesForModerator: postCategoriesForModerator,
            putCategoriesForModerator: putCategoriesForModerator,
            putModeratorsForCategory: putModeratorsForCategory,
            getForModerator: getForModerator
        };

        function getForModerator(modId) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/categories/' + modId
                });
        };

        function putModeratorsForCategory(data, catId) {
            return $http(
                {
                    method: 'PUT',
                    url: url + '/category/' + catId,
                    data: data
                });
        };

        function postCategoriesForModerator(data) {
            return $http(
                {
                    method: 'POST',
                    url: url,
                    data: data
                });
        };

        function putCategoriesForModerator(data, userId) {
            return $http(
                {
                    method: 'PUT',
                    url: url + '/' + userId,
                    data: data
                });
        };

        return service;


    }

})(angular);