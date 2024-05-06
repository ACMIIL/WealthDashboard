$(document).ready(function () {
    //SMFCategory();
    SchemeDetails();
});
//var GlobalUrl = "https://localhost:7217/";
var GlobalUrl = "https://investinmfapi.investmentz.com/";
var CommonWebsiteURL = "http://localhost:52206/";
function SMFCategory() {
    var html = "";
    $.ajax({
        type: "GET",
        url: GlobalUrl + "Accord/GetMFCategory",
        data: {
        },
        success: function (data) {
            $.each(data.data, function (key, value) {
                html = '<li class="me-5">'+
                        '<a class=" mf-sub-tab1" aria-current="page" data-Subcategory=' + value.category + ' onClick="SMFSubCategory(this)">' + value.category + '</a>'+
                        '</li>';
                        $("#Screenercate").append(html);
            });
            var filter = '<li class="me-5 mb-3">' +
                         '<a class="mf-sub-tab1-button ps-5"  onClick="ShowFilter(this)">Advanced Filter <i class="fa fa-angle-down"></i></a>' +
                         '</li>';
            $("#Screenercate").append(filter);
            //SMFSubCategory('Equity');
        },
        error: function (data) {

        }
    });
}

function SMFSubCategory(data) {
    $("#ScreenerSubCategorydiv").html('');
    var val = "";
    if (data == "Equity") {
        val = "Equity";
    }
    else {
        val = data.dataset.subcategory;
    }



    $.ajax({
        type: "GET",
        url: GlobalUrl + "Accord/GetMFSubCategory",
        data: {
            Category: val
        },
        success: function (data) {
            $.each(data.data, function (key, value) {
                var html = '<div id='+val+' class="col-md-2 col-4 mb-4">'+
                        '<div class="box-1">' +
                    '<input class="form-check-input" id=' + value.subCategory.trim() +'  type="checkbox" value="" id="flexCheckDefault">' +
                    '<label class="form-check-label lable4" for=' + value.subCategory.trim() +'  data-schemedetail=' + JSON.stringify(value.subCategory) + ' onclick="SchemeDetails(this)" for="flexCheckDefault" href="#">' +
                    '&nbsp;' + value.subCategory +
                        '</label>' +
                            '</div>'+
                        '</div>';

                $("#ScreenerSubCategorydiv").append(html);

                //$('.active').removeClass('active'); // remove existing active
                //$('#' + val).addClass('active'); // set current link as active
            });
            
        },
        error: function (data) {

        }
    });

}
function ShowFilter() {
    var css = document.getElementById('ScreenerSubCategorydiv').style.display;
    if (css == 'none') {
        
        $("#ScreenerSubCategorydiv").show();
        return;
    }
    else {
        $("#ScreenerSubCategorydiv").hide();
    }
    
}

function updateContent(fundName, fundDetails) {
    // Removed console.log statements

    // Display fund name in both a div and an input
    $("#fundNameDisplayDiv").text(fundName);
    $("#fundNameDisplayInput").val(fundName);
}

function GetInvestNow(event) {
    sessionStorage.clear();
    const ISIN = event.currentTarget.id;
    const values = event.currentTarget.dataset.encschemdetails;
    sessionStorage.setItem("ISIN", ISIN);
    sessionStorage.setItem("FolioNo", null);

    // Save the clicked data in sessionStorage for later use
    sessionStorage.setItem("selectedFund", values);  ``

    // Update the content dynamically
    //updateContent(value.schemeName, value);

    // Set fundNameDisplayInput value
   // $("#fundNameDisplayInput").val(value.schemeName);

    window.location.href = CommonWebsiteURL + 'MutualFund/InvestNow';
}

