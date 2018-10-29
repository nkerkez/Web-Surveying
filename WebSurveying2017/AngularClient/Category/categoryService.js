(function (angular) {

    angular.module("WebSurveying2017").factory('categoryService', categoryService);

    categoryService.$inject = ['$http'];
    function categoryService($http) {
        var url = "/api/categories";
        var service = {
            getCategories: getCategories,
            postCategory: postCategory,
            getCategory: getCategory,
            putCategory: putCategory,
            deleteEntity : deleteEntity
        };
        function getCategories() {

            return $http({
                method: 'GET',
                url : url
            });
        }

        function getCategory(categoryId) {
            return $http({
                method: 'GET',
                url: url + '/' + categoryId
            });
        }

        function postCategory(category) {
            return $http({
                method: 'POST',
                url: url,
                data: category
            });
        }

        function putCategory(category)
        {
            return $http({
                method: 'PUT',
                url: url + '/' + category.Id,
                data: category
            });
        }
        function deleteEntity(id) {
            return $http({
                method: 'DELETE',
                url: url + '/' + id
            });
        }
        return service;
    }
})(angular);