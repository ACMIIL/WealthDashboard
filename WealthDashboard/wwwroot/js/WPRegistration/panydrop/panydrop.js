angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {
       
        $scope.mobile = localStorage.getItem('Mnumber');
        $scope.data = [];
        $scope.ADDRESS = '';
        $scope.accountno = '';
        $scope.customerName = '';
        $scope.IFSCCode = '';
        var bankdetails = [];
        var Bankdata = [];
        $scope.isDisabled = true;
        $scope.invalidname = false;
        $scope.invalidAcc = false;
        $scope.invalidIFSC = false;
       

        
        $scope.GetBankDetails = function () {

            $http({
                url: 'https://ifsc.razorpay.com/' + $scope.IFSCCode,
                headers: {},
                method: 'GET',
                data: {}
            }).then(function (result) {

                var response = result;

                if (response.status === 200) {
                    bankdetails = result.data;

                    Bankdata = {
                        UserId: userId,
                        BankAccountTypeId: 1,
                        BankName: bankdetails.BANK,
                        IFSCCODE: bankdetails.IFSC,
                        BankBranchName: bankdetails.BRANCH,
                        BankAccountNumber: $scope.accountno,
                        BankCode: bankdetails.BANKCODE,
                        UPIID: bankdetails.UPI,
                        Bank_Address: bankdetails.ADDRESS,
                        City: bankdetails.CITY,
                        State: bankdetails.STATE,
                        BeneficiaryName: $scope.customerName
                    }

                    $scope.ADDRESS = Bankdata.Bank_Address
                    $scope.isDisabled = false;
                }
                else {
                    $scope.ADDRESS = 'Invalid IFSC Code'
                    $scope.isDisabled = true;
                }


            });
        }


        $scope.SaveBankDetails = function () {


            if ($scope.customerName == null || $scope.customerName == '' || $scope.customerName == undefined) {

                toastr.error('Please Enter Account holder Name', 'Account Holder Name !');

            }
            if ($scope.accountno == null || $scope.accountno == '' || $scope.accountno == undefined) {

                toastr.error('Please Enter Account Number', 'Account Number !');
            }
            if ($scope.IFSCCode == null || $scope.IFSCCode == '' || $scope.IFSCCode == undefined) {

                toastr.error('Please Enter IFSC Code', 'IFSC Code !');
            }
            

           
            $http({
                url: BaseURL + "DigioAPI/CentralizeValidatePennyDropAcc?RegistrationId=" + userId + "&IfscCode=" + $scope.IFSCCode + "&AccountNo=" + $scope.accountno + "&ClientName=" + $scope.customerName,
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (response) {

                
                if (response.status == "200" && response.data.data.data.verified == true) {

                    SaveBankDetails();
                   
                }
                else {
                    toastr.error('500Internal Server Error!', 'Something went wrong!');

                }
            }).catch(function (error) {
                console.error('Error occurred (UpdateUserBankAccountTypeStatus): ', error);
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
                    window.location.assign('/WP_Registration/WPRegistration/selfieVerification');
                }
                else {
                    toastr.error('500Internal Server Error!', 'Bank Details Update!');

                }
            }).catch(function (error) {
                console.error('Error occurred (UpdateUserBankAccountTypeStatus): ', error);
            });



        }

        $scope.changeacc = function () {
            var acc = $scope.accountno;
           
            if (acc.length < 1) {

                $scope.invalidAcc = 1;
            }
            else {
                $scope.invalidAcc = 0;
            }
           
        };
        
        $scope.changename = function () {
         
            var name = $scope.customerName;
           
           
            if (name.length < 1) {
                $scope.invalidname = 1
            }
            else {
                $scope.invalidname = 0
            }

        };

        $scope.changeifsc = function () {
           
            var ifsc = $scope.IFSCCode;
          
            if (ifsc.length < 1) {
                $scope.invalidIFSC = 1
            }
            else {
                $scope.invalidIFSC = 0
            }

        };
    });