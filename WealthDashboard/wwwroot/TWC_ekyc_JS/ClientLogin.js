let timerOn = true;
var Mobileotp = "";

$(document).ready(function () {    
    document.getElementById('txtMobileNumber').value = $('#MobileNumber').val();   
    CheckMobileExistsOrNot();
});

$('#txtMobileNumber').on("cut copy paste", function (e) {
    e.preventDefault();
});
$('#txtOTP').on("cut copy paste", function (e) {
    e.preventDefault();
});

$('#terms').click(function () {
    var url = "https://web.investmentz.com/imgupload/Brok_Plan/ACM%20Terms%20and%20Conditions.pdf";
    window.open(url, '_blank');
});

function SendMobileOtp() {
    var Mobilenumber = document.getElementById('txtMobileNumber').value;
    
    if (Mobilenumber === "undefined") {
        document.getElementById("txtMobileNumber").value = "";
        return;
    }
    var EmailId = "";   
    if (!Mobilenumber) {        
        return;
    }
    $.ajax({
        type: 'POST',
        url: CommonPageURL + "Login/GenerateMobileOTP",
        data: {
            EmailId: EmailId,
            MobileNo: Mobilenumber
        },
        success: function (data) {
            RDMtimer(60);
            $('#txtOTP').focus();
            showMessage('OTP Sent Successfully', 60000, 'green');
            document.getElementById('txtMobileNumber').readOnly = true;

        },
        error: function (response, error) {
        }
    });
}
function CheckOTP() {

    var MobileOTP = document.getElementById('txtOTP').value;
    if (MobileOTP.length === 6) {
        var Mobilenumber = document.getElementById('txtMobileNumber').value;
        var EmailId = "";
        $.ajax({
            type: 'POST',
            url: CommonPageURL + "Login/CheckOTP",
            data: {
                EmailId: " ",
                MobileNo: Mobilenumber
            },
            success: function (data) {

                if (MobileOTP == data.otpMobile) {
                    LoginLastVisit();

                }
                else {
                    showMessage('Please enter the correct OTP', 10000, 'red');
                }

            },
            error: function (response, error) {
            }
        });
    }
}

function SaveTempUCCData() {
    var MobileNo = document.getElementById('txtMobileNumber').value;
    var AgentCode = document.getElementById('Source').value;
    $.ajax({
        type: "POST",
        url: CommonPageURL + "Login/save_ucc_temp_details",
        data:
        {
            EmailId: "",
            MobileNo: MobileNo,
            BACode: $('#BACode').val(),
            Source: "",
            EmailRelation: "118",
            MobileRelation: "118",
            EmailUCC: "",
            MobileUCC: "",
            EmployeeRef: AgentCode,
            EmployeeRefID: AgentCode,
        },
        success: function (data) {
            if (data.ucc != null || data.ucc != "" || data.ucc != "undefined") {

                SaveRegistrationDetails(data.ucc);

            }
        },
        error: function (data) {
            console.log("Error");
            return false;
        }
    });
}

function isNumbereventandotp(e) {                   //By Siddhesh
    var a = [];
    var k = e.which;

    for (i = 48; i < 58; i++)
        a.push(i);

    if (!(a.indexOf(k) >= 0))
        e.preventDefault();

    var mob = $("#txtMobileNumber").val().length;

    if (mob == 10) {
        if ($('#txtMobileNumber').is('[readonly]') != true) {
            CheckMobileExistsOrNot();
        }
    }
}
function SaveRegistrationDetails(UCC) {
    var AgentCode = document.getElementById('Source').value;
    var jsonRegistrationdata = {
        "registrationId": 0,
        "inwardNo": "20" + UCC,
        "commonClientCode": parseInt(UCC),
        "clientCategoryId": 1,
        "bacode": $('#BACode').val(),
        "mobileNumber": document.getElementById('txtMobileNumber').value,
        "emailId": " ",
        "noOfHolders": 0,
        "userId": "acmiil",
        "employeeReferralCode": AgentCode,
        "EmployeeRefID": ""
    };
    console.log(jsonRegistrationdata);
    try {
        $.ajax({
            type: "POST",
            url: CommonAPIURL + "api/PrimaryDetails/InsertOrUpdateRegistrationDetailsDeclaration",
            data: JSON.stringify(jsonRegistrationdata),
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                if (data.message = "success") {
                    EncryptedUCC(UCC);
                }
            },
            error: function (data) {
            }
        })
    }
    catch
    {

    }
}

function CheckMobileExistsOrNot() {
    var mobileNumber = document.getElementById('txtMobileNumber').value;

    // Validate mobile number
    if (ValidateMobileNumber()) {
        $.ajax({
            type: "GET",
            url: CommonAPIURL + "api/UploadDocuments/CheckPanDetails",
            data: {
                Mobileno: mobileNumber,
            },
            success: function (data) {
                var message = data.data[0].msg;

                SendMobileOtp();
               
            },
            error: function (data) {
                console.log("Error: Contact Customer Care service");
                return false;
            }
        });
    }
}

function ValidateMobileNumber() {
    var mobileNumber = document.getElementById("txtMobileNumber").value;
    
    if (mobileNumber === "undefined") {
        document.getElementById("txtMobileNumber").value = "";
        return;
    }

    var expr = /^(0|91)?[6-9][0-9]{9}$/;
    if (!expr.test(mobileNumber)) {
        document.getElementById("txtMobileNumber").value = "";
        showMessage('Invalid Mobile Number', 7000, 'red');
        return false;
    }

    return true;
}


