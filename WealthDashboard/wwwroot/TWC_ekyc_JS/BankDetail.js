

$(document).ready(function () {
    $("#bankvarifi").removeClass('a');
    $("#bankvarifi").addClass('b');

    var urlParams = new URLSearchParams(window.location.search);
    var outputParam = urlParams.get('output');

    if (outputParam != null) {
        UpdateReversPennyDrop(outputParam);

    } else {
      CheckPennyDropStatus();
        console.log("Output parameter not found in the URL.");
    }
});


function GetQRURL() {
    var RegistrationID = $("#RegistrationId").val();
    $.ajax({
        type: "GET",
        url: CentralAPIURL + "api/Camspay/getqrurlTWC",
        data: {
            userId: RegistrationID,
            Application_Name: "EKYC"
        },
        success: function (data) {
            // Decode the Base64 string
            var decodedString = data.data;
            var urlIndex = decodedString.indexOf("https://");
            var extractedURL = decodedString.substring(urlIndex);
            // Log the decoded URL
            // Log the decoded URL
            //extractedURL = "";
            if (extractedURL != "" && extractedURL != null && extractedURL != undefined) {
                $('#redirectModal').modal('show');
                setTimeout(function () {
                    window.location.href = extractedURL;
                }, 3000);
            }
            else {
                $("#flush-collapse3").show();
                showDynamicMessage('Please Enter Account No And IFSC Code.', 4000, 'red', 'Pennysuccess');
                $("#CHequproof").show();
                return;
            }
        },
        error: function (data) {
            $("#PennyDrop").parent().removeClass("hidden-button");
            $("#flush-collapse3").show();
        }
    });
}

function GetBankDetailByIFSC() {
    if ($("#AccountNumber").val() == "" && $("#AccountNumber").val() == null && $("#AccountNumber").val() == undefined) {
        showDynamicMessage('Please Enter Valid Account No', 4000, 'red', 'Pennysuccess');
        return;
    }
    if ($("#IFSCCode").val() == "" && $("#IFSCCode").val() == null && $("#IFSCCode").val() == undefined) {
        showDynamicMessage('Please Enter Valid IFSC Code', 4000, 'red', 'Pennysuccess');
        return;
    }

    Ifsccodefinder();
    $("#Bankdetailsdiv").show();

}

