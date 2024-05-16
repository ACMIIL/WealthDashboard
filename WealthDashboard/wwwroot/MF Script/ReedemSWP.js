$(document).ready(function () {
    var FolioNumber = localStorage.getItem("FolioNo");
    var isin = localStorage.getItem("ISIN1");
    schemedata = JSON.parse(decodeURIComponent(localStorage.getItem("schemedata")));
    $("#UCC").val(UCC);

    //GetSchemeWiseFolioDetails(PanNo, FolioNumber, SchemeId)
    GetMinAmountAndQty(isin);
    $("div.myDiv").hide();
    GetSIPDates();

    
});
var SchemeWiseFolioDetail = [];
var GetMinAmout = [];
var RDMAllOTPDetails = [];
var schemedata = [];

$('#RedeemVolume').on('change', function () {
    var demovalue = $(this).val();
    $("div.myDiv").hide();
    $("#show" + demovalue).show();
    if (demovalue == 'One') {
        $("#RedeemAllUnit").val(SchemeWiseFolioDetail[0].bal_unit)
        $('#RedeemAllUnit').attr("disabled", "disabled");
    }
});

$('input[name="reddenswpnm"]').change(function () {
    if ($(this).val() === 'option2') {
        $("#ReedemDiv").hide();
        $("#SWPDIV").show();
    } else {
        $("#SWPDIV").hide();
        $("#ReedemDiv").show();
        //$("#additionalContent").find(".specific-element-to-hide").hide();
    }
});

function GetSchemeWiseFolioDetails(PanNo, FolioNumber, SchemeId) {
    var content = '';
    $.ajax
        ({
            type: "GET",
            url: CommonURL + "api/MFUserPortFolio/GetMFPortfolioDetailsBySchemeId",
            data: {
                Scheme_Id: SchemeId,
                FOLIONO: FolioNumber,
                Pan: PanNo
            },
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                SchemeWiseFolioDetail.push(data.data);
                GetMinAmountAndQty(data.data.isin);

                content = content + '<div class="p-4">' +
                    '<img src="../assets/AMCLogo/' + data.data.amcCode + '.jpg" style="width: 70px; height: 70px;">' +
                    '<h2 class="mt-4" style="font-size: 24px;" class="mt-4"><span>' + data.data.scheme_Name + '</span></h2>' +
                    '<hr>' +
                    '<p class="small"><img src="../assets/images/icons/warning-alert.png" style="width:14px;filter: invert(1);margin-top: -3px;"> Investment transactions completed after 02:00 PM are submitted to Mutual Fund Company for further processing on the next business day.</p>' +

                    '</div>';
                $("#ShowSubLogo").html(content);
            },
            error: function (data) {
                console.log(data);
            }
        });
}

function GetMinAmountAndQty(isin) {

    $.ajax
        ({
            type: "GET",
            url: GlobalUrl + "MFUserPortFolio/GetMinAmount",
            data: {
                ISIN: isin
            },
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                GetMinAmout.push(data.data);
                $("#MinAmt").html(parseFloat(data.data.minRedemptionAmt).toFixed(2));
                $("#AmtMultiples").html(parseFloat(data.data.redemptionAmtMultiplier).toFixed(2));
                $("#MinUnit").html(parseFloat(data.data.minRedemptionQty).toFixed(4));
                $("#MinMulti").html(parseFloat(data.data.redemptionQtyMultiplier).toFixed(4));
                $("#SWPMinAmt").html(parseFloat(data.data.minRedemptionAmt).toFixed(2));
            },
            error: function (data) {
                console.log(data);
            }
        });
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

