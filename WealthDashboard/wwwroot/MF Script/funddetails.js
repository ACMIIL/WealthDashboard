$(document).ready(function () {
    SchemeDetails(localStorage.getItem("Subcategory"));
})

function SchemeDetails(data) {
    $("#funddetailstb").html('');

   // const SchemeSubCategory = data.dataset.schemedetail;
    let URL = GlobalUrl + "MFUsers/GetInvestNowDetail";
    let datas = {
        SubCategory: data
    };

    $.ajax({
        type: "GET",
        url: URL,
        data: datas,
        success: function (data) {
            try {
                // Handle success
                $("#funddetailstb").html('');
                data.data.forEach((value) => {
                    const html = `
                        <tr>
                            <td>
                                <a href="#" id="${value.isin}" data-fundname="${value.accordSchemeName}" 
                                data-encschemdetails="${encodeURIComponent(JSON.stringify(value)) }"
                                    onClick="GetInvestNow1(event)"
                                    class="card-link-group">
                                    <strong>${value.accordSchemeName}</strong>
                                </a>
                            </td>
                            <td>
                                <span>${Array(5).fill('<img src="/images/Group.png" class="star">').join('')}</span>
                            </td>
                            <td class="text-success">${value.fiveyrret}%</td>
                            <td class="text-success">${value.thirdyrret}%</td>
                            <td class="text-success">${value.oneYrret}%</td>
                            <td class="text-success">${value.threeMonthret}%</td>
                            <td class="font-14">${value.aum}Cr</td>
                        </tr>`;
                    $("#funddetailstb").append(html);
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

function GetInvestNow1(data) {
    sessionStorage.clear();
    const ISIN = data.currentTarget.id;
    const values =  data.currentTarget.dataset.encschemdetails;
    sessionStorage.setItem("ISIN", ISIN);
    localStorage.setItem("FolioNo", null);

    // Save the clicked data in sessionStorage for later use
    sessionStorage.setItem("selectedFund", values); 

    // Update the content dynamically
    //updateContent(value.schemeName, value);

    // Set fundNameDisplayInput value
    //$("#fundNameDisplayInput").val(value.schemeName);

    window.location.href = CommonWebsiteURL + 'MutualFund/InvestNow';
}