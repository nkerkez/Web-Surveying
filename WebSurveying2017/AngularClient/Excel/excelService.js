(function (angular) {

    angular.module('WebSurveying2017').service('excelService', eService);

    eService.$inject = ['$http'];

    function eService($http) {
        var url = 'api/ExcelFiles';
        var service = {
            getFiles: getFiles
        }

        function getFiles(surveyId) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/' + surveyId
                });
        }


        return service;
    }

}) (angular);