$("#InsertRedeem").click(function () {

    var Volume = $("#RedeemVolume").val()
    var units = "";
    var amount = "";
    var Schemename = schemedata.schemeName;
    if (Volume == "") {
        swal.fire("Please Select Volume");
        return false;
    }

    if (Volume == "Two") {
        units = 0;
        amount = $("#RedeemAmt").val();
        if ($("#RedeemAmt").val() == "") {

            swal.fire("Redemption Amout cannot be blank");
            return false;
        }
        if (parseFloat($("#RedeemAmt").val()) > parseFloat(GetMinAmout[0].minRedemptionAmt)) {
            swal.fire('Check Minimum Redemption Amount');
            return false;
        }
        if (parseFloat(schemedata.currentValue.toFixed(2)) <= parseFloat($("#RedeemAmt").val())) {
            swal.fire('Check Minimum Redemption Amount');
            return false;
        }
    }
    if (Volume == "three") {
        amount = $("#RedeemUnit").val();
        if ($("#RedeemUnit").val() == "") {
            swal.fire("Redemption Unit cannot be blank");
            return false;
        }
        if (parseFloat(schemedata.balanceUnits).toFixed(4) <= parseFloat($("#RedeemUnit").val()).toFixed(4)) {
            swal.fire('Check Minimum Redemption Unit');
            return false;
        }
        if (parseFloat($("#RedeemUnit").val()).toFixed(4) > parseFloat(GetMinAmout[0].minRedemptionQty).toFixed(4)) {
            swal.fire('Check Minimum Redemption Unit');
            return false;
        }
    }
    if (Volume == "One") {
        amount = $("#RedeemAllUnit").val();
    }
    if (TermsChk.checked == false) {
        swal.fire('Please read and agree Terms & Conditions, Diclamer & Privacy Policy');
        return false;
    }

    if (units == "") {
        var details = "to Redeem Rs." + amount + " in " + Schemename
    }
    else {
        var details = "to Redeem Unit " + amount + " Unit in " + Schemename
    }
    swal.fire({
        title: "Are you sure?",
        text: "You want " + details,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                //swal.fire("Redeemption order placed successfully", {
                //    icon: "success",
                //});
                //if ($("#bRedeemi").hasClass('fa fa-check') == true) {
                SentRDMMessage()

                //}
            } else {
                swal("Your order is cancelled");
            }
        });
});
function SentRDMMessage() {
    var redata = {
        "ucc": UCC,
        "TransactionType": "Redeem"
    };
    $.ajax
        ({
            type: "POST",
            url: CommonWebsiteURL + "OrderAuthentication/AuthenticationInsert",
            data: JSON.stringify(redata),
            contentType: "application/json",
            success: function (data) {
                RDMAllOTPDetails.push(data);
                $("#RDMFirstHolderNo").html(data.fMobileNumber);
                $("#RedeemOtpTab").show();
                RDMtimer(120);
            },
            error: function (data) {
                console.log(data);
            }
        });
}

