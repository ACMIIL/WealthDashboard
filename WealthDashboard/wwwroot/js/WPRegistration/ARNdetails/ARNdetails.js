angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $http) {
        $scope.isvalied = false;
        $scope.arnFileName = '';
        // var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";
       // var userId = localStorage.getItem('userId')
        //  var userId = 'DD9FCCC1-5813-478E-85B0-B4EC903A7064';
        $scope.appPanNo = localStorage.getItem('appPanNo')
        //  $scope.appPanNo = 'ARLPY9715A'
        $scope.SubmitDetails = function () {

            if ($scope.ARNNUMBER == null || $scope.ARNNUMBER == '' || $scope.ARNNUMBER == undefined) {

                toastr.error('Please Enter ARN Number', 'ARN Number !');
            }
            else if ($scope.arnFileName == null || $scope.arnFileName == '' || $scope.arnFileName == undefined) {

                toastr.error('Please Upload ARN File', 'ARN File !');
            }
            else {

                localStorage.setItem('ARNNo', "ARN-" + $scope.ARNNUMBER);
                localStorage.setItem('GSTNumber', $scope.gstNo);
                localStorage.setItem('PMSNumber', $scope.PMSNo);
                var formData = new FormData();
                formData.append('UserId', userId);
                formData.append('ARNNo', "ARN-" + $scope.ARNNUMBER);
                formData.append('ARNfile', $scope.arnImage);
                formData.append('ImgName', $scope.arnFileName);
                formData.append('GSTFile', $scope.gstImage);
                formData.append('GSTNumber', $scope.gstNo);
                formData.append('GSTName', $scope.gstFileName);
                formData.append('PMSNumber', $scope.PMSNo);
                formData.append('PMSFile', $scope.pmsImage);
                formData.append('PMSName', $scope.pmsFileName);

                $http({
                    url: BaseURL + "User/AddARNDetails",
                    method: "POST",
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).then(function (response) {
                    if (response.data.code == "200") {
                        $http({
                            url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=1",
                            method: "GET",
                            headers: {}, // lowercase 'headers'
                            data: {}
                        }).then(function (res) {
                            if (res.data.code == "200") {

                                CheckPanConfirmation(userId)                             
                            }
                        });
                    } else {
                        toastr.error('500Internal Server Error!', 'ARN Submit!');
                    }
                }).catch(function (error) {
                    console.error('Error occurred:', error);
                });

            }

        };

        function CheckPanConfirmation(Id) {

            $http({
                url: BaseURL + "Account/GetCVLData?userId=" + Id,
                method: "GET",
                headers: {}, // lowercase 'headers'
                data: {}
            }).then(function (res) {

                if (res.data.code == "200") {

                   
                    if (res.data.data.appPanNo == 'null' || res.data.data.appPanNo == undefined || res.data.data.appPanNo=='') {
                        window.location.assign('/WP_Registration/WPRegistration/DigiLocker');
                    } else {
                        window.location.assign('/WP_Registration/WPRegistration/PersonalDetails');
                    }
                }
                else {
                    toastr.error('500Internal Server Error!', 'Fetching Details!');
                }

            });
       
        }
        $scope.allowNumbers = function (event) {
            var charCode = event.which || event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
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



        $scope.onFileSelect = function (input, type) {
            if (input.files && input.files.length > 0) {
                var file = input.files[0];
                var size = $scope.checkFileSize(file.size, type);
                if (type === 'arn') {
                    $scope.arnFilePath = file.name + ' (' + size + ')';
                    $scope.arnFileName = file.name;
                    $scope.arnImage = file;
                } else if (type === 'gst') {
                    $scope.gstFilePath = file.name + ' (' + size + ')';
                    $scope.gstImage = file;
                    $scope.gstFileName = file.name;
                } else if (type === 'pms') {
                    $scope.pmsFilePath = file.name + ' (' + size + ')';
                    $scope.pmsImage = file;
                    $scope.pmsFileName = file.name;

                }
                $scope.$apply();
            }
        };

        $scope.checkFileSize = function (bytes, type) {

            const maxSizeMB = 5;
            const fileSizeInMB = bytes / (1024 * 1024);
            if (fileSizeInMB > maxSizeMB) {

                if (type === 'arn') {
                    $scope.arnsize = true;
                } else if (type === 'gst') {
                    $scope.gstsize = true;
                } else if (type === 'pms') {
                    $scope.pmssize = true;
                }

            } else {

                if (type === 'arn') {
                    $scope.arnsize = false;
                } else if (type === 'gst') {
                    $scope.gstsize = false;
                } else if (type === 'pms') {
                    $scope.pmssize = false;
                }
                $scope.errorMessage = '';

            }
        };
        $scope.change = function () {
            var ARN = $scope.ARNNUMBER;
            if (ARN.length < 1) {
                $scope.check = true;
                $scope.isvalied = 1;
            } else if (ARN.length > 0) {
                $scope.check = false;
                $scope.isvalied = 0;
            }

        };
    });

