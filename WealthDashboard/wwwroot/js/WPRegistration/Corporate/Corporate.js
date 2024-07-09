angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http, $interval) {

        $scope.MobileNumber = [];
        $scope.display = '';
        $scope.resendOtp = false;
        $scope.OTPboxhide = false;
        $scope.mobilehide = '';


       function sendOTP() {

            var mobile = $scope.MobileNumber;

            $http({
                url: BaseURL + "User/MobileSendOTPforCorporate?parameter=" + mobile,
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
                    $scope.mobilehide = result;
                    localStorage.setItem('Mobilenumber', $scope.mobile)
                    //localStorage.setItem('Id', response.data.data.userId);
                   
                }

            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        };

       

        $scope.GetOTP = function () {

            if ($scope.MobileNumber.length == 10) {
                $scope.OTPboxhide = true;
                sendOTP();
            }
            else {
                $scope.OTPboxhide = false;
            }
           
        }





        $scope.ResendOTP = function () {

            //var mobile = localStorage.getItem('Mobilenumber');
            $http({
                url: BaseURL + "User/MobileSendOTPforCorporate?parameter=" + $scope.MobileNumber,
                method: "POST",
                Headers: {},
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    var result = '';
                    toastr.success('An OTP has been sent to ' + $scope.MobileNumber, 'Success!');
                }
            }).catch(function (error) {
                console.log('Error sending OTP:', error);
            });
        };

        $scope.VerifyOTP = function (Otp) {

            if (Otp.length == 4) {

                $http({
                    url: BaseURL + "User/MobileVerifyOtpCorporate?mobile=" + $scope.MobileNumber + "&MobileOtp=" + Otp,
                    method: "GET",
                    Headers: {},
                    data: {}
                }).then(function (response) {

                    if (response.data.code === 200) {
                        var message = response.data.data;
                        if (message == 'OTP Verified') {
                            $scope.OTPboxhide = false;
                            toastr.success(message, 'Success!')
                        }
                        else {
                            $scope.OTPboxhide = true;
                            this.toastr.error('Please enter the valid OTP', 'OTP Verify !');
                        }

                    }
                    else {
                        this.toastr.error('Please enter the valid OTP', 'OTP Verify !');
                    }
                }).catch(function (error) {
                    console.log('Error Sending Verify OTP:', error);
                });
            }
        };



        $scope.startTimer = function (seconds, prefix) {
            var timer = $interval(function () {
                var minutes = Math.floor(seconds / 60);
                var textSec = (seconds % 60 < 10) ? '0' + (seconds % 60) : (seconds % 60);
                $scope.display = `${prefix}${minutes}:${textSec}`;

                if (seconds === 0) {
                    console.log("finished");
                    $scope.resendOtp = true;
                    $interval.cancel(timer);
                }
                seconds--;
            }, 1000);


            if (seconds === 0) {
                $scope.resendOtp = true;
            }
        };

        var totalSeconds = 59;
        var prefix = '';
        $scope.startTimer(totalSeconds, prefix);


        $scope.preventPaste = function (event) {
            event.preventDefault();
        };

        $scope.change = function () {
            var OTPlength = $scope.OTPvalue;

            if (OTPlength.length !== 4) {
                $scope.isvalied = false;
            } else {
                var value = OTPlength.charAt(0);
                $scope.isvalied = true;
                for (var i = 0; i < OTPlength.length; i++) {
                    if (OTPlength[i] !== value) {
                        $scope.isvalied = false;
                        break;
                    }
                }
            }
        };


        $scope.Next = function () {

            $http({
                url: BaseURL + 'User/SaveCorporateLead?mobile=' + $scope.MobileNumber + '&email=' + $scope.email + '&name=' + $scope.name,
                method: "POST",
                Headers: {},
                data: {}
            }).then(function (response) {
                var result = response.data;
                if (result == 'Data has been saved successfully!') {
                    window.location.assign('/WP_Registration/WPRegistration/CorporateFileUpload');
                }
                else {
                    this.toastr.error(result, 'Error !');
                }
            });


           
        }

        $scope.checkEmail = function () {

            var input = $scope.email;
            var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
            if (input.length > 0) {
            }
            else {
            }
            if (!emailPattern.test(input)) {
                $scope.CheckEmailPattern = true;
            } else {
                $scope.CheckEmailPattern = false;
            }
            if (input.length == 0) {
                $scope.CheckEmailPattern = false;
            }
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
    });