$("#RDMVerifyOTP").click(function () {

    if (RDMAuthOne != true) {
        swal.fire("Kindly verify the OTP");
        return false;
    }
    else {
        InsertRedeemOrder();
    }

});
function InsertRedeemOrder() {
    var unit = $("#RedeemUnit").val();
    var RedeemAmt = $("#RedeemAmt").val();
    var ClientCode = $("#UCC").val();
    if (unit == "") {
        unit = 0;
    }
    if (RedeemAmt == "") {
        RedeemAmt = 0;
    }
    var volume = "";
    var minRed = "N";
    var Volume = $("#RedeemVolume").val();
    if (Volume == "One") {
        volume = "1";
        minRed = "Y";
    }
    if (Volume == "Two") {
        volume = "2";
        minRed = "Y";
    }
    if (Volume == "three") {
        volume = "3";

    }
    var redata = {
        "ucc": ClientCode,
        "isin": schemedata.isinNo,
        "volume": volume,
        "trnMode": "Physical",
        "pan": schemedata.pan,
        "units": parseFloat(unit),
        "amount": parseFloat(RedeemAmt),
        "folioNo": localStorage.getItem("FolioNo"),
        "minRed": minRed,
        "schemeName": schemedata.schemeName,
        "ordPlacedBy": ClientCode,
        "device": "0"
    };
    $.ajax
        ({
            type: "POST",
            url: CommonWebsiteURL + "InvestNow/RedeemInsert",
            data: JSON.stringify(redata),
            contentType: "application/json",
            success: function (data) {
                console.log(data)
                if (data.code == 200) {
                    window.location.href = CommonWebsiteURL + 'MutualFund/Transaction'
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}
var RDMAuthOne = false;
function OTPValidation(data) {
    var holder = 1;//data.dataset.holder;
    var enteredotp = data.value;
    if (enteredotp.length == 6) {
        $.ajax({
            type: "POST",
            url: GlobalUrl + "OrderAuthentication/Authenticate2FAOTP?ucc=" + $("#UCC").val() + "&holderno=" + holder + "&OTP=" + enteredotp + "",
            data: {
            },
            success: function (data) {
                if (holder == "1") {
                    if (data.data = "1") {
                        RDMAuthOne = true;
                        $("#FirstOTPVerify").show();
                        document.getElementById('RDMfirsttimercartdiv').style.display = 'none';
                        document.getElementById('RDMfirstresendcartotp').style.display = 'none';
                        $('#RDMFirstOtp').attr("disabled", "disabled");
                    }
                }
                //if (holder == "2") {
                //    if (data.data = "1") {
                //        auth2 = true;
                //        $("#SecondOTPVerify").show();
                //        document.getElementById('secondtimercartdiv').style.display = 'none';
                //        document.getElementById('secondresendcartotp').style.display = 'none';
                //        $('#SecondOtp').attr("disabled", "disabled");
                //    }
                //}
                //if (holder == "3") {
                //    if (data.data = "1") {
                //        auth3 = true;
                //        $("#ThirdOTPVerify").show();
                //        document.getElementById('thirdtimercartdiv').style.display = 'none';
                //        document.getElementById('thirdresendcartotp').style.display = 'none';
                //        $('#TherdOTP').attr("disabled", "disabled");
                //    }
                //}
            },
            error: function (data) {

            }
        });
    }
}

function RDMResendotpcart(holders, isswp) {
    var commonOrderID = RDMAllOTPDetails[0].commonOrderID;

    Resendcountdowndiv = holders;
    var mobileNo = "";
    var Email = "";
    //if resend otp
    if (Resendcountdowndiv == "1") {
        mobileNo = $("#RDMFirstHolderNo").html();
        Email = RDMAllOTPDetails[0].fEmailID;
        document.getElementById('RDMfirstresendcartotp').style.display = 'none';
    }
    var data = {
        "CommonOrderID": JSON.stringify(commonOrderID),
        "Holders": JSON.stringify(holders),
        "TransactionType": "",
        "UCC": ClientCode,
        "emailId": Email,
        "mobileNo": mobileNo
    }

    $.ajax
        ({
            type: "POST",
            url: CommonWebsiteURL + "OrderAuthentication/UpdateResendOTP",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (data) {
                if (data.holder == "1") {
                    const newotp = { "fholdOTP": data.holdOTP };
                    const result = mergeObjects(RDMAllOTPDetails[0], newotp);//replace value on same key
                    RDMAllOTPDetails[0] = result;
                }
                if (isswp == "swp") {
                    document.getElementById('SWPfirstresendcartotp').style.display = 'none';
                    SWPtimer(30);
                }
                else {
                    RDMtimer(30);
                }

            },
            error: function (data) {

            }
        })
}
let timerOn = true;
function RDMtimer(remaining) {
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;
    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    document.getElementById('RDMfirsttimercart').innerHTML = m + ':' + s;

    remaining -= 1;
    if (remaining >= 0 && timerOn) {
        setTimeout(function () {
            RDMtimer(remaining);
        }, 1000);
        return;
    }
    if (!timerOn) {
        // Do validate stuff here
        return;
    }
    document.getElementById('RDMfirstresendcartotp').style.display = 'block';
}

function GetSIPDates(SIPDates) {
    var SIPDates = "";
    const date1 = new Date();
    var GetServerDatTime = GetServerDateTime(date1);
    var FirstOrderFlag = true;
    var Frequency = $("#Frequency").val().toUpperCase();
    var start = "";
    var nextDay = "";
    $("#SWPDate").empty();
    var htmldates = '<option value="">Please Select SIP Date</option>';;
    var separator = GetServerDatTime.substring(3, 2);
    var sdate = GetServerDatTime.split(separator)[0].toString();
    var smonth = GetServerDatTime.split(separator)[1].toString();
    var syear = GetServerDatTime.split(separator)[2].toString().split(' ')[0];
    var i;

    if ($('#FirstOrderToday').prop("checked") == true) {
        FirstOrderFlag = true;
    }
    else {
        FirstOrderFlag = false;
    }
    if (GetServerDateTime != "") {
        $("#SIPDate").empty();
        if (FirstOrderFlag == true) {
            if (Frequency == "MONTHLY") {
                start = new Date(syear, (parseInt(smonth)).toString(), (parseInt(sdate)).toString() - 2)
            }
        }
        else {
            start = new Date(syear, (parseInt(smonth) - 1).toString(), (parseInt(sdate)).toString())
        }
    }
    nextDay = new Date(start);
    for (i = 1; i <= 31; i++) {
        var date = new Date(start),
            days = i;
        if (!isNaN(date.getTime())) {
            date.setDate(date.getDate() + days)
            if (SIPDates.split(',').indexOf(date.getDate().toString())) {
                nextDay = GetServerDateTime(date);
                if (SIPDates.toString().indexOf(i.toString())) {
                    htmldates += '<option value="' + nextDay + '">' + nextDay + '</option>';
                }
            }
        } else {
            swal.fire("Invalid Date");
        }
    }
    $("#SWPDate").append(htmldates);

    return false;
}

$("#SWPInstallment").change(function () {
    var SWPDate = $("#SWPDate").val();
    var Installments = $("#SWPInstallment").val();
    if (SWPDate == "") {
        swal.fire('Please Select Start Date');
        return false;
    }
    var str = $('#SWPDate').val();
    var date2 = new Date(str.split('/')[2], str.split('/')[1], parseInt(str.split('/')[0]) - 1);
    var date1 = new Date(str.split('/')[2] - 1, parseInt(str.split('/')[1]) - 1, str.split('/')[0]);
    var date = str.split('/')[1] + "/" + str.split('/')[0] + "/" + str.split('/')[2];

    var Frequency = $("#Frequency").val().toUpperCase();
    if (Frequency == "MONTHLY") {
        $("#endmonth").val(add_months(date2, parseInt(parseInt(Installments))).toString());
    }
});


function add_months(dt, n) {
    dt.setMonth(dt.getMonth() + 1);
    dt.setDate(dt.getDate());
    return GetServerDateTime( new Date(dt.setMonth(dt.getMonth() + parseInt(n - 2))));
}



$('#myselectionSWP').on('change', function () {
    var demovalue = $(this).val();
    $("div.myDivSWP").hide();
    $("#show" + demovalue).show();
});