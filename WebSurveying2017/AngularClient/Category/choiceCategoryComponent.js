(function (angular) {

    function choiceCategoryController(helpService, $rootScope, $state, categoryService, $uibModal) {
        var ctrl = this;

        ctrl.categories = [];

        ctrl.initCats = function (operation) {

            
                categoryService.getCategories().then(
                    function (response) {
                        ctrl.categories = response.data;

                        ctrl.openModal(operation);

                    },
                    function (error) {

                    });
            
        };

        ctrl.setCategory = function () {
            ctrl.initCats('SET');
        };

        
        ctrl.openModal = function (operation) {

            
            var modalInstance = $uibModal.open({

                templateUrl: 'AngularClient/Category/categoriesModal.html',
                controller: function () {

                    
                    var choiceCtrl = this;
                    choiceCtrl.state = 'VIEW';
                    choiceCtrl.rootCategories = ctrl.categories;
                    choiceCtrl.onClick = function (category) {
                            
                        if (category == null) {
                            ctrl.survey.CategoryId = null;
                            ctrl.categoryName = "Bez kategorije";
                        }
                        else {
                            ctrl.survey.CategoryId = category.Id;
                            ctrl.categoryName = category.Name;
                        }
                        
                            

                            
                        
                    }
                },
                controllerAs: 'vm',
                backdrop: false
            });

        };

        

    }
    angular.module('WebSurveying2017').component('choiceCategoryComponent', {
        templateUrl: 'AngularClient/Category/choiceCategory.html',
        bindings: {
            survey: '<'

        },
        controller: choiceCategoryController

    })

})(angular);