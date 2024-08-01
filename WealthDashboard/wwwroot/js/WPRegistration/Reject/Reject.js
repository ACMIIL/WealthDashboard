angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {



        $scope.isPanDateVisible = false;
        $scope.isPanFormateCorrect = false;
        $scope.panCorrectMessage = false;
        $scope.AadharError = true;
        $scope.GSTError = true;
        $scope.panMessage = '';
        $scope.data = [];
        $scope.Selfie = false;
        $scope.PanCard = false;
        $scope.AadhaarFront = false;
        $scope.AadhaarBack = false;
        $scope.CancelCheque = false;
        $scope.Signature = false;
        $scope.ARN = false;
        $scope.GST = false;
        $scope.PMS = false;
        $scope.userId = '';
        $scope.validformbutton = true;
        $scope.sizeError = {
            ARN: false,
            PMS: false, GST: false, PAN: false, Cheque: false, Aadhar: false, SIGN: false, Selfi: false
        }

        $scope.item = {
            ARN: '',
            PMS: '', GST:  '', PAN:  '', Cheque: '', Aadhar: ''
        }

        $scope.fileDetails = {
            ARNFilePath:'', ARNFileName: '', ARNFile: '',
            PMSFilePath:'', PMSFileName: '', PMSFile:  '',
            GSTFilePath:'', GSTFileName: '', GSTFile:  '',
            PANFilePath:'', PANFileName: '', PANFile:  '',
            AadharFilePath:'', AadharFileName: '', AadharFile:  '',
            SelfiFilePath:'', SelfiFileName: '', SelfiFile:  '',
            SignFilePath:'', SignFileName: '', SignFile:  '',
            ChequeFilePath:'', ChequeFileName: '', ChequeFile:  '',

        }

        GetData();
        function GetData() {

            $scope.userId = localStorage.getItem('userId');// '74DE3363-A041-478A-B97C-00B9CF697E21';// 
            $http({
                url: BaseURL + "User/GetRejectDocumentList?userid=" + $scope.userId,
                method: "Get",
                Headers: {},
                data: {}
            }).then(function (response) {
                if (response.data.code === 200) {
                    
                    $scope.data = JSON.parse(response.data.data);
                    for (var i = 0; i < $scope.data.length; i++) {
                        if ($scope.data[i].TypeName == 'Selfie') {
                            $scope.Selfie = true;
                        }
                        if ($scope.data[i].TypeName == 'PanCard') {
                            $scope.PanCard = true;
                        }
                        if ($scope.data[i].TypeName == 'AadhaarFront') {
                            $scope.AadhaarFront = true;
                        }
                        //if ($scope.data[i].TypeName = 'AadhaarBack') {

                        //}
                        if ($scope.data[i].TypeName == 'CancelCheque')
                        {
                            $scope.CancelCheque = true;
                        }
                        if ($scope.data[i].TypeName == 'Signature')
                        {
                            $scope.Signature = true;
                        }
                        if ($scope.data[i].TypeName == 'ARN')
                        {
                            $scope.ARN = true;
                        }
                        if ($scope.data[i].TypeName == 'GST')
                        {
                            $scope.GST = true;
                        }
                        if ($scope.data[i].TypeName == 'PMS') {

                            $scope.PMS = true;
                        }
                    }

                    if ($scope.data.length == 1 && $scope.CancelCheque == true) {
                        window.location.assign('/WP_Registration/WPRegistration/UploadChequeBankVerification');
                    }

                    //var result = '';
                    //toastr.success('An OTP has been sent to ' + $scope.MobileNumber, 'Title Success!');
                }
            }).catch(function (error) {
                console.log('Error sending OTP:', error);
            });
        };

        $scope.preventPaste = function (event) {
            event.preventDefault();
        };
        $scope.clickFileInputgst = function () {
            document.getElementById('fileInputgst').click();
        };

        $scope.clickFileInputarn = function () {
            document.getElementById('fileInputarn').click();
        };
        $scope.clickFileInputpms = function () {
            document.getElementById('fileInputpms').click();
        };


        $scope.clickFileInput = function ( id) {
            document.getElementById(id).click();
        };

        $scope.allowNumbers = function (event) {
            var charCode = event.which || event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
        };



        $scope.onFileSelect = function (input, type) {
            if (input.files && input.files.length > 0) {
                var file = input.files[0];
                var size = $scope.checkFileSize(file.size, type);
                 

                if (type === 'ARN') {
                    $scope.fileDetails.ARNFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.ARNFileName = file.name;
                    $scope.fileDetails.ARNFile = file;
                    
                } else if (type === 'GST') {
                    $scope.fileDetails.GSTFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.GSTFileName = file.name;
                    $scope.fileDetails.GSTFile = file;
                } else if (type === 'PMS') {
                    $scope.fileDetails.PMSFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.PMSFileName = file.name;
                    $scope.fileDetails.PMSFile = file;
                }
                else if (type === 'PAN') {
                    $scope.fileDetails.PANFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.PANFileName = file.name;
                    $scope.fileDetails.PANFile = file;
                }
                else if (type === 'Selfi') {
                    $scope.fileDetails.SelfiFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.SelfiFileName = file.name;
                    $scope.fileDetails.SelfiFile = file;
                }
                else if (type === 'SIGN') {
                    $scope.fileDetails.SignFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.SignFileName = file.name;
                    $scope.fileDetails.SignFile = file;
                }
                else if (type === 'Cheque') {
                    $scope.fileDetails.ChequeFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.ChequeFileName = file.name;
                    $scope.fileDetails.ChequeFile = file;
                }
                else if (type === 'Aadhar') {
                    $scope.fileDetails.AadharFilePath = file.name + ' (' + size + ')';
                    $scope.fileDetails.AadharFileName = file.name;
                    $scope.fileDetails.AadharFile = file;
                }



                $scope.$apply();
            }
        };
        $scope.CHeckPanNumber = function () {




            if ($scope.item.PAN.length > 5) {

                var input = $scope.item.PAN;
                // var panPattern = /[A-Z]{5}[0-9]{4}[A-Z]/;
                var panPattern = /^[A-Za-z]{5}[0-9]{4}[A-Za-z]$/;
                if (!panPattern.test(input)) {
                    $scope.isPanFormateCorrect = true;
                    $scope.panCorrectMessage = false;
                } else {
                    $scope.isPanFormateCorrect = false;
                    $scope.panCorrectMessage = true;
                }
            }
            else {
                $scope.isPanFormateCorrect = false;
                $scope.panCorrectMessage = false;

            }



        }
        $scope.checkFileSize = function (bytes, type) {
             
            const maxSizeMB = 4;
            const fileSizeInMB = bytes / (1024 * 1024);
            if (fileSizeInMB > maxSizeMB) {

                if (type === 'ARN') {
                    
                    $scope.sizeError.ARN = true;
                } else if (type === 'GST') {
                    $scope.sizeError.GST = true;
                } else if (type === 'PMS') {
                    $scope.sizeError.PMS = true;
                }
                else if (type === 'PAN') {
                    $scope.sizeError.PAN = true;
                }
                else if (type === 'Selfi') {
                    $scope.sizeError.Selfi = true;
                }
                else if (type === 'SIGN') {
                    $scope.sizeError.SIGN = true;
                }
                else if (type === 'Cheque') {
                    $scope.sizeError.Cheque = true;
                }
                else if (type === 'Aadhar') {
                    $scope.sizeError.Aadhar = true;
                }

            } else {

                if (type === 'ARN') {
                    $scope.arnsize = false;
                    $scope.sizeError.ARN = false;
                } else if (type === 'GST') {
                    $scope.sizeError.GST = false;
                } else if (type === 'PMS') {
                    $scope.sizeError.PMS = false;
                }
                else if (type === 'PAN') {
                    $scope.sizeError.PAN = false;
                }
                else if (type === 'Selfi') {
                    $scope.sizeError.Selfi = false;
                }
                else if (type === 'SIGN') {
                    $scope.sizeError.SIGN = false;
                }
                else if (type === 'Cheque') {
                    $scope.sizeError.Cheque = false;
                }
                else if (type === 'Aadhar') {
                    $scope.sizeError.Aadhar = false;
                }
              //  $scope.errorMessage = '';
              //  validform();
            }
        };


        function validform() {

            if ($scope.GSTError == false || $scope.AadharError == false || $scope.sizeError.PAN == true ||
                $scope.sizeError.ARN == true || $scope.sizeError.PMS == true || $scope.sizeError.CancelCheque == true || $scope.sizeError.SIGN == true ||
                $scope.sizeError.GST == true || $scope.sizeError.Aadhar == true || $scope.sizeError.SIGN == true || $scope.isPanFormateCorrect == true) {
                $scope.validformbutton = true;

            }
            else {

                $scope.validformbutton = false;
            }



        }



        $scope.isValidGSTIN=  function() {
            const gstinRegex = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
            $scope.GSTError = gstinRegex.test($scope.item.GST);
            validform();
        }

        $scope.ValideAadhar = function ()
        {
            const aadhaarRegex = /^[2-9]{1}[0-9]{11}$/;
            $scope.AadharError = aadhaarRegex.test($scope.item.Aadhar);
            validform();
        }
        $scope.SubmitDetails = function () {
            var array = [];
            var status = true;
            debugger
            if ($scope.Selfie == true) {
                if ($scope.fileDetails.SelfiFile.length == 0 || $scope.fileDetails.SelfiFile == '' ||
                    $scope.fileDetails.SelfiFile == null) {
                    toastr.error('Please Upload Selfie', 'Selfie !');
                    status = false;
                }
                else {
                    var item = { File: $scope.fileDetails.SelfiFile, DocumentTypeId: 1, FilePath: $scope.fileDetails.SelfiFilePath, FileName: $scope.fileDetails.SelfiFileName };
                    array.push(item);
                }
            }
            if ($scope.PanCard == true) {
                if ($scope.item.PAN == '') {
                    toastr.error('Please Enter the Pan', 'PAN !');
                    status = false;
                }

                if ($scope.fileDetails.PANFile == '' || $scope.fileDetails.PANFile == null
                    || $scope.fileDetails.PANFile.length == 0) {
                    toastr.error('Please upload the Pan', 'PAN !');
                    status = false;
                }
                else {
                    var item = { File: $scope.fileDetails.PANFile, DocumentTypeId: 2, FilePath: $scope.fileDetails.SeFilePath, FileName: $scope.fileDetails.SelfiFileName };
                    array.push(item);
                }
            }
            if ($scope.AadhaarFront == true) {
                if ($scope.item.Aadhar == '') {
                    toastr.error('Please Enter the Aadhar Number', 'Aadhar !');
                    status = false;
                }

                if ($scope.fileDetails.AadharFile == '' || $scope.fileDetails.AadharFile == null
                    || $scope.fileDetails.AadharFile.length == 0) {
                    toastr.error('Please Upload the Aadhar', 'Aadhar !');
                    status = false;
                } else {

                    var item = { File: $scope.fileDetails.AadharFile, DocumentTypeId: 3, FilePath: $scope.fileDetails.AadharFilePath, FileName: $scope.fileDetails.AadharFileName };
                    array.push(item);
                }



            }
            //if ($scope.data[i].TypeName == ==trueAadhaarBack==true) {

            //}
            //if ($scope.CancelCheque == true) {
            //    if ($scope.item.Cheque == '') {
            //        toastr.error('Please Enter the Bank Account Number', 'Account Number !');
            //        status = false;
            //    }

            //    if ($scope.fileDetails.ChequeFile == '' || $scope.fileDetails.ChequeFile == null
            //        || $scope.fileDetails.ChequeFile.length == 0) {
            //        toastr.error('Please Upload the  Cancel Cheque', 'Cheque !');
            //        status = false;
            //    }
            //    else {

            //        var item = { File: $scope.fileDetails.ChequeFile, DocumentTypeId: 5, FilePath: $scope.fileDetails.ChequeFilePath, FileName: $scope.fileDetails.ChequeFileName };
            //        array.push(item);
            //    }
            //}
            if ($scope.Signature == true) {
                if ($scope.fileDetails.SignFile == '' || $scope.fileDetails.SignFile == null
                    || $scope.fileDetails.SignFile.length == 0) {
                    toastr.error('Please Upload the Signature ', 'Sign !');
                    status = false;
                } else {
                    var item = { File: $scope.fileDetails.SignFile, DocumentTypeId: 6, FilePath: $scope.fileDetails.SignFilePath, FileName: $scope.fileDetails.SignFileName };
                    array.push(item);
                }
            }
            if ($scope.ARN == true) {
                if ($scope.item.ARN == '') {
                    toastr.error('Please Enter the ARN Number', ' ARN   !');
                    status = false;
                }

                if ($scope.fileDetails.ARNFile == '' || $scope.fileDetails.ARNFile == null
                    || $scope.fileDetails.ARNFile.length == 0) {
                    toastr.error('Please Upload the  Cancel Cheque', 'ARN !');
                    status = false;
                } else {
                    var item = { File: $scope.fileDetails.ARNFile, DocumentTypeId: 7, FilePath: $scope.fileDetails.ARNFilePath, FileName: $scope.fileDetails.ARNFileName };
                    array.push(item);
                }
            }
            if ($scope.GST == true) {
                if ($scope.item.GST == '') {
                    toastr.error('Please Enter the GST Number', ' GST   !');
                    status = false;
                }

                if ($scope.fileDetails.GSTFile == '' || $scope.fileDetails.GSTFile == null
                    || $scope.fileDetails.GSTFile.length == 0) {
                    toastr.error('Please Upload the    GST certification', 'GST !');
                    status = false;
                }
                else {
                    var item = { File: $scope.fileDetails.GSTFile, DocumentTypeId: 8, FilePath: $scope.fileDetails.GSTFilePath, FileName: $scope.fileDetails.GSTFileName };
                    array.push(item);
                }
            }
            if ($scope.PMS == true) {

                if ($scope.item.PMS == '') {
                    toastr.error('Please Enter the PMS Number', ' PMS   !');
                    status = false;
                }

                if ($scope.fileDetails.PMSFile == '' || $scope.fileDetails.PMSFile == null
                    || $scope.fileDetails.PMSFile.length == 0) {
                    toastr.error('Please Upload the PMS certification', 'PMS !');
                    status = false;
                } else {
                    var item = { File: $scope.fileDetails.PMSFile, DocumentTypeId: 9, FilePath: $scope.fileDetails.PMSFilePath, FileName: $scope.fileDetails.PMSFileName };
                    array.push(item);
                }
            }

            debugger
            if (status)
            {
                var query = {
                    "userId": $scope.userId,
                    "pan": $scope.item.PAN,
                    "arn": $scope.item.ARN,
                    "gst": $scope.item.GST,
                    "aadhar": $scope.item.Aadhar,
                    "pms": $scope.item.PMS
                }

                $http({
                    url: BaseURL + 'user/SaveReversJournyDetails',
                    method: 'POST',
                    headers: {},

                    data: query
                }).then(function (response) {

                    var result = response;
                    if (result.data.code === 200) {
                        uploadCancelCheque();

                    }
                    else {
                        toastr.error('500Internal Server Error!', 'Bank Details Update!');

                    }
                }).catch(function (error) {
                    console.log('Error occurred (UpdateUserBankAccountTypeStatus): ', error);
                });

                //upload file
                var count = 0;
                for (var i = 0; i < array.length; i++) {

                    var formdata = new FormData();
                    formdata.append('File', array[i].File);
                    formdata.append('ImgName', array[i].FileName);
                    formdata.append('UserId', $scope.userId);
                    formdata.append('DoucmentTypeId', array[i].DocumentTypeId);

                    $http({
                        url: BaseURL + 'User/UploadFiles',
                        method: 'POST',
                        headers: { 'Content-Type': undefined }, // Corrected content type
                        transformRequest: angular.identity,
                        data: formdata

                    }).then(function (response) {

                        var result = response;
                        debugger
                        if (result.data.code === 200) {
                             
                            if ((array.length - 1) == count && $scope.CancelCheque == true) {
                                window.location.assign('/WP_Registration/WPRegistration/UploadChequeBankVerification');
                            }
                            if ((array.length - 1) == count) {
                                DownloadPDF();
                            }
                            count = count + 1;
                        }

                    })


                }
            }
        }

        function DownloadPDF() {

            $http({
                url: BaseURL + "DigioAPI/DwonloadPDF?userId=" + $scope.userId,
                method: "Get",
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.code == "200") {
                    toastr.success(res.data.message, 'Title Success!');
                    //toaster.success(res.data.message,'Title Success!');
                    setTimeout(() => {
                        window.location.assign("https://dbo.wealthcompany.in/EsignTestAPI/api/eSign/" + res.data.data);
                        
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
        $scope.findItemIndex = function () {
            $scope.index = $scope.items.findIndex(function (element) {
                return element.id === parseInt($scope.itemIdToFind, 10);
            });
        };
    });