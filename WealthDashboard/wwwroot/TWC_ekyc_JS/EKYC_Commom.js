
var CommonPageURL = "http://localhost:52206/EKYC_MFJourney/";
var CommonAPIURL = "https://localhost:44394/";
var ApprovedePDFPath = "D:\\WelcomeDesk_ApprovedPDF\\";
//var CentralAPIURL = "https://devdigio.investmentz.com";
var CentralAPIURL = "https://localhost:44394/";
var DirectClient = "RC5555";

function isNumberKey(e) {                   
    var a = [];
    var k = e.which;

    for (i = 48; i < 58; i++)
        a.push(i);

    if (!(a.indexOf(k) >= 0))
        e.preventDefault();
}
function allowchar(event) {                 
    var charCode = event.which;
    if (!(charCode >= 65 && charCode <= 90) &&
        !(charCode >= 97 && charCode <= 122) &&
        (charCode === 32) && // space
        !(charCode === 8 || charCode === 0)) {
        event.preventDefault();
    }
}

function allowcharwithoutspace(event)
{
    var charCode = event.which;
    if (!(charCode >= 65 && charCode <= 90) &&
        !(charCode >= 97 && charCode <= 122) &&
        !(charCode === 32) && // space
        !(charCode === 8 || charCode === 0)) {
        event.preventDefault();
    }
}

function showMessage(message, duration, colour) {    
    const messageDiv = document.getElementById('Allmessage');
    messageDiv.innerText = message;
    messageDiv.style.display = 'block';
    messageDiv.style.color = colour;
    setTimeout(() => {
        messageDiv.style.display = 'none';
    }, duration);
}
function showDynamicMessage(message, duration, colour ,id) {   
    const messageDiv = document.getElementById(id);
    messageDiv.innerText = message;
    messageDiv.style.display = 'block';
    messageDiv.style.color = colour;
    setTimeout(() => {
        messageDiv.style.display = 'none';
    }, duration);
}

function SavePageDetails(RegistrationId, VisitURL) {
    var PageVisitData = {
        "pageVisitId": 0,
        "registrationId": parseInt(RegistrationId),
        "pageVisitURL": VisitURL,
        "pageFlag": false
    };
    try {
        $.ajax({
            type: "POST",
            url: CommonAPIURL + "api/PrimaryDetails/InsertOrUpdateClientPageVisit",
            data: JSON.stringify(PageVisitData),
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                if (data.Message = "Success") {
                }
                else {
                    swal({
                        title: "Page Details not saved",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                    });
                    return;
                }
            },
            error: function (data) {

                swal({
                    title: "Page Details not saved",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                });
            }
        });
    } catch (e) {
        swal({
            title: "Page Details not saved",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        });
    }
}

function showMessage(message, duration,colour) {
    const messageDiv = document.getElementById('Allmessage');
    messageDiv.innerText = message;
    messageDiv.style.display = 'block';
    messageDiv.style.color = colour;
    
    setTimeout(() => {
        messageDiv.style.display = 'none';
    }, duration);
}
function showDynamicMessage(message, duration, colour, id) {    // by siddhesh
    const messageDiv = document.getElementById(id);
    messageDiv.innerText = message;
    messageDiv.style.display = 'block';
    messageDiv.style.color = colour;
    setTimeout(() => {
        messageDiv.style.display = 'none';
    }, duration);
}

