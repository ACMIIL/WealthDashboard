angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http) {
      //  var BaseURL = "https://localhost:7222/";

        var params = getQueryParams();
        $scopeBANK = '';
        $scope.account_Name = '';
        $scope.IFSC = '';
        $scope.accountno = '';
        var Bankdata = [];
        var Branchdetails = [];
        function getQueryParams() {

            var queryString = window.location.search;
            var params = {};
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

                        var result = JSON.parse(data.data.data)

                        $scope.BANK = result.bankname;
                        $scope.account_Name = result.accountholdername;
                        $scope.IFSC = result.ifsc;
                        $scope.accountno = result.accountno;

                        GetBankBranchDetails();

                      

                    }
                    else {
                        window.location.assign('/WPRegistration/UploadChequeBankVerification');
                    }
                });

           
        }


        function GetBankBranchDetails() {
             
            $http({
                url: 'https://ifsc.razorpay.com/' + $scope.IFSC,
                headers: {},
                method: 'GET',
                data: {}
            }).then(function (result) {

                var response = result;

                if (response.status === 200) {

                    Branchdetails = response.data;

                    Bankdata = {
                        UserId: userId,
                        BankAccountTypeId: '1',
                        BankName: Branchdetails.BANK,
                        IFSCCODE: $scope.IFSC,
                        BankBranchName: Branchdetails.BRANCH,
                        BankAccountNumber: $scope.accountno,
                        BankCode: Branchdetails.BANKCODE,
                        UPIID: Branchdetails.UPI,
                        Bank_Address: Branchdetails.ADDRESS,
                        City: Branchdetails.CITY,
                        State: Branchdetails.STATE,
                        BeneficiaryName: $scope.account_Name

                    }
                    $scope.BANK = Bankdata.BankName;

                    SaveBankDetails();


                    
                }
                else {
                    alert('Data not found');
                }
                //return response.data;

            });
        }

        function SaveBankDetails() {
            $http({
                url: BaseURL + 'user/UpdateUserBankAccountTypeStatus',
                method: 'POST',
                headers: {},

                data: Bankdata
            }).then(function (response) {

                var result = response;
                if (result.data.code === 200) {

                }
                else {
                    toastr.error('500Internal Server Error!', 'Bank Details Update!');

                }
            }).catch(function (error) {
                console.error('Error occurred (UpdateUserBankAccountTypeStatus): ', error);
            });
            


        }




        $scope.Gotoselfie = function () {
           
            window.location.assign('/WPRegistration/selfieVerification');
        }


      

       
    });