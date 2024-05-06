angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http) {

        qrcoderedirect();
        function qrcoderedirect() {

            $http({
                url: BaseURL + "account/getqrurl?userId=" + userId,
                method: "GET",
                headers: {},
                data: {}
            }).then(function (response) {
                var data = response;

                if (data.data.code === 200) {

                    var url = data.data.data;
                    console.log(data);

                    url = url.replace('Found. Redirecting to ', '');
                    location.href = url;
                }
                else {
                    window.location.assign('/WPRegistration/UploadChequeBankVerification');
                }
            });

        }

    });
