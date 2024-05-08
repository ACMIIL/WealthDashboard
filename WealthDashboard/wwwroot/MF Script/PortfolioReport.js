$(document).ready(function () {
    GetPortfolioData();
});

function GetPortfolioData() {
    $.ajax({
        type: "GET",
        url: "https://mfssoacmiil.investmentz.com/" + "api/Portfolio/PortfolioDetails",
        data: {
            ClientName: "MOHMADTALHA MUSTAKBHAI SHEIKH",
            PanNo: "FVBPS9933K",
            group: "folioid"
        },
        success: function (data) {
            console.log(data);
            if (parseFloat(data.data.result.summary.gain) >= 0) {
                document.getElementById("AbsReturnAmt").classList.add("text-success");
                document.getElementById("absper").classList.add("bg-success-subtle");
                document.getElementById("absper").classList.add("text-success");
                document.getElementById("absper").classList.remove("bg-danger-subtle");
                document.getElementById("absper").classList.remove("text-danger");

                document.getElementById("inamt").classList.add("text-success");
                document.getElementById("inamt").classList.remove("text-danger");

                document.getElementById("cramt").classList.add("text-success");
                document.getElementById("cramt").classList.remove("text-danger");

                document.getElementById("totdaychg").classList.add("text-success");
                document.getElementById("totdaychg").classList.remove("text-danger");

                document.getElementById("perdaychg").classList.add("text-success");
                document.getElementById("perdaychg").classList.remove("text-danger");

                document.getElementById("cagr").classList.add("text-success");
                document.getElementById("cagr").classList.remove("text-danger");
            } else {
                document.getElementById("AbsReturnAmt").classList.add("text-danger");
                document.getElementById("absper").classList.remove("bg-success-subtle");
                document.getElementById("absper").classList.remove("text-success");
                document.getElementById("absper").classList.add("bg-danger-subtle");
                document.getElementById("absper").classList.add("text-danger");

                document.getElementById("inamt").classList.remove("text-success");
                document.getElementById("inamt").classList.add("text-danger");

                document.getElementById("cramt").classList.remove("text-success");
                document.getElementById("cramt").classList.add("text-danger");

                document.getElementById("totdaychg").classList.remove("text-success");
                document.getElementById("totdaychg").classList.add("text-danger");

                document.getElementById("perdaychg").classList.remove("text-success");
                document.getElementById("perdaychg").classList.add("text-danger");

                document.getElementById("cagr").classList.remove("text-success");
                document.getElementById("cagr").classList.add("text-danger");

            }
            
            $("#AbsReturnAmt").html("&#x20B9;" + new Intl.NumberFormat('en-IN').format(parseFloat(data.data.result.summary.gain).toFixed(2)));
            $("#AbsRetrunPercent").html(parseFloat(data.data.result.summary.absoluteReturn).toFixed(2) + "%");
            $("#InvestedAmt").html("&#x20B9;" + new Intl.NumberFormat('en-IN').format(parseFloat(data.data.result.summary.purchaseValue).toFixed(2)));
            $("#CurrentAmt").html("&#x20B9;" + new Intl.NumberFormat('en-IN').format(parseFloat(data.data.result.summary.currentValue).toFixed(2)));

            $("#TotalOneDayChanges").html("&#x20B9;" + new Intl.NumberFormat('en-IN').format(parseFloat(data.data.result.summary.oneDayChange).toFixed(2)));
            $("#TotalPercent").html(parseFloat(data.data.result.summary.changePercent).toFixed(2) + "%");

            $("#GetCAGR").html(parseFloat(data.data.result.summary.cagr).toFixed(2) + "%");

            var tabledata = '<table class="datatable" border="" id="potfolioTable">' +
                                '<thead style="background-color:#f2f2f2;"> ' +
                                        '<th>Scheme Name</th>' +
                                        '<th>Action</th>' +
                                        '<th>Invested</th>' +
                                        '<th>Units</th>' +
                                        '<th>P&L</th>' +
                                        '<th>NAV</th>' +
                                        //'<th>Day P&L</th>' +
                                        //'<th>CAGR</th>' +
                                        '<th>Current Valuation</th>' +
                                    '</tr>' +
                                '</thead>' +
                '<tbody>';

            $.each(data.data.result.data, function (index, value) {
                tabledata = tabledata + '<tr>' +
                                            '<td onclick="detailPort(this);" data-schemename="' + value.schemeName + '" data-category="' + value.objectiveName + '" data-isin="' + value.isinNo + '" data-folio="' + value.folioNo + '" data-scagr="' + value.cagr + '" data-purchasedate="' + value.purchaseDate + '" data-daychange="' + value.oneDayChange + '" data-daychangeper="' + value.changePercent + '" >' + value.schemeName + '<i class="fa fa-align-left" aria-hidden="true"></i></td>' +
                                            '<td>'+
                                                '<div class="dropdown">' +
                                                    '<button class="btn btn-link text-muted p-1 mt-n2 py-0 text-decoration-none fs-15" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                                                        '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-more-horizontal icon-sm">' +
                                                            '<circle cx="12" cy="12" r="1"></circle>' +
                                                            '<circle cx="19" cy="12" r="1"></circle>' +
                                                            '<circle cx="5" cy="12" r="1"></circle>' +
                                                        '</svg>' +
                                                    '</button>' +
                                                    '<div class="dropdown-menu dropdown-menu-end" style="">' +
                                                        '<a class="dropdown-item" data-schemedata="' + encodeURIComponent(JSON.stringify(value)) +'" onclick="AdditionalPurchase(this);" ><i class="ri-eye-fill align-bottom me-2 text-muted"></i>Buy More</a>' +
                                                        '<a class="dropdown-item" data-schemedata="' + encodeURIComponent(JSON.stringify(value)) +'" onclick="Switch(this);"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i>Switch</a>' +
                                                        '<a class="dropdown-item" data-schemedata="' + encodeURIComponent(JSON.stringify(value)) +'" onclick="Redeem(this);"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i>Redeem</a>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</td>' +
                                            '<td>' + parseFloat(value.purchaseValue).toFixed(2) + '</td>' +
                                            '<td>' + parseFloat(value.balanceUnits).toFixed(3) + '</td>' +
                                            '<td>' + parseFloat(value.gain).toFixed(2) + '</td>' +
                                            '<td>' + parseFloat(value.nav).toFixed(4) + '</td>' +
                                            //'<td>' + parseFloat(value.oneDayChange).toFixed(2) + '</td>' +
                                            //'<td>' + parseFloat(value.cagr).toFixed(2) + '</td>' +
                                            '<td>' + parseFloat(value.currentValue).toFixed(2) + '</td>' +
                                        '</tr>';
            });

            tabledata = tabledata + '</tbody></html>';

            $("#portfolioReport").append(tabledata);

            $("#potfolioTable").DataTable({
                "bLengthChange": false,
                "bSort": false
            });
        },
        error: function (data) {

        },
    });
}

