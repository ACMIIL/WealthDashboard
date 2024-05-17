$(document).ready(function () {
    $("#Personal").removeClass('a');
    $("#Personal").addClass('b');
    CentralizeGetDigioResponseData();
})

let timeInSeconds = 60;
var IsEmailVarified = false;

function OpenDigiLockerAPI() {
    var registartionid = $("#RegistrationId").val();
    var EncRegistrationId = $("#EncRegistrationId").val();
    var EncUCC = $("#EncUCC").val();
    var EncBaCode = $("#EncBACode").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Digio/CentralizeDigioWorkTemplate",
        data: {
            customer_identifier: $("#hiddenmobileno").val(),
            template_name: "ACM_KYC_WORKFLOW", //uat  -- "ACM_KYC_WORKFLOW" -PRd
            RegistrationId: registartionid
        },
        success: function (DigiLockerTemplate) {
            console.log(DigiLockerTemplate);
            var options = {
                environment: "production",
                callback: function (response) {
                    console.log(response);

                    if (response.message == "KYC Process Completed.") {
                        localStorage.setItem("Digiomessage", "Yes");
                        CentralizeGetDigioResponseData();
                        // window.location.assign(CommonPageURL + "Nominee/Nominee_Details" + '?encregi=' + "sLa6rsSqFy8");

                    }
                    else {
                        alert(" not done try again");
                        //window.location.assign(CommonPageURL + "Selfie/SelfieView" + '?encucc=' + encucc + "");
                    }
                    console.log("Signing completed successfully");
                },
                logo: "https://web.investmentz.com/imgupload/Stamp/fav-icon.png",
                theme: {
                    primaryColor: "#AB3498",
                    secondaryColor: "#000000"
                }
            }
            var digio = new Digio(options);
            var reqParam = DigiLockerTemplate.data.id + "/" + DigiLockerTemplate.data.reference_id + "/" + DigiLockerTemplate.data.customer_identifier;
            var ResposnseURL = "https://app.digio.in/#/gateway/login/" + reqParam + "?redirect_url=" + CommonPageURL + "Home/PAN_Details?encucc=" + EncUCC + "&rc_code=" + EncBaCode +"&status=success&digio_doc_id=" + DigiLockerTemplate.data.id + "&message=Success";
            console.log(ResposnseURL);
            ResposnseURL = ResposnseURL.toString().replace("?status", "&status");
            window.location.assign(ResposnseURL);
        },
        error: function (DigiLockerTemplate) {
            console.log("Error");
            return false;
        }
    });
}

var currentDate = new Date();
var eighteenYearsAgo = new Date(currentDate.getFullYear() - 18, currentDate.getMonth(), currentDate.getDate());
var today = eighteenYearsAgo.toISOString().split('T')[0];
$('#PanDob').attr('max', today);

function getpersonaldetailfromdgio() {
   
    var registartionid = $("#RegistrationId").val();
    $.ajax({
        type: "GET",
        url: CommonAPIURL + "api/ClientPersonalDetails/PersonalDetailsFromDigio",
        data: {
            Rid: parseInt(registartionid)
        },
        contentType: "application/json",
        success: function (data) {
            if (data.code == 200) {

                $("#txtPAN").val(data.data.paN_PANNO);

                $("#hiddenpanname").val(data.data.paN_NAME);
                $("#flush-collapseTwo").show();
                $("#hiddengender").val(data.data.paN_GENDER);
                if (data.data.paN_GENDER == "M") {
                    $('#btnradio1Male').attr('checked', 'checked');
                }
                else if (data.data.paN_GENDER == "F") {
                    $('#btnradioFemale').attr('checked', 'checked');
                }
                else {
                    $('#btnradioOthers').attr('checked', 'checked');
                }
                var dob = data.data.adhaR_DOB;
                const datePattern = /^\d{1,2} [a-zA-Z]{3} \d{4}$/;
                const datePattern1 = /^\d{2}-\d{2}-\d{4}$/;

                if (datePattern.test(dob)) {
                    const parsedDate = new Date(dob);
                    const year = parsedDate.getFullYear();
                    const month = String(parsedDate.getMonth() + 1).padStart(2, '0');
                    const day = String(parsedDate.getDate()).padStart(2, '0');
                    const formattedDate = `${year}-${month}-${day}`;
                    $("#PanDob").val(formattedDate);
                }
                else if (datePattern1.test(dob)) {
                    const parts = dob.split('/');
                    const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
                    $("#PanDob").val(formattedDate);
                }
                else {
                    const parts = dob.split('/');
                    const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
                    $("#PanDob").val(formattedDate);
                }
                $("#hiddenAdharno").val(data.data.adhaR_NO);
                $("#hiddenpermanantaddress").val(data.data.permanantaddress);
                $("#hiddendist").val(data.data.permanaT_DIST);
                $("#hiddenstate").val(data.data.permananT_STATE);
                $("#hiddenpincode").val(data.data.permananT_PINCODE);
                //$("#hiddenmobileno").val(data.data.mobileno);

                CheckPan();

            }
        },
        error: function (data) {

        }
    })
}

