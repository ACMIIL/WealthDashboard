var ApprovedePDFPath = "D:\\WelcomeDesk_ApprovedPDF\\";
encucc = $("#encucc").val();
$(document).ready(function () {
    $("#selfievarifi").removeClass('a');
    $("#selfievarifi").addClass('b');
    CheckSelfie();
    CheckEsign();
});

//function SaveGeoDetails() {
//    $.ajax({
//        type: "GET",
//        url: CommonPageURL + "Selfie/SaveGeoDetails",
//        data: {
//            RegistrationId: $("#RegistrationId").val()
//        },
//        success: function (data) {
//            console.log(data);
//        },
//        error: function (data) {
//        }
//    });
//}
function OpenSelfieView() {
    var rid = $("#RegistrationId").val();
    var mobileno = $("#hiddenmobileno").val();
    var enrid = $("#EncRegistrationId").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Selfie/SelfieDigioWorkTemplate",
        data: {
            customer_identifier: mobileno,
            template_name: "LIVE_DEMO",
            RegistrationId: rid
        },
        success: function (DigiLockerTemplate) {
            console.log(DigiLockerTemplate);
            var options = {
                environment: "production",
                callback: function (response) {

                    if (response.message == "KYC process completed") {
                        getDigioSelfieResponseData();
                        //window.location.assign(CommonPageURL + "Nominee/Nominee_Details" + '?encregi=' + "sLa6rsSqFy8");

                    }
                    else {
                        alert("Selfie not done try again");
                        //window.location.assign(CommonPageURL + "Selfie/SelfieView" + '?encucc=' + encucc + "");
                    }
                },
                logo: "https://web.investmentz.com/imgupload/Stamp/fav-icon.png",
                theme: {
                    primaryColor: "#AB3498",
                    secondaryColor: "#000000"
                }
            }
            var digio = new Digio(options);
            digio.init();
            var reqParam = DigiLockerTemplate.data.id + "/" + DigiLockerTemplate.data.reference_id + "/" + DigiLockerTemplate.data.customer_identifier;

            digio.submit(DigiLockerTemplate.data.id, DigiLockerTemplate.data.customer_identifier, DigiLockerTemplate.data.access_token.id);
            var ResposnseURL = "https://api.digio.in/#/gateway/login/" + reqParam + "?redirect_url=" + CommonPageURL + "Selfie/SelfieDigioVerificationView?EncRegistrationId="
                + enrid + "&status=success&digio_doc_id=" + DigiLockerTemplate.data.id + "&message=Success";
            console.log(ResposnseURL);
            ResposnseURL = ResposnseURL.toString().replace("?status", "&status");
            console.log(ResposnseURL);
            //window.open(ResposnseURL);
        },
        error: function (DigiLockerTemplate) {
            console.log("Error");
            return false;
        }
    });
}


function getDigioSelfieResponseData() {
    var UCC = $("#UCC").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Selfie/SelfieDigioResponseData",
        data: {
            RegistrationId: $("#RegistrationId").val(),
            InwardNo: "20" + UCC
        },
        success: function (DigiLockerResponse) {
            console.log(DigiLockerResponse);
            if (DigiLockerResponse == "OK") {
                SelfieStatus = "Success";

                $("#selfievarifi").removeClass('b');
                $("#selfievarifi").addClass('a');

                $("#esignvarifi").removeClass('a');
                $("#esignvarifi").addClass('b');

                $("#LivePhotocapture1").prop("src", "https://web.investmentz.com/imgupload/20" + UCC + "//20" + UCC + "_PassportPhoto.png");
                document.getElementById('takeselfie').style.display = "none";
                document.getElementById('Copylink').style.display = "none";
                document.getElementById('saveselfie').style.display = "block";
                document.getElementById('retakeselfie').style.display = "block";
                //document.getElementById('Displaypdf').style.display = "block";
                if ($('#BACode').val() === "RC319") {
                    Normal_BOIpdfgeneration("Normal", "No")
                }
                else {
                    Normalpdfgeneration("Normal", "No");
                }
            } else {
                alert("something went wrong");
            }
        },
        error: function (DigiLockerResponse) {

            return false;
        }
    });
}

function RetakeSelfie() {
    OpenSelfieView();
}

function CheckSelfie() {
    var UCC = $("#UCC").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Selfie/CheckSelfieDetails",
        data:
        {
            RegistrationId: $("#RegistrationId").val()
        },
        success: function (data) {
            if (data == "Present") {
                $("#selfievarifi").removeClass('b');
                $("#selfievarifi").addClass('a');

                $("#esignvarifi").removeClass('a');
                $("#esignvarifi").addClass('b');

                var UCC = $('#UCC').val();

                $("#LivePhotocapture1").prop("src", "https://web.investmentz.com/imgupload/20" + UCC + "//20" + UCC + "_PassportPhoto.png");
                document.getElementById('takeselfie').style.display = "none";
                document.getElementById('Copylink').style.display = "none";
                document.getElementById('saveselfie').style.display = "block";
                //document.getElementById('Displaypdf').style.display = "block";
                document.getElementById('retakeselfie').style.display = "block";
                if ($('#BACode').val() === "RC319") {
                    Normal_BOIpdfgeneration("BOI", "No");
                }
                else {
                    Normalpdfgeneration("Normal", "No");
                }
            }
        },
        error: function (data) {

        }
    });
}

