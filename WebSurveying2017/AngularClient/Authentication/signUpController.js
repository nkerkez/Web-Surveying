(function () {

    'use-strict'

    angular.module('WebSurveying2017').controller('signUpController', signUpCtrl);

    function signUpCtrl($state, authService, categoryService, $uibModal) {

        var ctrl = this;

        ctrl.signUpVM = {
            'Email': '',
            'FirstName': '',
            'LastName': '',
            'Password': '',
            'ConfirmPassword': '',
            'RoleName': 'User',
            'Gender': '1'
        };


        ctrl.signUpVM.Birthday = new Date(2004, 1, 1);

        ctrl.minDate = new Date(
            ctrl.signUpVM.Birthday.getFullYear()-100,
            ctrl.signUpVM.Birthday.getMonth(),
            ctrl.signUpVM.Birthday.getDate()
        );

        ctrl.maxDate = new Date(
            ctrl.signUpVM.Birthday.getFullYear(),
            ctrl.signUpVM.Birthday.getMonth() +10,
            ctrl.signUpVM.Birthday.getDate()
        );
        ctrl.message = "";

        ctrl.signUp = function () {
            
            authService.signUp(ctrl.signUpVM).then(
                function (response) {
                    ctrl.signUpVM = {
                        'Email': '',
                        'FirstName': '',
                        'LastName': '',
                        'Password': '',
                        'ConfirmPassword': '',
                        'RoleName': 'User',
                        'Gender': '1'
                    };
                    ctrl.errors = null;
                    ctrl.message = "Na Vašu Email adresu poslata je poruka koja sadrži link za aktivaciju profila";
                },
                function (error) {
                    ctrl.errors = error.data.ModelState;
                }

            );
        }

        
        

    }
    
})();