function blankfield() {
    $("#txtPAN").val('');
    $("#hiddenpanname").val('');
    $("#hiddengender").val('');
    $("#PanDob").val('');
    $("#hiddenAdharno").val('');
    $("#hiddenpermanantaddress").val('');
    $("#hiddendist").val('');
    $("#hiddenstate").val('');
    $("#hiddenpincode").val('');
    //$("#hiddenmobileno").val('');
}

function PanandPersonalSubmit() {
    var checkvalid = PanAndPersonalPageValidation();
    if (checkvalid != false) {
        showMessage('Please wait....', 40000, 'green');
        var panno = $("#txtPAN").val();
        var name = $("#hiddenpanname").val();
        var gender = $("#hiddengender").val();
        var dob = $("#PanDob").val();
        var adharno = $("#hiddenAdharno").val();
        if (adharno === "" || adharno === null) {
            showMessage('Please complete digilocker journey', 5000, 'red');
            //alert("digio process is incomplete or Aadhar number is not fetch from  digio please enter the aadhar number");
            return;
        }
        var address = $("#hiddenpermanantaddress").val();
        var distorcity = $("#hiddendist").val();
        var state = $("#hiddenstate").val();
        var pincode = $("#hiddenpincode").val();
        var mobile = $("#hiddenmobileno").val();

        var networth = $("#txtNetworth").val();
        var Fatherfirstspouce = $("#txtFatherspouce").val();
        var FatherMiddlespouce = "";// $("#txtFathermiddlespouce").val();
        var FatherLastspouce = ""//$("#txtFatherLastspouce").val();
        var Emailid = $("#txtEmailid").val();
        var otpno = $("#txtotpno").val();

        var maritalstatus = $('input[name="inlineRadioOptions1"]:checked').val();
        var Gender = $('input[name="inlineRadioOptions2"]:checked').val();
        var Relation = $('input[name="inlineRadioOptions3"]:checked').val();
        var AnnualIncome = $('input[name="inlineRadioOptions5"]:checked').val();
        var Investmentexperiance = $('input[name="inlineRadioOptions6"]:checked').val();
        //var EducationalQualification = $('input[name="inlineRadioOptions7"]:checked').val();
        var Occupation = $('input[name="inlineRadioOptions8"]:checked').val();

        var splitname = name.split(' ');
        var firstname = splitname[0];
        var middlename = splitname[1];
        var Lastname = splitname[2];
        var title = Gender;
        var registartionid = $("#RegistrationId").val();
        var EncRegistrationId = $("#EncRegistrationId").val();
        $.ajax({
            type: "POST",
            url: CommonPageURL + "Digio/InsertPersonalDetails",
            data: {
                PersonalDetailsId: 0,
                RegistrationId: registartionid,
                Title: parseInt(title),
                Bacode: "RC5555",
                ClientFullName: name,
                ClientFirstName: firstname,
                ClientMiddleName: middlename,
                ClientLastName: Lastname,
                ClientMotherName: "",
                ClientCategoryId: 1,
                ClientHolderId: 1,
                DateOfBirth: dob,
                PAN: panno,
                UID: adharno,
                Gender: parseInt(Gender),
                Education: parseInt(1),
                ClientsRelationId: 0,
                MaritalStatus: parseInt(maritalstatus),
                OccupationType: parseInt(Occupation),
                Telephone1: mobile,
                EmailId: Emailid,
                MobileNo: mobile,
                UserId: "ACMIIL",
                Pincode: pincode,
                Address: address,
                investmentExperienceId: parseInt(Investmentexperiance),
                annualIncomeId: parseInt(AnnualIncome),
                networth: parseInt(networth),
                fatherfirstname: Fatherfirstspouce,
                fathermiddlename: FatherMiddlespouce,
                fatherlastname: FatherLastspouce
            },
            success: function (data) {
                console.log(data);
                //alert("Details saved successfully.");
                if (data == "Ok") {
                    showMessage('Details saved successfully.', 5000, 'green');
                    SavePageDetails(registartionid, CommonPageURL + "Home/Bank_Details?encregistrationId=" + EncRegistrationId);
                    window.location.assign(CommonPageURL + "Home/Bank_Details?encregistrationId=" + EncRegistrationId);
                }
                else {
                    console.log("Digio/InsertPersonalDetails insert time error");
                }
            },
            error: function (data) {

            }
        })

    }
}

