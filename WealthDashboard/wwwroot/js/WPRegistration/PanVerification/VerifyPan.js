angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http) {
        var baseUrl = "https://localhost:7222/";
        //var  baseUrl="https://devwealthmaapi.investmentz.com
        $scope.pan = ''; 
        $scope.CheckPanCorrect = function () {
            $scope.pan = $scope.pan.toUpperCase();
            if ($scope.pan.length === 10) { 
               
                $http({
                    url: baseUrl + "api/user/checkPanDetails?pan=" + $scope.pan,
                    method: "GET",
                    headers: {},
                    data: {}
                }).then(function (response) {
                    var data = response;

                    if (data.data.code === 200) { 
                        var array = JSON.parse(data.data.data);
                        if (array.length > 0) { 
                            toaster.error('Pan already used, Please enter another Pan Card No', 'Duplicate Pan');
                            $scope.pan = ''; 
                        } else {
                            panVerify(); 
                        }
                    }
                });
            }
        };

        $scope.transformToUpper = function () {
            $scope.pan = $scope.pan.toUpperCase();
        };


        function panVerify() {

            if ($scope.pan.length === 10 && this.registerForm.controls['date'].valid) {



            }


        }
    })


    

 
