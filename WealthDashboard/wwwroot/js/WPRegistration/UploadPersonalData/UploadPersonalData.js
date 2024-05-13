angular.module('main', [])
    .controller('myController', function ($http, $scope) {
        $scope.aadharNo = '';
        $scope.email = '';
        $scope.pincode = '';
        $scope.city = '';
        $scope.country = '';
        $scope.address1 = '';
        $scope.address2 = '';
        $scope.address3 = '';
        $scope.pan = '';
        $scope.pansize = false;
        $scope.aadharsize = false;

        $scope.getStateCity = function() {

            let pincode = event.target.value;
            if (pincode.length == 6) {

                this.service.GetCityState_Pincode(pincode).subscribe((response: any) => {
                    if (response.code == "404") {
                        this.cityId = response.data.city_Code;
                        this.stateId = response.data.state_Code;
                        this.countryId = response.data.country_Code;
                        this.registerForm.controls['state'].patchValue(response.data.stateName);
                        this.registerForm.controls['country'].patchValue(response.data.countryName);
                        this.registerForm.controls['city'].patchValue(response.data.cityName);
                    }
                    else {

                    }
                });

            }
        }

    });