function CheckParents() {
    //document.getElementById('Selflabel').style.display = "none";
    //document.getElementById('spouselabel').style.display = "none";
    var Declaration = $('input[name="btnDeclaration"]:checked').val();
    var mobile = $("#hiddenmobileno").val();
    var Email = document.getElementById('txtEmailid').value;
    var status = emailvalidate(Email);
    if (status == true) {
        $.ajax({
            type: "GET",
            url: CommonPageURL + "Login/CheckemailDeclarationWithUCC",
            data: {
                email: Email,
                mobile: mobile,
                DeclarationType: Declaration
            },
            success: function (Emaildata) {
                var Message = Emaildata.errorMessage;
                if (Message == 'Mobile No self is already present ') {
                    document.getElementById('Selflabel').style.display = "block";
                }
                else {
                    //SendMobileOtp();
                }

            },
            error: function () {

            }
        });
    }
}

function CheckEmailDeclarationWithUCC() {
    $('.msghide').hide();
    //document.getElementById('spouselabel').style.display = "none";
    var Declaration = $('input[name="btnDeclaration"]:checked').val();
    var mobile = $("#hiddenmobileno").val();
    var Email = document.getElementById('txtEmailid').value;
    var status = emailvalidate(Email);
    if (status == true) {

        $.ajax({
            type: "GET",
            url: CommonPageURL + "Login/SendOtpToEmail",
            data: {
                email: Email,
                mobile: mobile
            },
            success: function (Emaildata) {
                if (Emaildata == "Send OTP") {
                    document.getElementById('otpsendemail').style.display = "block";
                    updateTimer();
                    return;
                }
                else {

                }

            },
            error: function () {

            }
        });
    }
}

function CheckOTP() {
    var otpid = document.getElementById('txtotpno').value;
    if (otpid.length == 6) {
        $('.msghide').hide();
        var MobileOTP = document.getElementById('txtotpno').value;
        var EmailId = document.getElementById('txtEmailid').value;
        var mobile = $("#hiddenmobileno").val();
        var Declaration = $('input[name="btnDeclaration"]:checked').val();
        $.ajax({
            type: 'POST',
            url: CommonPageURL + "Login/CheckOTPemail",
            data: {
                EmailId: EmailId,
                MobileNo: mobile,
                DeclarationType: Declaration,
                UCC: parseInt($("#UCC").val()),
                enterotp: otpid
            },
            success: function (data) {

                //if (data.errorMessage == "Email Id self is already present ") {
                //    document.getElementById('Selflabel').style.display = "block";

                //}
                //else if (data.errorMessage == "Spouse Present") {

                //    document.getElementById('spouselabel').style.display = "block";
                //    //$('.emaispouce').prop('disabled', true);
                //    // = true;
                //    return;
                //}else
                 if (data.errorMessage == "Incorrect Otp") {
                    document.getElementById('invalid').style.display = "block";
                    //IsEmailVarified = false;
                    return;
                }
                else {
                    document.getElementById('verify').style.display = "block";
                    $('.emaispouce').prop('disabled', true);
                    IsEmailVarified = true;
                    document.getElementById('reentereotpdiv').style.display = "none";
                    document.getElementById('timer').style.display = "none";
                    return;
                }
            },
            error: function (response, error) {
            }
        });
    } else {

    }

}

