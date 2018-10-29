(function (angular) {

    angular.module('WebSurveying2017').controller('updateCategoryController', addCategoryCtrl);


    function addCategoryCtrl(categoryService, surveyService, $stateParams, $state, $rootScope, $uibModal, $uibModalStack, $scope , helpService) {
        var ctrl = this;
        ctrl.categories = [];
        ctrl.state = 'ADD';
        ctrl.category = {};
        (function () {
            categoryService.getCategories().then(
                function (response) {
                    ctrl.rootCategories = response.data;
                },
                function (response) {

                }
            );


            categoryService.getCategory($stateParams.id).then(
                function (response) {
                    ctrl.category = response.data;
                },
                function (response) {

                }
            );
            
        })();

        ctrl.onClick = function (category) {
            console.log(category);
            ctrl.category.CategoryId = category.CategoryId;

            categoryService.putCategory(category).then(
                function (response) {
                    $scope.message = "Uspešno ste izmenili kategoriju.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                },
                function (response) {
                    ctrl.errors = response.data.ModelState;
                }
            );
           
        }

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }
    }
})(angular);