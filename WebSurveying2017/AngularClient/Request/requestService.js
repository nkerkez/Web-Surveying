(function (angular) {

    angular.module('WebSurveying2017').factory('requestService', rService);

    rService.$inject = ["$http"];

    function rService($http) {
        var url = 'api/requests';
        var service = {
            getRequests: getRequests,
            postRequest: postRequest,
            respondOnRequest: respondOnRequest
        };

        function getRequests(groupId) {
            return $http(
                {
                    'method': 'GET',
                    'url': url + '/group/' + groupId
                });
        }
        function respondOnRequest(obj) {
            return $http(
                {
                    'method': 'DELETE',
                    'url': url + '/' + obj.groupId + '/' + obj.userId + '/' + obj.response
                });
        }
        function postRequest(requestObj) {
            return $http(
                {
                    'method': 'POST', 
                    'url': url,
                    'data' : requestObj
                });
        }

        return service;
    }
})(angular);