function GetQRBankData() {
    var RegistrationID = $("#RegistrationId").val();
    $.ajax({
        type: "GET",
        url: CentralAPIURL + "api/Camspay/GetBankDetailForCams",
        data: {
            RegistrationID: RegistrationID,

        },
        success: function (data) {

            if (data.data.accountNo != "") {
                $("#pennyverify").val('verified');
                $('#AccountNumber').val(data.data.accountNo).prop('readonly', true);
                $('#IFSCCode').val(data.data.ifsC_Code).prop('readonly', true);
                $('#AccountHolderName').text(data.data.accountHolderName);
                $('#bankName').text(data.data.bankName);
                $('#branchName').text(data.data.branch);
                $('#bankAddress').text(data.data.address);
                $('#bankCity').text(data.data.city);
                $('#bankState').text(data.data.state);
                $("#PennyDrop").hide();
                $("#flush-collapse3").show();
                $("#Bankdetailsdiv").show();
            }
            else {
                /*$('#AccountNumber').val(data.data.accountNo).prop('readonly', false);
                $('#IFSCCode').val(data.data.ifsC_Code).prop('readonly', false);
                $("#PennyDrop").show();
                $("#flush-collapse3").show();
                $("#Bankdetailsdiv").hide();*/
                $("#flush-collapse3").show();
                showDynamicMessage('Please Enter Account No And IFSC Code.', 4000, 'red', 'Pennysuccess');
                $("#CHequproof").show();
                return;
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
src =" ~/wwwroot/Loader-logo-transparent.gif"

function CheckPennyDropStatus() {
    var RegistrationID = $("#RegistrationId").val();    
    $.ajax({
        type: "GET",
        url: CentralAPIURL + "api/Camspay/CheckPennyDropStatus",
        data: {
            RegistrationID: RegistrationID,
        },
        success: function (data) {
            if (data.data != null) {

                GetQRBankData();

            } else {

                GetQRURL();

            }
        },
        error: function (data) {
            $("#PennyDrop").parent().removeClass("hidden-button");
            $("#flush-collapse3").show();

            console.log(data);
        }
    });
}

function UpdateReversPennyDrop(outputValue) {


    $.ajax({
        type: "GET",
        url: CentralAPIURL + "api/Camspay/GetQRBankData",
        data: {
            data: outputValue
        },
        success: function (data) {

            CheckPennyDropStatusAfterGet();
        },
        error: function (data) {
            $("#PennyDrop").parent().removeClass("hidden-button");
            $("#flush-collapse3").show();
            console.log(data);
        }
    })
}


function CheckPennyDropStatusAfterGet() {
    var RegistrationID = $("#RegistrationId").val();
    $.ajax({
        type: "GET",
        url: CentralAPIURL + "api/Camspay/CheckPennyDropStatus",
        data: {
            RegistrationID: RegistrationID,
        },
        success: function (data) {
            if (data.data != null) {

                GetQRBankData();

            } else {

                GetQRURL();

            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}


/*
$("#PennyDrop").on('click', function () {
    Ifsccodefinder();
})*/





function Ifsccodefinder() {

    var beneficiaryIFSC = $("#IFSCCode").val();
    try {
        $.ajax({
            url: CommonAPIURL + "api/ClientBankDetails/GetClientIfscCodeDetails?Ifsccode=" + beneficiaryIFSC + "",
            type: 'GET',
            success: function (data) {
                console.log(data);
                if (data.message == "Success") {
                    $("#pennyverify").val('verified');
                    $("#ifscMasterId").val(data.data.ifscMasterId);
                    $("#bankName").val(data.data.bankName);
                    $("#bankCode").val(data.data.bankCode);
                    $("#ifscCode").val(data.data.ifscCode);
                    $("#micrCode").val(data.data.micrCode);

                    $('#AccountNumber').prop('readonly', true);
                    $('#IFSCCode').prop('readonly', true);
                    $('#AccountHolderName').text($("#ClientName").val());
                    $("#BenificiaryName").val($("#ClientName").val());
                    $('#bankName').text(data.data.bankName);
                    $('#branchName').text(data.data.branch);
                    $('#bankAddress').text(data.data.address);
                    $('#bankCity').text(data.data.city);
                    $('#bankState').text(data.data.state);

                    saveClientBankDetails(true);
                    //pennydropfunction();
                }
                else if (data.message == "Error -") {
                    showDynamicMessage('Please Enter Valid IFSC Code.', 4000, 'red', 'Pennysuccess');
                    return;
                }
                else {

                }
            },
            error: function (data) {
                
            }
        });
    }
    catch {
        
    }
}
/*
function pennydropfunction() {
    var beneficiaryAccountNo = $("#AccountNumber").val();
    var beneficiaryIFSC = $("#IFSCCode").val();
    var beneficiaryName = $("#ClientName").val();


    $.ajax({
        type: "POST",
        url: "https://digio.investmentz.com/api/DigioKYC/GetPennyDrop",
        contentType: "application/json",  // Set content type to JSON
        data: JSON.stringify({
            beneficiary_account_no: beneficiaryAccountNo,
            beneficiary_ifsc: beneficiaryIFSC,
            beneficiary_name: beneficiaryName,
            sourceType: "EKYC_NEW"
        }),
        success: function (data) {

            if (data.data.fuzzy_match_score >= 70) {
                $("#pennyverify").val('verified');
                $("#Pennysuccess").show();
                $("#fuzzy_match_score").val(data.data.fuzzy_match_score);
                $("#fuzzy_match_result").val(data.data.fuzzy_match_result);
                $("#BenificiaryName").val(data.data.beneficiary_name_with_bank);

                $("#refId").val(data.data.refId);
                showDynamicMessage('Bank verification completed successfully.', 4000, 'green', 'Pennysuccess');
                saveClientBankDetails(true);

            } else {
                showDynamicMessage('Bank verification failed. Please upload cancelled cheque.', 4000, 'red', 'Pennysuccess');
                $("#fuzzy_match_score").val('0');
                $("#fuzzy_match_result").val('0');
                $("#BenificiaryName").val($("#ClientName").val()); // if penny fail then set previous client name

                $("#refId").val('');
                $("#CHequproof").show();
                $("#Upload-Documents").show()
                saveClientBankDetails(false);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}*/



function saveClientBankDetails(verified) {

    var requestData = {
        "registrationId": parseInt($("#RegistrationId").val()),
        "accountNo": $("#AccountNumber").val(),
        "bankName": $("#bankName").val(),
        "bankCode": $("#bankCode").val(),
        "micrCode": $("#micrCode").val(),
        "accountTypeId": 35,
        "ifscMasterId": parseInt($("#ifscMasterId").val()),
        "ifscCode": $("#IFSCCode").val(),
        "defaultBank": 1,
        "upiId": "",
        "isActive": 1,
        "userId": "EKYC_NEW",
        "refId": $("#refId").val(), 
        "verified": verified,
        "verified_at": "",
        "beneficiary_name_with_bank": $("#BenificiaryName").val(), 
        "fuzzy_match_result": $("#fuzzy_match_result").val(),
        "fuzzy_match_score": parseInt($("#fuzzy_match_score").val()) 
    };

    // Make an AJAX POST request
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Home/InsertUpdatedBankDetails",
        data: requestData,
        contentType: "application/json",
        success: function (response) {

            console.log("Success:", response);

            // Check if the response indicates success
            if (response.Message === "Success") {

                console.log("Bank details saved successfully.");
            } else {
                // Handle failure
                console.log("Failed to save bank details. Response:" );
            }
        },
        error: function (error) {
            // Handle the error response
            console.error("Error:", error);
        }
    });
}




function CheckDigioStatus() {
    var RegistrationID = $("#RegistrationId").val();
    $.ajax({
        type: 'GET',
        url: CommonPageURL + "Digio/CheckDigioStatus",
        data: {
            RegistrationID: RegistrationID
        },
        success: function (data)
        {
            if (data.pan == 'True')
            {
                //$("#hiddpanstatus").val('True');
                //document.getElementById('PanCarddiv').style.display = "none";
            }
        },
        error: function (data)
        {
            console.log(data);
        }


    })
}

// Example usage:
