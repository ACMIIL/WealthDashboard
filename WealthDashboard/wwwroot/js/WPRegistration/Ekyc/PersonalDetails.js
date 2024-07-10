angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http, $interval) {
        getdata()

        // user/GetDigioLockerUserPersonalDetails
        // $scope.userId = localStorage.getItem('userId')
        $scope.mobile = localStorage.getItem('Mnumber');
        $scope.data = [];
        $scope.CheckEmailPattern = false;
        $scope.Emailid = '';
        $scope.displayy = '';
        $scope.sendOTP = false;
        $scope.CheckOtp = false;
        $scope.VerifyEmailID = '';
        var totalSeconds = 300;
        var prefix = '';
        function getdata() {
           // userId ='1BE51D13-BAD8-44A6-8842-C71C5E83E175'
            $scope.mobile = localStorage.getItem('Mnumber');          
            $http({
                url: BaseURL + "user/GetDigioLockerUserPersonalDetails?userId=" + userId,
                method: "GET",
                Headers: {},
                data: {}
            }).then(function (response) {
                $scope.data = JSON.parse(response.data.data);

                if ($scope.data[0].Email == '' || $scope.data[0].Email == null || $scope.data[0].Email == undefined) {
                    $scope.isValidOTP = false;

                }
                else {
                    $scope.CheckMaildVerify = true;
                    $scope.isValidOTP = true;
                }
            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        }


        $scope.gotoNext = function () {
            UpdateStatus();
        }

        $scope.checkEmail = function () {

            var input = document.getElementById('Mailid').value;

            var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
            if (input.length > 0) {
                document.getElementById('notVerify').style.display = 'none';

            }
            else {
                document.getElementById('notVerify').style.display = 'block';
            }
            if (!emailPattern.test(input)) {
                $scope.CheckEmailPattern = true;
                $scope.sendOTP = false;

                $scope.ResndOTP = false;
                $scope.OpenOTPbox = false;
            } else {
                $scope.CheckEmailPattern = false;
                $scope.sendOTP = true;
                $scope.ResndOTP = false;
            }
            if (input.length == 0) {
                $scope.CheckEmailPattern = false;
            }
        };
        $scope.EmailOtpVerify = function () {
            $scope.OpenOTPbox = true;
            $scope.startTimer(totalSeconds, prefix);
            var Emailid = document.getElementById('Mailid').value;
            document.getElementById('Minute').style.display='block';
            $http({
                url: BaseURL + "User/EmailSendOTP?Emailid=" + Emailid + "&Mobileno=" + $scope.mobile + "&Otptype=2",
                method: 'POST',
                headers: {},
                data: {}
            }).then(function (response) {
                var result = response;
                if (result.data.code === 200) {

                    this.toastr.success('Please check the OTP send in your EmailId,', 'Title Success!');
                    document.getElementById("Mailid").disabled = true; 
                    $scope.sendOTP = false;
                 
                    $scope.VerifyEmailID = Emailid

                   // window.location.assign('/WP_Registration/WPRegistration/qrbankverification');
                    //window.location.assign('/WP_Registration/WPRegistration/Panydrop');

                }

            })
        }

        $scope.EnterOTP = function () {
            $scope.InputOTP = document.getElementById('OtpInput').value
            if ($scope.InputOTP && $scope.InputOTP.length === 4 && /^\d+$/.test($scope.InputOTP)) {
                // OTP is valid

                $scope.VerifyOTP = true;
            } else {
                // OTP is invalid
                $scope.isValidOTP = false;
                $scope.VerifyOTP = false;
            }
        };

        $scope.OtpVerify = function () {
            if ($scope.InputOTP.length == 4) {
                $http({
                    url: BaseURL + "user/EmailVerifyOtp?Userid=" + userId + "&EmailOtp=" + $scope.InputOTP,
                    method: 'GET',
                    headers: {},
                    data: {}
                }).then(function (response) {
                    var result = response;
                    if (result.data.code === 200) {
                        this.toastr.success('Email Verify,', 'Title Success!');
                        $scope.ResndOTP = false;
                        $scope.OpenOTPbox = false;
                        $scope.VerifyOTP = false;
                        $scope.data[0].Email = $scope.VerifyEmailID;
                        $scope.isValidOTP = true;
                    }
                    else {
                        this.toastr.error('Please enter the valid email OTP', 'OTP Verify !');

                    }

                })
            }

        }
        function UpdateStatus() {
            $http({
                url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=4",
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (response) {
                var result = response;
                if (result.data.code === 200) {
                    window.location.assign('/WP_Registration/WPRegistration/qrbankverification')
                  //  window.location.assign('/WPRegistration/qrbankverification');
                }
            })

        }
        $scope.allowNumbers = function (event) {
            var charCode = event.which || event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
        };

        $scope.startTimer = function (seconds, prefix) {
            $scope.sendOTP = false;
            $scope.ResndOTP = false;
            var timer = $interval(function () {
                var minutes = Math.floor(seconds / 60);
                var textSec = (seconds % 60 < 10) ? '0' + (seconds % 60) : (seconds % 60);
               // $scope.display = `${prefix}${minutes}:${textSec}`;
                $scope.displayy = `${prefix}${minutes}:${textSec}`;

                if (seconds === 0) {
                    console.log("finished");
                    document.getElementById('Minute').style.display = 'none';
                    $scope.sendOTP = false;
                    $scope.ResndOTP = true;
                    document.getElementById("Mailid").disabled = false; 
                    $interval.cancel(timer);
                }
                seconds--;
            }, 1000);


            if (seconds === 0) {
                document.getElementById('Minute').style.display = 'none';
                $scope.sendOTP = false;
                $scope.ResndOTP = true;
                document.getElementById("Mailid").disabled = false; 
              
            }
        };

     

       
        
       
    });