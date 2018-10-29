(function() {
    'use-strict'

    function userCommCtrl($uibModal, $rootScope, likeOrDislikeCommentService, $state, commentService) {
        var ctrl = this;

        console.log(ctrl.comment);
       


        


    }

    angular.module('WebSurveying2017').component('userCommentComponent',
        {
            templateUrl: 'AngularClient/Comment/userComment.html',
            bindings: {
                comment : '<'
            },
            controller:
            userCommCtrl
        });

})();