function SaveTempUCCData() {
    var MobileNo = document.getElementById('txtMobileNumber').value;
    $.ajax({
        type: "POST",
        url: CommonPageURL + "Login/save_ucc_temp_details",
        data:
        {
            EmailId: "",
            MobileNo: MobileNo,
            BACode: "",
            Source: "",
            EmailRelation: "118",
            MobileRelation: $('input[name="btnAnnualIncome"]:checked').val(),
            EmailUCC: "",
            MobileUCC: "",
            EmployeeRef: "",
            EmployeeRefID: ""
        },
        success: function (data) {
            if (data.ucc != null || data.ucc != "" || data.ucc != "undefined") {
                EncryptedUCC(data.ucc);
            }
        },
        error: function (data) {
            console.log("Error");
            return false;
        }
    });

}

function emailvalidate(Email) {
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!emailPattern.test(Email)) {
        $("#invalidEmail").show();
        return false;
    }
    else {
        /*$("#checkmark").show();*/
        $("#invalidEmail").hide();
        return true;
    }
}

function CentralizeGetDigioResponseData() {
    var UCCNo = $("#UCC").val();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Digio/CentralizeDigioResponseData",
        data: {
            ucc: UCCNo,
            RegistrationId: parseInt($("#RegistrationId").val())
        },
        contentType: "application/json",
        success: function (DigiLockerResponse) {
            console.log(DigiLockerResponse);

            if (DigiLockerResponse == "OK") {
                console.log(DigiLockerResponse);
                getpersonaldetailfromdgio();
                digioStatus = "Success";
            } else {
                console.log(DigiLockerResponse);
                digioStatus = "failed";
               // document.getElementById('divAadharDetails').style = "block";
            }
        },
        error: function (DigiLockerResponse) {
            digioStatus = "failed";
            alert("Exceptions");
            return false;
        }
    });
}

function getCVLKRADATA() {
    $("#checkCVLKRADataFailedlabel").hide();
    $.ajax({
        type: "GET",
        url: CommonPageURL + "CVLKRA/GetCVLDATA",
        data: {
            RegistrationId: $('#RegistrationId').val(),
            PAN: $('#txtPAN').val(),
            DOB: $('#PanDob').val()
        },
        contentType: "application/Json",
        success: function (data) {
            console.log(data);
            if (data != 'Failed Mobile Details Invalid') {
                if (data.panno != '') {
                    $("#txtPAN").val(data.panno);
                    $("#hiddenpanname").val(data.fullname);
                    $("#hiddengender").val(data.gender);
                    $("#txtFatherspouce").val(data.fatherspouce);
                    $("#txtEmailid").val(data.emailid.toLowerCase());
                    if (data.gender == "M") {
                        $('#btnradio1Male').attr('checked', 'checked');
                    }
                    else if (data.gender == "F") {
                        $('#btnradioFemale').attr('checked', 'checked');
                    }
                    else {
                        $('#btnradioOthers').attr('checked', 'checked');
                    }
                    var dob = data.dob;
                    const datePattern = /^\d{1,2} [a-zA-Z]{3} \d{4}$/;
                    const datePattern1 = /^\d{2}-\d{2}-\d{4}$/;
                    if (datePattern.test(dob)) {
                        const parsedDate = new Date(dob);
                        const year = parsedDate.getFullYear();
                        const month = String(parsedDate.getMonth() + 1).padStart(2, '0');
                        const day = String(parsedDate.getDate()).padStart(2, '0');
                        const formattedDate = `${year}-${month}-${day}`;
                        $("#PanDob").val(formattedDate);
                    }
                    else if (datePattern1.test(dob)) {
                        const parts = dob.split('-');
                        const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
                        $("#PanDob").val(formattedDate);
                    }
                    else {
                        const parts = dob.split('/');
                        const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
                        $("#PanDob").val(formattedDate);
                    }
                    //alert("aadhar" + data.adharno.toString());                    
                    $("#hiddenpermanantaddress").val(data.per_address1 + data.per_address2 + data.per_address3);
                    $("#hiddendist").val(data.per_distorcity);
                    $("#hiddenstate").val(data.per_state);
                    $("#hiddenpincode").val(data.per_pincode);
                    //var countrycode = "91";
                    //if (data.mobile.startsWith("91")) {
                    //    var phoneNumber = data.mobile.substring(countrycode.length);
                    //    $("#hiddenmobileno").val(phoneNumber);
                    //}
                    //else {
                    //    $("#hiddenmobileno").val(data.mobile);
                    //}
                    alert(data.adharno.toString());
                    if (data.adharno.toString() != "N" && data.adharno != "") {
                        $("#hiddenAdharno").val(data.adharno);
                        $("#checkCVLKRADatalabel").show();
                        $("#flush-collapseTwo").show();
                        //document.getElementById('divdigioButton').style.display = "none";
                    }
                    else {
                        $("#checkCVLKRADatalabel").hide();
                        $("#flush-collapseTwo").hide();
                        //document.getElementById('divdigioButton').style.display = "block";
                        $("#checkCVLKRAAadharPenidng").show();
                    }
                    document.getElementById("txtPAN").readOnly = true;
                    $("#checkCVLKRADataFailedlabel").hide();
                }
                if (data.emailid != '') {
                    CheckEmailDeclarationWithUCC();
                }

            }
            else {
                //alert("Response");
                //$("#txtPAN").val('');
                $("#checkCVLKRADataFailedlabel").show();
                $("#flush-collapseTwo").hide();
                $("#checkCVLKRADatalabel").hide();
                document.getElementById('divdigioButton').style.display = "block";
                //$("#PanDob").val(Date);
            }
        },
        error: function (data) {
            alert("Error : " + data);
        }
    })
}

