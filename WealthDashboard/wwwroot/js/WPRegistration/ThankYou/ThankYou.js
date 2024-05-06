angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $window, $http, $interval,) {

       // var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com/";

        $scope.userId = userId;
       // $scope.userId = 'BEAAE512-CB30-4E49-9D3A-838FD316932D';
        $scope.mob = localStorage.getItem('Mnumber');
        ngOnInit()
        function ngOnInit() {

            if ($scope.userId === null || $scope.userId === undefined) {
                window.location.assign('/');
            }
            else {
                updateStatus()
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

           

                        sendMail();

                    
                
                   
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

                    SendDataToAcmiil();
                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });
        }

        function SendDataToAcmiil(){

            $http({
                url: BaseURL + "user/EmailSent?userid=" + $scope.userId,
                method: "GET",
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.code == "200") {

                   // window.location.href = 'https://madashboard.wealthcompany.in/';
                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });

        }

    });

