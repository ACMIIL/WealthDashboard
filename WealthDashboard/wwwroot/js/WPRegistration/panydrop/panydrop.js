angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {
        getdata()

        // user/GetDigioLockerUserPersonalDetails
       // $scope.userId = localStorage.getItem('userId')
        $scope. mobile = localStorage.getItem('Mnumber');
        $scope.data = [];
        $scope.accountno = '';
        $scope.customerName = '';
        $scope.IFSCCode = '';
        $scope.AccountType = 1;
        GetUserDetails();
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
                        BankAccountTypeId: $scope.AccountType,
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
                }
                else {
                    alert('Data not found');
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
            

            Bankdata.BankAccountTypeId = $scope.AccountType;


            $http({
                url: BaseURL + 'user/UpdateUserBankAccountTypeStatus',
                method: 'POST',
                headers: {},

                data: Bankdata
            }).then(function (response) {

                var result = response;
                if (result.data.code === 200) {
                    uploadCancelCheque();

                }
                else {
                    toastr.error('500Internal Server Error!', 'Bank Details Update!');

                }
            }).catch(function (error) {
                console.error('Error occurred (UpdateUserBankAccountTypeStatus): ', error);
            });
        }

        function GetUserDetails() {
            // api/User/GetUserDetails

            $http({
                url: BaseURL + 'User/GetUserDetails?userId =' + userId,
                method: 'Get',
                headers: {},

                data: Bankdata
            }).then(function (response) {

                var result = response;
                if (result.data.code === 200) {
                   // uploadCancelCheque();
                    var fullName = response.data.firstName + ' ' + response.data.middleName + ' ' + response.data.lastName;
                    $scope.customerName = fullName;
                }
                else {
                    toastr.error('500Internal Server Error!', 'Bank Details Update!');

                }
            }).catch(function (error) {
                console.error('Error occurred (UpdateUserBankAccountTypeStatus): ', error);
            });
        }




    });