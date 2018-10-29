(function (angular) {

    angular.module('WebSurveying2017').controller('categoryController', homeCtrl);


    function homeCtrl(categoryService, surveyService, $stateParams, $state, $rootScope, $uibModal, userCategoryService, helpService, $scope) {
        var ctrl = this;
        ctrl.state = 'VIEW';
        console.log($stateParams.categoryId);
        ctrl.categoryId = $stateParams.categoryId;
        ctrl.helpService = helpService;
        $scope.category = {};
        $scope.service = categoryService;
         
        ctrl.params =
            {
                enum: 5,
                isPublic: true,
                groupId: 0,
                categoryId: ctrl.categoryId,
                subCategories: ctrl.categoryId == 0 ? true : false,
                allSurveys: false,
                page: $stateParams.page,
                size: $stateParams.size,
                search: null,
                state: 3
            };

        (function () {
            var all = false;
            if ($stateParams.flag == 'yes')
                all = true;
            ctrl.params.subCategories = all;
            categoryService.getCategory($stateParams.categoryId).then(
                function (response) {
                    ctrl.category = response.data;
                    $scope.category = response.data;
                    

                },
                function (response) {

                }
            );
            
            categoryService.getCategories().then(
                function (response) {

                    ctrl.rootCategories = response.data;



                },
                function (error) {
                    console.log(error);
                });

            
            var s = '';
            ctrl.params.search = $stateParams.queryString != null ? $stateParams.queryString : '';

            surveyService.getSurveys(ctrl.params).then(
                function (response) {
                    ctrl.surveys = response.data.Models;
                    ctrl.currentPage = response.data.CurrentPage;
                    ctrl.totalItems = response.data.Count;
                    ctrl.size = response.data.Size;
                },
                function (response) {
                    console.log(response.data);
                }
            );

                

         
                 
            
        })();
        ctrl.create = function () {
            var obj = {
                CategoryId: ctrl.category.Id,
                Name: ctrl.name
            };
            
            categoryService.postCategory(obj).then(
                function (response) {
                    $state.go('home', { page: 1, size: 10, state: 3 });
                },
                function (response) {
                });
        }
        ctrl.searchSurveys = function (queryString) {
            console.log(queryString);
            $state.go('searchActiveSurveysWithCategory', { categoryId: $stateParams.categoryId, queryString: queryString, page: 1, size: 10});

        }

        ctrl.deleteCategory = function () {
            $scope.text = "Da li ste sigurni da želite da obrišete kategoriju pod nazivom " + ctrl.category.Name + " ?"
            $scope.Id = ctrl.category.Id;
            var modalInstance = $uibModal.open(
                {
                    templateUrl: 'AngularClient/Delete/deleteModal.html',
                    controller: 'deleteController',
                    controllerAs: 'deleteCtrl',
                    scope: $scope,
                    backdrop : false
                });
        }

        
        ctrl.pageChanged = function () {

            $state.go('searchActiveSurveysWithCategory', { categoryId: $stateParams.categoryId, queryString: queryString, page: ctrl.page, size: ctrl.size });

            

        }

        ctrl.onClick = function (category) {
            if (category == null) {

                $state.go('category', {
                    categoryId: 0, page: 1, size: 10, flag: 'yes'
                });
            }
            else {
                var flagAll = 'no';
                if (category.flagAll == true)
                    flagAll = 'yes';
                $state.go('category', {
                    categoryId: category.Id, page: 1, size: 10, flag: flagAll
                });
            }
        }

    };

})(angular);