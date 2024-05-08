angular.module('main', ['ngAnimate', 'toaster'])


    .controller('myController', function ($scope, toaster, $window, $http) {
        var params = getQueryParams();
        function getQueryParams() {

            var queryString = window.location.search;
            var params = {};
            $scope.result = [];
            var queryParams = new URLSearchParams(queryString);
            queryParams.forEach(function (value, key) {
                // Add the parameter to the object
                params[key] = value;
            });
            return params.output;
        }

        GetData();
        function GetData() {
            $http({

                url: BaseURL + 'account/GetQRBankData?data=' + params,
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (response) {

                if (response.data.code === 200) {


                    var data = response;

                    $scope.result = JSON.parse(data.data.data)

                    //$scope.BANK = result.bankname;
                    //$scope.account_Name = result.accountholdername;
                    //$scope.IFSC = result.ifsc;
                    //$scope.accountno = result.accountno;

                }
                
            }).catch(function (error) {
                console.error('Error occurred (GetQRBankData : Fail): ', error);
            })

            


        }

        $scope.GotoQRScanner = function () {
            window.location.assign('/WP_Registration/WPRegistration/QRBankVerification');
        }

        $scope.GotoPennyDrop = function () {
            window.location.assign('/WP_Registration/WPRegistration/Panydrop');
        }

        $scope.GotoUploadCheque = function () {
            window.location.assign('/WP_Registration/WPRegistration/UploadChequeBankVerification');
        }

    });