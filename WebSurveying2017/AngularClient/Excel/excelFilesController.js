(function (angular) {


    angular.module('WebSurveying2017').controller('excelFilesController', eCtrl);

    function eCtrl(excelService, $stateParams, $state) {
        var ctrl = this;
        (function () {
            excelService.getFiles($stateParams.id)
                .then(
                function (response) {
                    ctrl.files = response.data;
                },
                function (response) {
                    $state.go('error', {'code':response.status});
                }
                );
        })();

        ctrl.download = function (id) {

            window.open('http://localhost:49681/api/surveys/download/' + id);
            /*
            surveyService.download().then(
                function (response) {
               //     $state.go('user', { id: 1 });
                    },
                function (response) {
                    console.log('error');
                });
            */
        }
    }
}) (angular)