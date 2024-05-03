$(document).ready(function () {

   // getDigioSelfieResponseData();
});

function getDigioSelfieResponseData() {

    $.ajax({
        type: "GET",
        url: CommonPageURL + "Selfie/SelfieDigioResponseData",
        data: {
            RegistrationId: 4077,
            //InwardNo: "20" + ""
        },
        success: function (DigiLockerResponse) {
            console.log(DigiLockerResponse);
            if (DigiLockerResponse == "OK") {
                SelfieStatus = "Success";
                //alert("https://web.investmentz.com/imgupload/20" + localStorage.getItem("NewddlUCC") + "//20" + localStorage.getItem("NewddlUCC") + "_PassportPhoto.png");
                //$("#LivePhotocapture1").prop("src", "https://web.investmentz.com/imgupload/20" + localStorage.getItem("NewddlUCC") + "//20" + localStorage.getItem("NewddlUCC") + "_PassportPhoto.png");
                //document.getElementById('ContinueIPV').style.display = "block";
            } else {
                //SelfiePresentOrNot();
                // alert("https://web.investmentz.com/imgupload/20" + localStorage.getItem("NewddlUCC") + "//20" + localStorage.getItem("NewddlUCC") + "_PassportPhoto.png");
                //$("#LivePhotocapture1").prop("src", "https://web.investmentz.com/imgupload/20" + localStorage.getItem("NewddlUCC") + "//20" + localStorage.getItem("NewddlUCC") + "_PassportPhoto.png");
                //document.getElementById('ContinueIPV').style.display = "block";
            }
        },
        error: function (DigiLockerResponse) {

            return false;
        }
    });


}