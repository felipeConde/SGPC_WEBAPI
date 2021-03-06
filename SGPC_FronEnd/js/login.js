﻿(function () {
    'use strict';

    //=================================================
    // LOGIN
    //=================================================

    materialAdmin.controller('loginCtrl', ['$scope', '$http', '$rootScope', '$location', '$window', function ($scope, $http, $rootScope, $location, $window) {
        //Status

        this.login = 1;
        this.register = 0;
        this.forgot = 0;
        //this.showErro = false;
        //this.showSuccess = false;

        this.VerificaLogin = function (username, password) {
            //alert(this.username);
            //this.showErro = false;

            if (this.username == null || this.password == null) {
                $scope.showErro = true;
                return;
            }


            $http.post('api/Login', { username: this.username, password: this.password })
            .then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                //alert(response.data.Email);
                //this.showSuccess = true;
                $scope.showSuccess = true;
                $scope.showErro = false;
                SetCredentials(response.data.username, response.data.password, response.data.codigo, response.data.nome_usuario)
                var teste;
                //alert("passou");
                //$rootScope.globals = $cookieStore.get('globals') || {};
                if ($rootScope.globals.currentUser) {
                    //alert($rootScope.globals.currentUser.codigo);
                    $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata; // jshint ignore:line
                    //teste = $rootScope.globals.currentUser.username;
                    //$scope.Email = response.data.Email;
                    //$scope.Email = teste;

                    //SESSIONS DO USUÁRIO
                    $window.sessionStorage.setItem("codigo_usuario", $rootScope.globals.currentUser.codigo)
                    $window.sessionStorage.setItem("nome_usuario", $rootScope.globals.currentUser.nome_usuario)

                    //$scope.Email = $window.sessionStorage.getItem("codigo_usuario")
                    //redirect
                   
                    $http.post('user.ashx', $rootScope.globals.currentUser.codigo)
                    .then(function successCallback(response) {
                        //alert(response);
                        $window.location.href = 'main.aspx';
                    }, function errorCallback(response) {
                        alert(response.data);
                    });

                }

            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                //alert(this.showErro);
                //this.showErro = true;
                //alert(response);
                $scope.showErro = true;
                $scope.showSuccess = false;
                // alert("erro");
            });
            //.success(function (response) {
            //    //alert(response.username)
            //    callback(response);
            //});



        };

        function SetCredentials(username, password,codigo, nome_usuario) {
            var authdata = Base64.encode(username + ':' + password);
            
            $rootScope.globals = {
                currentUser: {
                    username: username,
                    authdata: authdata,
                    codigo: codigo,
                    nome_usuario:nome_usuario
                }
            };

            //$sessionStorage.globals = $rootScope.globals;
 
            $http.defaults.headers.common['Authorization'] = 'Basic ' + authdata; // jshint ignore:line
            //$cookie.put('globals', $rootScope.globals);
           
            //alert($rootScope.globals.currentUser.username);
        }
 
        function ClearCredentials() {
            $rootScope.globals = {};
            //$cookie.remove('globals');
            $http.defaults.headers.common.Authorization = 'Basic';
        }
    
        // Base64 encoding service used by AuthenticationService
        var Base64 = {

            keyStr: 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=',

            encode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                do {
                    chr1 = input.charCodeAt(i++);
                    chr2 = input.charCodeAt(i++);
                    chr3 = input.charCodeAt(i++);

                    enc1 = chr1 >> 2;
                    enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                    enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                    enc4 = chr3 & 63;

                    if (isNaN(chr2)) {
                        enc3 = enc4 = 64;
                    } else if (isNaN(chr3)) {
                        enc4 = 64;
                    }

                    output = output +
                        this.keyStr.charAt(enc1) +
                        this.keyStr.charAt(enc2) +
                        this.keyStr.charAt(enc3) +
                        this.keyStr.charAt(enc4);
                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";
                } while (i < input.length);

                return output;
            },

            decode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
                var base64test = /[^A-Za-z0-9\+\/\=]/g;
                if (base64test.exec(input)) {
                    window.alert("There were invalid base64 characters in the input text.\n" +
                        "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                        "Expect errors in decoding.");
                }
                input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

                do {
                    enc1 = this.keyStr.indexOf(input.charAt(i++));
                    enc2 = this.keyStr.indexOf(input.charAt(i++));
                    enc3 = this.keyStr.indexOf(input.charAt(i++));
                    enc4 = this.keyStr.indexOf(input.charAt(i++));

                    chr1 = (enc1 << 2) | (enc2 >> 4);
                    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                    chr3 = ((enc3 & 3) << 6) | enc4;

                    output = output + String.fromCharCode(chr1);

                    if (enc3 != 64) {
                        output = output + String.fromCharCode(chr2);
                    }
                    if (enc4 != 64) {
                        output = output + String.fromCharCode(chr3);
                    }

                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";

                } while (i < input.length);

                return output;
            }
        };



    }])

})();
