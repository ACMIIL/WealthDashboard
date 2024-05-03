var today = new Date().toISOString().split('T')[0];
$('#txtNomineeDOB').attr('max', today);
$('#SecNomineeDOB').attr('max', today);
$('#ThirdNomineeDOB').attr('max', today);
var secondgardiansave = false;
var thirdgardiansave = false;
$(document).ready(function () {

    $("#segmentvarifi").removeClass('a');
    $("#segmentvarifi").addClass('b');
});
function GetClientFirstPerAddressDetails() {
    if (document.getElementById('FNSameAsAddress').checked == true) {
        $.ajax({
            url: CommonAPIURL + "api/ClientPersonalDetails/GetClientPerAddressDetails",
            type: 'GET',
            data: {
                RegistrationId: $("#RegistrationId").val() //parseInt(document.getElementById('MainRegistrationId').value)
            },
            contentType: 'application/json',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var PermanentAddress = data.perAddress1 + ' ' + data.perAddress2 + ' ' + data.perAddress3;
                $('#FNAddressDetails').val(PermanentAddress);
                $('#FNPincode').val(data.perPincode);

                var appenddata1 = "<option value = '" + data.perCityCode + " '>" + data.perCity + " </option>";
                $("#ddlFirstNomineeCity").append(appenddata1);

                var StateMaster = "<option value = '" + data.perStateCode + " '>" + data.perState + " </option>";
                $("#FNState").append(StateMaster);

                CountryMaster = "<option value = '" + data.perCountryCode + " '>" + data.perCountry + " </option>";
                $("#FNCountry").append(CountryMaster);
            },
            error: function (data) {
                
                return;
            }
        });
    }
    else {
        if (document.getElementById('FNSameAsAddress').checked != true) {
            $('#FNAddressDetails').val('');
            $('#FNPincode').val('');
            $('#FNState').val('');
            $('#ddlFirstNomineeCity').val('');
            $('#FNCountry').val('');
        }
    }
}
function GetClientSecPerAddressDetails() {
    if (document.getElementById('SNSameAsAddress').checked == true) {
        $.ajax({
            url: CommonAPIURL + "api/ClientPersonalDetails/GetClientPerAddressDetails",
            type: 'GET',
            data: {
                RegistrationId: $("#RegistrationId").val() //parseInt(document.getElementById('MainRegistrationId').value)
            },
            contentType: 'application/json',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var PermanentAddress = data.perAddress1 + ' ' + data.perAddress2 + ' ' + data.perAddress3;
                $('#SNAddressDetails').val(PermanentAddress);
                $('#SNPincode').val(data.perPincode);

                var appenddata1 = "<option value = '" + data.perCityCode + " '>" + data.perCity + " </option>";
                $("#ddlSecNomineeCity").append(appenddata1);

                var StateMaster = "<option value = '" + data.perStateCode + " '>" + data.perState + " </option>";
                $("#SNState").append(StateMaster);

                CountryMaster = "<option value = '" + data.perCountryCode + " '>" + data.perCountry + " </option>";
                $("#SNCountry").append(CountryMaster);
            },
            error: function (data) {
               
                return;
            }
        });
    }
    else {
        if (document.getElementById('SNSameAsAddress').checked != true) {
            $('#SNAddressDetails').val('');
            $('#SNPincode').val('');
            $('#SNState').val('');
            $('#ddlSecNomineeCity').val('');
            $('#SNCountry').val('');
        }
    }

}

function GetClientTrdPerAddDetails() {
    if (document.getElementById('TNSameAsAddress').checked == true) {
        $.ajax({
            url: CommonAPIURL + "api/ClientPersonalDetails/GetClientPerAddressDetails",
            type: 'GET',
            data: {
                RegistrationId: $("#RegistrationId").val() //parseInt(document.getElementById('MainRegistrationId').value)
            },
            contentType: 'application/json',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var PermanentAddress = data.perAddress1 + ' ' + data.perAddress2 + ' ' + data.perAddress3;
                $('#TNAddressDetails').val(PermanentAddress);
                $('#TNPincode').val(data.perPincode);

                var appenddata1 = "<option value = '" + data.perCityCode + " '>" + data.perCity + " </option>";
                $("#ddlthNomineeCity").append(appenddata1);

                var StateMaster = "<option value = '" + data.perStateCode + " '>" + data.perState + " </option>";
                $("#TNState").append(StateMaster);

                CountryMaster = "<option value = '" + data.perCountryCode + " '>" + data.perCountry + " </option>";
                $("#TNCountry").append(CountryMaster);
            },
            error: function (data) {
                
                return;
            }
        });
    }
    else {
        if (document.getElementById('TNSameAsAddress').checked != true) {
            $('#TNAddressDetails').val('');
            $('#TNPincode').val('');
            $('#TNState').val('');
            $('#ddlthNomineeCity').val('');
            $('#TNCountry').val('');
        }
    }

}

