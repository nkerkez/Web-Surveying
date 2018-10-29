(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017Test',[]).controller('testController', testCtrl);

    function testCtrl($location, $scope) {
        var ctrl = this;

        ctrl.text = "Nikola";
        /*


        (
            function getAccessToken() {
                console.log('test')
                if (location.hash) {
                    if (location.hash.split('access_token=')) {
                        var accessToken = location.hash.split('access_token=')[1].split('&')[0];
                        if (accessToken) {
                            console.log(accessToken);
                            isUserRegistered(accessToken);
                        }
                    }
                }
            })();

        function isUserRegistered(accessToken) {
            $.ajax({
                url: '/api/Account/UserInfo',
                method: 'GET',
                headers: {
                    'content-type': 'application/JSON',
                    'Authorization': 'Bearer ' + accessToken
                },
                success: function (response) {
                    if (response.HasRegistered) {
                        console.log(response);
                        localStorage.setItem('accessToken', accessToken);
                        localStorage.setItem('userName', response.Email);
                        window.location.href = "Index.cshtml";
                    }
                    else {
                        console.log(response);
                        signupExternalUser(accessToken);
                    }
                }
            });
        }

        function signupExternalUser(accessToken) {
            $.ajax({
                url: '/api/Account/RegisterExternal',
                method: 'POST',
                headers: {
                    'content-type': 'application/json',
                    'Authorization': 'Bearer ' + accessToken
                },
                success: function () {
                    window.location.href = "/api/Account/ExternalLogin?provider=Google&response_type=token&client_id=self&redirect_uri=http%3A%2F%2Flocalhost%3A49681%2F%23%21%2Flogin&state=GerGr5JlYx4t_KpsK57GFSxVueteyBunu02xJTak5m01";
                }
            });

        }
        */
        console.log('test')

    }
})(angular);