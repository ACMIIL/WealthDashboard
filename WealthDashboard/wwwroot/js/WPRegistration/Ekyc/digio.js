angular.module('main', ['ngAnimate', 'toaster'])
    .controller('myController', function ($scope, toaster, $location, $window, $http) {

        $scope.userId = localStorage.getItem('userId')

    $scope.Digio=function()
        {
            debugger

        var mobile = localStorage.getItem('Mnumber');
            var Agentsrno = localStorage.getItem('srno')
            var template = 'ACM_KYC_WORKFLOW'
            $http({
                url: BaseURL + "DigioAPI/CentralizeDigioWorkTemplate?customer_identifier=" + mobile + "&template_name=" + template + "&RegistrationId=" + Agentsrno + "&userId=" + userId,
                method: "GET",
                Headers: {},
                data: {}
            }).then(function (response) {
                //this.service.CentralizeDigioWorkTemplate(mobile, 'ACM_KYC_WORKFLOW', this.userId, Agentsrno).subscribe((response: any) => {
                //    debugger

                console.log(response);
                if (response.data.code == '200') {
                    var option = {
                        environment: "production",//PRODUCTION
                        callback: function (response) {

                            if (response.hasOwnProperty("true")) {
                                /// UploadPageEncryptionKey("this.userId");
                            }
                            else {
                            }
                            console.log("Signing completed successfully");
                            closeEvent(response);

                        },
                        logo: "https://web.investmentz.com/imgupload/Stamp/fav-icon.png",
                        theme: {
                            primaryColor: "#AB3498",
                            secondaryColor: "#000000"
                        }
                    }

                    var digio = new Digio(option);

                    digio.init();
                    digio.submit(response.data.data.data.id, response.data.data.data.customer_identifier, response.data.data.data.access_token.id);
                    var reqParam = response.data.data.data.id + "/" + response.data.data.data.reference_id + "/" + response.data.data.data.customer_identifier;
                    var CommonPageURL = "https://maregistration.wealthcompany.in/";
                    // var ResposnseURL = "https://app.digio.in/#/gateway/login/" + reqParam + "?redirect_url='https://localhost:7222/api/DigioAPI/CebtralizeDigilockerresponse?RegistrationId=4221F97B-BF03-448C-B353-56F980189922'";




                    var ResposnseURL = "https://app.digio.in/#/gateway/login/" + reqParam + "?redirect_url=" + CommonPageURL + "pan?EncRegistrationId="
                        + $scope.userId + "&status=success&digio_doc_id=" + response.data.data.data.id + "&message=Success";
                    ResposnseURL = ResposnseURL.toString().replace("?status", "&status")
                    ResposnseURL = ResposnseURL.toString().replace("?status", "&status")
                    console.log(ResposnseURL);
                    //window.open(ResposnseURL);

                    //this.router.navigate(['/qrscanner'])
                }
                else {
                    this.router.navigate(['/addPersonalDetails'])
                }



            }).catch(function (error) {
                console.error('Error sending OTP:', error);
            });
        }

        // });


 



        function closeEvent(data) {
            if (data.error_code == "CANCELLED") {

                toastr.error(data.message, 'Digio Locker Error');
            }
            else {
                var userId = localStorage.getItem('userId');

                //api/DigioAPI/CebtralizeDigilockerresponse
                $http({
                    url: BaseURL + "DigioAPI/CebtralizeDigilockerresponse?RegistrationId=" + userId,
                    method: "Get",
                    Headers: {},
                    data: {}
                }).then(function (response) {

                    if (response.data.code = '200') {
                        //api/User/UpdateUserStatus
                        $http({
                            url: BaseURL + "User/UpdateUserStatus?userId=" + userId + "&status=" + 4,
                            method: "GET",
                            Headers: {},
                            data: {}
                        }).then(function (res) {
                            if (res.data.code == "200") {
                                window.location.href = "/WPRegistration/PersonalDetails";
                            }
                        });

                    }
                }).catch(function (error) {
                    console.error('Error sending OTP:', error);
                });
            }
        }

  });