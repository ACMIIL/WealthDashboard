$(document).ready(function () {
    SchemeDetails_globalsearch();
    clientGlobalSearch();
})

let suggestions = [];
const clientoptions = []; // Your options here

const searchInput = document.querySelector(".globalsearchInput");
//const input = searchInput.querySelector("input");
//const resultBox = searchInput.querySelector(".resultBox");
//const icon = searchInput.querySelector(".icon");
//let linkTag = searchInput.querySelector("a");
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
//input.onkeyup = (e) => {
//    var thisval = $("#SearchScheme").val();
//    if (thisval.length >= 3) {
//        let userData = e.target.value; //user enetered data
//        let emptyArray = [];
//        if (userData) {
//            emptyArray = suggestions.filter((data) => {
//                //filtering array value and user characters to lowercase and return only those words which are start with user enetered chars
//                return data.toLocaleLowerCase().startsWith(userData.toLocaleLowerCase());
//            });
//            emptyArray = emptyArray.map((data) => {
//                // passing return data inside li tag
//                return data = '<li>' + data + '</li>';
//            });
//            searchInput.classList.add("active"); //show autocomplete box
//            showSuggestions(emptyArray);
//            let allList = resultBox.querySelectorAll("li");
//            for (let i = 0; i < allList.length; i++) {
//                //adding onclick attribute in all li tag
//                allList[i].setAttribute("onclick", "search_selectfund(this)");
//            }
//        } else {
//            //searchInput.classList.remove("mfSchemeCompareSearchBox"); //hide autocomplete box
//            search_fundatadrops(e)
//        }
//    }
//}

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

                    clientoptions.push(value.CommonClientCode + '-' + value.ClientName);
                    autocomplete(document.getElementById("myInput"), clientoptions);
                    //$("#list-timezone").append('<option>' + value.CommonClientCode + '-' + value.ClientName + '</option>');
                })
            },
            error: function (data) {
                console.log(data);
            }

        });

}
//const options = ["Option 1", "Option 2", "Option 3"]; // Your options here

function filterOptions(inputValue) {
    const resultsDiv = document.getElementById('search-results');
    resultsDiv.innerHTML = ''; // Clear previous results

    if (inputValue.length >= 3) {
        const filteredOptions = clientoptions.filter(option => option.toLowerCase().includes(inputValue.toLowerCase()));
        if (filteredOptions.length > 0) {
            const ul = document.createElement('ul');
            filteredOptions.forEach(option => {
                const li = document.createElement('li');
                li.textContent = option;
                ul.appendChild(li);
            });
            resultsDiv.appendChild(ul);
            $("#search-results").show();
        } else {
            
            resultsDiv.textContent = 'No results found';
        }
    }
    else {
        $("#search-results").hide();
    }
}



function autocomplete(inp, arr) {
    var currentFocus;
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        this.parentNode.appendChild(a);
        for (i = 0; i < arr.length; i++) {
            if (arr[i].toUpperCase().indexOf(val.toUpperCase()) > -1) {
                b = document.createElement("DIV");
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                b.addEventListener("click", function (e) {
                    inp.value = this.getElementsByTagName("input")[0].value;
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });
    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            currentFocus++;
            addActive(x);
        } else if (e.keyCode == 38) {
            currentFocus--;
            addActive(x);
        } else if (e.keyCode == 13) {
            e.preventDefault();
            if (currentFocus > -1) {
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {
        if (!x) return false;
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        x[currentFocus].classList.add("autocomplete-active");
        x[currentFocus].scrollIntoView({ behavior: "smooth", block: "nearest" });
    }
    function removeActive(x) {
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}
