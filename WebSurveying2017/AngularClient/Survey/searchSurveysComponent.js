(function (angular) {

    function searchSurveysController($uibModal, categoryService, $stateParams, helpService) {
        var ctrl = this;
        console.log($stateParams);
        ctrl.selectedCategories = [];
        ctrl.showChoiceCategoriesButton = true;
        (function () {
            if ($stateParams.categoryId != null) {
                ctrl.showChoiceCategoriesButton = false;
            }
            if ($stateParams.queryString != null && $stateParams.queryString != '') {
                ctrl.obj = helpService.fromQueryStringToObj($stateParams.queryString);
                
                if (ctrl.obj.ListOfCategories) {
                    console.log(ctrl.obj.ListOfCategories);
                    if (Array.isArray(ctrl.obj.ListOfCategories))
                        ctrl.selectedCategories = ctrl.selectedCategories.concat(ctrl.obj.ListOfCategories);
                    else
                        ctrl.selectedCategories.push(ctrl.obj.ListOfCategories);
                    console.log(ctrl.selectedCategories);
                }
                
            }
            else {
                ctrl.obj = {
                    Name: '',
                    Description: '',
                    AuthorFirstName: '',
                    AuthorLastName: '',
                    QuestionText: ''
                }
            }
                
        }) ();
       

        


        ctrl.search = function () {
            ctrl.queryString = $('#searchSurveysForm').serialize();
            ctrl.queryString = '?' + ctrl.queryString;

            angular.forEach(ctrl.selectedCategories, function (category) {
                ctrl.queryString += '&ListOfCategories=' + category;
            });
            console.log(ctrl.queryString)
            ctrl.searchSurveys({ queryString: ctrl.queryString });
        }

        ctrl.initCategotyFlags = function (categories) {
            angular.forEach(categories, function (c) {

                console.log(ctrl.selectedCategories, c.Id)
                if (ctrl.selectedCategories.includes(c.Id.toString())) {
                    c.IsModerator = true;
                }

                ctrl.initCategotyFlags(c.SubCategories);
            })
        }
        ctrl.initCatsAndOpenModal = function () {

            

            categoryService.getCategories().then(
                function (response) {
                    ctrl.categories = response.data;
                    ctrl.initCategotyFlags(ctrl.categories);
                    ctrl.openModal();

                },
                function (error) {

                });

        };


        ctrl.openModal = function () {

            

            var modalInstance = $uibModal.open({

                templateUrl: 'AngularClient/Category/categoriesModal.html',
                controller: function () {
                    console.log('a233');

                    var choiceCtrl = this;

                    choiceCtrl.state = 'SEARCH';
                    choiceCtrl.rootCategories = ctrl.categories;
                    choiceCtrl.selectedCategories = ctrl.selectedCategories;

                    // u ovom slucaju kategorija je niz kategorija
                    choiceCtrl.onClick = function (category) {
                        

                        ctrl.ListOfCategories = category
                        ctrl.categoryName = category.Name;
                        console.log(ctrl.ListOfCategories);


                    }
                },
                controllerAs: 'vm',
                backdrop: false
            });

        };
    }

    angular.module('WebSurveying2017').component('searchSurveysComponent',
        {
            templateUrl: 'AngularClient/Survey/searchSurveys.html',
            bindings:
            {
                searchSurveys: '&'
            },
            controller: searchSurveysController
        });
})(angular);