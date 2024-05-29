

//var GlobalUrl = "https://investinmfapi.investmentz.com/api/";

//var clientapi = "https://clientprofileapi.investmentz.com/api/UsersDetails/GetUsersPersonalDetails?ClientCode=4100467";
//var CommonWebsiteURL = "https://wealthcompany.in/";
//var CommonWebsiteURL = "http://localhost:52206/";
$(document).ready(function () {
    
    GetclientDetails();
    SentMessage();
    GetCartUserDetails();

});
var CartDetails = [];
var AllOTPDetails = [];
var arrindex = [];
var TotalAmount = 0;
var ListOrderId = [];
var getclntdetail = [];
var countdowndiv;
function GetCartUserDetails() {
    var content = '';
    var PaymentMode = "";
    var FolioNumber = "";
    debugger;
    $.ajax(
        {
            type: "GET",
            url: GlobalUrl + "MFUsers/UserGetCartDetail",
            data: {
                UCC: document.getElementById('UCC').value
            },
            success: function (data) {
                CartDetails.push(data.data);
              
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function GetclientDetails() {
    var content = '';
    var PaymentMode = "";
    var FolioNumber = "";

    $.ajax(
        {
            type: "GET",
            url: "https://clientprofileapi.investmentz.com/api/UsersDetails/GetUsersPersonalDetails?ClientCode=" + document.getElementById('UCC').value,
            data: {
                
            },
            success: function (data) {
                $("#AccountNo").val(data.data.accountNo);
                getclntdetail.push(data.data);
               // CartDetails.push(data.data);
                console.log(data);
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function SentMessage() {
    var redata = {
        "ucc": document.getElementById('UCC').value,
        "TransactionType": "Buy"
    };
    $.ajax
        ({
            type: "POST",
            url: CommonWebsiteURL + "OrderAuthentication/AuthenticationInsert",
            data: JSON.stringify(redata),
            contentType: "application/json",
            success: function (data) {
                debugger;
                AllOTPDetails.push(data);
                $("#mobilenospan").html(data.fMobileNumber);
                timer(20);
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function InsertBseOrder() {
    TotalAmount = 0;
    $.each(CartDetails[0], function (index, value) {

        TotalAmount = parseFloat(value.amount) + parseFloat(TotalAmount);
        if (CartDetails[0][index].transactionType == "SIP") {
            if (value.folioNo == "null") {
                value.folioNo = "";
            }
            else {
                value.folioNo
            }
            var InsertBSECraeteOrder = {
                "ucc": value.ucc,
                "paymentmode": value.paymentMode,
                "siplumpsum": value.transactionType,
                "schemecode": value.schemeCode,
                "buySell": "Buy",// value.ucc,
                "dematPhy": value.transactionMode,
                "ordLumpsumValue": parseFloat(0),// value.amount,
                "sipStartDate": value.sipStartDate,
                "sipAmount": parseFloat(value.amount),// value.ucc,////
                "sipInstallMents": parseInt(value.sipInstallment),
                "empBaLoginType": "B",//value.orderBy,
                "loginId": value.orderBy,
                "fundIntoSrNo": parseFloat(value.srNo),
                "isFirstOrder": value.firstOrderToday,
                "foliono": value.folioNo,
                "mandate_SrNo": "", //Controller se pass hota he
                "mandate_Flag": "P",//value.ucc,
                "orderplaced": value.orderBy,
                "commonOrderId": JSON.stringify(AllOTPDetails[0].commonOrderID),
                "frequency": value.frequency

            }
        }
        else {
            if (value.folioNo == "null") {
                value.folioNo = "";
            }
            else {
                value.folioNo
            }
            var InsertBSECraeteOrder = {
                "ucc": value.ucc,
                "paymentmode": value.paymentMode,
                "siplumpsum": value.transactionType,
                "schemecode": value.schemeCode,
                "buySell": "Buy",// value.ucc,
                "dematPhy": value.transactionMode,
                "ordLumpsumValue": parseFloat(value.amount),
                "sipStartDate": value.sipStartDate,
                "sipAmount": parseFloat(0),// value.ucc,////
                "sipInstallMents": 0,
                "empBaLoginType": "B",// value.orderBy,
                "loginId": value.orderBy,
                "fundIntoSrNo": parseFloat(value.srNo),
                "isFirstOrder": value.firstOrderToday,
                "foliono": value.folioNo,
                "mandate_SrNo": "", //value.ucc,
                "mandate_Flag": "P",//value.ucc,
                "orderplaced": value.orderBy,
                "commonOrderId": JSON.stringify(AllOTPDetails[0].commonOrderID),
                "frequency": value.frequency

            }
        }

        $.ajax(
            {
                type: "POST",
                url: CommonWebsiteURL + "OrderAuthentication/InsertBSECraeteOrder",
                data: JSON.stringify(InsertBSECraeteOrder),
                contentType: "application/json",
                success: function (data) {

                    ListOrderId.push(data.empBaOrdOrderId);
                    arrindex.push(index);

                    if (CartDetails[0].length == arrindex.length) {

                        PaymentGet(TotalAmount);
                    }
                },
                error: function (data) {
                    alert("error In data");
                    return;
                }
            })


    });
    //alert(arrindex.length);

}

let timerOn = true;

function timer(remaining) {
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;
    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    // if (countdowndiv == "1") {
    
    document.getElementById('firsttimercartdiv').style.display = 'block';
    document.getElementById('authfirsttimercart').innerHTML = m + ':' + s;
    //}
    remaining -= 1;
    if (remaining >= 0 && timerOn) {
        setTimeout(function () {
            timer(remaining);
        }, 1000);
        return;
    }
    if (!timerOn) {
        // Do validate stuff here
        return;
    }
    //if (countdowndiv == "1") {
        //if (auth1 != true) {
    document.getElementById('resendotpdiv').style.display = 'block';
    document.getElementById('firsttimercartdiv').style.display = 'none';
        //}
    //}
}

$("#rsndotp").click(function () {
    document.getElementById('resendotpdiv').style.display = 'none';
    timer(20);
    Resendotpcart(1);
});

function PaymentGet(TotalAmount) {
    var ord = JSON.stringify(ListOrderId)
    var MftrnId = ord.replace(/[\[\]'"]+/g, '')
    //$scope.Loader = true;
    $.ajax({
        type: "POST",
        url: CommonWebsiteURL + "OrderAuthentication/Paymentrequest",
        data: {
            payeeBankAccountNo: document.getElementById('AccountNo').value,// "5413581771",//accno,
            payeeBankID: "HD4",    //need to add BankID
            currencyCode: "INR",
            payeeLoginID: document.getElementById('UCC').value,
            PayAmount: TotalAmount,        // need to get payment account
            MFTransactionID: MftrnId, //"11",  // need to generate random number
            RequestSource: "1"
        },
        success: function (data1) {

            //$scope.Loader = false;
            if (data1 == "") {
                alert("Payment Fail");
            }
            if (data1.code == 200) {
                ////$("#modalOnLoadforpayment").fadeOut(200);
                var paymentlink = data1.data.url;
                ////$scope.Loader = false;
                window.location.href = paymentlink;
                //$('#plz_wait').modal('hide');
                //swal.fire("Payment Link Send On email Successfully..");
            }
            else {
                alert("Payment Fail");
            }
        },
        error: function (data1) {
            console.log(data1);
        }
    });
}
$("#btnOtpverity").click(function () {
    var otp = '';
    var otpInputs = document.querySelectorAll('.otp-input input');
    otpInputs.forEach(function (input) {
        otp += input.value;
    });

    if (otp.length != 6) {
        alert("Please Enter otp");
    }
    else {
        OTPValidation(otp);
    }
    console.log(otp);

})
function OTPValidation(otp) {
    var holder = "1";
        $.ajax({
            type: "POST",
            url: GlobalUrl + "OrderAuthentication/Authenticate2FAOTP?ucc=" + $("#UCC").val() + "&holderno=" + holder + "&OTP=" + otp + "",
            data: {
            },
            success: function (data) {
                if (data.data == "1") {
                    $("#loading-bar-spinner").show();
                    $("#btnOtpverity").prop('disabled', true);
                    InsertBseOrder();
                    //alert("Enter order");
                }
                else {
                    alert("OTP not varified.Please Enter Correct OTP");
                }
            },
            error: function (data) {

            }
        });
}

function Resendotpcart(holders) {
    var commonOrderID = AllOTPDetails[0].commonOrderID;
    var transactionType = "";//CartDetails[0][0].transactionType;
    Resendcountdowndiv = holders;
    var mobileNo = "";
    var Email = "";
    //if resend otp
    //if (Resendcountdowndiv == "1") {
    //    mobileNo = $("#FirstHolderNo").html();
    //    Email = AllOTPDetails[0].fEmailID;
    //    document.getElementById('firstresendcartotp').style.display = 'none';
    //}

    var data = {
        "CommonOrderID": JSON.stringify(commonOrderID),
        "Holders": JSON.stringify(holders),
        "TransactionType": "",
        "UCC": document.getElementById('UCC').value,
        "emailId": getclntdetail[0].emailId,
        "mobileNo": getclntdetail[0].mobileNo
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
                    //const result = mergeObjects(AllOTPDetails[0], newotp);//replace value on same key
                    AllOTPDetails[0] = result;
                }
            },
            error: function (data) {

            }
        })
}