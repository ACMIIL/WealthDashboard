angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $window, $http) {
        $scope.arnFileName = '';
        // var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";

       // var userId = localStorage.getItem('userId')

        $scope.clickFileInputarn = function () {
            document.getElementById('fileInput').click();
        };
        $scope.onFileSelect = function (input, type) {
            if (input.files && input.files.length > 0) {
                var file = input.files[0];
                var size = $scope.checkFileSize(file.size, type);

                if (type === 'signature') {
                    $scope.arnFilePath = file.name + ' (' + size + ')';
                    $scope.arnFileName = file.name;
                    $scope.arnImage = file;
                }
                $scope.$apply();
            }
        };

        $scope.checkFileSize = function (bytes, type) {

            const maxSizeMB = 5;
            const fileSizeInMB = bytes / (1024 * 1024);
            if (fileSizeInMB > maxSizeMB) {

                if (type === 'signature') {
                    $scope.imagesize = true;
                }

            } else {

                if (type === 'signature') {

                    $scope.imagesize = false;
                }
                $scope.errorMessage = '';

            }
        };


        var canvas = document.getElementById('signatureCanvas');
        var ctx = canvas.getContext('2d');
        var isDrawing = false;

        canvas.addEventListener('mousedown', function (event) {
            isDrawing = true;
            ctx.beginPath();
            ctx.moveTo(event.offsetX, event.offsetY);
        });

        canvas.addEventListener('mousemove', function (event) {
            if (isDrawing) {
                ctx.lineTo(event.offsetX, event.offsetY);
                ctx.stroke();
            }
        });

        canvas.addEventListener('mouseup', function () {
            isDrawing = false;
        });

        $scope.clearCanvas = function () {

            ctx.clearRect(0, 0, canvas.width, canvas.height);
        };
        $scope.uploadSignature = function () {

            var imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
            var isEmpty = imageData.data.every(function (pixel) {
                return pixel === 0;
            });

            if (isEmpty == true && $scope.arnFileName.length < 1) {

                toastr.error('Please Select One Field For Signature', 'Signature!');

            }
            else {
                var formData = new FormData();
                if ($scope.arnFileName.length > 0) {
                    formData.append('File', $scope.arnImage);
                    formData.append('ImgName', $scope.arnFileName);
                } else {
                    const imageName = 'signature.png';
                    const imageBlob = dataURItoBlob(canvas.toDataURL().split(',')[1]);
                    const imageFile = new File([imageBlob], imageName, { type: 'image/png' });
                    formData.append('File', imageFile);
                    formData.append('ImgName', imageName);
                }

                // formData.append('UserId', 'BEAAE512-CB30-4E49-9D3A-838FD316932D');
                formData.append('UserId', userId);
                formData.append('DoucmentTypeId', "8");


                $http({
                    url: BaseURL + "User/UploadFiles",
                    method: "POST",
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).then(function (response) {
                    if (response.data.code == "200") {
                        $http({
                            url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=9",
                            method: "GET",
                            headers: {},
                            data: {}
                        }).then(function (res) {
                            if (res.data.code == "200") {
                               // window.location.assign('/WP_Registration/WPRegistration/Thankyou');
                                DownloadPDF();
                            }
                        });
                    } else {
                        toastr.error('500Internal Server Error!', 'Upload Signature!');
                    }
                }).catch(function (error) {
                    console.error('Error occurred:', error);
                });

            }


        };


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

        function dataURItoBlob(dataURI) {
            const byteString = window.atob(dataURI);
            const arrayBuffer = new ArrayBuffer(byteString.length);
            const int8Array = new Uint8Array(arrayBuffer);
            for (let i = 0; i < byteString.length; i++) {
                int8Array[i] = byteString.charCodeAt(i);
            }
            const blob = new Blob([int8Array], { type: 'image/png' });
            return blob;
        }


    });

