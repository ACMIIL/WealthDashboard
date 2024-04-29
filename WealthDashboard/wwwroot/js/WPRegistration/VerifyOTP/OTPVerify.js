angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http, $interval) {
        var baseUrl = "https://localhost:7222/";
        //var  baseUrl="https://devwealthmaapi.investmentz.com/";
        $scope.MobileNumber = [];
        $scope.display = '';
        $scope.resendOtp = false;
        SendOTP();
        function SendOTP() {
            $scope.MobileNumber = localStorage.getItem('mobhide') ? localStorage.getItem('mobhide').toString() : '';
            toastr.success('An OTP has been sent to ' + $scope.MobileNumber, 'Title Success!');
        }
        $scope.ResendOTP = function () {

            var mobile = localStorage.getItem('Mnumber');
            $http({
                url: baseUrl + "api/User/MobileSendOTP?parameter=" + mobile + "&updateMobile=none&Otptype=1",
                method: "POST",
                Headers: {},
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    var result = '';
                    toastr.success('An OTP has been sent to ' + $scope.MobileNumber, 'Title Success!');
                }
            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        };

        $scope.VerifyOTP = function (Otp) {

            var userId = localStorage.getItem('userId')

            $http({
                url: baseUrl + "api/user/MobileVerifyOtp?Userid=" + userId + "&MobileOtp=" + Otp,
                method: "GET",
                Headers: {},
                data: {}
            }).then(function (response) {

                if (response.data.code === 200) {
                    var agentSrno = response.data.data.agentSrno;
                    localStorage.setItem('srno', agentSrno);
                    Getuserstapes(userId);
                }
                else {
                    this.toastr.error('Please enter the valid OTP', 'OTP Verify !');
                }
            }).catch(function (error) {
                console.error('Error Sending Verify OTP:', error);
            });
        };


        function Getuserstapes(userId) {
          
            $http({
                url: baseUrl + "api/User/GetUserDetails?userId=" + userId,
                method: "GET",
                headers: {}, // lowercase 'headers'
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    var status = response.data.data.status;
                    var host = window.location.hostname;
                    var baseurl1 = "";
                    // Check if the host is localhost or 127.0.0.1
                    if (host === "localhost" || host === "127.0.0.1") {
                        baseurl1 = "http://localhost:52206/"
                    }
                    else {
                        baseurl1 = baseUrl1 + "WPRegistration/CreateSession?userid=" + userId
                    }

                    $http({
                        url:   "http://localhost:52206/WPRegistration/CreateSession?userid=" + userId,
                        method: "GET",
                        headers: {}, // lowercase 'headers'
                        data: {}
                    }).then(function (response1) {

                        if (status === 0 ) {
                            window.location.assign('/WPRegistration/PanVerification');
                        } else if (status === 1) {
                            window.location.assign('/WPRegistration/ARNdetails');
                        } else if (status === 2) {
                            $location.path('/WPRegistration/DigiLocker');
                        } else if (status === 3) {
                            window.location.href = "/WPRegistration/PersonalDetails";
                        } else if (status === 4 ) {
                           // $location.path('/WPRegistration/DigiLocker');
                            window.location.href = "/WPRegistration/QRBankVerification";
                            //$location.path('/qrscanner');
                        } else if (status === 5) {
                            window.location.href = '/WPRegistration/QRBAnkDetails';
                        } else if (status === 6) {
                            window.location.href = '/WPRegistration/SelfieVerification';
                        } else if (status === 7) {
                            window.location.href = '/WPRegistration/SignatureVerification'
                        }
                        else if (status === 8) {
                            window.location.href = '/WPRegistration/Thankyou'
                        }
                        else if (status === 9) {
                           // show dashbord
                        }
                    }).catch(function (error) {
                        console.error('Error Something went wrong:', error);
                    });

                  
                }
            }).catch(function (error) {
                console.error('Error Something went wrong:', error);
            });
        }

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
   
    });
