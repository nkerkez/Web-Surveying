(function (angular) {

    angular.module('WebSurveying2017').controller('homeController', homeCtrl);

    
    function homeCtrl(categoryService, surveyService, $stateParams, $state, $rootScope) {
        var ctrl = this;
        ctrl.state = 'VIEW';
        ctrl.typeOfSurveys = 'all';
        ctrl.params =
            {
            enum: 5,
            isPublic: true,
            groupId: 0,
            categoryId: 0,
            subCategories: false,
            allSurveys : true,
            page: $stateParams.page,
            size: $stateParams.size,
            state : $stateParams.state,
            search : null
            };

        (function () {

            
            var username = localStorage.getItem('username');
            $rootScope.username = username;

            
            categoryService.getCategories().then(
                function (response) {
                    ctrl.rootCategories = response.data;
                },
                function (response) {
                    console.log(response.data);
                });

            
            
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
                        console.log(response.data);
                    }
                );
            //} else {

            //    categoryService.getCategoryWithSurveys($stateParams.id).then(
            //        function (response) {
            //            ctrl.surveys = response.data.Surveys;
            //        },
            //        function (response) {
            //            console.log(response.data);
            //        }
            //    );
            //}
        })();

        ctrl.pageChanged = function () {
            console.log('bbbbb')
            $state.go("searchAllSurveys", { "page": ctrl.currentPage, "size": $stateParams.size, queryString: $stateParams.queryString, state : $stateParams.state });


        }

        ctrl.searchSurveys = function (queryString)
        {
            console.log(queryString);
            $state.go('searchAllSurveys', { queryString: queryString, page: 1, size: 10, state : $stateParams.state });
            
        }

        ctrl.changeStateValue = function (state) {
            $state.go('searchAllSurveys', { queryString: $stateParams.queryString, page: 1, size: 10, state: state });
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