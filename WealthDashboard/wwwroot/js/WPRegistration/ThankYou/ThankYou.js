angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $window, $http, $interval, toaster) {

       // var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";

        $scope.userId = userId;
       // $scope.userId = 'BEAAE512-CB30-4E49-9D3A-838FD316932D';
        $scope.mob = localStorage.getItem('Mnumber');
        ngOnInit()
        function ngOnInit() {

            if ($scope.userId === null || $scope.userId === undefined) {
                window.location.assign('/wp_registration/wpregistration/index');
            }
            else {
                updateStatus();
            }

        }
        function updateStatus() {

            $http({
                url: BaseURL + "User/UpdateUserStatus?userId=" + $scope.userId + "&status=9",
                method: "GET",
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.code == "200") {

                    DownloadPDF();
                }

            }).catch(function (error) {
                console.error('Error occurred:', error);
            });

        }

        function sendMail() {

            $http({
                url: BaseURL + "user/EmailSent?USerID=" + $scope.userId ,
                method: "GET",
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.code == "200") {

                    setTimeout(() => {
                        window.location.assign('/MutualFund/Main');
                    }, 3000); 

                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });
        }

        function DownloadPDF() {

            $http({
                url: BaseURL + "PDF/GenerateMaRegistrationForm?UserID=" + $scope.userId,
                method: "POST",
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.message === "MA Registration PDF Genrated Successfully Please Check Genrated PDF file.") {
                    toastr.success(res.data.message,'Title Success!');
                    //toaster.success(res.data.message,'Title Success!');
                    //setTimeout(() => {
                    //    window.location.assign('/MutualFund/Main');
                    //}, 3000);

                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });
        }
        
       
    });

