$(document).ready(function () {
    GetTransactionData();
});

function GetTransactionData() {
    var mydata = {
        "schemedropdownlist": "allvalue",
        "clientcode": UCC,
        "loginRole": "C",
        "bacode": RCCode
    }

    $.ajax({
            type: "POST",
            url: CommonApiURL + "api/MFUsers/GetTransation",
            data: JSON.stringify(mydata),
            contentType: "application/json",
            success: function (data) {
                //console.log(data);
                var htmlval = "";
                htmlval = htmlval + '<table class="display table table-bordered dataTable" border="1" id="transactionTable">' +
                                        '<thead>' +
                                            '<th>DateTime</th>' +
                                            '<th>Scheme Name</th>' +
                                            '<th>Type</th>' +
                                            '<th>Amount</th>' +
                                            '<th>Unit</th>' +
                                            '<th>Mode</th>' +
                                            '<th>Status</th>' +
                                        '</thead>' +
                                        '<tbody>';
                $.each(data.data, function (index, value) {
                    var status = "";
                    var colour = "";
                    if (value.order_Status == "J" || value.order_Status == "E" || value.order_Status == "F" || value.order_Status == "R") {
                        status = "FAILED"
                        colour = "btn-danger";
                    }
                    if (value.order_Status == "B" || value.order_Status == "G") {
                        status = "REJECTED"
                        colour = "btn-success";
                    }
                    if (value.order_Status == "" || value.order_Status == "C") {
                        status = "CONFIRMED"
                        colour = "btn-success";
                    }
                    if (value.order_Status == "A") {
                        status = "ALLOTTED"
                        colour = "btn-success";
                    }
                    if (value.order_Status == "X") {
                        status = "CANCELLED"
                        colour = "btn-warning";
                    }
                    if (value.order_Status == "Y" || value.order_Status == "O") {
                        status = "CANCEL REQ."
                        colour = "btn-warning";
                    }
                    if (value.order_Status == "I" || value.order_Status == "U") {
                        status = "MODIFIED REQ."
                        colour = "btn-primary";
                    }
                    if (value.order_Status == "M") {
                        status = "MODIFIED"
                        colour = "btn-primary";
                    }
                    if (value.order_Status == "Z") {
                        status = "AMO"
                        colour = "btn-primary";
                    }
                    if (value.order_Status == "N" || value.order_Status == "P") {
                        status = "PLACED"
                        colour = "btn-success";
                    }
                    if (value.order_Status == "D") {
                        status = "SUCESS"
                        colour = "btn-success";
                    }
                    if (value.order_Status == "T") {
                        status = "Pending Payment"
                        colour = "btn-success";
                    }
                    if (value.order_Status == "S") {
                        status = "AMO(Pending Payment)"
                        colour = "btn-success";
                    }

                    htmlval = htmlval + '<tr>' +
                                            '<td>' + moment(value.bseOrdDate_dt).format('D MMM YYYY, HH:mm') + '</td>' +
                                            '<td>' + value.schemeName + '</td>' +
                                            '<td>' + value.bseOrdlumpsip_s + '</td>' +
                                            '<td>' + value.bseOrdAmount_n + '</td>' +
                                            '<td>' + value.unit + '</td>' +
                                            '<td>' + value.bseOrdDPTrans_s + '</td>' +
                                            '<td><button class="btn ' + colour + '" data-transdata="' + encodeURIComponent(JSON.stringify(value)) +'" onclick="OrderStatus(this);" >'+status+'</button></td>' +
                                        '</tr>';
                });

                htmlval = htmlval + '</tbody></html>';

                $("#transactionReport").append(htmlval);

                $("#transactionTable").DataTable({
                    "bLengthChange": false,
                    "bSort": false
                });
            },
            error: function (data) {

            }
     });
}

function OrderStatus(data) {
    var decodedData = decodeURIComponent(data.dataset.transdata);
    var jsonObject = JSON.parse(decodedData);
    console.log(jsonObject);

    var stringdata = jsonObject.bseOrdOutputResponse_s;
    if (stringdata == null || stringdata == "") {

    } else {
        var str = stringdata;
        var res = str.split("|");
        var status;
        if (res.length >= 6) {
            status = res[6]
            var rejectionReason = status;
        }
        else {
            status = res[1];
        }
        var finalstatus = status.split(" ");
    }

    $("#date").html(jsonObject.bseOrdDate_dt);
    $("#ClientCode").html(jsonObject.bseOrdUcc_n);
    $("#Schemecode").html(jsonObject.bseOrdSchemeCode_s);
    $("#ISIN").html(jsonObject.isin);
    $("#Settlement").html(jsonObject.settlementType);
    $("#OrderType").html(jsonObject.bseOrdBuySell_s);
    $("#DP").html(jsonObject.bseOrdDPTrans_s);
    $("#ClientName").html(jsonObject.clientName);
    $("#FolioNo").html(jsonObject.bseOrdFolioNo_s);
    $("#SUBCode").html(jsonObject.ifA_CODE);
    $("#EUIN").html(jsonObject.euin);
    $("#TransctionType").html(jsonObject.bseOrdPaymentType);
    $("#SchemeName").html(jsonObject.schemeName);

    if (jsonObject.bseOrdOutputResponse_s == "") {
        $("#Rejection").html(val.sysBseOrdRejectReason);
    } else {
        $("#Rejection").html(status);
    }

    $("#rejected").modal('show');
}