function Normal_BOIpdfgeneration(PDFSelect, EsignType) {
    $.ajax({
        url: CommonPageURL + "PDFGenerate/GeneratePDF_BOI",
        type: "GET",
        data: {
            RegistrationId: parseInt($("#RegistrationId").val()), //3971,
            PDFSelect: PDFSelect,
            EsignType: EsignType
        },
        success: function (data) {

            if (data == "PDF has been generated successfully!") {
                saveApprovedPDF(PDFSelect, EsignType);
            }
            else {
                alert(data);
            }
        },
        error: function (data) {
            alert(data);
        }
    })
}



function Normalpdfgeneration(PDFSelect, EsignType) {
    var PDFUrl = "";
    if ($('#BACode').val() === "RC319") {
        PDFSelect = "BOI";
        alert(PDFSelect);
        PDFUrl = CommonPageURL + "PDFGenerate/GeneratePDF_BOI";
    }
    else {
        PDFUrl = CommonPageURL + "PDFGenerate/GeneratePDF"
    }
    $.ajax({
        url: PDFUrl,
        type: "GET",
        data: {
            RegistrationId: parseInt($("#RegistrationId").val()),
            PDFSelect: PDFSelect,
            EsignType: EsignType
        },
        success: function (data) {

            if (data == "PDF has been generated successfully!") {

                saveApprovedPDF(PDFSelect, EsignType);
                alert(data);
            }
            else {
                alert(data);
            }
        },
        error: function (data) {
            console.log(data);
            alert("Error: PDF Issue Conatct to Administartor.");
        }
    })
}

function saveApprovedPDF(PDFSelect, EsignType) {
    var enrid = $("#EncRegistrationId").val();
    var FilePath = "";
    var FileName = "";
    
    if (PDFSelect == 'BOI' && EsignType == "No") {
        FilePath = ApprovedePDFPath + "20" + document.getElementById("UCC").value + "/20" + document.getElementById("UCC").value + "Normal_BOI.pdf";
        FileName = "20" + document.getElementById("UCC").value + "Normal_BOI.pdf";
    }
    else if (PDFSelect == 'BOI' && EsignType == "Yes") {
        FilePath = ApprovedePDFPath + "20" + document.getElementById("UCC").value + "/20" + document.getElementById("UCC").value + "_BOI.pdf";
        FileName = "20" + document.getElementById("UCC").value + "_BOI.pdf";
    }
    else if (PDFSelect == "Esign") {
        FilePath = ApprovedePDFPath + "20" + document.getElementById("UCC").value + "/20" + document.getElementById("UCC").value + ".pdf";
        FileName = "20" + document.getElementById("UCC").value + ".pdf";
    }
    else {
        FilePath = ApprovedePDFPath + "20" + document.getElementById("UCC").value + "/20" + document.getElementById("UCC").value + "Normal.pdf";
        FileName = "20" + document.getElementById("UCC").value + "Normal.pdf";
    }

    var jsonApprovedata = {
        "registrationId": parseInt(document.getElementById('RegistrationId').value),
        "fileName": FileName,
        "filePath": FilePath,
        "userId": "ACMIIL",
        "pDFType": PDFSelect
    }

    $.ajax({
        url: CommonAPIURL + "api/ApprovedPDF/InsertOrUpdateApprovedPDFDetails",
        type: "POST",
        data: JSON.stringify(jsonApprovedata),
        contentType: "application/json",
        success: function (data) {

            console.log(data);
            if (data.Message = "Success") {
                if (PDFSelect == 'BOI' && EsignType == "Yes") {
                    GenerateEsignPDF();
                }
                else if (PDFSelect == "Esign" ) {
                    GenerateEsignPDF();
                }
                else {
                    //document.getElementById('Displaypdf').style.display = "block";
                    //ViewNormalPDF();
                }
            }
            else {
                alert("Issues in records inserted into table");
            }
        },
        error: function (data) {
            alert("Error :" + data);
            return;
        }
    });
}

//$('#Displaypdf').click(function () {
//    ViewNormalPDF();
//});

function ViewNormalPDF() {

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

function GenerateEsignPDF() {

    $.ajax({
        url: CommonPageURL + "PDFGenerate/GenarateEsign",
        type: "GET",
        data: {
            RegistrationId: parseInt($("#RegistrationId").val())
        },
        success: function (data) {
            
            window.location.assign(data.data);
        },
        error: function (data) {
            alert("Error" + data);
        }
    })
}


function CheckEsign() {
    $.ajax({

        type: "GET",
        url: CommonPageURL + "Selfie/CheckEsignStatus",
        data:
        {
            RegistrationId: $("#RegistrationId").val()
        },
        success: function (data) {
            if (data == 'Present') {

                $("#saveselfie").attr("disabled", true);
                $("#saveselfie").css('cursor', 'not-allowed');

                $("#esignvarifi").removeClass('b');
                $("#esignvarifi").addClass('a');

                $("#thnks").removeClass('a');
                $("#thnks").addClass('b');

                document.getElementById('CheckEsign').style.display = "block";
            }
        },
        error: function (data) {

        }
    })
}
function CopyURL() {
    var EncRegistrationId = $("#EncRegistrationId").val();
    var page = CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId;
    var el = document.createElement('textarea');
    el.value = page;
    el.setAttribute('readonly', '');
    el.style = {
        position: 'absolute',
        left: '-9999px'
    };
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
    showDynamicMessage('URL Copied !!', 3000, 'green', 'CopyURL');

}





