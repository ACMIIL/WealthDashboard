$(document).ready(function () {
    localStorage.setItem("EditFlag", null);
    $("#UCC").val(UCC);
    getpersonaldetail();
    GetCartUserDetails();
});
var ucc = UCC;
var HolderCount = [];
var ListOrderId = [];
var arrindex = [];
var CartDetails = [];
var AllOTPDetails = [];
var countdowndiv;
var sameentry;
function GetCartUserDetails() {
    var content = '';
    var PaymentMode = "";
    var FolioNumber = "";
    
    $.ajax(
        {
            type: "GET",
            url: GlobalUrl + "MFUsers/UserGetCartDetail",
            data: {
                UCC: document.getElementById('UCC').value
            },
            success: function (data) {
                CartDetails.push(data.data);
                //$("#CartCount").html(data.data.length);
                $.each(data.data, function (index, value) {
                    if (value.folioNo == "" || value.folioNo == "null") {
                        FolioNumber = "-";
                    } else {
                        FolioNumber = value.folioNo
                    }

                    if (value.paymentMode == "PaymentGateWay") {
                        PaymentMode = "Payment GateWay";
                    }
                    else {
                        PaymentMode = "Mandate";
                    }
                    var html = //`<div class="col">
                        `<div class="col-md-6 col-12 mb-3">
                            <div class="mf-card ">
                                <div class="row">
                                    <div class="col-md-9 col-9">
                                        <div class="d-flex">
                                            <img src="/images/basket-logo.jpg" alt="" style="width:24px;height:24px;" />
                                            <div class="card-title">
                                                <h5>${value.schemeType}</h5>
                                                <h6>${value.schemeName} </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-3">
                                        <div class="d-flex">
                                            <button id="${value.srNo}" style="cursor:pointer;" onClick="DeleteCartOrder()" type="button" class="btn "><i style="font-size:24px;"  class="fa">&#xf014;</i></button>
                                            <button style="cursor:pointer;" type="button" class="btn "><img src="/images/edit.png" alt="Alternate Text" style="font-size:24px;" /></button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-12">
                                        <div class="card-details">
                                            <h5>
                                                Investment Type
                                            </h5>
                                            <h6>${value.transactionType}</h6>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-12">
                                        <div class="card-details">
                                            <h5>
                                                Folio no
                                            </h5>
                                            <h6>${FolioNumber}</h6>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-12">
                                        <div class="card-details">
                                            <h5>
                                                Investment Amt
                                            </h5>
                                            <h6>${value.amount}</h6>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-12">
                                        <div class="card-details">
                                            <h5>
                                                Nav (As on ${value.navDate})
                                            </h5>
                                            <h6>${value.naV_VALUE}</h6>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-12">
                                        <div class="card-details">
                                            <h5>
                                                Payment Mode
                                            </h5>
                                            <h6>${PaymentMode}</h6>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    //</div>`;
                    $("#GetCartDetails").append(html);
                });


                if (data.data.length > 0) {
                    $("#cartorderbtndiv").show();
                }
                
            },
            error: function (data) {
                console.log(data);
            }
        });
}



function DeleteCartOrder() {
    var SrNo = event.currentTarget.id;
    swal.fire({
        title: "Are you sure?",
        text: "For Delete",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax
                    ({
                        type: "GET",
                        url: GlobalUrl + "MFUsers/UserCartDeleteItem",
                        data: {
                            SrNo: SrNo
                        },
                        success: function (data) {
                            if (data.code == 200) {
                                swal.fire("Deleted");
                                window.location.reload();
                            }
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    });
            } else {
                swal("Your order is cancelled");
            }
        });

}
$("#ConfirmOrder").click(function () {

    if (checksamepaymentgatway() != false) {
        $("#Otpmodal").modal('show');
        GetHolderCount();
    }
    else {
        swal.fire('Please select similar payment mode for all your current cart items. You can edit your cart items to set similar payment mode');
    }
    //$("#FirstDiv").show();

    //timer(180);
    
});
function GetHolderCount() {
    $.ajax
        ({
            type: "GET",
            url: GlobalUrl + "OrderAuthentication/HolderCount",
            data: {
                UCC: document.getElementById('UCC').value,
                Option: "2"
            },
            success: function (data) {
                if (data.code == 200) {
                    SentMessage()
                    HolderCount.push(data.data);
                    if (data.data == "1") {
                        $("#FirstDiv").show();
                        countdowndiv = "1";
                    }
                    if (data.data == "2") {
                        $("#FirstDiv").show();
                        $("#SecondDiv").show();
                        countdowndiv = "2";
                    }
                    if (data.data == "3") {
                        $("#SecondDiv").show();
                        $("#FirstDiv").show();
                        $("#ThirdDiv").show();
                        countdowndiv = "3";
                    }
                    timer(120);
                }
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
                //AllOTPDetails.push(data);
                $("#FirstHolderNo").html(data.fMobileNumber);
                $("#SecondHolderNo").html(data.sMobileNumber);
                $("#ThirdHolderNo").html(data.tMobileNumber);

            },
            error: function (data) {
                console.log(data);
            }
        });
}

function verifyOtp() {
    $.ajax({
        type: "POST",
        url: "",
        data: {

        },
        success: function (data) {

        },
        error: function (data) {

        }
    });
}

function InsertBseOrder() {

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
                //var paymentlink = data1.data.url;
                ////$scope.Loader = false;
                //window.location.href = paymentlink;
                $('#plz_wait').modal('hide');
                swal.fire("Payment Link Send On email Successfully..");
            }
            else {
                swal.fire("Payment Fail");
            }
        },
        error: function (data1) {
            console.log(data1);
        }
    });
}

function getpersonaldetail() {
    $.ajax(
        {
            type: "GET",
            url: CommonWebsiteURL + "OrderAuthentication/GetPrimaryDetails",
            data: {
                UCC: document.getElementById('UCC').value
            },
            success: function (data) {
                $("#panNo").val(data.panNo);
                $("#AccountNo").val(data.accountNo);
            },
            error: function (data) {

            }
        })
}

function checksamepaymentgatway() {
    sameentry = true;
    $.each(CartDetails[0], function (index, value) {
        if (CartDetails[0][0].paymentMode != value.paymentMode) {
            sameentry = false;
            return sameentry;
        }
    });
    return sameentry;
}

let timerOn = true;

function timer(remaining) {
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;
    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    if (countdowndiv == "1") {
        document.getElementById('firsttimercart').innerHTML = m + ':' + s;
    }
    if (countdowndiv == "2") {
        document.getElementById('firsttimercart').innerHTML = m + ':' + s;
        document.getElementById('secondtimercart').innerHTML = m + ':' + s;
    }
    if (countdowndiv == "3") {
        document.getElementById('firsttimercart').innerHTML = m + ':' + s;
        document.getElementById('secondtimercart').innerHTML = m + ':' + s;
        document.getElementById('thirdtimercart').innerHTML = m + ':' + s;
    }
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
    if (countdowndiv == "1") {
        document.getElementById('firstresendcartotp').style.display = 'block';
    }
    if (countdowndiv == "2") {
        document.getElementById('firstresendcartotp').style.display = 'block';
        document.getElementById('secondresendcartotp').style.display = 'block';
    }
    if (countdowndiv == "3") {
        document.getElementById('firstresendcartotp').style.display = 'block';
        document.getElementById('secondresendcartotp').style.display = 'block';
        document.getElementById('thirdresendcartotp').style.display = 'block';
    }
}