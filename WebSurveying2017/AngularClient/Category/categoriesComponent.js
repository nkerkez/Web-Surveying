(function (angular) {

    function categoriesController($uibModalStack, $rootScope, categoryService, $stateParams, helpService) {

        var ctrl = this;
        ctrl.category = {};
        ctrl.selectedCategories = [];
        ctrl.updateFlag = false; //update or add
        
        (function () {
           
        }) ();
        ctrl.initModCats = function (parentList) {
            
            angular.forEach(parentList, function (parent) {
                if (parent.IsModerator)
                    ctrl.addCategory(parent);

                ctrl.initModCats(parent.SubCategories);
            });
            
        }
        

        ctrl.$onInit = function () {
            if (ctrl.category == null)
                ctrl.category = {};
            if (ctrl.state == 'MODERATOR') {
                ctrl.initModCats(ctrl.roots);
                if (ctrl.selectedCategories.length > 0)
                    ctrl.updateFlag = true;
            }
            
        };
        
        ctrl.checkboxClick = function (category) {

            console.log(1)
            category.IsModerator = !category.IsModerator;
            
            if (category.IsModerator === true) {
                ctrl.addCategory(category);
            }
            else
            {

                ctrl.removeCategory(category);

            }
            console.log(ctrl.selectedCategories);
            
        };
        ctrl.addCategory = function (category)
        {
            if (!ctrl.selectedCategories.includes(category.Id.toString())) {
                ctrl.selectedCategories.push(category.Id.toString());
                category.IsModerator = true;
            }
            console.log(ctrl.selectedCategories);
        }

        ctrl.removeCategory = function (category) {
            angular.forEach(ctrl.selectedCategories, function (cat) {
                var _index = ctrl.selectedCategories.indexOf(cat);
                if (cat == category.Id) {
                    ctrl.selectedCategories.splice(_index, 1);
                    category.IsModerator = false;
                }

            });
        }

        ctrl.removeWithSub = function (category) {
            ctrl.removeCategory(category);
            category.IsModerator = false;
                angular.forEach(category.SubCategories, function (cat) {
                    ctrl.removeWithSub(cat);

                });

            

            

        };

        ctrl.addWithSub = function (category)
        {
            ctrl.addCategory(category);
            category.IsModerator = true;
            angular.forEach(category.SubCategories, function (cat) {
                ctrl.addWithSub(cat);

            });

            
        }

        ctrl.a = function (c)
        {
            console.log('bbbbbbbbbbbb')
            categoryService.putCategory(c).then(
                function (response) {
                    $state.go('home');
                },
                function (response) {

                }
            );
        }

        ctrl.addCategoriesForSearch = function ()
        {
            ctrl.onCategoryClick({ category: ctrl.selectedCategories });
            $uibModalStack.dismissAll();
        }

        ctrl.addCategoriesToModerator = function () {

            console.log(ctrl.updateFlag);

            var category = ctrl.selectedCategories;
            category.isUpdate = ctrl.updateFlag;
            

            ctrl.onCategoryClick({ category: category });
            
            $uibModalStack.dismissAll();
        }
        ctrl.categoryClick = function (category, flag) {
            if (category != null)
                category.flagAll = flag;
            console.log(ctrl.category)
            if (ctrl.state === 'ADD') {
                if (category != null)
                    ctrl.category.CategoryId = category.Id;
                else
                    ctrl.category.CategoryId = null;

                console.log(ctrl.category)
                ctrl.onCategoryClick({ category: ctrl.category });

                $uibModalStack.dismissAll();
            }
            else if (ctrl.state === 'SET' || ctrl.state === 'VIEW'){
            //    console.log(category.Id);
                ctrl.onCategoryClick({ category: category });

                $uibModalStack.dismissAll();
            }
            
            
            
        };
    };
    
    angular.module('WebSurveying2017').component('categoriesComponent', {
        templateUrl: 'AngularClient/Category/categories.html',
        bindings: {
            roots: '=',
            onCategoryClick: '&',
            state: '<', 
            category: '<',
            selectedCategories : '<'
        },
        controller : categoriesController
    });
})(angular);