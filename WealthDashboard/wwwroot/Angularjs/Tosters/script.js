angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http) {
        getdata()
    $scope.pop = function(){
        toaster.pop('success', "title", '<ul><li>fhfghfghfgh</li></ul>', 5000, 'trustedHtml');
    };
    $scope.pip = function(){
        toaster.error("title", "error message");   
    };   
    
    $scope.clear = function(){
        toaster.clear();
    };


       function getdata()
        {
            $http({
                url: 'https://localhost:7120/WeatherForecast',
                method: "GET",
                data: {}
            })
                .then(function (response) {
                    

                    console.log(response);

                    // $scope.excelData.push(result[0]);
                },
                    function (response) { // optional
                        // failed
                    });

        }

});