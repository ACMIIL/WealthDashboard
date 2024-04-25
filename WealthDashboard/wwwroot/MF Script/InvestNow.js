$(document).ready(function () {
    Checkmandate();
});
var uCC = UCC;
function GetInvestNowDetails(ISIN) {
    var content = '';
    $.ajax(
        {
            type: "GET",
            url: CommonURL + "api/MFUsers/GetInvestNowDetailByID",
            data: {
                ISIN: ISIN
            },
            success: function (data) {
                //content = content + '<img src="../assets/AMCLogo/' + data.data.amcCode + '.jpg" style="width: 70px; height: 70px;"> ' +
                //    '<h2 class="mt-4" style="font-size: 24px;" class="mt-4"><span id="SchemeName">' + data.data.schemeName + '</span></h2>' +
                //    '<hr>' +
                //    '<p class="small"><img src="../assets/images/icons/warning-alert.png" style="width:14px;filter: invert(1);margin-top: -3px;"> Investment transactions completed after 02:00 PM are submitted to Mutual Fund Company for further processing on the next business day.</p>';


               // GetInvestNowDeatils.push({ Details: data.data })
                $("#MinAmount").html(data.data.sipMinInstallmentAmt);
                $("#Multiples").html(data.data.sipMultiplierAmt);
                $("#MinInstallments").html(data.data.sipMinInstallmentNo);
                $("#LumpsumMinAmount").html(data.data.minPurchaseAmt);
                $("#LumpsumMultiples").html(data.data.purchaseAmtMultiplier);
                GetSIPDates(data.data.sipDates)

                $("#DisplatSchemeDetails").html(content);
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function GetSIPDates(SIPDates) {
    var GetServerDatTime = localStorage.getItem("ServerDateTime");
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

    if ($('#PaymentGateway').prop("checked") == true) {
        FirstOrderFlag = true;
    }
    else {
        FirstOrderFlag = false;
    }
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
                nextDay = date.toInputFormat();
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
