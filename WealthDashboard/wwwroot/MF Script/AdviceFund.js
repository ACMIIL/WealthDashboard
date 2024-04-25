$(document).ready(function () {
    MFCategory();
    
});
//var GlobalUrl = "https://localhost:7217/api/";
var GlobalUrl = "https://investinmfapi.investmentz.com/api/";
function MFCategory() {
    var html = "";
    $.ajax({
        type: "GET",
        url: GlobalUrl + "Accord/GetMFCategory",
        data: {
        },
        success: function (data) {
            $.each(data.data, function(key,value){
                html =  '<div class="col-md-2 col-6 mt-4 mb-4">'+
                    '<button type="button" id='+ value.category +' class="btn box-1" data-Subcategory=' + value.category +' onClick="MFSubCategory(this)" >' +
                        '<label class="lable3">' + value.category +'</label>' +
                        '</button>' +
                        '</div>'
                $("#CategoryDiv").append(html);
            });
            MFSubCategory('Equity');
        },
        error: function (data) {

        }
    });
}
function MFSubCategory(data) {
    $("#MFSubCategoryDiv").html('');
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
                var html = '<div class="col-md-4 col-6 mt-3 mb-3">' +
                    '<div class="card-mf-schemes d-flex ">' +
                    '<div class="col-md-1 col-1">' +
                    '<img src="/images/basket-logo.jpg" class="card-mf-icon">' +
                    '</div>' +
                    '<div class="col-md-10 col-10">' +
                    '<a class="card-title-2 font-16 ps-2" style="cursor: pointer;" data-schemedetail=' + JSON.stringify(value.subCategory) + ' onclick="redirecttofunddetail(this)">' + value.subCategory +'</a>' +
                    '</div>' +
                    '<div class="col-md-1 col-1 text-end">' +
                    '<img src="~/images/ic_round-favorite-selected.png " class="card-mf-heart">' +
                    '</div>' +
                    '</div>' +
                    '</div>';
                $("#MFSubCategoryDiv").append(html);

                $('.active').removeClass('active'); // remove existing active
                $('#'+val).addClass('active'); // set current link as active
            });
        },
        error: function (data) {

        }
    });
    
}

function redirecttofunddetail(data) {
    const SchemeSubCategory = data.dataset.schemedetail;
    localStorage.setItem("Subcategory", SchemeSubCategory);
    window.location.href = "FundDetails";
}

