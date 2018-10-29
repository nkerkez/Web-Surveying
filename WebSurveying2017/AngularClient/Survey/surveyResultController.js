(function (angular) {

    angular.module('WebSurveying2017').controller('surveyResultController', surveyResultCtrl);


    function surveyResultCtrl(surveyService, $stateParams, $state) {

        var vm = this;
        vm.per = [];
        vm.selected = "1";
        vm.perSelectClick = function () {
            console.log(vm.selected);

            vm.survey.Questions.forEach(function (value, index) {
                
                if (vm.selected == "1")
                {
                    
                    vm.per[index] = String(vm.survey.NumbOfUsers);
                }
                else if (vm.selected == "2")
                {
                    console.log(2);
                    vm.per[index] = String(value.NumbOfUsers);
                }
                else
                {

                    
                    vm.per[index] = String(value.NumbOfUA);
                }
            });
        };

        console.log($stateParams.id, $stateParams.userId);
        vm.questionResult = function (id) {
            $state.go('question', { id: id });
        }
        surveyService.getWithResult($stateParams.id, $stateParams.userId).then(
            function (response) {
                console.log(response.data);
                vm.survey = response.data;
                for (var i = 0; i < vm.survey.Questions.length; i++) {
                   vm.per[i] = String(vm.survey.NumbOfUsers);
                }
                
            },
            function (error) {

            });
    }

})(angular);