angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $window, $http) {
        $scope.arnFileName = '';
       // var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";

      //  var userId = localStorage.getItem('userId')

        var camera_button = document.querySelector("#start-camera");
        var Retake_button = document.querySelector("#start-retake");
        var video = document.querySelector("#video");
        var click_button = document.querySelector("#click-photo");
        var canvas = document.querySelector("#canvas");

        Retake_button.addEventListener('click', async function () {
            var myModal = new bootstrap.Modal(document.getElementById('pandetails'));
            myModal.show();
            var stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
            video.srcObject = stream;
        });

        camera_button.addEventListener('click', async function () {
            var myModal = new bootstrap.Modal(document.getElementById('pandetails'));
            myModal.show();
            var stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
            video.srcObject = stream;
        });

        click_button.addEventListener('click', function () {
            var canvas = document.getElementById('canvas');
            canvas.style.display = 'block';
            //Start Camera Button Hide
            var Selfiebutton = document.getElementById('start-camera');
            Selfiebutton.style.display = 'none';
            //Logo images Hide
            var Selfielogo = document.getElementById('Selfieimage');
            Selfielogo.style.display = 'none';
            // Retake button in block

            var Retakebutton = document.getElementById('start-retake');
            Retakebutton.style.display = 'block';

            canvas.getContext('2d').drawImage(video, 0, 0, canvas.width, canvas.height);
            var image_data_url = canvas.toDataURL('image/jpeg');

        });
      $scope.Submit = function () {

            const imageName = 'Selfi.png';
            var canvas = document.getElementById('canvas');

            var ctx = canvas.getContext('2d');
            var drawingData = ctx.getImageData(0, 0, canvas.width, canvas.height).data;
            var hasDrawing = drawingData.some(function (alpha) {
                return alpha !== 0;
            });

            if (hasDrawing) {
                var imageDataUrl = canvas.toDataURL('image/jpeg');
                const imageBlob = dataURItoBlob(imageDataUrl);
                const imageFile = new File([imageBlob], imageName, { type: 'image/png' });
                var formData = new FormData();
                formData.append('File', imageFile);
                formData.append('ImgName', imageName);
                //   formData.append('UserId', 'BEAAE512-CB30-4E49-9D3A-838FD316932D');
                formData.append('UserId', userId);
                formData.append('DoucmentTypeId', "1");
                $http({
                    url: BaseURL + "User/SaveSelfi",
                    method: "POST",
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).then(function (response) {
                    if (response.data.code == "200") {
                        $http({
                            url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=7",
                            method: "GET",
                            headers: {},
                            data: {}
                        }).then(function (res) {
                            if (res.data.code == "200") {

                                window.location.assign('/WP_Registration/WPRegistration/SignatureVerification');
                            }
                        });
                    } else {
                        toastr.error('500Internal Server Error!', 'Upload Selfie!');
                    }
                }).catch(function (error) {
                    console.error('Error occurred:', error);
                });

                alert('Selfie taken successfully!');
            } else {
                toastr.error('Please take a selfie before proceeding.', 'Upload Selfie!');
            }
        };
        function dataURItoBlob(dataURI) {

            const parts = dataURI.split(',');
            const mimeString = parts[0].split(':')[1].split(';')[0];
            const byteString = atob(parts[1]);

            const arrayBuffer = new ArrayBuffer(byteString.length);
            const uint8Array = new Uint8Array(arrayBuffer);

            for (let i = 0; i < byteString.length; i++) {
                uint8Array[i] = byteString.charCodeAt(i);
            }
            const blob = new Blob([uint8Array], { type: mimeString });
            return blob;

        }
    });
