angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, $location, $window, $http, $interval, toaster) {




        $scope.name = '';
        $scope.PAN = '';
        $scope.Mobile = '';
        $scope.Email = '';
        $scope.chkMutualFund = false;
        $scope.chkEquity = false;
        $scope.chkRMS = false;
        $scope.chkAIF = false;
        $scope.chkIBasket = false;
        $scope.chkFixedIncome = false;
        $scope.chkStartup = false;
        $scope.chkRealState = false;
        $scope.chkFundraising = false;
        $scope.Interested = '';
        $scope.Interestedid = '';
        $scope.GetData = [];
        $scope.agentcode = '';
        var InsertData = [];
        $scope.isPanFormateCorrect = false;
        $scope.inputvalid = false;
        var BaseURL =DasboardAPIURL;
        function saveData() {

            $http({
                url: BaseURL + 'LMS/InsertLMSData',
                method: 'POST',
                headers: {},
                data: InsertData
            }).then(function (res) {
                if (res.data.statusCode == "200") {
                    toastr.success('Data has been saved successfully!', 'Title Success!');

                    setTimeout(() => {
                        location.reload();
                    }, 3000);

                }

            }).catch(function (error) {
                toastr.error('Error occurred:', error);
            });
        }

        $scope.importData = function () {
            var filedata = document.getElementById("inputexcel");
            var file = filedata.files[0];
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;

                if (file.name.endsWith('.csv')) {
                    // Handle CSV file
                    var lines = data.split('\n');

                    for (var i = 0; i < jsonData.length; i++) {
                        InputData = {
                            Customer_Name: lines[i].Customer_Name.toString(),
                            mobile: lines[i].mobile.toString(),
                            email: lines[i].email.toString(),
                            pan: lines[i].pan.toString(),
                            interested_in: lines[i].interested_in.toString(),
                            interested_in_id: lines[i].interested_in_id.toString(),
                            userID: SessionData.Userid.toString()
                        }

                        InsertData.push(InputData);
                    }


                    saveData();
                    console.log(InsertData);

                }
                else if (file.name.endsWith('.xls') || file.name.endsWith('.xlsx')) {
                    // Handle Excel file
                    var workbook = XLSX.read(data, { type: 'binary' });
                    var sheetName = workbook.SheetNames[0];
                    var worksheet = workbook.Sheets[sheetName];
                    var jsonData = XLSX.utils.sheet_to_json(worksheet);
                    for (var i = 0; i < jsonData.length; i++) {
                        InputData = {
                            Customer_Name: jsonData[i].Customer_Name.toString(),
                            mobile: jsonData[i].mobile.toString(),
                            email: jsonData[i].email.toString(),
                            pan: jsonData[i].pan.toString(),
                            interested_in: jsonData[i].interested_in.toString(),
                            interested_in_id: jsonData[i].interested_in_id.toString(),
                            userID: SessionData.Userid.toString()
                        }

                        InsertData.push(InputData);
                    }


                    saveData();

                    console.log(InsertData);
                }
                else {
                    toastr.error('Unsupported file format', 'Error!');
                }
            };

            if (file == undefined) {
                InsertDataByInput();
            }
            else {
                reader.readAsBinaryString(file);
            }

        };


        function InsertDataByInput() {

            if ($scope.name == null || $scope.name == '' || $scope.name == undefined) {

                toastr.error('Please Enter Client Name', 'Error !');

            }
            //else if ($scope.PAN == null || $scope.PAN == '' || $scope.PAN == undefined) {

            //    toastr.error('Please Enter PAN Number', 'PAN Number !');
            //}
            else if ($scope.Mobile == null || $scope.Mobile == '' || $scope.Mobile == undefined) {

                toastr.error('Please Enter Mobile Number', 'Error !');
            }

            else if ($scope.inputvalid == true) {

                toastr.error('Please Enter Valid Mobile Number', 'Error!');
            }
            //else if ($scope.Email == null || $scope.Email == '' || $scope.Email == undefined) {

            //    toastr.error('Please Enter Email-ID', 'Email !');
            //}
            else {
                checkProduct();



                for (var i = 0; i < 1; i++) {
                    InputData = {
                        Customer_Name: $scope.name.toString(),
                        mobile: $scope.Mobile.toString(),
                        email: $scope.Email,
                        pan: $scope.PAN,
                        interested_in: $scope.Interested.toString(),
                        interested_in_id: $scope.Interestedid.toString(),
                        userID: SessionData.Userid.toString()
                    }

                    InsertData.push(InputData);
                }

                saveData();
            }
        }



        function checkProduct() {
            var Interestedin = '';
            var Interestedin_id = '';

            if ($scope.chkMutualFund == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',Mutual fund';
                    Interestedin_id = Interestedin_id + ',1';
                }
                else {
                    Interestedin = 'Mutual fund'
                    Interestedin_id = Interestedin_id + '1';
                }
            }
            if ($scope.chkEquity == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',Equity';
                    Interestedin_id = Interestedin_id + ',2';
                }
                else {
                    Interestedin = 'Equity';
                    Interestedin_id = Interestedin_id + '2';
                }

            }
            if ($scope.chkRMS == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',RMS';
                    Interestedin_id = Interestedin_id + ',3';
                }
                else {
                    Interestedin = 'RMS';
                    Interestedin_id = Interestedin_id + '3';
                }

            }
            if ($scope.chkAIF == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',AIF';
                    Interestedin_id = Interestedin_id + ',4';
                }
                else {
                    Interestedin = 'AIF';
                    Interestedin_id = Interestedin_id + '4';
                }

            }
            if ($scope.chkIBasket == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',IBasket';
                    Interestedin_id = Interestedin_id + ',5';
                }
                else {
                    Interestedin = 'IBasket';
                    Interestedin_id = Interestedin_id + '5';
                }

            }
            if ($scope.chkFixedIncome == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',Fixed Income';
                    Interestedin_id = Interestedin_id + ',6';
                }
                else {
                    Interestedin = 'Fixed Income';
                    Interestedin_id = Interestedin_id + '6';
                }

            }
            if ($scope.chkStartup == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',Start Up';
                    Interestedin_id = Interestedin_id + ',7';
                }
                else {
                    Interestedin = 'Start Up';
                    Interestedin_id = Interestedin_id + '7';
                }

            }
            if ($scope.chkRealState == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',Real Estate';
                    Interestedin_id = Interestedin_id + ',8';
                }
                else {
                    Interestedin = 'Real Estate'
                    Interestedin_id = Interestedin_id + '8';
                }

            }
            if ($scope.chkFundraising == true) {
                if (Interestedin != '') {
                    Interestedin = Interestedin + ',Fundraising';
                    Interestedin_id = Interestedin_id + ',9';
                }
                else {
                    Interestedin = 'Fundraising';
                    Interestedin_id = Interestedin_id + '9';
                }

            }


            $scope.Interested = Interestedin.toString();
            $scope.Interestedid = Interestedin_id.toString();
        }



        $scope.allowNumbers = function (event) {
            var charCode = event.which || event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
        };
        $scope.preventPaste = function (event) {
            event.preventDefault();
        };

        $scope.change = function () {
            var mobile = $scope.Mobile;
            var value = mobile.charAt(0);
            var numbers = [1, 2, 3, 4, 5];
            $scope.isvalied = numbers.find(function (e) {
                return e == value;
            });

            if (mobile.length < 10) {
                $scope.check = true;
            } else {
                $scope.check = true;
                for (var i = 0; i < mobile.length; i++) {
                    if (mobile[i] != value) {
                        $scope.check = false;

                        angular.element(document.getElementById('InputEmail')).focus();
                        break;
                    }
                }
                if ($scope.check) {
                    $scope.isvalied = 1;
                }
            }
        };


        $scope.ValidateMobileNumber = function () {
            var mobileNumber = document.getElementById("InputMobile");
            var lblError = document.getElementById("lblError");
            lblError.innerHTML = "";
            if (mobileNumber.value.length > 3) {
                //var expr = /^(\+\d{1,3}[- ]?)?\d{10}$/;
                var expr = /^[789]\d{9}$/;

                if (!expr.test(mobileNumber.value)) {
                    lblError.innerHTML = "Invalid Mobile Number.";
                    $scope.inputvalid = true;
                }
                else {
                    $scope.inputvalid = false;
                }

            }
        }



        $scope.CHeckPanNumber = function () {




            if ($scope.PAN.length > 5) {

                var input = $scope.PAN;
                // var panPattern = /[A-Z]{5}[0-9]{4}[A-Z]/;
                var panPattern = /^[A-Za-z]{5}[0-9]{4}[A-Za-z]$/;
                if (!panPattern.test(input)) {
                    $scope.isPanFormateCorrect = true;
                } else {
                    $scope.isPanFormateCorrect = false;
                }
            }
            else {
                $scope.isPanFormateCorrect = false;

            }

        }


        $scope.checkEmail = function () {

            var input = $scope.Email;
            var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
            if (input.length > 0) {
            }
            else {
            }
            if (!emailPattern.test(input)) {
                $scope.CheckEmailPattern = true;
            } else {
                $scope.CheckEmailPattern = false;
            }
            if (input.length == 0) {
                $scope.CheckEmailPattern = false;
            }
        };

        //============================================================================================

        ///for view page

        $scope.FetchData = function () {

            $scope.agentcode = SessionData.AgentSrno;

            $http({
                url: BaseURL + 'LMS/GetLMSData?Userid=' + SessionData.Userid.toString(),
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.statusCode == "200") {
                    $scope.GetData = res.data.data;
                }

            }).catch(function (error) {
                toastr.error('Error occurred:', error);
            });
        }

        $scope.EmailSend = function (mobile, customerName, email) {
            var Mobileno = '';
            var agentcode = '';

            $http({
                url: BaseURL + 'Client/GetEncryptData?plainText=' + mobile,
                method: 'GET',
                headers: {},
                data: {}
            }).then(function (res) {
                if (res.data.statusCode == "200") {
                    Mobileno = res.data.data;

                    $http({
                        url: BaseURL + 'Client/GetEncryptData?plainText=' + $scope.agentcode,
                        method: 'GET',
                        headers: {},
                        data: {}
                    }).then(function (res) {
                        if (res.data.statusCode == "200") {
                            agentcode = res.data.data;
                            url = 'https://wealthcompany.in/EKYC_MFJourney/home/loginview?WPCode=' + agentcode + '&MobNo=' + Mobileno + '&sourceType=LMS'
                            sendsms(mobile, customerName, url, email);
                            // window.open('/EKYC_MFJourney/home/loginview?WPCode=' + agentcode + '&MobNo=' + Mobileno + '&sourceType=LMS', '_blank');
                        }
                    })

                }

            }).catch(function (error) {
                toastr.error('Error occurred:', error);
            });

        }

            async function sendsms(mobile, customerName, url, email) {
              
            const response = await fetch(`https://api-ssl.bitly.com/v4/shorten`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer 52826353308d9b1ad4ff0787953accc2c84cf130' // Replace with your Bitly access token
                },
                body: JSON.stringify({ long_url: url })
            });
            const data = await response.json();
            if (data.link == null || data.link == '' || data.link == undefined) {

                toastr.error('link short issue:', error);
            }
            else {

                $http({
                    url: BaseURL + 'LMS/SendMessage?mobile=' + mobile + "&custmorName=" + customerName + "&url=" + data.link + "&email=" + email,
                    method: 'GET',
                    headers: {},
                    data: {}
                }).then(function (res) {
                    if (res.data.statusCode == "200") {
                        Mobileno = res.data.data;
                        toastr.success('Link send on your given mobile ' + Mobileno + ' also on your email ' + email + '  please check ');
                    }
                }).catch(function (error) {
                    toastr.error('Error occurred:', error);
                });

            }

        }



    });