function RDMtimer(remaining) {
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;
    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    document.getElementById('timer').innerHTML = m + ':' + s;
    remaining -= 1;
    if (remaining >= 0 && timerOn) {
        setTimeout(function () {
            RDMtimer(remaining);
        }, 1000);
        return;
    }
    if (!timerOn) {
        // Do validate stuff here
        return;
    }
    document.getElementById('Resendotp').style.display = "block";
}
function CheckMobileDeclarationWithUCC() {
  
    if (document.getElementById('txtMobileNumber').value != "") {
        document.getElementById('ALLSegments').style.display = "block";
        var Declaration = $('input[name="btnAnnualIncome"]:checked').val();

        $.ajax({
            type: "GET",
            url: CommonPageURL + "Login/CheckMobileDeclarationWithUCC",
            data: {
                MobileNo: document.getElementById('txtMobileNumber').value,
                DeclarationType: Declaration
            },
            success: function (Emaildata) {
                var Message = Emaildata.errorMessage;
                if (Message == 'Mobile No self is already present ') {
                    document.getElementById("btnradioself").disabled = true;
                    document.getElementById("btnradioself").checked = false;
                    showMessage('Self Already Present', 7000, 'red');
                }
                else if (Message == 'Spouse Present') {
                    showMessage('Spouse Already Present', 7000, 'red');
                    document.getElementById("btnradioSpouse").disabled = true;
                    document.getElementById("btnradioSpouse").checked = false;
                }
                else if (Message == 'Parent Present') {
                    showMessage('Parent Already Present', 7000, 'red');
                    document.getElementById("btnradioParent").disabled = true;
                    document.getElementById("btnradioParent").checked = false;
                }
                else if (Message == 'Child Present') {
                    showMessage('Child Already Present', 7000, 'red');
                    document.getElementById("btnradioChildren").disabled = true;
                    document.getElementById("btnradioChildren").checked = false;
                }
                else {
                    CheckOTP();
                }
            },
            error: function () {

            }
        });
    }
    else {
        alert("enter mobile no");
    }
}


function CheckMobileDeclarationWithUCC123() {
    
    if (document.getElementById('txtMobileNumber').value != "") {
        document.getElementById('ALLSegments').style.display = "block";
        var Declaration = $('input[name="btnAnnualIncome"]:checked').val();

        $.ajax({
            type: "GET",
            url: CommonPageURL + "Login/CheckMobileDeclarationWithUCC",
            data: {
                MobileNo: document.getElementById('txtMobileNumber').value,
                DeclarationType: Declaration
            },
            success: function (Emaildata) {
                var Message = Emaildata.errorMessage;
                if (Message == 'Mobile No self is already present ') {
                    document.getElementById("btnradioself").disabled = true;
                    document.getElementById("btnradioself").checked = false;
                    showMessage('Self Already Present', 7000, 'red');
                }
                else if (Message == 'Spouse Present') {
                    showMessage('Spouse Already Present', 7000, 'red');
                    document.getElementById("btnradioSpouse").disabled = true;
                    document.getElementById("btnradioSpouse").checked = false;
                }
                else if (Message == 'Parent Present') {
                    showMessage('Parent Already Present', 7000, 'red');
                    document.getElementById("btnradioParent").disabled = true;
                    document.getElementById("btnradioParent").checked = false;
                }
                else if (Message == 'Child Present') {
                    showMessage('Child Already Present', 7000, 'red');
                    document.getElementById("btnradioChildren").disabled = true;
                    document.getElementById("btnradioChildren").checked = false;
                }
                else {
                    CheckOTP();
                }
            },
            error: function () {

            }
        });
    }
    else {
        alert("enter mobile no");
    }
}


function ResendOTP() {
    //document.getElementById('OTPLabel').style.display = "none";
    var Declaration = $('input[name="btnAnnualIncome"]:checked').val();
    var mobileNumber = document.getElementById('txtMobileNumber').value;
    if (mobileNumber.length === 10) {
        SendMobileOtp();
        document.getElementById('Resendotp').style.display = "none";
    }
}
function EncryptedUCC(UCC1) {

    $.ajax({
        type: "GET",
        url: CommonPageURL + "Home/EncryptedUCC",
        data: {
            UCC: UCC1
        },
        success: function (dataUCC) {

            if (dataUCC.ucc != "0") {
                window.location.assign(CommonPageURL + "Home/PAN_Details" + '?encucc=' + dataUCC.encUCC + "&rc_code=" + dataUCC.encBACode);
                SavePageDetails(CommonPageURL + "Home/PAN_Details" + '?encucc=' + dataUCC.encUCC + "&rc_code=" + dataUCC.encBACode);
            }

            else {
                alert("else part executed MEans LAst Page Visit page Redirect to URL")
                window.location.assign(CommonPageURL + "Home/PAN_Details" + '?encucc=' + dataUCC.encUCC + "&rc_code=" + dataUCC.encBACode);

            }
        },
        error: function (dataUCC) {

            console.log("Error");
            return false;
        }
    });
}

function LoginLastVisit() {
    $.ajax({
        type: "GET",
        url: CommonPageURL + "Login/LoginLastVisit",
        data:
        {
            MobileNo: document.getElementById('txtMobileNumber').value,
        },
        success: function (data) {
            if (data.lastloginURL != null) {
                var redirectLink = data.lastloginURL;
                window.location.assign(redirectLink);
            }
            else {
                //if ($('input[name="btnAnnualIncome"]:checked').length == 0) {

                //    showMessage('Please Select Declaration', 7000, 'red');
                //    return;
                //}
                //else { }

                SaveTempUCCData();
            }

        },
        error: function (data) {

        }

    })
}




