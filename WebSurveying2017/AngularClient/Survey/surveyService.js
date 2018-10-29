(function (angular) {

    'use-strict'
    angular.module('WebSurveying2017').factory('surveyService', surveyService);

    surveyService.$inject = ['$http'];

    function surveyService($http) {

        var url = '/api/surveys';

        var service = {
            getSurveys : getSurveys,
            getAvailableSurveys: getAvailableSurveys,
            getForCategory: getForCategory,
            getPublic: getPublic,
            getForGroup: getForGroup,
            getForAuthor: getForAuthor,
            getFilledForUser: getFilledForUser,
            getSurvey: getSurvey,
            getSurveyWithUserAnswers: getSurveyWithUserAnswers,
            addSurvey: addSurvey,
            getWithResult: getWithResult,
            changeStateOfSurvey: changeStateOfSurvey,
            putSurvey: putSurvey,
            getFavoriteSurveys: getFavoriteSurveys,
            download: download,
            saveFile: saveFile,
            changeCategory: changeCategory,
            deleteEntity: deleteEntity
        };

        function deleteEntity(id) {
            return $http(
                {
                    method: 'DELETE',
                    url: url + '/' + id

                });


        }
        function download() {
            return $http(
                {
                    method: 'GET',
                    url: url + '/download',
                    responseType: 'arraybuffer'
                });

        }
        function putSurvey(survey) {

            return $http({
                method: 'PUT',
                url: url + '/' + survey.Id,
                data: survey,
                headers: {
                    "Content-Type": "application/json"
                }
            });
        }

        function changeStateOfSurvey(survey) {


            return $http({
                method: 'PUT',
                url: url + '/changeState/' + survey.Id,
                data: survey
            });
        }
        function getSurveys(obj) {
            return $http(
                {
                    method: 'GET',
                    headers:
                        {
                            'Content-Type' :'application/json'
                        },
                    url: url + '/' + obj.enum + '/' + obj.isPublic +  '/' +  obj.groupId  + '/' + obj.categoryId + '/' + obj.subCategories + '/' + obj.allSurveys + '/' + obj.state + '/' +  obj.page + '/' + obj.size + obj.search 
                     
                });
        }

        function changeCategory(survey) {
            return $http({
                method: 'PUT',
                url: url + '/changeCategory',
                data: survey,
                headers: {
                    "Content-Type": "application/json"
                }
            });

        }
        function addSurvey(survey) {
         
            return $http({
                method: 'POST',
                url: url,
                data: survey
            });
        }

        function saveFile(id) {

            return $http({
                method: 'GET',
                url: url + '/' + id + '/save'
            });
        }

        function getAvailableSurveys(search, page, size) {

            return $http({
                method: 'GET',
                url: url + '/active/' + page + '/' + size + search
            });

        }
        function getFavoriteSurveys(search, page, size) {

            return $http({
                method: 'GET',
                url: url + '/active/favorite/' + page + '/' + size + search
            });

        }
        function getForCategory(id, search, page, size, all ) {

            return $http({
                method: 'GET',
                url: url + '/active/category/' + id + '/' + all + '/' + page + '/' + size + '' +  search
            });
        }
        function getFilledForUser(search, page, size) {

            return $http({
                method: 'GET',
                url: url + '/active/filled/' + page + '/' + size + '' + search
            });
        }
        function getPublic(search, page, size) {

            return $http({
                method: 'GET',
                url: url + '/active/public/' + page + '/' + size + '' + search
            });
        }
        function getForGroup(id, search, page, size) {

            return $http({
                method: 'GET',
                url: url + '/active/group/' + id + '/' + page + '/' + size  + '' + search
            });
        }
        function getForAuthor(search, page, size) {

            return $http({
                method: 'GET',
                url: url + '/active/author/' + page + '/' + size + '' + search
            });
        }
        function getSurvey(surveyId) {

            return $http({
                method: 'GET',
                url: url + '/' + surveyId
            });
        }

        function getSurveyWithUserAnswers(surveyId) {

            return $http({
                method: 'GET',
                url: url + '/useranswers/' + surveyId
            });
        }

        function getWithResult(surveyId, userId) {

            return $http({
                method: 'GET',
                url: url + '/results/' + surveyId + "/" + userId
            });
        }
        return service;
    }
})(angular);