function updateTimer() {
    const timerElement = document.getElementById('timer');
    timerElement.textContent = timeInSeconds;

    if (timeInSeconds > 0) {
        timeInSeconds--;
        setTimeout(updateTimer, 1000); // Update every 1 second
    }
    else {
        document.getElementById('Resendotp').style.display = "block";
    }
}

function PanAndPersonalPageValidation() {
    var panno = document.getElementById('txtPAN').value;
    var dob = document.getElementById('PanDob').value;
    var fathername = document.getElementById('txtFatherspouce').value;
    //var fatherlastname = document.getElementById('txtFatherLastspouce').value;
    //var Networth = document.getElementById('txtNetworth').value; 

    if (panno == "") {
        showMessage('Please Enter Pan.', 4000, 'red');
        //alert("Enter Pan");
        return false;
    }
    if (dob == "") {
        showMessage('Please Select Date Of Birth.', 4000, 'red');
        //alert("Select Date Of Birth");
        return false;
    }
    if (fathername == "") {
        showMessage('Please Enter Spouse Or Father First Name.', 4000, 'red');
        //alert("Enter Spouse Or Father First Name");
        return false;
    }
    if (IsEmailVarified == false) {
        showMessage('Please verify Email Id.', 4000, 'red');
        //alert("Enter Spouse Or Father Last Name");
        return false;
    }
    //if (fatherlastname == "") {
    //    showMessage('Please Enter Spouse Or Father Last Name.', 4000, 'red');
    //    //alert("Enter Spouse Or Father Last Name");
    //    return false;
    //}
    //if (Networth == "") {
    //    showMessage('Please Enter Networth.', 4000, 'red');
    //    return false;
    //}



}

function opendobfield() {
    var pan = document.getElementById('txtPAN').value;
    var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    if (regpan.test(pan)) {
        CheckPan();

    }
    else {
        $("#dobdiv").hide();
    }
}

function CheckPan() {
    $.ajax({
        type: "GET",
        url: CommonAPIURL + "api/UploadDocuments/CheckPanDetails",
        data:
        {
            PancardNo: $("#txtPAN").val()
        },
        contentType: "application/json",
        success: function (data) {
            var Message = data.data[0].msg;
            if (Message == 'PAN is already Present') {
                showDynamicMessage('PAN is already Present', 5000, 'red', 'Panalert');
                $("#flush-collapseTwo").hide();
                $("#txtPAN").val('');
                blankfield();
                return false;
            }
            else {
                $("#dobdiv").show();
                return true;
            }
        },
        error: function () {

        }

    });
}