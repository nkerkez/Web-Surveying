(function () {

    'use-strict'

    angular.module('WebSurveying2017').controller('resetPasswordController', resetPass);

    function resetPass($state, $stateParams, authService, categoryService, $uibModal) {

        var ctrl = this;

        ctrl.resetVM = {
            Email: '',
            Password : '',
            ConfirmPassword : '',
            Code: $stateParams.token
        }

        ctrl.resetPassword = function () {
            console.log(ctrl.resetVM)

            if (ctrl.resetVM.Email.length < 1 || ctrl.resetVM.Password.length < 1 || ctrl.resetVM.ConfirmPassword.length < 1) {
                ctrl.errorMessage = "Email i šifra moraju biti popunjeni";
                return;
            }
            else if (ctrl.resetVM.Password.length < 5) {
                ctrl.errorMessage = "Šifra mora imati više od 4 karaktera";

                return;
            }
            else if (ctrl.resetVM.Password != ctrl.resetVM.ConfirmPassword) {
                ctrl.errorMessage = "Šifre se ne poklapaju, unesite ih opet";
                return;
            }

            authService.resetPasswordWithToken (ctrl.resetVM).then(
                function (response) {
                    $state.go("login");
                },
                function (response) {
                    if (response.data.ModelState)
                        ctrl.errors = response.data.ModelState;
                });
        }

    }

})();