function AdditionalPurchase(data) {
    var decodedData = decodeURIComponent(data.dataset.schemedata);
    var jsonObject = JSON.parse(decodedData);
    console.log(jsonObject);
}

function Switch(data) {
    var decodedData = decodeURIComponent(data.dataset.schemedata);
    var jsonObject = JSON.parse(decodedData);
    console.log(jsonObject);
}

function Redeem(data) {
    var decodedData = decodeURIComponent(data.dataset.schemedata);
    var jsonObject = JSON.parse(decodedData);
    console.log(jsonObject);
}

function detailPort(data) {

    // Parse the date string into a Date object
    var date = new Date(data.dataset.purchasedate);

    // Format the date
    var formattedDate = ("0" + date.getUTCDate()).slice(-2) + "-" +
        ("0" + (date.getUTCMonth() + 1)).slice(-2) + "-" +
        date.getUTCFullYear();

    $("#schemeName").html(data.dataset.schemename);
    $("#category").html(data.dataset.category);
    $("#isin").html(data.dataset.isin);
    $("#folioNo").html(data.dataset.folio);
    $("#scagr").html(data.dataset.cagr);
    $("#purchaseDate").html(formattedDate);
    $("#dayChange").html(parseFloat(data.dataset.daychange).toFixed(4));
    $("#dayChangePer").html(parseFloat(data.dataset.daychangeper).toFixed(4));

    $("#chargeModal").modal('show');
}