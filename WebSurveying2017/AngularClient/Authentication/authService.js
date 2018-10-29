(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').factory('authService', ['$http', '$q', function ($http, $q) {

        var url = 'api/Account'

        var authServiceFactory = {};
        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var _forgotPassword = function (model) {
            return $http(
                {
                    method: 'POST',
                    url: url + "/ForgotPassword",
                    data: model
                });

        }

        var _checkRegistration = function (accessToken) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/UserInfo',
                    headers: {
                        'content-type': 'application/JSON',
                        'Authorization': 'Bearer ' + accessToken
                    }

                });
        }

        var _signupExternalUser = function (accessToken) {
            return $http(
                {
                    method: 'POST',
                    url: url + '/RegisterExternal',
                    headers: {
                        'content-type': 'application/JSON',
                        'Authorization': 'Bearer ' + accessToken
                    }

                });
           

        }

        var _resetPasswordWithToken = function (data) {
            return $http(
                {
                    method: 'POST',
                    url: url + "/ResetPassword",
                    data: data
                });
        }
        var _login = function (loginData) {
           
            return $http({
                method: 'POST',
                data: loginData,
                url: url + '/Login'
            });

        };

        var resetPassword = function (obj)
        {
            return $http({
                method: 'POST',
                data: obj,
                url: url + '/ChangePassword'
                
            });
        }
        var _signUp = function (signUpVM) {
            return $http({
                method: 'POST',
                url: url + '/Register',
                data : signUpVM
            });
        };

        var _logout = function () {
            return $http({
                method: 'POST',
                url : url + '/Logout'
            });
        };

        authServiceFactory.signUp = _signUp;
        authServiceFactory.login = _login;
        authServiceFactory.resetPassword = resetPassword;
        authServiceFactory.logout = _logout;
        authServiceFactory.forgotPassword = _forgotPassword;
        authServiceFactory.resetPasswordWithToken = _resetPasswordWithToken;
        authServiceFactory.checkRegistration = _checkRegistration;
        authServiceFactory.signupExternalUser = _signupExternalUser;
        return authServiceFactory;

    }]);

})(angular);