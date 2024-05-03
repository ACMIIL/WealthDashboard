var IsNSE_BSEcheck = false;
$(document).ready(function () {
    $("#segmentvarifi").removeClass('a');
    $("#segmentvarifi").addClass('b');
    Brockarageplan();
    if ($("#BACode").val() == "RC319" || $("#BACode").val() == "RC115") {
        $("#divboicust").show();
    }
});
$("#DPID").on("change", function () {
    // Call the getDepositoryMaster function when the value changes
    getDepositoryMaster();

});
function getDepositoryMaster() {
    var dpIdValue = $("#DPID").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Segment/GetDepositoryMaster",
        data: {
            option: "1",
            DPID: dpIdValue
        },
        contentType: "application/json",
        success: function (data) {
            console.log("Data received:", data);
            // Parse the JSON string into a JavaScript object
            //var parsedData = JSON.parse(data);
            if (data !== null && data.dpid[0].dpid && data.dpid[0].dpid.length > 0) {
                $("#DepositoryName").val(data.dpid[0].depository);
                $("#DPName").val(data.dpid[0].dpName);
            } else {
                Swal.fire("Not Found");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
}

$("#btnradioExistingdemat").on("change", function () {
    var checkbox = document.getElementById('btnradioExistingdemat');
    /*if (checkbox.checked) {*/
    //document.getElementById('CKCurrency').checked = false;
    //document.getElementById('CKDerivatvs').checked = false;
    //document.getElementById('ckslbm').checked = false;
    //$("#boicdetails").show();
    //$(".categorycheck").attr("disabled", false);
    //$("#ckslbm").attr("disabled", true);

    //$("#Cash_MFund").attr("checked", true);

    if (checkbox.checked) {
        document.getElementById('CKCurrency').checked = false;
        document.getElementById('CKDerivatvs').checked = false;
        document.getElementById('ckslbm').checked = false;
        $("#boicdetails").show();
        $("#CMLfilediv").show();

        $(".categorycheck").attr("disabled", false);
        $(".forboidisbl").attr("disabled", true);
        $("#Hideicomdiv").hide();
        document.getElementById('cmlfilespan').style.display = "none";
        document.getElementById('incomefilespan').style.display = "none";
        document.getElementById('Nomineewantdiv').style.display = "none";
        document.getElementById('ispoliticallydiv').style.display = "none";

        $("#DPID").attr('readonly', false).val('');
        $("#DepositoryName").attr('readonly', false).val('');
        $("#DPName").attr('readonly', false).val('');

        $("#CMLfile").val('');
        $('#trimank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/BOISL%20Trim%20Brokerage%20Plan%20-%20ACM.pdf");
        $('#Premiumank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/BOISL%20PREMIUM%20Brokerage%20Plan%20-%20ACM.pdf");

    }
    else {

    }
    //}
    //else {

    //}

})
$("#btnradioBankofindia").on("change", function () {
    var checkbox = document.getElementById('btnradioBankofindia');
    if (checkbox.checked) {
        document.getElementById('CKCurrency').checked = false;
        document.getElementById('CKDerivatvs').checked = false;
        document.getElementById('ckslbm').checked = false;
        document.getElementById('CkecMTF').checked = false;
        $("#boicdetails").show();
        $("#CMLfilediv").show();
        $(".categorycheck").attr("disabled", false);
        $(".forboidisbl").attr("disabled", true);
        document.getElementById('Nomineewantdiv').style.display = "none";
        document.getElementById('ispoliticallydiv').style.display = "none";
        $("#DPID").attr('readonly', true).val('11901');
        $("#DepositoryName").attr('readonly', true).val('CDSL');
        $("#DPName").attr('readonly', true).val('BOI SHAREHOLDING LIMITED');
        $("#Hideicomdiv").hide();

        $("#upload").val('');

        $('#trimank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/BOISL%20Trim%20Brokerage%20Plan%20-%20ACM.pdf");
        $('#Premiumank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/BOISL%20PREMIUM%20Brokerage%20Plan%20-%20ACM.pdf");

        //document.getElementById('WithoutBOIDiv').style.display = "none";
        //document.getElementById('WithBOIDiv').style.display = "block";

    }
    else {

    }

})


$("#btnradioNewDematAccount").on("change", function () {
    var checkbox = document.getElementById('btnradioNewDematAccount');
    if (checkbox.checked) {
        document.getElementById('Cash_MFund').checked = true;
        $("#boicdetails").hide();
        $(".categorycheck").attr("disabled", false);
        $("#CMLfilediv").hide();
        document.getElementById('cmlfilespan').style.display = "none";
        document.getElementById('incomefilespan').style.display = "none";

        document.getElementById('Nomineewantdiv').style.display = "block";
        document.getElementById('ispoliticallydiv').style.display = "block";
        $("#CMLfile").val('');

        $('#trimank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/Trim%20Brokerage%20Plan-ACM.pdf");
        $('#Premiumank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/Premium%20Brokerage%20Plan-ACM.pdf");
    }
    else {

    }

})
$("#OnlyMF").on("change", function () {
    var checkbox = document.getElementById('OnlyMF');
    if (checkbox.checked) {
        document.getElementById('CKCurrency').checked = false;
        document.getElementById('CKDerivatvs').checked = false;
        document.getElementById('ckslbm').checked = false;
        document.getElementById('Cash_MFund').checked = false;
        $(".categorycheck").attr("disabled", true);
        $(".categorycheck").attr("checked", false);
        $("#boicdetails").hide();
        $("#CMLfilediv").hide();
        $("#CMLfile").val('');
        $("#upload").val('');
        document.getElementById('cmlfilespan').style.display = "none";
        document.getElementById('incomefilespan').style.display = "none";

        document.getElementById('Nomineewantdiv').style.display = "block";
        document.getElementById('ispoliticallydiv').style.display = "block";

        $('#trimank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/Trim%20Brokerage%20Plan-ACM.pdf");
        $('#Premiumank').attr('href', "https://web.investmentz.com/imgupload/Brok_Plan/Premium%20Brokerage%20Plan-ACM.pdf");

        //document.getElementById("Categorisdiv").disabled = true;
        // div.attr('disabled', 'disabled');
    }
    else {
        $("#boicdetails").show();
    }

})

function InsertSegmentDetails() {
    var isvalid = Segmentvalidation();
    alert(isvalid);
    if (isvalid != false && isvalid !=undefined) {
        var segmentmasterid = 0;
        var SegEquityIsActive = "";
        var CashMFIPOIsActive = "";
        var currencyIsActive = "";
        var derivativeIsActive = "";
        var slbmIsActive = "";
        var Cash = "";
        var MF = "";
        var IPO = "";
        var currency = "";
        var derivative = "";
        var slbm = "";
        var mtf = "";
        var mtfIsActive = "";
        var RegID = $("#RegistrationId").val();
        if (document.getElementById("Cash_MFund").checked == true) {

            Cash = 1;
            MF = 2;
            IPO = 4;
            CashMFIPOIsActive = true;
        }
        else {
            Cash = 1;
            MF = 2;
            IPO = 4;
            CashMFIPOIsActive = false;
        }
        if (document.getElementById("CKCurrency").checked == true) {
            currency = 3;
            currencyIsActive = true;
        }
        else {
            currency = 3;
            currencyIsActive = false;
        }

        if (document.getElementById("CKDerivatvs").checked == true) {
            derivative = 5;
            derivativeIsActive = true;

        }
        else {
            derivative = 5;
            derivativeIsActive = false;
        }
        if (document.getElementById("ckslbm").checked == true) {
            slbm = 7;
            slbmIsActive = true;
        }
        else {
            slbm = 7;
            slbmIsActive = false;
        }
        if (document.getElementById("CkecMTF").checked == true) {
            mtf = 8;
            mtfIsActive = true;
        }
        else {
            mtf = 8;
            mtfIsActive = false;
        }
        var jsonSegmenData = [{

            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(Cash),
            "isActive": CashMFIPOIsActive,
            "userId": "EKYCtwc"
        },
        {
            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(MF),
            "isActive": CashMFIPOIsActive,
            "userId": "EKYCtwc"
        },
        {
            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(IPO),
            "isActive": CashMFIPOIsActive,
            "userId": "EKYCtwc"
        },
        {
            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(currency),
            "isActive": currencyIsActive,
            "userId": "EKYCtwc"
        },
        {
            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(derivative),
            "isActive": derivativeIsActive,
            "userId": "EKYCtwc"
        },
        {
            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(slbm),
            "isActive": slbmIsActive,
            "userId": "EKYCtwc"
        },
        {
            "clientSegmentId": 0,
            "registrationId": RegID,
            "segmentMasterId": parseInt(mtf),
            "isActive": mtfIsActive,
            "userId": "EKYCtwc"
        }];
        var OnlyMf = 0;
        var BACode = "";

        var accountpreferance = $('input[name="btnDemataccountprff"]:checked').val();
        var Politicallyexposed = $('input[name="Politicallyexposed"]:checked').val();
        var settlementfrequency = $('input[name="rdosettlement"]:checked').val();
        //var indianresident = $('input[name="indianresident"]:checked').val();
        var dpid = $("#DPID").val();
        var EnterClientID = $("#EnterClientID").val();
        var DepositoryName = $("#DepositoryName").val();
        var DPName = $("#DPName").val();
        if ($("#BACode").val() == "RC319" || $("#BACode").val() == "RC115") {
            var terrifplan = $('input[name="btnTariffPlan"]:checked').val();
            var brokrage = parseInt(1);
        }
        else {
            var terrifplan = $('select[name=btnTariffPlan]').val();
            var brokrage = parseInt($("#drpbrockPlan").val());
        }
        if (accountpreferance == "Bankofindia") {
            BACode = "RC319";
        }
        else {
            BACode = $("#BACode").val();
        }

        if (accountpreferance == "OnlyMF") {
            OnlyMf = 1;
        }
        else {
            OnlyMf = 0;
        }


        var IncomeProof = document.getElementById('upload').files[0];
        var CMLfile = document.getElementById('CMLfile').files[0];
        var insertSegmentModel = new FormData();
        insertSegmentModel.append('Segmentdta', JSON.stringify(jsonSegmenData));
        insertSegmentModel.append('OnlyMf', OnlyMf);
        insertSegmentModel.append('DpId', dpid);
        insertSegmentModel.append('BOID', EnterClientID);
        insertSegmentModel.append('DepositoryName', DepositoryName);
        insertSegmentModel.append('UserId', BACode);
        insertSegmentModel.append('RID', RegID);
        insertSegmentModel.append('IncomeProof', IncomeProof);
        insertSegmentModel.append('tarrifplan', parseInt(terrifplan));
        insertSegmentModel.append('BrokragePlan', brokrage);
        insertSegmentModel.append('IncomeProof', IncomeProof);
        insertSegmentModel.append('CMLfile', CMLfile);
        insertSegmentModel.append('Residency', parseInt(0));
        insertSegmentModel.append('PoliticallyExposePerson', parseInt(Politicallyexposed));
        insertSegmentModel.append('settlementfrequency', parseInt(settlementfrequency));
        var encregi = $("#EncRegistrationId").val();
        $.ajax({
            url: CommonPageURL + "Segment/InsertOrUpdateSegment",
            type: "POST",
            cache: false,
            contentType: false,
            processData: false,
            data: insertSegmentModel,
            success: function (data) {
                if (data == 'Ok') {                   
                    if (document.getElementById("btniaddlater").checked == true) {
                        SavePageDetails(RegID, CommonPageURL + "Selfie/SelfieView" + '?encregistrationId=' + encregi + "");
                        window.location.assign(CommonPageURL + "Selfie/SelfieView" + '?encregistrationId=' + encregi + "");
                    }
                    else {
                        SavePageDetails(RegID, CommonPageURL + "Nominee/Nominee_Details" + '?encregistrationId=' + encregi + "");
                        window.location.assign(CommonPageURL + "Nominee/Nominee_Details" + '?encregistrationId=' + encregi + "");
                    }
                }
                else {
                    console.log("Segment/InsertOrUpdateSegment insert time error");
                }
            },
            error: function (data) {

            }
        });
    }


}

$("#CKCurrency,#CKDerivatvs").on('change', function () {
    if (document.getElementById("CKCurrency").checked == true) {
        $("#Hideicomdiv").show();
    }
    else if (document.getElementById("CKDerivatvs").checked == true) {
        $("#Hideicomdiv").show();
    }
    else {
        $("#Hideicomdiv").hide();
        $("#upload").val('');
        $("#cmlfilespan").val('');
        document.getElementById('cmlfilespan').style.display = "none";
        document.getElementById('incomefilespan').style.display = "none";
    }
})


function FileUploadmsg() {


    var IncomeProof = document.getElementById('upload').files[0];
    var CMLfile = document.getElementById('CMLfile').files[0];

    if (IncomeProof != undefined) {
        var size = IncomeProof.size / 1024;
        var type = IncomeProof.type;
        if (type == 'application/pdf') {
            if (size > 5120) {

                alert('Uploaded document size should not exceed 5mb.');
                $("#upload").val('');
                $("#incomefilespan").hide();

                return;
            }
            else {
                $("#incomefilespan").show();
                return;
            }
        }
        else {

            alert('Uploaded document pdf format.');
            $("#upload").val('');
            $("#incomefilespan").hide();
            return false;
        }
    }
    else {
        $("#incomefilespan").hide();
    }
    if (CMLfile != undefined) {
        var size = CMLfile.size / 1024;
        var type = CMLfile.type;
        if (type != 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' && type != 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' && type != 'text/csv') {
            if (size > 5120) {
                alert('Uploaded document size should not exceed 5mb.');
                $("#CMLfile").val('');
                $("#cmlfilespan").hide();
                return;
            }
            else {
                $("#cmlfilespan").show();
                return;
            }
        }
        else {
            $("#CMLfile").val('');
            $("#cmlfilespan").hide();
            alert('Please Upload Correct format document.');
        }

    }
    else {

        $("#cvlfilespan").hide();
    }
}

function Segmentvalidation() {

    //boi and existing demat if
    var dpid = $("#DPID").val();
    var ClientID = $("#EnterClientID").val();
    var DepositoryName = $("#DepositoryName").val();
    var DPName = $("#DPName").val();

    var IncomeProof = document.getElementById('upload').files[0];
    var CMLfile = document.getElementById('CMLfile').files[0];

    var existingcheckbox = document.getElementById('btnradioExistingdemat');
    var OnlyMF = document.getElementById('OnlyMF');
    //var boicheckbox = document.getElementById('btnradioBankofindia');
    var CKCurrency = document.getElementById('CKCurrency');
    var CKDerivatvs = document.getElementById('CKDerivatvs');
    
    if (IsNSE_BSEcheck == false) {
        if (document.getElementById('btnAllExchangebse').checked) {
            IsNSE_BSEcheck = true;
        }
    }
    
    if (CKCurrency.checked) {
        if (IncomeProof == undefined) {
            showMessage('Please Upload Income Proof.', 4000, 'red');
            // alert("Upload Income Proof")
            return false;
        }
    }

    if (CKDerivatvs.checked) {
        if (IncomeProof == undefined) {
            showMessage('Please Upload Income Proof.', 4000, 'red');
            //alert("Upload Income Proof")
            return false;
        }
    }

    if (document.getElementById('Cash_MFund').checked != true) {
        showMessage('Please select Categories.', 4000, 'red');
        return false;
    }
    if (IsNSE_BSEcheck != true) {
        showMessage('Please select Segment.', 4000, 'red');
        return false;
    }
    if (existingcheckbox.checked) {
        if (dpid == "") {
            showMessage('Enter DP ID.', 4000, 'red');
            return false;
        }
        if (ClientID == "") {
            showMessage('Please Enter ClientID.', 4000, 'red');
            return false;
        }
        if (DepositoryName == "") {
            showMessage('Please Enter Depository Name.', 4000, 'red');
            return false;
        }
        if (DPName == "") {
            showMessage('Please Enter DP Name.', 4000, 'red');
            return false;
        }
        if (CMLfile == undefined) {
            showMessage('Please Upload CML Proof.', 4000, 'red');
            return false;
        }
        if (document.getElementById('Cash_MFund').checked != true) {
            showMessage('Please Select Categories.', 4000, 'red');
            return false;
        }
    }   
    var isTermValid = TandCValidation();

    if (isTermValid == false) {
        showMessage('Please Select Term and Conditions.', 4000, 'red');
        return false;
    }



}

function TandCValidation() {
    var TAndCDeclaration = document.getElementById('TAndCDeclaration');
    var TCStandinginsr = document.getElementById('TCStandinginsr');
    var TCAcceptDP = document.getElementById('TCAcceptDP');
    var TandC = document.getElementById('TandC');
    var TCMITC = document.getElementById('TCMITC');

    if (TAndCDeclaration.checked != true) {
        return false
    }
    if (TCStandinginsr.checked != true) {
        return false
    }
    if (TCAcceptDP.checked != true) {
        return false
    }
    if (TandC.checked != true) {
        return false
    }
    if (TCMITC.checked != true) {
        return false
    }
}

$("#btnAllExchangebse,#btnAllExchangeNSE").change(function () {
    var BSE = document.getElementById('btnAllExchangebse').checked
    var NSE = document.getElementById('btnAllExchangeNSE').checked
    if (BSE == true) {
        IsNSE_BSEcheck = true;
    }
    else if (NSE == true) {
        IsNSE_BSEcheck = true;
    }
    else {
        IsNSE_BSEcheck = false;
    }
})

function IsPDFpasswordProtected() {
    var insertSegmentModel = new FormData();
    insertSegmentModel.append('IncomeProof', document.getElementById('upload').files[0]);

    $.ajax({
        url: CommonPageURL + "Segment/CheckPDFPassWordProtected",
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        data: insertSegmentModel,
        success: function (data) {

            if (data == false) {
                FileUploadmsg();
            }
            else {
                $("#upload").val('');
                $("#incomefilespan").hide();
                //showMessage('This file cannot be accepted as it is password protected.', 4000, 'red');
                alert("This file cannot be accepted as it is password protected.");
                return;
            }


        },
        error: function (data) {
            return data;
        }
    });
}

function Brockarageplan() {
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Segment/Brockarageplan",
        data: {

        },
        success: function (data) {
            $.each(data, function (index, value) {

                /*var html = `<option PlanDescription value=>` + PlanDescription + `</option>`;*/
                $("#drpbrockPlan").append(`<option value=` + value.tariffPlanId + `>` + value.planDescription + `</option>`);
            });
        },
        error: function (data) {

        }
    });
}