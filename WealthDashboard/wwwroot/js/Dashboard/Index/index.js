angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $window, $http) {
        var BaseURL = DasboardAPIURL;
        
 
        $scope.Totalrevanue = '';
        $scope.ProductName = '';
        $scope.result = '';
        $scope.activeproduct = 10;
        $scope.totalinvestment = 0;
        $scope.totalInvestedValue = 0;
        $scope.totalcurrentmontinvestment = 0;
        $scope.totalCurrentMonthValue = 0;
        $scope.totallastmonthinvestment = 0;
        $scope.lastMonthInvestedValue = 0;
      
        DocumentReady();
        function DocumentReady() {
            $http({
                method: 'GET',
               // url: BaseURL + 'Dashboard/GetWPTotalRevanue', data: {}, headers: { 'Content-Type': 'application/json' }
                url: '/Dashboard/Dashboard/indexdeta', data: {}, headers: { 'Content-Type': 'application/json' }
                    }).
             //.then(function (res) {
            //    $scope.result = (res.data)
            //    if ($scope.result.statusCode == "200") {
            //        $scope.Totalrevanue = $scope.result.data1; $scope.ProductName = $scope.result.data2;
            //        $scope.Totalrevanue.forEach(function (item) {
            //            $scope.totalInvestedValue += parseInt(item.totInvestedValue);
            //            $scope.totalCurrentValue += parseInt(item.totalCurrentValue);
            //            $scope.lastMonthInvestedValue += parseInt(item.lastMonthInvestedValue);
            //        });
            //    } else {
            //        toastr.error('500Internal Server Error!', 'Fetching Details!');
            //    }
            //});
            then(function (res) {
                $scope.result = JSON.parse(JSON.parse(res.data))
                if ($scope.result.statusCode == "200") {
                    $scope.Totalrevanue = $scope.result.data1; $scope.ProductName = $scope.result.data2;
                    $scope.Totalrevanue.forEach(function (item) {
                        $scope.totalinvestment += parseInt(item.totInvestedValue);                      
                        $scope.totalInvestedValue += parseInt(item.totalCurrentValue);
                    
                        $scope.totalcurrentmontinvestment += parseInt(item.currentMonthInvestedValue);
                        $scope.totalCurrentMonthValue += parseInt(item.currentMonthValue);
                  
                        $scope.totallastmonthinvestment += parseInt(item.lastMonthInvestedValue);
                        $scope.lastMonthInvestedValue += parseInt(item.lastMonthCurrentValue);
                    });
                } else {
                    toastr.error('500Internal Server Error!', 'Fetching Details!');
                }
            }).catch(function (error) {
                toastr.error('Error occurred:', error);
            });
        }
        $scope.GetValuebyid = function (x) {
            $scope.activeproduct = x;
            if (x == 10) {
                $scope.totalinvestment = 0;
                $scope.totalInvestedValue = 0;
                $scope.totalcurrentmontinvestment = 0;
                $scope.totalCurrentMonthValue = 0;
                $scope.totallastmonthinvestment = 0;
                $scope.lastMonthInvestedValue = 0;
                DocumentReady();
                return;
            }

            for (var i = 0; i < $scope.Totalrevanue.length; i++) {
                if  (parseInt($scope.Totalrevanue[i].productId) === x)  
                {
                    $scope.totalinvestment = 0;
                    $scope.totalInvestedValue = 0;
                    $scope.totalcurrentmontinvestment = 0;
                    $scope.totalCurrentMonthValue = 0;
                    $scope.totallastmonthinvestment = 0;
                    $scope.lastMonthInvestedValue = 0;

                  /*  $scope.totalInvestedValue += parseInt($scope.Totalrevanue[i].totInvestedValue);*/

                    $scope.totalinvestment += parseInt($scope.Totalrevanue[i].totInvestedValue);
                    $scope.totalInvestedValue += parseInt($scope.Totalrevanue[i].totalCurrentValue);

                    $scope.totalcurrentmontinvestment += parseInt($scope.Totalrevanue[i].currentMonthInvestedValue);
                    $scope.totalCurrentMonthValue += parseInt($scope.Totalrevanue[i].currentMonthValue);

                    $scope.totallastmonthinvestment += parseInt($scope.Totalrevanue[i].lastMonthInvestedValue);
                    $scope.lastMonthInvestedValue += parseInt($scope.Totalrevanue[i].lastMonthCurrentValue);
                }
            }
        }
    });