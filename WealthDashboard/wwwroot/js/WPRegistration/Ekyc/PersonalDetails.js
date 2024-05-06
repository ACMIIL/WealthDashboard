angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {
        getdata()

        // user/GetDigioLockerUserPersonalDetails
       // $scope.userId = localStorage.getItem('userId')
        $scope. mobile = localStorage.getItem('Mnumber');
        $scope.data = [];
        function getdata() {
           // localStorage.getItem('userId')
            $scope.mobile = localStorage.getItem('Mnumber');
            $http({
                url: BaseURL + "user/GetDigioLockerUserPersonalDetails?userId=" + userId,
                method: "GET",
                Headers: {},
                data: {}
            }).then(function (response) {
                $scope.data = JSON.parse(response.data.data);
            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        }


        $scope.gotoNext = function () {
            UpdateStatus();
            //window.location.assign('/WPRegistration/qrbankverification');

            // window.location.assign('/WPRegistration/UploadChequeBankverification');
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
                    window.location.assign('/WPRegistration/qrbankverification');
                }

            })


        }



    });