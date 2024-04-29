angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http) {


        $scope.sendOTPp = function () {
            alert("hello");
        };


    });
