$(document).ready(function () {
    SchemeDetails_globalsearch();
    clientGlobalSearch();
})

let suggestions = [];

const searchInput = document.querySelector(".globalsearchInput");
const input = searchInput.querySelector("input");
const resultBox = searchInput.querySelector(".resultBox");
const icon = searchInput.querySelector(".icon");
let linkTag = searchInput.querySelector("a");
let webLink;


function SchemeDetails_globalsearch() {
    GetReturnArrays = "";

    $.ajax(
        {
            url: CommonApiURL + "api/GlobalSearch/GetGlobalSearchDeatils",
            type: "GET",
            data: {

            },
            dataType: "json",
            success: function (data) {
                $.each(data.data, function (index, value) {
                    localStorage.setItem("ISIN", value.isin);
                    localStorage.setItem("SchmeName", value.schemeName);
                    suggestions.push(value.schemeName + '-' + value.isin);

                    //suggestions.push(value.schemeName + "<div id='abc' style='display:none;'> '-' " + value.isin + "</div>");

                })
            },
            error: function (data) {
                console.log(data);
            }

        });
}

// if user press any key and release
input.onkeyup = (e) => {
    var thisval = $("#SearchScheme").val();
    if (thisval.length >= 3) {
        let userData = e.target.value; //user enetered data
        let emptyArray = [];
        if (userData) {
            emptyArray = suggestions.filter((data) => {
                //filtering array value and user characters to lowercase and return only those words which are start with user enetered chars
                return data.toLocaleLowerCase().startsWith(userData.toLocaleLowerCase());
            });
            emptyArray = emptyArray.map((data) => {
                // passing return data inside li tag
                return data = '<li>' + data + '</li>';
            });
            searchInput.classList.add("active"); //show autocomplete box
            showSuggestions(emptyArray);
            let allList = resultBox.querySelectorAll("li");
            for (let i = 0; i < allList.length; i++) {
                //adding onclick attribute in all li tag
                allList[i].setAttribute("onclick", "search_selectfund(this)");
            }
        } else {
            //searchInput.classList.remove("mfSchemeCompareSearchBox"); //hide autocomplete box
            search_fundatadrops(e)
        }
    }
}

function search_fundatadrop(e) {
    let userData = e.target.value; //user enetered data
    let emptyArray = [];
    //if (userData) {
    emptyArray = suggestions.filter((data) => {
        //filtering array value and user characters to lowercase and return only those words which are start with user enetered chars
        return data.toLocaleLowerCase().startsWith(userData.toLocaleLowerCase());
    });
    emptyArray = emptyArray.map((data) => {
        // passing return data inside li tag
        return data = '<li>' + data + '</li>';
    });
    searchInput.classList.add("active"); //show autocomplete box
    showSuggestions(emptyArray);
    let allList = resultBox.querySelectorAll("li");
    for (let i = 0; i < allList.length; i++) {
        //adding onclick attribute in all li tag
        allList[i].setAttribute("onclick", "search_selectfund(this)");
    }
}

function clientGlobalSearch() {
    debugger;
    var RcCode = $("#HiddenLogiId").val();
    var RcCode = RCCode;// $("#HiddenLogiId").val();

    $.ajax(
        {
            url: "https://trade.investmentz.com/InvestmentzAPI/api/EmpBaClients?EmpBACode=" + RcCode + "&Option=1",
            type: "GET",
            data: {
                //SubCategory: SchemeSubCategory
            },
            dataType: "json",
            success: function (data) {
                $.each(data.EmpBAClientMaster, function (index, value) {

                    $("#list-timezone").append('<option>' + value.CommonClientCode + '-' + value.ClientName + '</option>');
                })
            },
            error: function (data) {
                console.log(data);
            }

        });

}