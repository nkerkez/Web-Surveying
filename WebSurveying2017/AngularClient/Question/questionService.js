(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('questionService', questService);

    questService.$inject = ['$http'];
    function questService($http) {

        var url = "api/questions";
        var service = {
            getQuestion: getQuestion
        };

        function getQuestion(id) {
            return $http({
                method: 'GET',
                url: url + '/' + id
            });
        }

        return service;
    }
})(angular);