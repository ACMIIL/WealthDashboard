angular.module('main', ['ngAnimate', 'toaster'])

.controller('myController', function($scope, toaster, $window) {

    $scope.pop = function(){
        toaster.pop('success', "title", '<ul><li>jhsgfhdrjhhdfgjdf</li></ul>', 5000, 'trustedHtml');
    };
    $scope.pip = function(){
        toaster.error("title", "error message");
    };    

    $scope.clear = function(){
        toaster.clear();
    };
});