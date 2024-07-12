angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {

        $scope.data = [];
        GetData();
         function GetData() {

            var userId = '74DE3363-A041-478A-B97C-00B9CF697E21';// localStorage.getItem('userId')
            $http({
                url: BaseURL + "User/GetRejectDocumentList?userid=" + userId,
                method: "Get",
                Headers: {},
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    debugger;
                    $scope.data = JSON.parse(response.data.data);
                    //var result = '';
                    //toastr.success('An OTP has been sent to ' + $scope.MobileNumber, 'Title Success!');
                }
            }).catch(function (error) {
                console.log('Error sending OTP:', error);
            });
        };
    });