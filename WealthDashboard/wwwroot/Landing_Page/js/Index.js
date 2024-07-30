var app = angular.module('myApp', []);

app.directive('fileChange', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var fn = $parse(attrs.fileChange);
            element.on('change', function (event) {
                scope.$apply(function () {
                    fn(scope, { $event: event });
                });
            });
        }
    };
}]);

app.controller('myCtrl', function ($scope, $http, $sce, $timeout) {
  
    $scope.imageHowtoHelpUrls = [];
    $scope.mergedHowtoHelpData = [];
    $scope.activeTabIndex = 0; 
    $scope.ispatner = 0;


    $scope.showbutton=function(val)
    {
        $scope.ispatner = val;
    }
    $scope.showpage = function (val) {
        window.location.href = val
    };
    function getData() {
        var url =   '/Home/MainManu';
        $http.get(url)
            .then(function (response) {
                var data = response.data;
                if (data && data.data && data.data.data.images) {
                    // Extract image names from full paths
                    $scope.imageUrls = data.data.data.images.map(function (imageUrl) {
                        return {
                            name: imageUrl.substring(imageUrl.lastIndexOf('/') + 1), // Extracting the image name
                            url: imageUrl // Keeping the full URL for reference if needed
                        };
                    });

                    if (data && data.data && data.data.data.sliderData) {
                        $scope.sliderDatas = data.data.data.sliderData;

                        // Filter, sort, and map slider data
                        $scope.mergedData = $scope.sliderDatas
                            .filter(function (sliderItem) {
                                return sliderItem.active;
                            })
                            .sort(function (a, b) {
                                return a.numberingPosition - b.numberingPosition;
                            })
                            .map(function (sliderItem) {
                                var imageUrl = $scope.imageUrls.find(function (image) {
                                    return image.name === sliderItem.sliderImages;
                                });

                                return {
                                    header: sliderItem.header,
                                    content: sliderItem.content,
                                    url: sliderItem.url,
                                    trustedSliderImages: $sce.trustAsHtml(`<img src="${imageUrl ? imageUrl.url : ''}" class="d-block w-100" alt="..." style="width: 100%; height: auto;">`),
                                    sliderimagesPath: sliderItem.sliderImages,
                                    id: sliderItem.id,
                                    active: sliderItem.active,
                                    numberingPosition: sliderItem.numberingPosition,
                                    editMode: false,
                                    delete: false,
                                    HideEdit: false,
                                    isNew: false
                                };
                            });
                    } else {
                        console.error('No slider data found in response.');
                    }
                } else {
                    console.error('No images found in response.');
                }
            })
            .catch(function (error) {
                console.error('Error occurred:', error);
            });
        Content1();
        Content2();
        HowToHelp();
        RangeOfProduct();
        OurTeam();
    }
    $scope.trustAsHtml = function (html) {
        return $sce.trustAsHtml(html);
    };

    function Content1() {
        var url =  '/Home/MainManuContent1';
        $http.get(url)
            .then(function (response) {
                var data = response.data;
                if (data && data.data && data.data.data.sliderData) {
                    $scope.Content1Datas = data.data.data.sliderData;

                    // Filter for active items and sort by numberingPosition
                    $scope.mergedContent1Data = $scope.Content1Datas
                        .filter(function (sliderItem) {
                            return sliderItem.active; // Only include active items
                        })
                        .sort(function (a, b) {
                            return a.numberingPosition - b.numberingPosition; // Sort by numberingPosition
                        })
                        .map(function (sliderItem) {
                            return {
                                header: sliderItem.header,
                                content: sliderItem.content,
                                id: sliderItem.id,
                                active: sliderItem.active,
                                numberingPosition: sliderItem.numberingPosition
                            };
                        });
                } else {
                    console.error('No slider data found in response.');
                }
            })
            .catch(function (error) {
                console.error('Error occurred:', error);
            });
    }

    function Content2() {
        var url =  '/Home/MainManuContent2';

        $http.get(url)
            .then(function (response) {
                var data = response.data;
                if (data && data.data && data.data.data.sliderData) {
                    $scope.Content2Datas = data.data.data.sliderData;

                    // Filter for active items and sort by numberingPosition
                    $scope.mergedContent2Data = $scope.Content2Datas
                        .filter(function (sliderItem) {
                            return sliderItem.active; // Only include active items
                        })
                        .sort(function (a, b) {
                            return a.numberingPosition - b.numberingPosition; // Sort by numberingPosition
                        })
                        .map(function (sliderItem) {
                            return {
                                content: sliderItem.content,
                                id: sliderItem.id,
                                active: sliderItem.active,
                                numberingPosition: sliderItem.numberingPosition
                            };
                        });
                } else {
                    console.error('No slider data found in response.');
                }
            })
            .catch(function (error) {
                console.error('Error occurred:', error);
            });
    }


    $scope.trustAsHtmlHowtoHelp = function (html) {
        return $sce.trustAsHtml(html);
    };


    function HowToHelp() {
        var url =   '/Home/GetDataTWChelp';
        $http.get(url)
            .then(function (response) {
                var data = response.data;
                if (data && data.data && data.data.data.images) {
                    $scope.imageHowtoHelpUrls = data.data.data.images.map(function (imageUrl) {
                        return {
                            name: imageUrl.substring(imageUrl.lastIndexOf('/') + 1),
                            url: imageUrl
                        };
                    });

                    if (data && data.data && data.data.data.sliderData) {
                        $scope.HowtoHelpDatas = data.data.data.sliderData;

                        // Filter for active items and sort by numberingPosition
                        $scope.mergedHowtoHelpData = $scope.HowtoHelpDatas
                            .filter(function (sliderItem) {
                                return sliderItem.active; // Only include active items
                            })
                            .sort(function (a, b) {
                                return a.numberingPosition - b.numberingPosition; // Sort by numberingPosition
                            })
                            .map(function (sliderItem) {
                                var imageUrl = $scope.imageHowtoHelpUrls.find(function (image) {
                                    return image.name === sliderItem.sliderImages;
                                });
                                return {
                                    header: sliderItem.header,
                                    headercontent: sliderItem.headerContent,
                                    content: sliderItem.content,
                                    sliderimagesPath: sliderItem.sliderImages,
                                    id: sliderItem.id,
                                    trustedSliderImages: $sce.trustAsHtml('<img src="' + (imageUrl ? imageUrl.url : '') + '" class="w-100" alt="' + sliderItem.header + '" />')
                                };
                            });

                        // Load the image for the first tab by default
                        $scope.activeTabIndex = 0;
                        $scope.loadImageForTab(0);
                    } else {
                        console.error('No slider data found in response.');
                    }
                } else {
                    console.error('No images found in response.');
                }
            })
            .catch(function (error) {
                console.error('Error occurred:', error);
            });
    }
    // Function to load images for a specific tab
    $scope.loadImageForTab = function (index) {
        var sliderItem = $scope.mergedHowtoHelpData[index];
        var imageUrl = $scope.imageHowtoHelpUrls.find(function (image) {
            return image.name === sliderItem.sliderimagesPath;
        });
        sliderItem.trustedSliderImages = $sce.trustAsHtml(`<img src="${imageUrl ? imageUrl.url : ''}" class="w-100">`);
        $scope.activeTabIndex = index; // Update the active tab index
    };

    $scope.trustAsHtmlRangeOfProduct = function (html) {
        return $sce.trustAsHtml(html);
    };


    function RangeOfProduct() {
        var url =   '/Home/GetDataProducts';
        $http.get(url)
            .then(function (response) {
                var data = response.data;
                if (data && data.data && data.data.data.images) {
                    // Extract image names from full paths
                    $scope.imageRangeOfProductsUrls = data.data.data.images.map(function (imageUrl) {
                        return {
                            name: imageUrl.substring(imageUrl.lastIndexOf('/') + 1),
                            url: imageUrl
                        };
                    });

                    if (data && data.data && data.data.data.sliderData) {
                        $scope.RangeOfProductsDatas = data.data.data.sliderData;

                        // Filter for active items and sort by numberingPosition
                        $scope.mergedRangeOfProductsData = $scope.RangeOfProductsDatas
                            .filter(function (sliderItem) {
                                return sliderItem.active; // Only include active items
                            })
                            .sort(function (a, b) {
                                return a.numberingPosition - b.numberingPosition; // Sort by numberingPosition
                            })
                            .map(function (sliderItem) {
                                var imageUrl = $scope.imageRangeOfProductsUrls.find(function (image) {
                                    return image.name === sliderItem.sliderImages;
                                });
                                return {
                                    Products: sliderItem.header,
                                    content: sliderItem.content,
                                    sliderImagess: `<img src="${imageUrl ? imageUrl.url : ''}" alt="" class="img-fluid">`,
                                    sliderimagesPath: sliderItem.sliderImages,
                                    id: sliderItem.id
                                };
                            });

                        $scope.mergedRangeOfProductsData.forEach(function (item) {
                            item.trustedSliderImages = $sce.trustAsHtml(item.sliderImagess);
                        });
                    } else {
                        console.error('No slider data found in response.');
                    }
                } else {
                    console.error('No images found in response.');
                }
            })
            .catch(function (error) {
                console.error('Error occurred:', error);
            });
    }



    $scope.trustAsHtmlOurTeam = function (html) {
        return $sce.trustAsHtml(html);
    };

    function OurTeam() {
        var url =   '/Home/GetDataOurTeam';
        $http.get(url)
            .then(function (response) {
                var data = response.data;
                if (data && data.data && data.data.data.images) {
                    $scope.imageTeamUrls = data.data.data.images.map(function (imageUrl) {
                        return {
                            name: imageUrl.substring(imageUrl.lastIndexOf('/') + 1),
                            url: imageUrl
                        };
                    });

                    if (data && data.data && data.data.data.sliderData) {
                        $scope.TeamDatas = data.data.data.sliderData;

                        // Filter for active items and sort by numberingPosition
                        $scope.mergedteamData = $scope.TeamDatas
                            .filter(function (sliderItem) {
                                return sliderItem.active; // Only include active items
                            })
                            .sort(function (a, b) {
                                return a.numberingPosition - b.numberingPosition; // Sort by numberingPosition
                            })
                            .map(function (sliderItem) {
                                var imageUrl = $scope.imageTeamUrls.find(function (image) {
                                    return image.name === sliderItem.sliderImages;
                                });
                                return {
                                    Name: sliderItem.name,
                                    Position: sliderItem.position,
                                    content: sliderItem.content,
                                    sliderImagess: `<img src="${imageUrl ? imageUrl.url : ''}" class="w-100" alt="${sliderItem.name}" />`,
                                    sliderimagesPath: sliderItem.sliderImages,
                                    id: sliderItem.id
                                };
                            });

                        $scope.mergedteamData.forEach(function (item) {
                            item.trustedSliderImages = $sce.trustAsHtml(item.sliderImagess);
                        });

                        // Use $timeout to ensure DOM is updated before initializing Slick slider
                        $timeout(function () {
                            $('.single-item').slick({
                                dots: true,
                                infinite: true,
                                speed: 300,
                                slidesToShow: 1,
                                adaptiveHeight: true
                            });
                        }, 0);
                    } else {
                        console.error('No slider data found in response.');
                    }
                } else {
                    console.error('No images found in response.');
                }
            })
            .catch(function (error) {
                console.error('Error occurred:', error);
            });
    }



    $scope.openModal = function (index) {
        $scope.selectedTeamData = $scope.mergedteamData[index];
    };

    getData();



    //-----------------------------------------------------------login code------------------------------------
    $scope.userid = '';
    $scope.password = '';
    $scope.selectedColor = '';
    $scope.inputType = 'password';
    $scope.Login = function () {
        if ($scope.userid == null || $scope.userid == undefined || $scope.userid == '') {

            alert('Please Enter your UserName', 'Error!');

        }
        else if ($scope.password == null || $scope.password == undefined || $scope.password == '') {
            alert('Enter Your Password', 'Error!');

        }
        else {

            var query = JSON.stringify({ // Data to send to the server
                userID: $scope.userid,
                password: $scope.password

            });

            $http({
                url: "/home/Login?userid=" + $scope.userid + "&password=" + $scope.password,//https://uattwcapi.wealthcompany.in/api/Admin/UserAuthenticationadmin",
                method: "POST",
                Headers: {},
                data: {}
            }).then(function (response) {
                if (response.data == 9) {

                    alert('Your journey is in progress.', 'Try Again After  Sometime!');
                }
                else if (response.data === true || response.data === 'true') {
                    var result = '';
                    //alert("sucess")
                    window.location.assign('/Dashboard/Dashboard/index')
                    // toastr.success('An OTP has been sent to ' + $scope.MobileNumber, 'Title Success!');
                }
                else {
                    alert('Invalid Username & Password ', 'Try Again!');
                    $scope.userid = '';
                    $scope.password = '';
                    //  alert("Login faild!")
                }
            }).catch(function (error) {
                console.log('Error sending OTP:', error);
            });

        }

    }


    $scope.handleKeyPress = function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            $scope.Login();
            // $scope.showAlert();
        }
    };

    $scope.toggleInputType = function () {
        $scope.inputType = ($scope.inputType === 'text') ? 'password' : 'text';
    };

    $scope.register = function (value) {
        if (value == 'wp') {
            window.location.assign('/WP_Registration/WPRegistration/index')
        }
        else {
            window.location.assign('/EKYC_MFJourney/home/loginview')
        }

    }
});


