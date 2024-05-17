angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $http) {
        $scope.isDisabled = false;
        $scope.aadharNo = ''
        $scope.PinCodev = ''
        $scope.state = ''
        $scope.city = ''
        $scope.Country = ''
        $scope.address1 = ''
        $scope.address2 = ''
        $scope.address3 = ''
        //   userId = '2EB4BBE3-5C47-4BFC-97CE-9B02EC931AAB'
        $scope.allowNumbers = function (event) {
            var charCode = event.which || event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
        };
        $scope.preventPaste = function (event) {
            event.preventDefault();
        };

        $scope.checkEmail = function () {

            var input = $scope.email;
            var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
            if (input.length > 0) {
            }
            else {
            }
            if (!emailPattern.test(input)) {
                $scope.CheckEmailPattern = true;
            } else {
                $scope.CheckEmailPattern = false;
            }
            if (input.length == 0) {
                $scope.CheckEmailPattern = false;
            }
        };
        $scope.clickFileInputpan = function () {
            document.getElementById('fileInputpan').click();
        };
        $scope.clickFileInputaddhar = function () {
            document.getElementById('fileInputaddhar').click();
        };
        $scope.onFileSelect = function (input, type) {
            if (input.files && input.files.length > 0) {
                var file = input.files[0];
                var size = $scope.checkFileSize(file.size, type);
                if (type === 'addhar') {
                    $scope.addharFilePath = file.name + ' (' + size + ')';
                    $scope.addharFileName = file.name;
                    $scope.addharImage = file;
                } else if (type === 'Pan') {
                    $scope.PanFilePath = file.name + ' (' + size + ')';
                    $scope.PanImage = file;
                    $scope.PanFileName = file.name;
                }
                $scope.$apply();
            }
        };
        $scope.GetCityCode = function (value) {
            if (value == '') {
                $scope.city = '';
                $scope.state = '';
                $scope.Country = '';
                $scope.isDisabled = false;
            }
            else {
                $http({
                    url: BaseURL + "Account/CityNameByPinCode?PinCode=" + value,
                    method: "GET",
                    headers: {}, // lowercase 'headers'
                    data: {}
                }).then(function (res) {

                    if (res.data.code == "200") {
                        if (res.data.data.message == 'Succeeded') {
                            $scope.city = res.data.data.city;
                            $scope.state = res.data.data.state;
                            $scope.Country = res.data.data.country;
                            $scope.isDisabled = true;
                            var elements = document.getElementsByClassName('ChangeColor');
                            for (var i = 0; i < elements.length; i++) {
                                elements[i].style.backgroundColor = '#0E1D22';
                            }
                        }
                        else if (res.data.data.message == 'PinCodeNotMatch') {
                            $scope.city = '';
                            $scope.state = '';
                            $scope.Country = '';
                            $scope.isDisabled = false;
                        }
                    }
                });
            }
        };

        $scope.validateInput = function (event) {
            var keyCode = event.which || event.keyCode;
            if ((keyCode >= 65 && keyCode <= 90) || // Alphabets (A-Z)
                (keyCode >= 97 && keyCode <= 122) || // Alphabets (a-z)
                keyCode === 8 || // Backspace
                keyCode === 9 || // Tab
                keyCode === 13 || // Enter
                (keyCode >= 37 && keyCode <= 40)) { // Arrow keys
                return true;
            } else {
                event.preventDefault();
                return false;
            }
        };

        $scope.FindValue = function (value) {
            if (value == 'add1') {
                var Add1 = $scope.address1;
                if (Add1.length == 40) {
                    $scope.Address1Valid = true;
                }
                else {
                    $scope.Address1Valid = false;
                }
            }
            if (value == 'add2') {
                if ($scope.address2.length == 30) {
                    $scope.Address2Valid = true;
                }
                else {
                    $scope.Address2Valid = false;
                }

            }

        }
        $scope.checkFileSize = function (bytes, type) {
            const maxSizeMB = 5;
            const fileSizeInMB = bytes / (1024 * 1024);
            if (fileSizeInMB > maxSizeMB) {
                if (type === 'addhar') {
                    $scope.Addharsize = true;
                } else if (type === 'Pan') {
                    $scope.Pansize = true;
                }
            } else {

                if (type === 'addhar') {
                    $scope.Addharsize = false;
                } else if (type === 'Pan') {
                    $scope.Pansize = false;
                }
                $scope.errorMessage = '';

            }
        };

        $scope.SubmitDetails = function () {


            if ($scope.aadharNo == null || $scope.aadharNo == undefined || $scope.aadharNo == '') {

                toastr.error('Aadhar Field  is Mandatory!', 'Error!');
            }
            else if ($scope.email == null || $scope.email == undefined || $scope.email == '') {

                toastr.error('Email Field  is Mandatory!', 'Error!');
            }
            else if ($scope.PinCodev == null || $scope.PinCodev == undefined || $scope.PinCodev == '') {

                toastr.error('Pincode Field  is Mandatory!', 'Error!');
            }
            else if ($scope.city == null || $scope.city == undefined || $scope.city == '') {

                toastr.error('City  is Mandatory!', 'Error!');
            }
            else if ($scope.state == null || $scope.state == undefined || $scope.state == '') {

                toastr.error('State Field  is Mandatory!', 'Error!');

            }
            else if ($scope.Country == null || $scope.Country == undefined || $scope.Country == '') {

                toastr.error('Country Field  is Mandatory!', 'Error!');
            }
            else if ($scope.address1 == null || $scope.address1 == undefined || $scope.address1 == '') {

                toastr.error('Address1 Field  is Mandatory!', 'Error!');
            }
            else if ($scope.PanFileName == null || $scope.PanFileName == undefined || $scope.PanFileName == '') {

                toastr.error('Upload Pan !', 'Error!');
            }
            else if ($scope.addharFileName == null || $scope.addharFileName == undefined || $scope.addharFileName == '') {

                toastr.error('Upload Aadhar !', 'Error!');
            }
            else {
                var dataForm = JSON.stringify({
                    'Aadhaar_No': $scope.aadharNo,
                    'Pincode': $scope.PinCodev,
                    'State': $scope.state,
                    'City': $scope.city,
                    'Country': $scope.Country,
                    'Address1': $scope.address1,
                    'Address2': $scope.address2,
                    'Address3': $scope.address3,
                    'UserId': userId
                });

                savefile($scope.addharImage, $scope.addharFileName, 2)
                savefile($scope.PanImage, $scope.PanFileName, 1)

                $http({
                    url: BaseURL + "User/SaveUserPersonalDetails",
                    method: "POST",
                    headers: {}, // lowercase 'headers'
                    data: dataForm
                }).then(function (res) {

                    if (res.data.code == "200") {
                        updatesatus();


                    } else {
                        toastr.error('500Internal Server Error', 'Submit Personal Details');
                    }
                });

            }
        };

        function savefile(file, imgName, documentType) {
            var formdata = new FormData();
            formdata.append('File', file);
            formdata.append('ImgName', imgName);
            formdata.append('UserId', userId);
            formdata.append('DoucmentTypeId', documentType);
            $http({
                url: BaseURL + "User/UploadFiles",
                method: "POST",
                headers: {
                    'Content-Type': undefined
                },
                transformRequest: angular.identity,
                data: formdata
            }).then(function (response) {
                if (response.data.code == "200") {

                } else {
                    toastr.error('500Internal Server Error!', 'Error!');
                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });

        }
        function updatesatus() {
            $http({
                url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=4",
                method: "GET",
                headers: {}, // lowercase 'headers'
                data: {}
            }).then(function (res) {
                if (res.data.code == "200") {

                    toastr.success('record created successfully', 'Title Success!');
                    setTimeout(function () {
                        window.location.assign('/WP_Registration/WPRegistration/qrbankverification');
                    }, 2000);

                }
            });

        }
    });