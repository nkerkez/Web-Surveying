(function (angular) {

    function likeOrDislikeCompController(likeOrDislikeSurveyService, likeOrDislikeCommentService, helpService, $rootScope, $state, $uibModal,$scope, $uibModalStack) {
        var ctrl = this;

        ctrl.helpService = helpService;

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }

        ctrl.likeOrDislike = function (obj) {

            var service = ctrl.getService();
            console.log(ctrl.entity.LoggedUserLikeOrDislike);
            $scope.message = "";
            if (obj.IsLike)
                $scope.message = "Uspešno ste like-ovali komentar."
            else
                $scope.message = "Uspešno ste dislike-ovali komentar."
            if (ctrl.entity.LoggedUserLikeOrDislike != null) {
                service.putLikeOrDislike(obj).then(
                    
                    function (response) {

                        
                        $scope.image = "../AngularClient/successs.png";
                        ctrl.openModal();

                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                            $state.go('comment', { surveyId: ctrl.entity.SurveyId, id: ctrl.entity.Id }, { reload: true });
                        }, 2666);
                        
                    },
                    function (error) {
                        $scope.message = "Glasanje nije izvršeno. Došlo je do greške.";
                        $scope.image = "../AngularClient/negative.png";
                        ctrl.openModal();
                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                        }, 2666);
                    }
                );
            }
            else {
                service.likeOrDislike(obj).then(
                    function (response) {
                        
                        $scope.image = "../AngularClient/successs.png";
                        ctrl.openModal();

                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                            $state.go('comment', { surveyId: ctrl.entity.SurveyId, id: ctrl.entity.Id }, { reload: true });
                        }, 2666);
                        
                    },
                    function (error) {
                        $scope.message = "Glasanje nije izvršeno. Došlo je do greške.";
                        $scope.image = "../AngularClient/negative.png";
                        ctrl.openModal();
                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                        }, 2666);
                    }
                );
            }
        };


        ctrl.likeOrDislikeClick = function (isLike) {

            
            if (ctrl.entity.Name)
            {
                var obj = {
                    SurveyId: ctrl.entity.Id,
                    IsLike: isLike
                };
            }
            else
            {
                var obj = {
                    CommentId: ctrl.entity.Id,
                    IsLike: isLike
                };
            }
            
            
            ctrl.likeOrDislike(obj);


        };

        ctrl.viewLikes = function () {
            if (ctrl.entity.Name)
                $state.go('likeordislikesurveylist', { id: ctrl.entity.Id });
            else
                $state.go('likeordislikecommentlist', { id: ctrl.entity.Id });
        };

        
        ctrl.getService = function () {
            if (ctrl.entity.Name)
                return likeOrDislikeSurveyService;
            return likeOrDislikeCommentService;
        }

    }
    angular.module('WebSurveying2017').component('likeOrDislikeComponent', {
        templateUrl: 'AngularClient/LikeOrDislike/likeOrDislike.html',
        bindings: {
            entity : '<'

        },
        controller: likeOrDislikeCompController
    })

})(angular);