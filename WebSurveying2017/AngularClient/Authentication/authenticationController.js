(function () {

    'use-strict'

    angular.module('WebSurveying2017').controller('authenticationController', authCtrl);

    function authCtrl($location, $scope, authService, $state, $scope, $uibModalStack, $uibModal) {
        var ctrl = this;
        ctrl.message = null;
        ctrl.loginVM = {
            Username: '',
            Password: ''
        };

        ctrl.authExternalProvider = function (provider) {

            var redirectUri = location.protocol + '//' + location.host + '/test.html';
            console.log(redirectUri);
            
            var externalProviderUrl = "/api/Account/ExternalLogin?provider=" + provider
                + "&response_type=token&client_id=self"
                + "&redirect_uri=" + redirectUri;
            window.$windowScope = $scope;

           // window.location = redirectUri;
           var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
            
        };
      
        ctrl.forgotPassword = function () {
                authService.forgotPassword(ctrl.loginVM).then(
                    function (response) {
                        ctrl.message = "Na Vašu email adresu poslat je link za resetovanje šifre."
                    },
                    function (response) {
                        ctrl.errors = null;
                        ctrl.errors = response.data.ModelState;
                    });
         }

        ctrl.login = function () {


            if (ctrl.loginVM.Username.length < 1 || ctrl.loginVM.Password.length < 1) {
                ctrl.errorMessage = "Email i šifra moraju biti popunjeni"
                return;
            }
            else if (ctrl.loginVM.Password.length < 6)
            {
                ctrl.errorMessage = "Šifra mora imati više od 5 karaktera"
                return;
            }

            var loginData = {
                userName: ctrl.loginVM.Username,
                password: ctrl.loginVM.Password
            };

            authService.login(loginData).then(
                function (response) {
                    localStorage.setItem('token', response.data.access_token);
                    localStorage.setItem('username', response.data.userName);
                    localStorage.setItem('role', response.data.role);
                    localStorage.setItem('id', response.data.id);
                    $state.go('home', { page : 1, size : 10, state : 3});
                },
                function (response) {
                    if (response.data.ModelState)
                        ctrl.errors = response.data.ModelState;
                });

            
            
        };

        ctrl.logout = function () {
            authService.logout().then(
                function (response) {
                    localStorage.removeItem('token');
                    localStorage.removeItem('username');
                    localStorage.removeItem('role');
                    localStorage.removeItem('id');
                    $state.go('login');
                },
                function (error)
                {
                    alert('Greška');
                }
            );
        };
        $scope.authCompletedCB = function (fragment) {
            console.log(fragment);
            ctrl.checkRegistration(fragment.access_token);
        }

        function getAccessToken() {
            if (location.hash) {
                if (location.hash.split('access_token=')) {
                    var accessToken = location.hash.split('access_token=')[1].split('&')[0];
                    if (accessToken) {
                        ctrl.checkRegistration(accessToken);
                    }
                }
            }
        }

        ctrl.checkRegistration = function (accessToken) {
            authService.checkRegistration(accessToken).then(
                function (response) {
                    if (response.data.HasRegistered) {
                        localStorage.setItem('username', response.data.Email);
                        localStorage.setItem('role', response.data.Role);
                        localStorage.setItem('id', response.data.Id);
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }
                    else {
                        
                        ctrl.signupExternalUser(accessToken);
                    }
                },
                function (response) {
                    alert("Greška");
                });
        }

        ctrl.signupExternalUser = function (accessToken) {
            authService.signupExternalUser(accessToken).then(
                function (response) {
                    localStorage.setItem('token', response.data.access_token);
                    localStorage.setItem('username', response.data.userName);
                    localStorage.setItem('role', response.data.role);
                    localStorage.setItem('id', response.data.id);
                    $state.go('home', { page: 1, size: 10, state: 3 });
                },
                function (response) {
                    $scope.message = "Registracija nije uspešno izvršena. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                });
        }

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }
        
    }
})();