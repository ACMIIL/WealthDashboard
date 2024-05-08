


angular.module('main', ['ngAnimate', 'toaster'])

    .controller('myController', function ($scope, toaster, $window, $http) {
       // var BaseURL = "https://localhost:7222/";
        //var  BaseURL="https://devwealthmaapi.investmentz.com


        $scope.pan = '';
        $scope.pandate = new Date();
        $scope.Name = '';
        $scope.DOB = '';

        $scope.isChecked = false;
        $scope.isDisabled = true;
        $scope.isvalidDate = '';

        $scope.isPanDateVisible = false;
        $scope.isPanFormateCorrect = false;
        $scope.panCorrectMessage = false;





        $scope.PanDetails = function () {



            var inputDate = document.getElementById('mydate');
            $http({
                url: BaseURL + "Account/GetCVLKRADetailsPan?userId=" + userId + "&PanNo=" + $scope.pan + "&DOB=" + inputDate.value.toString() + "&mob=" + localStorage.getItem('Mnumber'),
                method: "GET",
                headers: {},
                data: {}
            }).then(function (response) {
                var data = response;


                if (data.data.code === 200) {
                    var CVLKRA = data.data.data;


                    /*localStorage.setItem('appDOB', CVLKRA.appDOB);*/
                    /*localStorage.setItem('appEmail', CVLKRA.appEmail);*/
                    /*localStorage.setItem('appErrorDesc', CVLKRA.appErrorDesc);*/
                    /*localStorage.setItem('appGen', CVLKRA.appGen);*/
                    /*localStorage.setItem('appMobNo', CVLKRA.appMobNo);*/
                    /*localStorage.setItem('appName', CVLKRA.appName);*/
                    /*localStorage.setItem('appPanNo', CVLKRA.appPanNo);*/
                    /*localStorage.setItem('appPerAdd1', CVLKRA.appPerAdd1);*/
                    /*localStorage.setItem('appPerAdd2', CVLKRA.appPerAdd2);*/
                    /*localStorage.setItem('appPerAdd3', CVLKRA.appPerAdd3);*/
                    /*localStorage.setItem('appPerCity', CVLKRA.appPerCity);*/
                    /*localStorage.setItem('appPerPinCode', CVLKRA.appPerPinCode);*/
                    /*localStorage.setItem('appPerState', CVLKRA.appPerState);*/
                    /*localStorage.setItem('appUIDNo', CVLKRA.appUIDNo);*/
                    /*localStorage.setItem('PanDob', $scope.pandate);*/
                    //localStorage.setItem('Name', CVLKRA.appName);
                    //localStorage.setItem('DOB', CVLKRA.appDOB);
                    //$scope.Name = localStorage.getItem('appName');
                    //$scope.DOB = localStorage.getItem('DOB');

                    $scope.Name = CVLKRA.appName;
                    $scope.DOB = CVLKRA.appDOB;

                    var myModal = new bootstrap.Modal(document.getElementById('pandetailspopup'));

                    myModal.show();
                }
                else {

                    /*localStorage.setItem('appDOB', null);*/
                    /*localStorage.setItem('appEmail', null);*/
                    /*localStorage.setItem('appErrorDesc', null);*/
                    /*localStorage.setItem('appGen', null);*/
                    /*localStorage.setItem('appMobNo', null);*/
                    /*localStorage.setItem('appName', null);*/
                    /*localStorage.setItem('appPanNo', null);*/
                    /*localStorage.setItem('appPerAdd1', null);*/
                    /*localStorage.setItem('appPerAdd2', null);*/
                    /*localStorage.setItem('appPerAdd3', null);*/
                    /*localStorage.setItem('appPerCity', null);*/
                    /*localStorage.setItem('appPerPinCode', null);*/
                    /*localStorage.setItem('appPerState', null);*/
                    /*localStorage.setItem('appUIDNo', null);*/
                    /*localStorage.setItem('PanDob', null);*/
                    /*localStorage.setItem('Name', null);*/
                    /*localStorage.setItem('DOB', null);*/
                    $scope.Name = '';
                    $scope.DOB = '';
                    var myModal = new bootstrap.Modal(document.getElementById('pandetailspopup'));
                    myModal.show();
                }

            })





        }

        $scope.PanDetailnext = function () {
            UpdateStatus();
            /* window.location.assign('/WPRegistration/ARNdetails');*/
        }

        function UpdateStatus() {

            $http({
                url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=1",
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (response) {

                var result = response;

                if (result.data.code === 200) {
                    window.location.assign('/WP_Registration/WPRegistration/ARNdetails');
                }

            })


        }
        $scope.check = function (current) {


            var inputDate = document.getElementById('mydate');

            if (current.isChecked == true && inputDate.value != '' && $scope.pan != undefined) {


                $scope.isDisabled = false;
            }
            else {
                $scope.isDisabled = true;
            }
        };




        $scope.displayDate = function () {

            if ($scope.pan == null || $scope.pan.length == 0) {

                $scope.isPanDateVisible = false;

            }
            else {

                $scope.isPanDateVisible = true;
            }

        }



        $scope.CHeckPanNumber = function () {




            if ($scope.pan.length > 5) {

                var input = $scope.pan;
                // var panPattern = /[A-Z]{5}[0-9]{4}[A-Z]/;
                var panPattern = /^[A-Za-z]{5}[0-9]{4}[A-Za-z]$/;



                if (!panPattern.test(input)) {
                    $scope.isPanFormateCorrect = true;
                    $scope.panCorrectMessage = false;
                } else {
                    $scope.isPanFormateCorrect = false;
                    $scope.panCorrectMessage = true;
                }
            }
            else {
                $scope.isPanFormateCorrect = false;
                $scope.panCorrectMessage = false;

            }

        }


        $scope.RefreshPage = function () {
            location.reload();
        }


       


    })





