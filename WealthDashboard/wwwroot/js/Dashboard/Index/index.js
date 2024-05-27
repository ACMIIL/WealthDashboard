angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope,$location, $window, $http) {
        var BaseURL = DasboardAPIURL;
        $scope.Totalrevanue = '';
        $scope.ProductName = '';
        $scope.result = '';
        $scope.totalInvestedValue = 0;
        $scope.totalCurrentValue = 0;
        $scope.lastMonthInvestedValue = 0;
        DocumentReady();
        function DocumentReady() {
            $http({
                method: 'GET',url:'/Dashboard/Dashboard/indexdeta',data: {},headers:{'Content-Type':'application/json'} 
            }).then(function (res) {
                $scope.result = JSON.parse(JSON.parse(res.data))
                if ($scope.result.statusCode == "200") {$scope.Totalrevanue = $scope.result.data1; $scope.ProductName = $scope.result.data2;
                        $scope.Totalrevanue.forEach(function (item) {
                        $scope.totalInvestedValue += parseInt(item.totInvestedValue);
                        $scope.totalCurrentValue += parseInt(item.totalCurrentValue);
                        $scope.lastMonthInvestedValue += parseInt(item.lastMonthInvestedValue);
                    });

                } else {
                    toastr.error('500Internal Server Error!', 'Fetching Details!');
                }
            });

        }

        $scope.GetValuebyid = function (x) {
              
            for (var i = 0;i < $scope.Totalrevanue.length; i++)
            {
                if (parseInt($scope.Totalrevanue[i].productId) === x) {
                    $scope.totalInvestedValue = 0;
                    $scope.totalCurrentValue = 0;
                    $scope.lastMonthInvestedValue = 0;
                    $scope.totalInvestedValue += parseInt($scope.Totalrevanue[i].totInvestedValue);
                    $scope.totalCurrentValue += parseInt($scope.Totalrevanue[i].totalCurrentValue);
                    $scope.lastMonthInvestedValue += parseInt($scope.Totalrevanue[i].lastMonthInvestedValue);
                }
            }
           
        }

    });

