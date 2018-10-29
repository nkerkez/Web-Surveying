(function(angular){

    angular.module('WebSurveying2017').controller('groupsController', groupCtrl);

    function groupCtrl(groupService, $stateParams, $state) {
        var ctrl = this;
        ctrl.groups = [];

        (function () {
           
            var s = '';
            if ($stateParams.queryString != null)
                s = $stateParams.queryString;


            groupService.getGroups(s, $stateParams.page, $stateParams.size).then(
                function (response) {
                    ctrl.groups = response.data.Models;
                    ctrl.currentPage = response.data.CurrentPage;
                    ctrl.totalItems = response.data.Count;
                    ctrl.size = response.data.Size;
                },
                function (response) {

                }
            );

        })();

        ctrl.searchGroups = function (queryString) {
            
            $state.go('searchGroups', { queryString: queryString, page: 1, size: 10 });

        }

        ctrl.pageChanged = function () {
           
            $state.go("searchGroups", { "page": ctrl.currentPage, "size": $stateParams.size, queryString: $stateParams.queryString });


        }
    }

})(angular);


/*
var input1 = "";
var input2 = "";
var kojiInputPopunjavas = 0;
function kliknuoNaInput() {
    // ako si kliknuo na prvi input kojiInputPopunjavas ti je 1
    // ako si kliknuo na drugi input kojiInputPopunjavas ti je 2
    
}

function kliknuoDugme() {

    // ako je kojiInputPopunjavas = 1 onda dodajes na input1
    // ako je kojiInputPopunjavas = 2 onda dodajes na input2

    //input1 = input1 + vrednostNaKoju si kliknuo;
    //input1 ti je string vidis gore navodnike; kod stringa ti je operator + nalepljivanje
    // primer input1 = "123" , onda ce ti input1 = input1 + 45 biti "12345"
}
*/