function SchemeDetails() {
    $("#scrtbody").html('');

    //const SchemeSubCategory = data.dataset.schemedetail;
    let URL = GlobalUrl + "MFUsers/GetScreenerData";
    //let URL = "https://localhost:7217/MFUsers/GetScreenerData";
    //let datas = {
    //    SubCategory: SchemeSubCategory
    //};

    $.ajax({
        type: "GET",
        url: URL,
        data: {},
        success: function (data) {
            try {
                // Handle success
                $("#scrtbody").html('');
                data.data.forEach((value) => {
                    const html = `
                        <tr>
                            <td>
                                <a href="#" id="${value.isin}" data-fundname="${value.schemeName}" 
                                data-encschemdetails="${encodeURIComponent(JSON.stringify(value)) }"
                                    onClick="GetInvestNow(event)"
                                    class="card-link-group">
                                    <strong>${value.schemeName}</strong>
                                </a>
                            </td>
                            <td class="text-success">${value.fiveyrret}%</td>
                            <td class="text-success">${value.thirdyrret}%</td>
                            <td class="text-success">${value.oneYrret}%</td>
                            <td class="text-success">${value.threeMonthret}%</td>
                            <td class="font-14">${value.aum}Cr</td>
                        </tr>`;
                    $("#scrtbody").append(html);
                });
            } catch (error) {
                console.error(error);
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
}

//function GetInvestNow(event, value) {
//    const ISIN = event.currentTarget.id;
//    localStorage.setItem("ISIN", ISIN);
//    localStorage.setItem("FolioNo", null);
//    window.location.href = CommonWebsiteURL + 'MutualFund/SIPFund';
//}


//function GetInvestNow(event, value) {
//    console.log("GetInvestNow called");
//    const ISIN = event.currentTarget.id;
//    localStorage.setItem("ISIN", ISIN);
//    localStorage.setItem("FolioNo", null);
//    window.location.href = CommonWebsiteURL + 'MutualFund/SIPFund';
//}
function Rearrangedata(data, Return) {
    var sortedData = [];
    if (Return == "1") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.onemonthret.localeCompare(a.onemonthret)
        });
    }
    else if (Return == "3") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.threeMonthret.localeCompare(a.threeMonthret)
        });
    }
    else if (Return == "6") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.sixmonthret.localeCompare(a.sixmonthret)
        });
    }
    else if (Return == "9") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.nineMonth.localeCompare(a.nineMonth)
        });
    }
    else if (Return == "12") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.oneYrret.localeCompare(a.oneYrret)
        });
    }
    else if (Return == "24") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.twoyearret.localeCompare(a.twoyearret)
        });
    }
    else if (Return == "36") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.thirdyrret.localeCompare(a.thirdyrret)
        });
    }
    else if (Return == "60") {
        for (var i in data.data) sortedData.push(data.data[i]);
        sortedData.sort(function (a, b) {
            return b.fiveyrret.localeCompare(a.fiveyrret)
        });
    }
    else if (Return == "AUM") {
        sortedData = data.data.sort((a, b) => {
            if (parseInt(b.aum) !== parseInt(a.aum)) {
                return parseInt(b.aum) - parseInt(a.aum);
            }
            return (parseInt(b.aum.split('.')[1], 10) || 0) - (parseInt(a.aum.split('.')[1], 10) || 0);
        });
    }

    var content = "";
    var Returndata = "";
    $.each(sortedData, function (index, value) {
        var Return = $("#ReturnYears").val();
        if (Return == "1") {
            Returndata = value.onemonthret;
        }
        else if (Return == "3") {
            Returndata = value.threeMonthret;
        }
        else if (Return == "6") {
            Returndata = value.sixmonthret;
        }
        else if (Return == "9") {
            Returndata = value.nineMonth;
        }
        else if (Return == "12") {
            Returndata = value.oneYrret;
        }
        else if (Return == "24") {
            Returndata = value.twoyearret;
        }
        else if (Return == "36") {
            Returndata = value.thirdyrret;
        }
        else if (Return == "60") {
            Returndata = value.fiveyrret;
        }
        else if (Return == "60") {
            Returndata = value.fiveyrret;
        }
        content = content + '<div class="row border-bottom"> <div class="col-md-1 col-12 text-center">' +
            '<div class="ps-1 pt-2 mt-2 mb-3">' +
            '<img src="../assets/AMCLogo/' + value.amcCode + '.jpg" class="img-fluid equity-img">' +
            '</div>' +
            '</div>' +
            '<div class="col-md-4 col-12 ps-0 pe-0">' +
            '<div class="title-link1 pt-4 mt-2 mobile-text-center">' +
            '<a href="#" id="' + value.isin + '" data-fundname="' + value.schemeName + '" onClick="GetSchemeDetails(this)" class="card-link-group">' +
            '<strong>' + value.schemeName + '</strong>' +
            '</a>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-3 col-6 pe-0 text-center">' +
            '<div class="mt-3 mb-3">' +
            '<div>Fund Size</div>' +
            '<div>₹' + value.aum + ' Crs</div>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-2 col-6 text-center">' +
            '<div class="mt-3 mb-3">' +
            '<div>Return(p.a)</div>' +
            '<div style="color: green;"><span id="' + value.isin + '">' + Returndata + '</span></div>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-2 col-12 pt-3 text-center">' +
            '<div class="mb-3">' +
            '<a id=' + value.isin + ' href="#" onClick="GetInvestNow()" class="btn btn-light btn-rounded BtnInvestNow">Invest Now</a>' +
            '<label><input type="checkbox" id="cmpr_' + value.isin + '" data-isin=' + value.isin + ' data-fundname=' + JSON.stringify(value.schemeName) + ' data-return=' + Returndata + ' onchange="comparefunds(this)" style="width:16px; height:16px; position:relative; top:3px;"> Add to compare</label>' +
            '</div>' +
            '</div>' +
            '</div>';
    })

    $("#SchemeDetails").html(content);
}