function GetNomineePincodeDetails() {
    $('#ddlFirstNomineeCity').html('');
    $('#FNState').val('');
    $('#FNCountry').val('');

    if (document.getElementById('FNPincode').value == '') {
        showMessage('Please Enter First Nominee Pincode', 5000, 'red');  
    }
    $.ajax(
        {
            type: "POST",
            url: CommonPageURL + "Nominee/GetByPincode",
            data: {
                Pincode: document.getElementById('FNPincode').value
            },
            success: function (data) {
                if (data.city_Name != null) {

                    console.log(data);
                    var appenddata1 = "<option value = '" + data.city_id + " '>" + data.city_Name + " </option>";
                    $("#ddlFirstNomineeCity").append(appenddata1);

                    var StateMaster = "<option value = '" + data.state_Code + " '>" + data.stateName + " </option>";
                    $("#FNState").append(StateMaster);

                    CountryMaster = "<option value = '" + data.country_Code + " '>" + data.countryName + " </option>";
                    $("#FNCountry").append(CountryMaster);

                }
                else {

                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function SNPincodeDetails() {
    $('#ddlSecNomineeCity').html('');
    $('#SNState').val('');
    $('#SNCountry').val('');

    if (document.getElementById('SNPincode').value == '') {
        showDynamicMessage('Please Enter Second Nominee Pincode', 5000, 'red','SDynamicalert');
    }
    $.ajax(
        {
            type: "POST",
            url: CommonPageURL + "Nominee/GetByPincode",
            data: {
                Pincode: document.getElementById('SNPincode').value
            },
            success: function (data) {
                if (data.city_Name != null) {

                    console.log(data);
                    var appenddata1 = "<option value = '" + data.city_id + " '>" + data.city_Name + " </option>";
                    $("#ddlSecNomineeCity").append(appenddata1);

                    var StateMaster = "<option value = '" + data.state_Code + " '>" + data.stateName + " </option>";
                    $("#SNState").append(StateMaster);

                    CountryMaster = "<option value = '" + data.country_Code + " '>" + data.countryName + " </option>";
                    $("#SNCountry").append(CountryMaster);

                }
                else {

                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}

function TNPincodeDetails() {
    $('#ddlthNomineeCity').html('');
    $('#TNState').val('');
    $('#TNCountry').val('');

    if (document.getElementById('TNPincode').value == '') {
        showDynamicMessage('Please Enter Third Nominee Pincode', 5000, 'red', 'TDynamicalert');
    }
    $.ajax(
        {
            type: "POST",
            url: CommonPageURL + "Nominee/GetByPincode",
            data: {
                Pincode: document.getElementById('TNPincode').value
            },
            success: function (data) {
                if (data.city_Name != null) {

                    console.log(data);
                    var appenddata1 = "<option value = '" + data.city_id + " '>" + data.city_Name + " </option>";
                    $("#ddlthNomineeCity").append(appenddata1);

                    var StateMaster = "<option value = '" + data.state_Code + " '>" + data.stateName + " </option>";
                    $("#TNState").append(StateMaster);

                    CountryMaster = "<option value = '" + data.country_Code + " '>" + data.countryName + " </option>";
                    $("#TNCountry").append(CountryMaster);

                }
                else {

                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function FGuardianPincode() {
    $('#FGCity').html('');
    $('#FGState').val('');
    $('#FGCountry').val('');

    if (document.getElementById('FGPincode').value == '') {
        showMessage('Please Enter First Nominee Pincode', 5000, 'red', 'SDynamicalert');
    }
    $.ajax(
        {
            type: "POST",
            url: CommonPageURL + "Nominee/GetByPincode",
            data: {
                Pincode: document.getElementById('FGPincode').value
            },
            success: function (data) {
                if (data.city_Name != null) {

                    console.log(data);
                    var appenddata1 = "<option value = '" + data.city_id + " '>" + data.city_Name + " </option>";
                    $("#FGCity").append(appenddata1);

                    var StateMaster = "<option value = '" + data.state_Code + " '>" + data.stateName + " </option>";
                    $("#FGState").append(StateMaster);

                    CountryMaster = "<option value = '" + data.country_Code + " '>" + data.countryName + " </option>";
                    $("#FGCountry").append(CountryMaster);

                }
                else {

                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}

function SGuardianPincode() {
    $('#SGCity').html('');
    $('#SGState').val('');
    $('#SGCountry').val('');

    if (document.getElementById('SGPincode').value == '') {
        showDynamicMessage('Please Enter Second Nominee Pincode', 5000, 'red', 'SDynamicalert');
    }
    $.ajax(
        {
            type: "POST",
            url: CommonPageURL + "Nominee/GetByPincode",
            data: {
                Pincode: document.getElementById('SGPincode').value
            },
            success: function (data) {
                if (data.city_Name != null) {

                    console.log(data);
                    var appenddata1 = "<option value = '" + data.city_id + " '>" + data.city_Name + " </option>";
                    $("#SGCity").append(appenddata1);

                    var StateMaster = "<option value = '" + data.state_Code + " '>" + data.stateName + " </option>";
                    $("#SGState").append(StateMaster);

                    CountryMaster = "<option value = '" + data.country_Code + " '>" + data.countryName + " </option>";
                    $("#SGCountry").append(CountryMaster);

                }
                else {

                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}

function TGuardianPincode() {
    $('#TGCity').html('');
    $('#TGState').val('');
    $('#TGCountry').val('');

    if (document.getElementById('TGPincode').value == '') {
        showDynamicMessage('Please Enter Third Nominee Pincode', 5000, 'red', 'TDynamicalert');
    }
    $.ajax(
        {
            type: "POST",
            url: CommonPageURL + "Nominee/GetByPincode",
            data: {
                Pincode: document.getElementById('TGPincode').value
            },
            success: function (data) {
                if (data.city_Name != null) {

                    console.log(data);
                    var appenddata1 = "<option value = '" + data.city_id + " '>" + data.city_Name + " </option>";
                    $("#TGCity").append(appenddata1);

                    var StateMaster = "<option value = '" + data.state_Code + " '>" + data.stateName + " </option>";
                    $("#TGState").append(StateMaster);

                    CountryMaster = "<option value = '" + data.country_Code + " '>" + data.countryName + " </option>";
                    $("#TGCountry").append(CountryMaster);

                }
                else {

                }
            },
            error: function (data) {
                console.log(data);
            }
        });
}
function CheckMinorFirstNominee() {
    var FirstNomineAge = getAge(document.getElementById('txtNomineeDOB').value);

    if (FirstNomineAge >= 18 && FirstNomineAge != "NaN") {
        document.getElementById("div1stNomineeGuardianDetails").style.display = "none";

    }
    else {
        document.getElementById("div1stNomineeGuardianDetails").style.display = "block";


    }
}
function CheckMinorSecNominee() {
    var SecNomineAge = getAge(document.getElementById('SecNomineeDOB').value);

    if (SecNomineAge >= 18 && SecNomineAge != "NaN") {
        document.getElementById("SecNGuardian").style.display = "none";

    }
    else {
        document.getElementById("SecNGuardian").style.display = "block";
    }
}

function CheckMinorThirdNominee() {
    var ThirdNomineAge = getAge(document.getElementById('ThirdNomineeDOB').value);

    if (ThirdNomineAge >= 18 && ThirdNomineAge != "NaN") {
        document.getElementById("thdNGuardian").style.display = "none";

    }
    else {
        document.getElementById("thdNGuardian").style.display = "block";
    }
}
function getAge(dateString) {
    var today = new Date();
    var birthDate = new Date(dateString);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age;
}

function SaveNomineeDetails() {
    var checkfirstnominee = FirstNomineeValidation();
    if (checkfirstnominee == false)
    {
        return;
    }

    var percentage = document.getElementById('PercentageAllocation').value;
    if (percentage != 100) {
        showMessage("percentage should be 100", 5000, 'red');
        return;
    }
    var res = document.getElementById('FNSecurityRes').checked;
    if (res == false)
    {
        showMessage("Please select Residual Security", 5000, 'red');
        return;
    }

    Saveguardian = getAge(document.getElementById('txtNomineeDOB').value)

    if (Saveguardian < 18 && Saveguardian != "NaN")
    {
        var guardianfirst = FirstGuardianvalidation();
        if (guardianfirst == false) {
            return;
        }

    }  
    var EncRegistrationId = $("#EncRegistrationId").val();
    var RelationType = $('input[name="btnRelation"]:checked').val();
    var address = document.getElementById('FNAddressDetails').value;
    var Address1 = address.substring(0, 50);
    var Address2 = address.substring(50, 100);
    var Address3 = address.substring(100, 150);
    NomineeJson = [{
        "clientNomineeId": 0,
        "registrationId": $("#RegistrationId").val(),
        "nomineeType": 76,
        "title": "",
        "nomineeFirstName": document.getElementById('txtFristNomineeName').value,
        "nomineeMiddleName": "",
        "nomineeLastName": document.getElementById('txtFristNomineeLastName').value,
        "dobNominee": document.getElementById('txtNomineeDOB').value,
        "isMinor": false,
        "relationshipType": parseInt(RelationType),
        "fileName": "",
        "filePath": "",
        "fileName2": "",
        "filePath2": "",
        "address1": Address1,
        "address2": Address2,
        "address3": Address3,
        "pincode": document.getElementById('FNPincode').value,
        "cityCode": parseInt(document.getElementById('ddlFirstNomineeCity').value),
        "stateCode": parseInt(document.getElementById('FNState').value),
        "countryCode": parseInt(document.getElementById('FNCountry').value),
        "percentageAllocate": document.getElementById('PercentageAllocation').value,
        "sameAsAddress": document.getElementById('FNSameAsAddress').checked,
        "isResidualSecurities": document.getElementById('FNSecurityRes').checked,
        "userId": "acmiil"

    }]
    console.log(NomineeJson);
   
    $.ajax({
        url: CommonAPIURL + "api/ClientNominee/InsertOrUpdateClientNomineeUploadDocuments",
        type: 'POST',
        data: JSON.stringify(NomineeJson),
        contentType: "application/json",
        success: function (data) {
            if (data.message == "Success") {

                Saveguardian = getAge(document.getElementById('txtNomineeDOB').value)
                if (Saveguardian < 18 && Saveguardian != "NaN") {
                    SaveFirstGuadianDetails();

                }

                SavePageDetails($("#RegistrationId").val(), CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId);
                window.location.assign(CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId);
            }
            else {

            }

        },
        error: function (data) {

        }
    });
    
}
/*
function SaveSecondNomineeDetails() {

    var EncRegistrationId = $("#EncRegistrationId").val();
    var checkfirstnominee = FirstNomineeValidation();
    if (checkfirstnominee == false) {
        return;
    }

    Saveguardian = getAge(document.getElementById('txtNomineeDOB').value);
    if (Saveguardian < 18 && Saveguardian != "NaN") {
        var guardianfirst = FirstGuardianvalidation();
        if (guardianfirst == false) {
            return;
        }

    }

    var res1 = document.getElementById('FNSecurityRes');
    //var res2 = document.getElementById('SNSecurityRes');
    var atLeastOneChecked = res1.checked;// || res2.checked;
    if (atLeastOneChecked) {      
      
    } else {
        showDynamicMessage("Please select any one Residual Security", 5000, 'red','SDynamicalert');
        return;
        
    }

    var checksecondnominee = SecondNomineeValidation();
    if (checksecondnominee == false) {
        return;
    }
    var Secguardian = getAge(document.getElementById('SecNomineeDOB').value);
    if (Secguardian < 18 && Secguardian != "NaN")
    {
        var guardiansecond = secondGuardianvalidation();
        if (guardiansecond == false) {
            return;
        }
    }
    if (document.getElementById('OpenSecondN').style.display == "block") {
        var percentage = parseInt(document.getElementById('PercentageAllocation').value);
        var check = parseInt(document.getElementById('SecPerAllocation').value);
        var total = percentage + check;
        if (total != 100) {
            showDynamicMessage('Both First And Second Nominee Should be 100', 5000, 'red', 'SDynamicalert');
            return;
        }
    }
    var RelationType2 = $('input[name="btnSRelation"]:checked').val();
    var address2 = document.getElementById('SNAddressDetails').value;
    var SNAddress1 = address2.substring(0, 50);
    var SNAddress2 = address2.substring(50, 100);
    var SNAddress3 = address2.substring(100, 150);

    var RelationType1 = $('input[name="btnRelation"]:checked').val();
    var address = document.getElementById('FNAddressDetails').value;
    var Address1 = address.substring(0, 50);
    var Address2 = address.substring(50, 100);
    var Address3 = address.substring(100, 150);



    var NomineeJson = [
       {
            "clientNomineeId": 0,
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 76,
            "title": "",
            "nomineeFirstName": document.getElementById('txtFristNomineeName').value,
            "nomineeMiddleName": "",
            "nomineeLastName": document.getElementById('txtFristNomineeLastName').value,
            "dobNominee": document.getElementById('txtNomineeDOB').value,
            "isMinor": false,
            "relationshipType": parseInt(RelationType1),
            "fileName": "",
            "filePath": "",
            "fileName2": "",
            "filePath2": "",
            "address1": Address1,
            "address2": Address2,
            "address3": Address3,
            "pincode": document.getElementById('FNPincode').value,
            "cityCode": parseInt(document.getElementById('ddlFirstNomineeCity').value),
            "stateCode": parseInt(document.getElementById('FNState').value),
            "countryCode": parseInt(document.getElementById('FNCountry').value),
            "percentageAllocate": document.getElementById('PercentageAllocation').value,
            "sameAsAddress": document.getElementById('FNSameAsAddress').checked,
            "isResidualSecurities": document.getElementById('FNSecurityRes').checked,
            "userId": "acmiil"

        },

        {
            "clientNomineeId": 0,
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 77,
            "title": "",
            "nomineeFirstName": document.getElementById('txtSecondNomineeName').value,
            "nomineeMiddleName": "",
            "nomineeLastName": document.getElementById('txtSecondNomineeLastName').value,
            "dobNominee": document.getElementById('SecNomineeDOB').value,
            "isMinor": false,
            "relationshipType": parseInt(RelationType2),
            "fileName": "",
            "filePath": "",
            "fileName2": "",
            "filePath2": "",
            "address1": SNAddress1,
            "address2": SNAddress2,
            "address3": SNAddress3,
            "pincode": document.getElementById('SNPincode').value,
            "cityCode": parseInt(document.getElementById('ddlSecNomineeCity').value),
            "stateCode": parseInt(document.getElementById('SNState').value),
            "countryCode": parseInt(document.getElementById('SNCountry').value),
            "percentageAllocate": document.getElementById('SecPerAllocation').value,
            "sameAsAddress": document.getElementById('SNSameAsAddress').checked,
            "isResidualSecurities": document.getElementById('SNSecurityRes').checked,
            "userId": "acmiil"
        }
    ]
    console.log(NomineeJson);
    $.ajax({
        url: CommonAPIURL + "api/ClientNominee/InsertOrUpdateClientNomineeUploadDocuments",
        type: 'POST',
        data: JSON.stringify(NomineeJson),
        contentType: "application/json",
        success: function (data) {
            if (data.message == "Success")
            {  
                var Saveguardian = getAge(document.getElementById('txtNomineeDOB').value)
                if (Saveguardian < 18 && Saveguardian != "NaN") {
                    SaveFirstGuadianDetails();

                }
                if (secondgardiansave == false) {  // second gauardian save func change false to true
                    var Secguardian = getAge(document.getElementById('SecNomineeDOB').value);
                    if (Secguardian < 18 && Secguardian != "NaN") {

                        SaveSeconGuardian();
                    }
                }
                SavePageDetails($("#RegistrationId").val(), CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId);
                window.location.assign(CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId);
            }
            else {

            }

        },
        error: function (data) {

        }
    });
}*/
/*
function SaveThirdNomineeDetails() {

    var EncRegistrationId = $("#EncRegistrationId").val();

    var checkfirstnominee = FirstNomineeValidation();
    if (checkfirstnominee == false) {
        return;
    }

    Saveguardian = getAge(document.getElementById('txtNomineeDOB').value);
    if (Saveguardian < 18 && Saveguardian != "NaN") {
        var guardianfirst = FirstGuardianvalidation();
        if (guardianfirst == false) {
            return;
        }

    }
    var checksecondnominee = SecondNomineeValidation();
    if (checksecondnominee == false) {
        return;
    }

    var Secguardian = getAge(document.getElementById('SecNomineeDOB').value);
    if (Secguardian < 18 && Secguardian != "NaN") {
        var guardiansecond = secondGuardianvalidation();
        if (guardiansecond == false) {
            return;
        }
    }
    var checkthirdnominee = ThirdNomineeValidation();
    if (checkthirdnominee == false) {
        return;
    }

    var Thrdguardian = getAge(document.getElementById('ThirdNomineeDOB').value);
    if (Thrdguardian < 18 && Thrdguardian != "NaN")
    {

        var guardianthird = thirdguardianvalidation();
        if (guardianthird == false) {
            return;
        }
    }

    var res1 = document.getElementById('FNSecurityRes');
    //var res2 = document.getElementById('SNSecurityRes');
    //var res3 = document.getElementById('TNSecurityRes');
    var atLeastOneChecked = res1.checked;// || res2.checked || res3.checked;
    if (atLeastOneChecked) {

    } else {
        showDynamicMessage("Please select any one Residual Security", 5000, 'red', 'TDynamicalert');
        return;

    }


    if (document.getElementById('OpenThirdN').style.display == "block") {
        var Fpercentage = parseInt(document.getElementById('PercentageAllocation').value);
        var Spercentage = parseInt(document.getElementById('SecPerAllocation').value);
        var Tpercentage = parseInt(document.getElementById('ThirdPerAllocation').value);

        var TotalAllocation = Fpercentage + Spercentage + Tpercentage;
        if (TotalAllocation != 100) {
            showDynamicMessage('Both First Second And Third Nominee Should be 100', 5000, 'red', 'TDynamicalert');
            return;
        }

    }

    var SRelationType = $('input[name="btnSRelation"]:checked').val();
    var address2 = document.getElementById('SNAddressDetails').value;
    var SNAddress1 = address2.substring(0, 50);
    var SNAddress2 = address2.substring(50, 100);
    var SNAddress3 = address2.substring(100, 150);

    var RelationType = $('input[name="btnRelation"]:checked').val();
    var address = document.getElementById('FNAddressDetails').value;
    var Address1 = address.substring(0, 50);
    var Address2 = address.substring(50, 100);
    var Address3 = address.substring(100, 150);

    var TRelationType = $('input[name="btnTRelation"]:checked').val();
    var address3 = document.getElementById('TNAddressDetails').value;
    var TNAddress1 = address3.substring(0, 50);
    var TNAddress2 = address3.substring(50, 100);
    var TNAddress3 = address3.substring(100, 150);

    var NomineeJson = [
        {
            "clientNomineeId": 0,
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 76,
            "title": "",
            "nomineeFirstName": document.getElementById('txtFristNomineeName').value,
            "nomineeMiddleName": "",
            "nomineeLastName": document.getElementById('txtFristNomineeLastName').value,
            "dobNominee": document.getElementById('txtNomineeDOB').value,
            "isMinor": false,
            "relationshipType": parseInt(RelationType),
            "fileName": "",
            "filePath": "",
            "fileName2": "",
            "filePath2": "",
            "address1": Address1,
            "address2": Address2,
            "address3": Address3,
            "pincode": document.getElementById('FNPincode').value,
            "cityCode": parseInt(document.getElementById('ddlFirstNomineeCity').value),
            "stateCode": parseInt(document.getElementById('FNState').value),
            "countryCode": parseInt(document.getElementById('FNCountry').value),
            "percentageAllocate": document.getElementById('PercentageAllocation').value,
            "sameAsAddress": document.getElementById('FNSameAsAddress').checked ,
            "isResidualSecurities": document.getElementById('FNSecurityRes').checked,
            "userId": "acmiil"

        },

        {
            "clientNomineeId": 0,
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 77,
            "title": "",
            "nomineeFirstName": document.getElementById('txtSecondNomineeName').value,
            "nomineeMiddleName": "",
            "nomineeLastName": document.getElementById('txtSecondNomineeLastName').value,
            "dobNominee": document.getElementById('SecNomineeDOB').value,
            "isMinor": false,
            "relationshipType": parseInt(SRelationType),
            "fileName": "",
            "filePath": "",
            "fileName2": "",
            "filePath2": "",
            "address1": SNAddress1,
            "address2": SNAddress2,
            "address3": SNAddress3,
            "pincode": document.getElementById('SNPincode').value,
            "cityCode": parseInt(document.getElementById('ddlSecNomineeCity').value),
            "stateCode": parseInt(document.getElementById('SNState').value),
            "countryCode": parseInt(document.getElementById('SNCountry').value),
            "percentageAllocate": document.getElementById('SecPerAllocation').value,
            "sameAsAddress": document.getElementById('SNSameAsAddress').checked,
            "isResidualSecurities": document.getElementById('SNSecurityRes').checked,
            "userId": "acmiil"
        },
        {
            "clientNomineeId": 0,
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 78,
            "title": "",
            "nomineeFirstName": document.getElementById('txtThirdNomineeName').value,
            "nomineeMiddleName": "",
            "nomineeLastName": document.getElementById('txtThirdNomineeLastName').value,
            "dobNominee": document.getElementById('ThirdNomineeDOB').value,
            "isMinor": false,
            "relationshipType": parseInt(TRelationType),
            "fileName": "",
            "filePath": " ",
            "fileName2": "",
            "filePath2": "",
            "address1": TNAddress1,
            "address2": TNAddress2,
            "address3": TNAddress3,
            "pincode": document.getElementById('TNPincode').value,
            "cityCode": parseInt(document.getElementById('ddlthNomineeCity').value),
            "stateCode": parseInt(document.getElementById('TNState').value),
            "countryCode": parseInt(document.getElementById('TNCountry').value),
            "percentageAllocate": document.getElementById('ThirdPerAllocation').value,
            "sameAsAddress": document.getElementById('TNSameAsAddress').checked,
            "isResidualSecurities": document.getElementById('TNSecurityRes').checked,
            "userId": "acmiil"
        }
    ]
    console.log(NomineeJson);
    $.ajax({
        url: CommonAPIURL + "api/ClientNominee/InsertOrUpdateClientNomineeUploadDocuments",
        type: 'POST',
        data: JSON.stringify(NomineeJson),
        contentType: "application/json",
        success: function (data) {
            if (data.message == "Success") {
                
                var Saveguardian = getAge(document.getElementById('txtNomineeDOB').value)
                if (Saveguardian < 18 && Saveguardian != "NaN") {
                    SaveFirstGuadianDetails();

                }
                
                if (secondgardiansave == false) {  // second gauardian save func change false to true
                    var Secguardian = getAge(document.getElementById('SecNomineeDOB').value);
                    if (Secguardian < 18 && Secguardian != "NaN") {

                        SaveSeconGuardian();
                    }
                }
                if (thirdgardiansave == false) {  // third gauardian save func change false to true
                    var Thrdguardian = getAge(document.getElementById('ThirdNomineeDOB').value);
                    if (Thrdguardian < 18 && Thrdguardian != "NaN") {

                        SaveThirdGuardian();
                    }
                }
                
                SavePageDetails($("#RegistrationId").val(), CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId);
                window.location.assign(CommonPageURL + "Selfie/SelfieView?encregistrationId=" + EncRegistrationId);
            }
            else {

            }

        },
        error: function (data) {

        }
    });

}
*/
function SaveFirstGuadianDetails() {
    var GRelationType = $('input[name="btnGRelation"]:checked').val();
    var FGuardian = document.getElementById('GuardianAddress').value;
    var FGAddress1 = FGuardian.substring(0, 50);
    var FGAddress2 = FGuardian.substring(50, 100);
    var FGAddress3 = FGuardian.substring(100, 150);

    var GuardianJSON = [
        {
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 76,
            "guardianFirstName": document.getElementById('GuardianName').value,
            "guardianMiddleName": "",
            "guardianLastName": document.getElementById('GLastName').value,
            "address1": FGAddress1,
            "address2": FGAddress2,
            "address3": FGAddress3,
            "fileName": "",
            "filePath": "",
            "pincode": document.getElementById('FGPincode').value,
            "cityCode": parseInt(document.getElementById('FGCity').value),
            "stateCode": parseInt(document.getElementById('FGState').value),
            "countryCode": parseInt(document.getElementById('FGCountry').value),
            "guardianRelationshipType": parseInt(GRelationType),
            "entryBy": "acmiil",
            //"proffType": parseInt(document.getElementById('ddlFirstGuardianProffType').value)
        }
    ]
    console.log(JSON.stringify(GuardianJSON));
    $.ajax({
        url: CommonAPIURL + "api/ClientNominee/InsertOrUpdateClientGuardianDetails",
        type: 'POST',
        data: JSON.stringify(GuardianJSON),
        contentType: "application/json",
        success: function (data) {
            //console.log(data);
            if (data.message == "Success")
            {
                var Secguardian = getAge(document.getElementById('SecNomineeDOB').value);
                if (Secguardian < 18 && Secguardian != "NaN") {

                    SaveSeconGuardian();
                }

            }
            return;
        },
        error: function (data) {
           
        }
    });
}

function SaveSeconGuardian() { 
    var SGRelationType = $('input[name="btnSGRelation"]:checked').val();
    var SGuardian = document.getElementById('SGuardianAddress').value;
    var SGAddress1 = SGuardian.substring(0, 50);
    var SGAddress2 = SGuardian.substring(50, 100);
    var SGAddress3 = SGuardian.substring(100, 150);
    var GuardianJSON = [
        {
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 77,
            "guardianFirstName": document.getElementById('SGuardianName').value,
            "guardianMiddleName": "",
            "guardianLastName": document.getElementById('SGLastName').value,
            "address1": SGAddress1,
            "address2": SGAddress2,
            "address3": SGAddress3,
            "fileName": "",
            "filePath": "",
            "pincode": document.getElementById('SGPincode').value,
            "cityCode": parseInt(document.getElementById('SGCity').value),
            "stateCode": parseInt(document.getElementById('SGState').value),
            "countryCode": parseInt(document.getElementById('SGCountry').value),
            "guardianRelationshipType": parseInt(SGRelationType),
            "entryBy": "acmiil",
            //"proffType": parseInt(document.getElementById('ddlFirstGuardianProffType').value)
        }
    ]
    console.log(JSON.stringify(GuardianJSON));
    $.ajax({
        url: CommonAPIURL + "api/ClientNominee/InsertOrUpdateClientGuardianDetails",
        type: 'POST',
        data: JSON.stringify(GuardianJSON),
        contentType: "application/json",
        success: function (data) {

            //console.log(data);
            if (data.message == "Success") {
                secondgardiansave = true; 
                var Thrdguardian = getAge(document.getElementById('ThirdNomineeDOB').value);
                if (Thrdguardian < 18 && Thrdguardian != "NaN") {

                    SaveThirdGuardian();
                }
            }
            return;
        },
        error: function (data) {
            
        }
    });
}

function SaveThirdGuardian() {
    var SGRelationType = $('input[name="btnTGRelation"]:checked').val();
    TGuardianAddress
    var TGuardian = document.getElementById('TGuardianAddress').value;
    var TGAddress1 = TGuardian.substring(0, 50);
    var TGAddress2 = TGuardian.substring(50, 100);
    var TGAddress3 = TGuardian.substring(100, 150);
    var GuardianJSON = [
        {
            "registrationId": $("#RegistrationId").val(),
            "nomineeType": 78,
            "guardianFirstName": document.getElementById('TGuardianName').value,
            "guardianMiddleName": "",
            "guardianLastName": document.getElementById('TGLastName').value,
            "address1": TGAddress1,
            "address2": TGAddress2,
            "address3": TGAddress3,
            "fileName": "",
            "filePath": "",
            "pincode": document.getElementById('TGPincode').value,
            "cityCode": parseInt(document.getElementById('TGCity').value),
            "stateCode": parseInt(document.getElementById('TGState').value),
            "countryCode": parseInt(document.getElementById('TGCountry').value),
            "guardianRelationshipType": parseInt(SGRelationType),
            "entryBy": "acmiil",
            //"proffType": parseInt(document.getElementById('ddlFirstGuardianProffType').value)
        }
    ]
    console.log(JSON.stringify(GuardianJSON));
    $.ajax({
        url: CommonAPIURL + "api/ClientNominee/InsertOrUpdateClientGuardianDetails",
        type: 'POST',
        data: JSON.stringify(GuardianJSON),
        contentType: "application/json",
        success: function (data) {
            //console.log(data);
            if (data.message == "Success") {

                thirdgardiansave = true;
            }
            return;
        },
        error: function (data) {
           
        }
    });
}
function GuardianFirstPerAddressDetails() {
    if (document.getElementById('GAddressCheck').checked == true) {
        $.ajax({
            url: CommonAPIURL + "api/ClientPersonalDetails/GetClientPerAddressDetails",
            type: 'GET',
            data: {
                RegistrationId: $("#RegistrationId").val() //parseInt(document.getElementById('MainRegistrationId').value)
            },
            contentType: 'application/json',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var GPermanentAddress = data.perAddress1 + ' ' + data.perAddress2 + ' ' + data.perAddress3;
                $('#GuardianAddress').val(GPermanentAddress);

                $('#FGPincode').val(data.perPincode);

                var appenddata1 = "<option value = '" + data.perCityCode + " '>" + data.perCity + " </option>";
                $("#FGCity").append(appenddata1);

                var StateMaster = "<option value = '" + data.perStateCode + " '>" + data.perState + " </option>";
                $("#FGState").append(StateMaster);

                CountryMaster = "<option value = '" + data.perCountryCode + " '>" + data.perCountry + " </option>";
                $("#FGCountry").append(CountryMaster);

            },
            error: function (data) {
                
                return;
            }
        });
    }
    else {
        if (document.getElementById('GAddressCheck').checked != true) {
            $('#GuardianAddress').val('');
            $('#FGPincode').val('');
            $('#FGState').val('');
            $('#FGCity').val('');
            $('#FGCountry').val('');
        }
    }
}

function SecGPerAddressDetails() {
    if (document.getElementById('SGAddressCheck').checked == true) {
        $.ajax({
            url: CommonAPIURL + "api/ClientPersonalDetails/GetClientPerAddressDetails",
            type: 'GET',
            data: {
                RegistrationId: $("#RegistrationId").val() //parseInt(document.getElementById('MainRegistrationId').value)
            },
            contentType: 'application/json',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var GPermanentAddress = data.perAddress1 + ' ' + data.perAddress2 + ' ' + data.perAddress3;
                $('#SGuardianAddress').val(GPermanentAddress);

                $('#SGPincode').val(data.perPincode);

                var appenddata1 = "<option value = '" + data.perCityCode + " '>" + data.perCity + " </option>";
                $("#SGCity").append(appenddata1);

                var StateMaster = "<option value = '" + data.perStateCode + " '>" + data.perState + " </option>";
                $("#SGState").append(StateMaster);

                CountryMaster = "<option value = '" + data.perCountryCode + " '>" + data.perCountry + " </option>";
                $("#SGCountry").append(CountryMaster);

            },
            error: function (data) {
                
                return;
            }
        });
    }
    else {
        if (document.getElementById('FNSameAsAddress').checked != true) {
            $('#SGuardianAddress').val('');
            $('#SGPincode').val('');
            $('#SGCity').val('');
            $('#SGState').val('');
            $('#SGCountry').val('');
        }
    }
}

function ThdGPerAddressDetails() {
    if (document.getElementById('TGAddressCheck').checked == true) {
        $.ajax({
            url: CommonAPIURL + "api/ClientPersonalDetails/GetClientPerAddressDetails",
            type: 'GET',
            data: {
                RegistrationId: $("#RegistrationId").val() //parseInt(document.getElementById('MainRegistrationId').value)
            },
            contentType: 'application/json',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var GPermanentAddress = data.perAddress1 + ' ' + data.perAddress2 + ' ' + data.perAddress3;
                $('#TGuardianAddress').val(GPermanentAddress);

                $('#TGPincode').val(data.perPincode);

                var appenddata1 = "<option value = '" + data.perCityCode + " '>" + data.perCity + " </option>";
                $("#TGCity").append(appenddata1);

                var StateMaster = "<option value = '" + data.perStateCode + " '>" + data.perState + " </option>";
                $("#TGState").append(StateMaster);

                CountryMaster = "<option value = '" + data.perCountryCode + " '>" + data.perCountry + " </option>";
                $("#TGCountry").append(CountryMaster);

            },
            error: function (data) {
               
                return;
            }
        });
    }
    else {
        if (document.getElementById('TGAddressCheck').checked != true) {
            $('#TGuardianAddress').val('');
            $('#TGPincode').val('');
            $('#TGCity').val('');
            $('#TGState').val('');
            $('#TGCountry').val('');
        }
    }
}
function OpenSecondNominee() {
    var checkfirstnominee = FirstNomineeValidation();
    if (checkfirstnominee == false) {
        return;
    }
    Saveguardian = getAge(document.getElementById('txtNomineeDOB').value)
    if (Saveguardian < 18 && Saveguardian != "NaN") {
        var guardianfirst = FirstGuardianvalidation();
        if (guardianfirst == false) {
            return;
        }

    }  

    document.getElementById('OpenSecondN').style.display = "block";
    document.getElementById('btnsecnom').style.display = "none";
    document.getElementById('btnSndNominee').style.display = "none";

}
function OpenThirdNominee() {

    var checkfirstnominee = FirstNomineeValidation();
    if (checkfirstnominee == false) {
        return;
    }
    Saveguardian = getAge(document.getElementById('txtNomineeDOB').value)
    if (Saveguardian < 18 && Saveguardian != "NaN") {
        var guardianfirst = FirstGuardianvalidation();
        if (guardianfirst == false) {
            return;
        }

    }  

    var checksecondnominee = SecondNomineeValidation();
    if (checksecondnominee == false) {
        return;
    }
    var Secguardian = getAge(document.getElementById('SecNomineeDOB').value);
    if (Secguardian < 18 && Secguardian != "NaN") {
        var guardiansecond = secondGuardianvalidation();
        if (guardiansecond == false) {
            return;
        }
    }

    document.getElementById('OpenThirdN').style.display = "block";
    document.getElementById('btnthrdnomi').style.display = "none";
    document.getElementById('btnthdNominee').style.display = "none";
}

function FirstNomineeValidation() {
    if (document.getElementById('txtFristNomineeName').value == null || document.getElementById('txtFristNomineeName').value == "") {       
        showMessage('Please Enter First Nominee Name', 5000, 'red');
        return false;
    }
    if (document.getElementById('txtNomineeDOB').value == null || document.getElementById('txtNomineeDOB').value == "") {
        showMessage("Please Enter First Nominee Date Of Birth", 5000, 'red');
        return false;
    }
    if (document.getElementById('PercentageAllocation').value == null || document.getElementById('PercentageAllocation').value == "") {
        showMessage("Please Enter First Nominee Percentage", 5000, 'red');
        return false;
    }
    if (document.getElementById('FNAddressDetails').value == null || document.getElementById('FNAddressDetails').value == "") {
        showMessage("Please Enter First Nominee AddressDetails", 5000, 'red');
        return false;
    }
    if (document.getElementById('FNPincode').value == null || document.getElementById('FNPincode').value == "") {
        showMessage("Please Enter First Nominee Pincode", 5000, 'red');
        return false;
    }
    if (document.getElementById('ddlFirstNomineeCity').value == null || document.getElementById('ddlFirstNomineeCity').value == "") {
        showMessage("Please Enter First Nominee City", 5000, 'red');
        return false;
    }
    if (document.getElementById('FNState').value == null || document.getElementById('FNState').value == "") {
        showMessage("Please Enter First Nominee State", 5000, 'red');
        return false;
    }
    if (document.getElementById('FNCountry').value == null || document.getElementById('FNCountry').value == "") {
        showMessage("Please Enter First Nominee Country", 5000, 'red');
        return false;
    }


}

function SecondNomineeValidation() {

    if (document.getElementById('OpenSecondN').style.display == "block") {
        if (document.getElementById('txtSecondNomineeName').value == null || document.getElementById('txtSecondNomineeName').value == "") {
            showDynamicMessage('Please Enter Second Nominee Name', 5000, 'red', 'SDynamicalert');

            return false;
        }
        if (document.getElementById('SecNomineeDOB').value == null || document.getElementById('SecNomineeDOB').value == "") {
            showDynamicMessage('Please Enter Second Nominee DOB', 5000, 'red', 'SDynamicalert');
            return false;
        }
        if (document.getElementById('SecPerAllocation').value == null || document.getElementById('SecPerAllocation').value == "") {
            showDynamicMessage('Please Enter Second Nominee Percentage', 5000, 'red', 'SDynamicalert');
            return false;
        }
        if (document.getElementById('SNAddressDetails').value == null || document.getElementById('SNAddressDetails').value == "") {
            showDynamicMessage('Please Enter Second Nominee Address', 5000, 'red', 'SDynamicalert' );
            return false;
        }
        if (document.getElementById('SNPincode').value == null || document.getElementById('SNPincode').value == "") {
            showDynamicMessage('Please Enter Second Nominee Pincode', 5000, 'red', 'SDynamicalert');
            return false;
        }
        if (document.getElementById('ddlSecNomineeCity').value == null || document.getElementById('ddlSecNomineeCity').value == "") {
            showDynamicMessage('Please Enter Second Nominee City', 5000, 'red', 'SDynamicalert');
            return false;
        }
        if (document.getElementById('SNState').value == null || document.getElementById('SNState').value == "") {
            showDynamicMessage('Please Enter Second Nominee State', 5000, 'red', 'SDynamicalert');
            return false;
        }
        if (document.getElementById('SNCountry').value == null || document.getElementById('SNCountry').value == "") {
            showDynamicMessage('Please Enter Second Nominee Country', 5000, 'red', 'SDynamicalert');
            return false;
        }
    }
}

function ThirdNomineeValidation() {

    //check Validation On Save Button
    if (document.getElementById('OpenThirdN').style.display == "block") {
        if (document.getElementById('txtThirdNomineeName').value == null || document.getElementById('txtThirdNomineeName').value == "") {
            showDynamicMessage('Please Enter Third Nominee Name', 5000, 'red','TDynamicalert');
            return false;
        }
        if (document.getElementById('ThirdNomineeDOB').value == null || document.getElementById('ThirdNomineeDOB').value == "") {
            showDynamicMessage('Please Enter Third Nominee DOB', 5000, 'red', 'TDynamicalert');
            return false;
        }
        if (document.getElementById('ThirdPerAllocation').value == null || document.getElementById('ThirdPerAllocation').value == "") {
            showDynamicMessage('Please Enter Third Nominee Percentage', 5000, 'red', 'TDynamicalert');
            return false;
        }
        if (document.getElementById('TNAddressDetails').value == null || document.getElementById('TNAddressDetails').value == "") {
            showDynamicMessage('Please Enter Third Nominee Address', 5000, 'red', 'TDynamicalert');
            return false;
        }
        if (document.getElementById('TNPincode').value == null || document.getElementById('TNPincode').value == "") {
            showDynamicMessage('Please Enter Third Nominee Pincode', 5000, 'red', 'TDynamicalert');
            return false;
        }
        if (document.getElementById('ddlthNomineeCity').value == null || document.getElementById('ddlthNomineeCity').value == "") {
            showDynamicMessage('Please Enter Third Nominee City', 5000, 'red', 'TDynamicalert');
            return false;
        }
        if (document.getElementById('TNState').value == null || document.getElementById('TNState').value == "") {
            showDynamicMessage('Please Enter Third Nominee State', 5000, 'red', 'TDynamicalert');
            return false;
        }
        if (document.getElementById('TNCountry').value == null || document.getElementById('TNCountry').value == "") {
            showDynamicMessage('Please Enter Third Nominee Country', 5000, 'red', 'TDynamicalert');
            return false;
        }
    }
}

function FirstGuardianvalidation()
{
    if (document.getElementById('GuardianName').value == null || document.getElementById('GuardianName').value == "") {
        showMessage('Please Enter First Guardian Name', 5000, 'red');
        return false;
    }
    if (document.getElementById('GuardianAddress').value == null || document.getElementById('GuardianAddress').value == "") {
        showMessage('Please Enter First Guardian Address', 5000, 'red');
        return false;
    }
    if (document.getElementById('FGPincode').value == null || document.getElementById('FGPincode').value == "") {
        showMessage('Please Enter First Guardian Pincode', 5000, 'red');
        return false;
    }
    if (document.getElementById('FGCity').value == null || document.getElementById('FGCity').value == "") {
        showMessage('Please Enter First Guardian City', 5000, 'red');
        return false;
    }
    if (document.getElementById('FGState').value == null || document.getElementById('FGState').value == "") {
        showMessage('Please Enter First Guardian State', 5000, 'red');
        return false;
    }
    if (document.getElementById('FGCountry').value == null || document.getElementById('FGCountry').value == "") {
        showMessage('Please Enter First Guardian Country', 5000, 'red');
        return false;
    }

}

function secondGuardianvalidation()
{
    if (document.getElementById('SGuardianName').value == null || document.getElementById('SGuardianName').value == "") {
        showDynamicMessage('Please Enter Second Guardian Name', 5000, 'red','SDynamicalert');
        return false;
    }
    if (document.getElementById('SGuardianAddress').value == null || document.getElementById('SGuardianAddress').value == "") {
        showDynamicMessage('Please Enter Second Guardian Address', 5000, 'red', 'SDynamicalert');
        return false;
    }
    if (document.getElementById('SGPincode').value == null || document.getElementById('SGPincode').value == "") {
        showDynamicMessage('Please Enter Second Guardian Pincode', 5000, 'red', 'SDynamicalert');
        return false;
    }
    if (document.getElementById('SGCity').value == null || document.getElementById('SGCity').value == "") {
        showDynamicMessage('Please Enter Second Guardian City', 5000, 'red', 'SDynamicalert');
        return false;
    }
    if (document.getElementById('SGState').value == null || document.getElementById('SGState').value == "") {
        showDynamicMessage('Please Enter Second Guardian State', 5000, 'red', 'SDynamicalert');
        return false;
    }
    if (document.getElementById('SNCountry').value == null || document.getElementById('SNCountry').value == "") {
        showDynamicMessage('Please Enter Second Guardian Country', 5000, 'red', 'SDynamicalert');
        return false;
    }
}

function thirdguardianvalidation()
{
    if (document.getElementById('TGuardianName').value == null || document.getElementById('TGuardianName').value == "") {
        showDynamicMessage('Please Enter Third Guardian Name', 5000, 'red','TDynamicalert');
        return false;
    }
    if (document.getElementById('TGuardianAddress').value == null || document.getElementById('TGuardianAddress').value == "") {
        showDynamicMessage('Please Enter Third Guardian Address', 5000, 'red', 'TDynamicalert');
        return false;
    }
    if (document.getElementById('TGPincode').value == null || document.getElementById('TGPincode').value == "") {
        showDynamicMessage('Please Enter Third Guardian Pincode', 5000, 'red', 'TDynamicalert');
        return false;
    }
    if (document.getElementById('TGCity').value == null || document.getElementById('TGCity').value == "") {
        showDynamicMessage('Please Enter Third Guardian City', 5000, 'red', 'TDynamicalert');
        return false;
    }
    if (document.getElementById('TGState').value == null || document.getElementById('TGState').value == "") {
        showDynamicMessage('Please Enter Third Guardian State', 5000, 'red', 'TDynamicalert');
        return false;
    }
    if (document.getElementById('TGCountry').value == null || document.getElementById('TGCountry').value == "") {
        showDynamicMessage('Please Enter Third Guardian Country', 5000, 'red', 'TDynamicalert');
        return false;
    }

}

function CloseSecondNominee() {

    document.getElementById('OpenSecondN').style.display = "none";
    document.getElementById('OpenThirdN').style.display = "none";
    document.getElementById('btnsecnom').style.display = "block";
    document.getElementById('btnSndNominee').style.display = "block";
    document.getElementById('txtSecondNomineeName').value = " ";
    document.getElementById('txtSecondNomineeLastName').value = " ";
    document.getElementById('SecNomineeDOB').value = " ";
    document.getElementById('SecPerAllocation').value = " ";
    document.getElementById('SNAddressDetails').value = " ";
    document.getElementById('SNPincode').value = " ";
    document.getElementById('ddlSecNomineeCity').value = " ";
    document.getElementById('SNState').value = " ";
    document.getElementById('SNCountry').value = " ";

}
function ThirdCLoseNominee() {
    document.getElementById('OpenThirdN').style.display = "none";
    document.getElementById('btnthdNominee').style.display = "block";
    document.getElementById('btnthrdnomi').style.display = "block";
    document.getElementById('txtThirdNomineeName').value = " ";
    document.getElementById('txtThirdNomineeLastName').value = " ";
    document.getElementById('ThirdNomineeDOB').value = " ";
    document.getElementById('ThirdPerAllocation').value = " ";
    document.getElementById('TNAddressDetails').value = " ";
    document.getElementById('TNPincode').value = " ";
    document.getElementById('ddlthNomineeCity').value = " ";
    document.getElementById('TNState').value = " ";
    document.getElementById('TNCountry').value = " ";
}
function SNSecurityRes()
{   
 document.getElementById('SNSecurityRes').checked = true;
 //document.getElementById('FNSecurityRes').checked = false;
 //document.getElementById('TNSecurityRes').checked = false;   
}

function FNSecurityRes()
{
  document.getElementById('FNSecurityRes').checked = true
 // document.getElementById('SNSecurityRes').checked = false;
  //document.getElementById('TNSecurityRes').checked = false;  
}

function TNSecurityRes()
{ 
   document.getElementById('TNSecurityRes').checked = true
  // document.getElementById('SNSecurityRes').checked = false;
  // document.getElementById('FNSecurityRes').checked = false;
}
