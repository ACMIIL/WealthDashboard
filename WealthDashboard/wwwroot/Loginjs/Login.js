$(document).ready(function () {
    $("#VerifyMobile").val(localStorage.getItem("phoneNumber"))
});
var checkmobileno = "https://localhost:7064/api/AgentLogin/CheckUser?MobileNo=";
var AgentOTPSend = "https://localhost:7064/api/AgentLogin/UpdateOTP?Mobile=";
var VerifyOTP = "https://localhost:7064/api/AgentLogin/VerifyOTP";
var BaseURL = "https://localhost:7064";

function SendOtp() {
    // Get the phone number input value
    var phoneNumber = $("#phoneNumberInput").val();

    // Validate phone number
    if (phoneNumber === "") {
        Swal.fire({
            position: "Center",
            icon: "error",
            title: "<span style='color: black;'>Mobile number cannot be blank.</span>",
            showConfirmButton: true,
            background: '#ECEFF1',
            border: '5px solid #CAAA86',
            width: '350px ',
            //timer: 1500
        });
        return;
    }

    // Check if the phone number contains only digits
    if (!/^\d+$/.test(phoneNumber)) {
        Swal.fire({

            position: "Center",
            icon: "error",
            title: "<span style='color: black;'>Mobile number should contain only digits.</span>",
            showConfirmButton: true,
            background: '#ECEFF1',
            border: '5px solid #CAAA86',
            width: '350px ',
        });
        return;
    }

    // Check if the phone number is exactly 10 digits
    if (phoneNumber.length !== 10) {
        Swal.fire({
            position: "top-Center",
            icon: "error",
            title: "<span style='color: black;'>Phone number should be exactly 10 digits.</span>",
            showConfirmButton: true,
            background: '#ECEFF1',
            border: '5px solid #CAAA86',
            width: '350px ',
        });
        return;
    }

    // If all validation checks pass, proceed with the AJAX call
    $.ajax({
        url: checkmobileno + phoneNumber,
        method: "POST",
        data: {},
        success: function (data) {
            if (data.data.userFound === true) {
                // If user is found, call SendOTPAgent with the same phone number
                SendOTPAgent(phoneNumber);
            } else {
                // If user is not found, show an alert
                Swal.fire({
                    position: "center",
                    icon: "error",
                    title: "<span style='color: white;'>User not found.</span>",
                    showConfirmButton: true,
                    background: '#ffd9b3',
                    border: '1px solid #CAAA86',
                    width: '350px',
                    customClass: {
                        heightAuto: false,
                        popup: 'custom-popup-class'
                    }
                    //timer: 1500
                });
            }
        },
        error: function (data) {
            // Handle error
        }
    });

    function SendOTPAgent(phoneNumber) {

        localStorage.setItem("phoneNumber", phoneNumber);
        $.ajax({
            type: "POST",
            url: AgentOTPSend + phoneNumber,
            data: {},
            success: function (data) {
                if (data.data && data.data !== "") {
                    localStorage.setItem("SessionID", data.data);
                    // If user is found, call SendOTPAgent with the same phone number
                    Swal.fire({
                        position: "center",
                        icon: "success",
                        title: "<span style='color: white;'>OTP sent successfully.</span>",
                        showConfirmButton: true,
                        background: '#8D8E8F',
                        border: '1px solid #CAAA86',
                        width: '350px',
                        customClass: {
                            heightAuto: false,
                            popup: 'custom-popup-class'
                        }
                        //timer: 1500
                    });
                    localStorage.getItem("phoneNumber");

                    window.location.href = "login/login?phoneNumber=" + phoneNumber;
                    
                }
                else {
                       // If user is not found, show an alert
                         Swal.fire({
                        position: "center",
                        icon: "error",
                        title: "<span style='color: white;'>Somthing went wrong.</span>",
                        showConfirmButton: true,
                        background: '#ffd9b3',
                        border: '1px solid #CAAA86',
                        width: '350px',
                        customClass: {
                            heightAuto: false,
                            popup: 'custom-popup-class'
                        }
                        //timer: 1500
                    });
                }


            },
            error: function (data) {
                console.log(data);
            }
        });
    }

  
}



$("#Submitbutton").click(function () {
    var MobileOTP = $("#EnterOTP").val();
    var phoneNumber = localStorage.getItem("phoneNumber");
    var SessionID = localStorage.getItem("SessionID");

    // Validate MobileOTP
    if (MobileOTP === "" || !/^\d{6}$/.test(MobileOTP)) {
        // Show an alert for invalid MobileOTP
        Swal.fire({
            position: "center",
            icon: "error",
            title: "<span style='color: white;'>Invalid OTP format. Please enter a 6-digit number.</span>",
            showConfirmButton: true,
            background: '#ffd9b3',
            border: '1px solid #CAAA86',
            width: '350px',
            customClass: {
                heightAuto: false,
                popup: 'custom-popup-class'
            }
        });
        return; // Stop further processing if validation fails
    }

    // Proceed with AJAX request if validation passes
    $.ajax({
        method: "POST",
        url: BaseURL + "/api/AgentLogin/VerifyOTP",
        contentType: 'application/json',  // Add this line
        data: JSON.stringify({
            mobile: phoneNumber,
            mobileOTP: MobileOTP,
            sessionId: SessionID
        }),
        success: function (data) {
            if (data.data == "OTP Verified") {

                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "<span style='color: white;'>OTP VERIFIED SUCCESSFULLY</span>",
                    showConfirmButton: true,
                    background: '#ffd9b3',
                    border: '1px solid #CAAA86',
                    width: '350px',
                    customClass: {
                        heightAuto: false,
                        popup: 'custom-popup-class'
                    }
                });
                
                    
                window.location.href = "MutualFund/Main"
                
            }
            else {
                // If user is not found, show an alert
                Swal.fire({
                    position: "center",
                    icon: "error",
                    title: "<span style='color: white;'>Invalid OTP. Please try again.</span>",
                    showConfirmButton: true,
                    background: '#ffd9b3',
                    border: '1px solid #CAAA86',
                    width: '350px',
                    customClass: {
                        heightAuto: false,
                        popup: 'custom-popup-class'
                    }
                });
            }
        },
        error: function (data) {
            // Handle error
        }
    });
});



