angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {
        getdata()

        // api/user/GetDigioLockerUserPersonalDetails
        $scope.userId = localStorage.getItem('userId')
        $scope. mobile = localStorage.getItem('Mnumber');
        $scope.data = [];
        function getdata() {
            $scope.userId = localStorage.getItem('userId')
            $scope.mobile = localStorage.getItem('Mnumber');
            $http({
                url: BaseURL + "user/GetDigioLockerUserPersonalDetails?userId=" + $scope.userId,
                method: "GET",
                Headers: {},
                data: {}
            }).then(function (response) {
                $scope.data = JSON.parse(response.data.data);
            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        }





    });