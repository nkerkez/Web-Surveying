(function(){
    'use-strict'


    angular.module('WebSurveying2017').controller('commentController', commentCtrl);

    function commentCtrl(surveyService, commentService, $stateParams) {
        var ctrl = this;
        ctrl.comments = [];
        ctrl.extendedComents = [];

        ctrl.commentId = $stateParams.id;

        ctrl.initShowSubs = function (list, value) {

            angular.forEach(list, function (item) {
                item.showSubs = true;
                ctrl.initShowSubs(item.SubComments, value);
            });
        };

        (function () {
            commentService.getComment($stateParams.id).then(
                function (response) {
                    console.log(response.data);
                    ctrl.comments.push(response.data);
                    ctrl.initShowSubs(ctrl.comments, true);
                },
                function (error) {

                }
            );

            surveyService.getSurvey($stateParams.surveyId).then(
                function (response) {
                    ctrl.survey = response.data;
                },
                function (error) {

                }
            );


        })();

    }

})();