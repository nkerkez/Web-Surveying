(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('userExtendService', uService);

    uService.$inject = ['$http'];

    function uService($http) {

        var url = "api/UserExtend";
        var service = {
            get: get
        };

        function get(id) {
            return $http(
                {
                    method: 'GET',
                    url : url + '/' + id
                }
            );
        };

        return service;

    };
})(angular);