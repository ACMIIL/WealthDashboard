angular.module('main', ['ngAnimate', 'toaster'])
   
    .controller('myController', function ($scope, toaster, $window, $http, $interval) {
     //   var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";
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
                url: BaseURL + "User/MobileSendOTP?parameter=" + mobile + "&updateMobile=none&Otptype=1",
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
                url: BaseURL + "user/MobileVerifyOtp?Userid=" + userId + "&MobileOtp=" + Otp,
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
                url: BaseURL + "User/GetUserDetails?userId=" + userId,
                method: "GET",
                headers: {}, // lowercase 'headers'
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    var status = response.data.data.status;
                    var host = window.location.hostname;
                    var BaseURL1 = "";
                    // Check if the host is localhost or 127.0.0.1 
                    //if (host === "localhost" || host === "127.0.0.1") {
                    //    BaseURL1 = "http://localhost:52206/WP_Registration/WPRegistration/" + "CreateSession?userid=" + userId;
                    //}
                    //else {
                    //    BaseURL1 = ApplicationURL + "CreateSession?userid=" + userId;
                    //}

                    $http({
                        url: "/WP_Registration/WPRegistration/CreateSession?userid=" + userId,
                        method: "GET",
                        headers: {}, // lowercase 'headers'
                    }).then(function (response1) {

                        if (status === 0 ) {
                            window.location.assign('/WP_Registration/WPRegistration/PanVerification');
                        } else if (status === 1) {
                            window.location.assign('/WP_Registration/WPRegistration/ARNdetails');
                        } else if (status === 2) {
                            $location.path('/WP_Registration/WPRegistration/DigiLocker');
                        } else if (status === 3) {
                            window.location.href = "/WP_Registration/WPRegistration/PersonalDetails";
                        } else if (status === 4 ) {
                           // $location.path('/WP_Registration/WPRegistration/DigiLocker');
                            window.location.href = "/WP_Registration/WPRegistration/QRBankVerification";
                            //$location.path('/qrscanner');
                        } else if (status === 5) {
                            window.location.href = '/WP_Registration/WPRegistration/QRBAnkDetails';
                        } else if (status === 6) {
                            window.location.href = '/WP_Registration/WPRegistration/SelfieVerification';
                        } else if (status === 7) {
                            window.location.href = '/WP_Registration/WPRegistration/SignatureVerification'
                        }
                        else if (status === 8) {
                            window.location.href = '/WP_Registration/WPRegistration/Thankyou'
                        }
                        else if (status === 9) {
                            // show dashbord
                            window.location.href = '/home/index'
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
