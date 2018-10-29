(function(){
    'use-strict'

    angular.module('WebSurveying2017').controller('commentsController',commentsCtrl);

    function commentsCtrl(commentService, $stateParams, surveyService, $state) {

        var ctrl = this;

        (function () {

            commentService.getComments($stateParams.id, $stateParams.page, $stateParams.size).then(
                function (response) {
                    ctrl.comments = response.data.Models;
                    ctrl.currentPage = response.data.CurrentPage;
                    ctrl.totalItems = response.data.Count;
                    console.log(response.data);
                },
                function (error) {

                }
            );

            surveyService.getSurvey($stateParams.id).then(
                function (response) {
                    ctrl.survey = response.data;
                    
                },
                function (error) {

                });
        })();

        ctrl.pageChanged = function ()
        {
            console.log('aaaa')
            $state.go("surveyComments", { "page": ctrl.currentPage, "size": 10, 'id': $stateParams.id  });


        }
    };
})();