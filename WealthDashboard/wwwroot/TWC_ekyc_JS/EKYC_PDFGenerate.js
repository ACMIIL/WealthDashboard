var ApprovedePDFPath = "D:\\WelcomeDesk_ApprovedPDF\\";


$(document).ready(function () {
    GetPhoto();
    $('#lblEmailID').text($('#EmailId').val());
    $('#lblMobile').text($('#MObileNo').val()); 

});

function GetPhoto() {

    var UCC = $('#UCC').val();
    $("#GetSelfie").prop("src", "https://web.investmentz.com/imgupload/20" + UCC + "//20" + UCC + "_PassportPhoto.png");

}


function EsignPdfgeneration() {

    $.ajax({
        url: CommonPageURL + "PDFGenerate/GeneratePDF",
        type: "GET",
        data: {
            RegistrationId: parseInt($("#RegistrationId").val()), //3971, 
            PDFSelect: "Normal",
            EsignType: "Yes"
        },
        success: function (data) {
            //debugger;
            if (data.data != null) {

                EsignSaveApprovedPDF();
                alert("Esign PDF Details Save successfully.!!!!");
                window.location.assign(data.data);
            }
            else {
                alert("Esign Link Not Generator");
            }
        },
        error: function (data) {

        }
    })
}

function ViewPDF() {
    $.ajax({
        type: "GET",
        url: CommonAPIURL + "api/ApprovedPDF/GetApprovedPDFData",
        data: {
            registrationId: parseInt($("#RegistrationId").val())
        },
        success: function (data) {
            var FilePath = data.filePath;
            window.open(FilePath);
        },
        error: function (data) {
            console.log("Error");
            return false;
        }
    });
}





function EsignSaveApprovedPDF() {
    var FilePath = "";
    var FileName = "";

    FilePath = ApprovedePDFPath + "20" + document.getElementById("UCC").value + "/20" + document.getElementById("UCC").value + ".pdf";
    FileName = "20" + document.getElementById("UCC").value + ".pdf";

    var jsonApprovedata = {
        "registrationId": parseInt(document.getElementById('RegistrationId').value),
        "fileName": FileName,
        "filePath": FilePath,
        "userId": "ACMIIL",
        "pDFType": "Esign"
    }
    $.ajax({
        url: CommonAPIURL + "api/ApprovedPDF/InsertOrUpdateApprovedPDFDetails",
        type: "POST",
        data: JSON.stringify(jsonApprovedata),
        contentType: "application/json",
        success: function (data) {
            console.log(data);
            if (data.Message = "Success") {
                alert("PDF Details Save successfully.!!!!");
            }
            else {
                swal({
                    title: "Issue in generating PDF, please connect our customer support department",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                });
            }
        },
        error: function (data) {
            alert("Error :" + data);
            return;
        }
    });
}