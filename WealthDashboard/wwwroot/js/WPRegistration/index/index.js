//import { URLConstants } from '../../wwwroot/js/WPRegistration/ConstUrl.js';
//debugger
//alert(URLConstants);
angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {
        $scope.isvalied = false;
       //   var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";
        var hostUrl = window.location.href
       
        $scope.sendOTP = function () {
            var mobile = $scope.mobile;
            var updatemobile = "none";

            $http({
                url: BaseURL + "User/MobileSendOTP?parameter=" + mobile + "&updateMobile=" + updatemobile + "&Otptype=1",
                method: "POST",
                Headers: {},
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    var result = '';
                    for (var index = 0; index < mobile.length; index++) {
                        var element = mobile[index];
                        if (index <= 1 || index > 7) {
                            result += "x";
                        } else {
                            result += mobile[index];
                        }
                    }
                    localStorage.setItem('mobhide', result);
                    localStorage.setItem('Mnumber', $scope.mobile)
                    localStorage.setItem('userId', response.data.data.userId);
                   // var message = 'A OTP has been send to ' + result + '.';
                   // toastr.success(message, ' OTP Send!');
                  
                    window.location.assign('/WP_Registration/WPRegistration/VerifyOTP');
                }

            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        };
        $scope.allowNumbers = function (event) {
            var charCode = event.which || event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
        };
        $scope.preventPaste = function (event) {
            event.preventDefault();
        };

        $scope.change = function () {
            var mobile = $scope.mobile;
            var value = mobile.charAt(0);
            var numbers = [1, 2, 3, 4, 5];
            $scope.isvalied = numbers.find(function (e) {
                return e == value;
            });

            if (mobile.length < 10) {
                $scope.check = true;
            } else {
                $scope.check = true;
                for (var i = 0; i < mobile.length; i++) {
                    if (mobile[i] != value) {
                        $scope.check = false;

                        angular.element(document.getElementById('btn')).focus();
                        break;
                    }
                }
                if ($scope.check) {
                    $scope.isvalied = 1;
                }
            }
        };

    });

