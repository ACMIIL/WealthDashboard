
$(document).ready(function () {

    CentralizeGetDigioResponseData();
});
function CentralizeGetDigioResponseData() {
    // localStorage.getItem("ddlUCC");
    var UCCNo = $("#UCC").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Digio/CentralizeDigioResponseData",
        data: {
            ucc: UCCNo,
            RegistrationId: parseInt(document.getElementById('MainRegistrationId').value)
        },
        success: function (DigiLockerResponse) {
            console.log(DigiLockerResponse);

            if (DigiLockerResponse == "OK") {
                digioStatus = "Success";
            } else {
                digioStatus = "failed";
                //document.getElementById('divAadharDetails').style = "block";
            }
        },
        error: function (DigiLockerResponse) {
            digioStatus = "failed";
            alert("Exceptions");
            return false;
        }
    });
}