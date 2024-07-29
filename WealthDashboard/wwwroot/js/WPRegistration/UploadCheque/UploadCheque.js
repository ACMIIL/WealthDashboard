﻿angular.module('main', [])
    .controller('mycontroller', function ($scope, $http) {
      //  var BaseURL = "https://localhost:7222/";
        $scope.ADDRESS = '';
        $scope.Account_Name = '';
        $scope.Account_Num = ''
        $scope.AccountType = '1';
        $scope.IFSCCode = '';
        $scope.reverseJourny = localStorage.getItem('reverseJourny');
        var error={
            address: false, accountName: false, accountType: false, IFSCCode: false, account_Num:false,cheque:false
        }
        var bankdetails = [];
        var Bankdata = [];

       

        $scope.CancelChequeFileName = '';
            $scope.clickFileInput = function () {
            document.getElementById('fileInput').click();
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
                        BankAccountNumber: $scope.Account_Num,
                        BankCode: bankdetails.BANKCODE,
                        UPIID: bankdetails.UPI,
                        Bank_Address: bankdetails.ADDRESS,
                        City: bankdetails.CITY,
                        State: bankdetails.STATE,
                        BeneficiaryName: $scope.Account_Name
                    }

                    $scope.ADDRESS = Bankdata.Bank_Address
                }
                else{
                    alert('Data not found');
                }


            });
        }


            $scope.SaveBankDetails = function () {

                error = {
                    address: false, accountName: false, accountType: false, IFSCCode: false, account_Num: false, cheque: false
                }
                if ($scope.Account_Name == null || $scope.Account_Name == '' || $scope.Account_Name == undefined) {

                    toastr.error('Please Enter Account holder Name', 'Account Holder Name !');
                    error.accountName = true;
                }
                if ($scope.Account_Num == null || $scope.Account_Num == '' || $scope.Account_Num == undefined) {

                    toastr.error('Please Enter Account Number', 'Account Number !');
                    error.account_Num = true;
                }
                if ($scope.IFSCCode == null || $scope.IFSCCode == '' || $scope.IFSCCode == undefined) {

                    toastr.error('Please Enter IFSC Code', 'IFSC Code !');
                    error.accountName = true;
                }
                if ($scope.CancelChequeFileName == null || $scope.CancelChequeFileName == '' || $scope.CancelChequeFileName == undefined) {

                    toastr.error('Please select cancel cheque', 'Cancel Cheque !');
                    error.cheque = true;
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


        function uploadCancelCheque(){
            var formdata = new FormData();
            formdata.append('File', $scope.CancelCheque);
            formdata.append('ImgName', $scope.CancelChequeFileName);
            formdata.append('UserId', userId);
            formdata.append('DoucmentTypeId', "4");

            // Now you can use the formdata object as needed





            $http({
                url: BaseURL + 'User/UploadFiles',
                method: 'POST',
                headers: { 'Content-Type': undefined }, // Corrected content type
                transformRequest: angular.identity,
                data: formdata
    
            }).then(function (response) {

                var result = response;

                if (result.data.code === 200 &&
                    ($scope.reverseJourny == false || $scope.reverseJourny == 'false'
                        || $scope.reverseJourny == '' || $scope.reverseJourny == undefined || $scope.reverseJourny == null)) {
                    UpdateStatus();

                }
                else {
                    DownloadPDF();
                }

            })

           
        }

        function DownloadPDF() {

            $http({
                url: BaseURL + "DigioAPI/DwonloadPDF?userId=" + userId,
                method: "Get",
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.code == "200") {
                    toastr.success(res.data.message, 'Title Success!');
                    //toaster.success(res.data.message,'Title Success!');
                    setTimeout(() => {
                        window.location.assign('/Home/Index');
                    }, 3000);

                }
                else {
                    toastr.error('Something went wrong', 'PDF Download!');
                    window.location.assign('/Home/Index');
                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });
        }
        function UpdateStatus() {

            $http({
                url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=6",
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (response) {

                var result = response;

                if (result.data.code === 200) {
                    window.location.assign('/WP_Registration/WPRegistration/SelfieVerification');
                }

            })


        }



        $scope.onFileSelect = function (input) {
            if (input.files && input.files.length > 0) {
                var file = input.files[0];
                var size = $scope.checkFileSize(file.size);

              
                    $scope.FilePath = file.name + ' (' + size + ')';
                    $scope.CancelChequeFileName = file.name;
                    $scope.CancelCheque = file;
               
                $scope.$apply();
            }
        };

        $scope.checkFileSize = function (bytes) {

            const maxSizeMB = 5;
            const fileSizeInMB = bytes / (1024 * 1024);
            if (fileSizeInMB > maxSizeMB) {
               
                $scope.filesize = true;
               

            } else {
                $scope.filesize = false;
               
                $scope.errorMessage = '';

            }
        };


    });