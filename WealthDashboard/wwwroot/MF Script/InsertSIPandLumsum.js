var FolioNo = localStorage.getItem("FolioNo");
$(document).ready(function () {

    $("#BACode").val('RC4048');
    $("#UCC").val(UCC);
    Checkmandate();
    GetInvestNowDetails(sessionStorage.ISIN);

    // Initially hide the additional content
    $("#additionalContent").hide();

    // Handle radio button change event
    $('input[name="inlineRadioOptions"]').change(function () {
        if ($(this).val() === 'option2') {
            $("#additionalContent").show();
            $("#additionalContent2").hide();
        } else {
            $("#additionalContent2").show();
            $("#additionalContent").hide();
            $("#additionalContent").find(".specific-element-to-hide").hide();
        }
    });
});

function GetInvestNowDetails(ISIN) {

    //var data = JSON.parse(decodeURIComponent(sessionStorage.selectedFund));
    //$("#fundNameDisplayInput").val(data.schemeName);
    //$("#MinAmount").html(data.sipMinInstallmentAmt);
    //$("#Multiples").html(data.sipMultiplierAmt);
    //$("#MinInstallments").html(data.sipMinInstallmentNo);
    //$("#LumpsumMinAmount").html(data.minPurchaseAmt);
    //$("#LumpsumMultiples").html(data.purchaseAmtMultiplier);
    //GetSIPDates(data.sipDates)
    var content = '';
    $.ajax(
        {
            type: "GET",
            url: GlobalUrl + "MFUsers/GetInvestNowDetailByID",
            data: {
                ISIN: ISIN
            },
            success: function (data) {
                //content = content + '<img src="../assets/AMCLogo/' + data.data.amcCode + '.jpg" style="width: 70px; height: 70px;"> ' +
                //    '<h2 class="mt-4" style="font-size: 24px;" class="mt-4"><span id="SchemeName">' + data.data.schemeName + '</span></h2>' +
                //    '<hr>' +
                //    '<p class="small"><img src="../assets/images/icons/warning-swal.fire.png" style="width:14px;filter: invert(1);margin-top: -3px;"> Investment transactions completed after 02:00 PM are submitted to Mutual Fund Company for further processing on the next business day.</p>';


                // GetInvestNowDeatils.push({ Details: data.data })
                $("#fundNameDisplayInput").val(data.data.schemeName);
                $("#MinAmount").html(data.data.sipMinInstallmentAmt);
                $("#Multiples").html(data.data.sipMultiplierAmt);
                $("#MinInstallments").html(data.data.sipMinInstallmentNo);
                $("#LumpsumMinAmount").html(parseFloat(data.data.minPurchaseAmt).toFixed(2));
                $("#LumpsumMultiples").html(parseFloat(data.data.purchaseAmtMultiplier).toFixed(2));
                GetSIPDates(data.data.sipDates)

                $("#DisplatSchemeDetails").html(content);
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function GetSIPDates(SIPDates) {
    const date1 = new Date();
    var GetServerDatTime = GetServerDateTime(date1);
    var FirstOrderFlag = true;
    var Frequency = $("#Frequency").val().toUpperCase();
    var start = "";
    var nextDay = "";
    var htmldates = '<option value="">Please Select SIP Date</option>';;
    var separator = GetServerDatTime.substring(3, 2);
    var sdate = GetServerDatTime.split(separator)[0].toString();
    var smonth = GetServerDatTime.split(separator)[1].toString();
    var syear = GetServerDatTime.split(separator)[2].toString().split(' ')[0];
    var i;

    //if ($('#PaymentGateway').prop("checked") == true) {
        //FirstOrderFlag = true;
    //}
   // else {
        FirstOrderFlag = false;
    //}
    if (GetServerDateTime != "") {
        $("#SIPDate").empty();
        if (FirstOrderFlag == true) {
            if (Frequency == "MONTHLY") {
                start = new Date(syear, (parseInt(smonth)).toString(), (parseInt(sdate)).toString() - 3) //yyyy, mm, dd
            }
        }
        else {
            start = new Date(syear, (parseInt(smonth) - 1).toString(), (parseInt(sdate) + 2).toString())
        }
    }
    nextDay = new Date(start);
    for (i = 1; i <= 31; i++) {

        var date = new Date(start),
            days = i;

        if (!isNaN(date.getTime())) {
            date.setDate(date.getDate() + days)
            if (SIPDates.split(',').indexOf(date.getDate().toString()) >= 0) {
                nextDay = GetServerDateTime(date);
                if (SIPDates.toString().indexOf(i.toString())) { //it checks if this date is allowed by bse or not
                    htmldates += '<option value="' + nextDay + '">' + nextDay + '</option>';
                }
            }
        } else {
            swal.fire("Invalid Date");
        }
    }
    $("#SIPDate").append(htmldates);

    return false;
}

function GetServerDateTime(date) {

    if (!(date instanceof Date)) {
        return "Invalid Date";
    }

    // Get day, month, and year
    let day = date.getDate();
    let month = date.getMonth() + 1; // Adding 1 because months are zero-based
    let year = date.getFullYear();

    // Pad day and month with leading zeros if necessary
    day = day < 10 ? "0" + day : day;
    month = month < 10 ? "0" + month : month;
    var formdate = day + "/" + month + "/" + year;
     
    //localStorage.setItem("ServerDateTime", formdate);
    // Return formatted date
    return formdate;
}
$("#SendSIPdataCart").click(function () {

    if ($("#SIPMinAmt").val() == "" || $("#SIPMinAmt").val() == '0') {
        //swal.fire('Monthly Sip Amount cannot be blank or zero');
        swal.fire('Monthly Sip Amount cannot be blank or zero');
        return false;
    }

    if ($("#SIPDate").val() == "") {
        //swal.fire('Please Select Start Date');
        swal.fire('Please Select Start Date');
        return false;
    }

    if ($("#SIPMinInstallmentNo").val() == "" || $("#SIPMinInstallmentNo").val() == '0') {
        swal.fire('No. of Installments cannot be blank or zero');
        return false;
    }
    if (parseInt($("#MinInstallments").html()) > parseInt($("#SIPMinInstallmentNo").val())) {
        swal.fire('SIP Installments should be greater than minimum Installments');
        //$("#SIPMinAmt").val('');
        return false;
    }
    if ($('#TermsChk').prop("checked") == false) {
        swal.fire('Please read and agree Terms & Conditions, Diclamer & Privacy Policy');
        return false;
    }

    //if (parseInt($("#MinAmount").val()) > parseInt($("#SIPMinAmt").val())) {
    //    swal.fire('Sip amount should be greater than or equal to minimum amount');
    //    return false;
    //}
    var SIPAmount = parseInt($("#SIPMinAmt").val());
    var minimumamount = parseInt($("#MinAmount").html());
    if (SIPAmount < minimumamount) {
        swal.fire('SIP amount should be greater than minimum amount');
        //$("#SIPMinAmt").val('');
        return false;
    }
    var FirstOrderFlag = "";
    var PaymentMode = "";
    if ($('#PaymentGatewayLumpsum').prop("checked") == true) {
        FirstOrderFlag = "Yes";
        PaymentMode = "PaymentGateWay";
    }
    else {
        FirstOrderFlag = "No";
        PaymentMode = "Mandate";
    }

    if (FolioNo == null) {
        FolioNo = "";
    }
    var SIPAmount = $("#SIPMinAmt").val();
    var SIPInstallmentNo = $("#SIPMinInstallmentNo").val();
    var Frequency = $("#Frequency").val();
    var SIPDate = $("#SIPDate").val();
    //swal.fire($('#UserSrNo').val());
    //if ($('#UserSrNo').val() == "" || $('#UserSrNo').val() == null) {
    //    swal.fire("Data not received from selected Scheme");
    //    return;
    //}
    var data = JSON.parse(decodeURIComponent(sessionStorage.selectedFund));
    var redata = {
        "userSrNo": "",
        "ucc": $('#UCC').val(),
        "schemeCode": data.schemeCode,
        "schemeName": data.schemeName,
        "transactionType": "SIP",
        "transactionMode": "Physical",
        "amount": SIPAmount,
        "folioNo": FolioNo,
        "sipInstallment": SIPInstallmentNo,
        "sipStartDate": SIPDate,
        "firstOrderToday": FirstOrderFlag,
        "frequency": Frequency,
        "paymentMode": PaymentMode,
        "orderBy": $("#BACode").val(),
        "isin": data.isin
    };
    investnowcart(redata);
});

$("#SendlumpsumdataCart").click(function () {
    if ($("#LumpsumAmt").val() == "" || $("#LumpsumAmt").val() == '0') {
        swal.fire('Monthly Lumpsum Amount cannot be blank or zero');
        return false;
    }
    var PaymentMode = "";
    if ($('#PaymentGatewayLumpsum').prop("checked") == true) {
        PaymentMode = "PaymentGateWay";
    }
    else {
        PaymentMode = "Mandate";
    }

    if (FolioNo == null) {
        FolioNo = "";
    }
    else {
        FolioNo = FolioNo
    }

    var data = JSON.parse(decodeURIComponent(sessionStorage.selectedFund));

    var LumpsumAmount = $("#LumpsumAmt").val();
    var redata = {
        "userSrNo": "",
        "ucc": document.getElementById('UCC').value,
        "schemeCode": data.schemeCode,
        "schemeName": data.schemeName,
        "transactionType": "Lumpsum",
        "transactionMode": "Physical",
        "amount": LumpsumAmount,
        "folioNo": FolioNo,
        "sipInstallment": "",
        "sipStartDate": "",
        "firstOrderToday": "",
        "frequency": "",
        "paymentMode": PaymentMode,
        "orderBy": $("#BACode").val(),
        "isin": data.isin
    };
    

    investnowcart(redata);
})

function investnowcart(redata) {
    $.ajax
        ({
            type: "POST",
            url: CommonWebsiteURL + "InvestNow/InvestNowInsert",
            data: JSON.stringify(redata),
            contentType: "application/json",
            success: function (data) {
                console.log(data)
                if (data.code == 200) {
                    window.location.href = CommonWebsiteURL + 'MutualFund/CartDetail';
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}

function Checkmandate() {
    var ucc = $("#UCC").val();
    //var ucc = UCC;
    $.ajax({
        type: "GET",
        url: GlobalUrl + "Emandate/EmandateCheck?ClientCode=" + ucc + "",
        data: {
        },
        success: function (data) {
            if (data.data.length == 0) {
                var html = `<div class="form-check form-check-inline">
                                        <input onchange="checkmandatefn()" class="form-check-input" type="radio" name="RadioOptions" id="PaymentGatewayLumpsum" value="option1" checked>
                                        <label class="form-check-label font-14-500" name="PaymentGatewayLumpsum" for="PaymentGatewayLumpsum"> Payment Gateway</label>
                                    </div>`;
                $("#paymentmodediv").html(html);
            }
            if (data.data[0].mandateStatus = "ACTIVE") {
                $("#hiddenmandateamt").val(parseInt(data.data[0].rupeesD));
                var html = `<div class="form-check form-check-inline">
                                        <input onchange="checkmandatefn()" class="form-check-input" type="radio" name="RadioOptions" id="PaymentGatewayLumpsum" value="option1" checked>
                                        <label class="form-check-label font-14-500" name="PaymentGatewayLumpsum" for="PaymentGatewayLumpsum"> Payment Gateway</label>
                                    </div>
                                    <div class="form-check form-check-inline ms-5">
                                        <input onchange="checkmandatefn()" class="form-check-input" type="radio" name="RadioOptions" id="Mandate" value="option2">
                                        <label class="form-check-label font-14-500" name="MandateLumpsum" for="Mandate">Mandate</label>
                                    </div>`;
                $("#paymentmodediv").html(html);
            }
        },
        error: function (data) {

        }
    });
}

$("#SIPMinAmt").change(function () {
    var mandateamt = parseInt($("#hiddenmandateamt").val());
    var amt = parseInt($("#SIPMinAmt").val());
    var ismandatecheck = $('#Mandate').prop("checked");
    if (amt > mandateamt && ismandatecheck == true) {
        document.getElementById("SIPMinAmt").value = "";
        swal.fire("The mandate amount should be greater than the order value; please register the e-mandate with a greater limit.");
        return false;
    }
})
function checkmandatefn() {
    var mandateamt = parseInt($("#hiddenmandateamt").val());
    var amt = parseInt($("#SIPMinAmt").val());
    var ismandatecheck = $('#Mandate').prop("checked");
    if (amt > mandateamt && ismandatecheck == true) {
        document.getElementById("SIPMinAmt").value = "";
        swal.fire("The mandate amount should be greater than the order value; please register the e-mandate with a greater limit.");
        return false;
    }
}

