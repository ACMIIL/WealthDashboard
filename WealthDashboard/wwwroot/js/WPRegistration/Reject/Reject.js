angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {



        $scope.isPanDateVisible = false;
        $scope.isPanFormateCorrect = false;
        $scope.panCorrectMessage = false;
        $scope.panMessage = '';
        $scope.data = [];
        $scope.Selfie = true;
        $scope.PanCard = true;
        $scope.AadhaarFront = true;
        $scope.AadhaarBack = true;
        $scope.CancelCheque = true;
        $scope.Signature = true;
        $scope.ARN = true;
        $scope.GST = true;
        $scope.PMS = true;

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

            const maxSizeMB = 5;
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

            }
        };

    });