window.onbeforeunload = function () {
    localStorage.removeItem(Token);
    return '';
};

$(document).ready(function () {
    $("#VerifyMobile").val(localStorage.getItem("phoneNumber"));
});

const BaseURL = "https://localhost:7064";
const RedirectBaseURL = "http://localhost:52206";
localhost: 52206
function showSuccessMessage(message, imagePath, imageWidth, imageHeight) {
    Swal.fire({
        position: "center",
        icon: "success",
        title: message,
        showConfirmButton: false,
        width: '350px',
        customClass: {
            heightAuto: false,
            popup: 'custom-popup-class login-form-1' // Add 'login-form-1' class to the popup
        },
        timer: 5000,
        timerProgressBar: true,
        imageUrl: imagePath,
        imageWidth: imageWidth,
        imageHeight: imageHeight,
        //html: '<div style="color: white;">' + message + '</div>', // Add this line to render HTML content
    });
}


function showErrorMessage(message) {
    Swal.fire({
        position: "center",
        icon: "error",
        title: `<span style='color: white;'>${message}</span>`,
        showConfirmButton: true,
        width: '350px',
        customClass: {
            heightAuto: false,
            popup: 'custom-popup-class login-form-1'
        },
        imageUrl: '/images/twc-logo.png',
        imageWidth: 90,
        imageHeight: 40,
    });
}

function showSuccessMessageWithDelay(message, delay) {
    Swal.fire({
        position: "center",
        icon: "success",
        title: `<span style='color: white;'>${message}</span>`,
        showConfirmButton: false,
        width: '350px',
        customClass: {
            heightAuto: false,
            popup: 'custom-popup-class login-form-1'
        },
        timer: delay,
        timerProgressBar: true,
        imageUrl: '/images/twc-logo.png',
        imageWidth: 90,
        imageHeight: 40,
    });
}

function validatePhoneNumber(phoneNumber) {
    if (phoneNumber === "") {
        showErrorMessage("Mobile number cannot be blank.");
        return false;
    }

    if (!/^\d+$/.test(phoneNumber)) {
        showErrorMessage("Mobile number should contain only digits.");
        return false;
    }

    if (phoneNumber.length !== 10) {
        showErrorMessage("Phone number should be exactly 10 digits.");
        return false;
    }

    return true;
}

function SendOtp() {
    const phoneNumber = $("#phoneNumberInput").val();

    if (!validatePhoneNumber(phoneNumber)) {
        return;
    }

    $.ajax({
        url: `${BaseURL}/api/AgentLogin/CheckUser?MobileNo=${phoneNumber}`,
        method: "POST",
        data: {},
    })
        .done(function (data) {
            if (data.data.userFound === true) {
                SendOTPAgent(phoneNumber);
            } else {
                showErrorMessage("User not found.");
            }
        })
        .fail(function () {
            // Handle error
        });
}

function SendOTPAgent(phoneNumber) {
    localStorage.setItem("phoneNumber", phoneNumber);

    $.ajax({
        type: "POST",
        url: `${BaseURL}/api/AgentLogin/UpdateOTP?Mobile=${phoneNumber}`,
        data: {},
    })
        .done(function (data) {
            if (data.code = '200' && data.message == "OTP sent successfully.") {
                showSuccessMessage("OTP sent successfully.", '/images/twc-logo.png', 90, 40);
                setTimeout(function () {
                    window.location.href = `${RedirectBaseURL}/login/OTPVerify`;
                }, 5000);
            } else {
                throw new Error("Something went wrong.");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            showErrorMessage(`Error: ${errorThrown}`);
        });
}

$("#Submitbutton").click(function () {
    const MobileOTP = $("#EnterOTP").val();
    const phoneNumber = localStorage.getItem("phoneNumber");
    const TokenID = localStorage.getItem("Token");

    if (MobileOTP === "" || !/^\d{6}$/.test(MobileOTP)) {
        showErrorMessage("Invalid OTP format. Please enter a 6-digit number.");
        return;
    }

    $.ajax({
        method: "POST",
        url: `${BaseURL}/api/AgentLogin/VerifyOTP`,
        contentType: 'application/json',
        data: JSON.stringify({
            mobile: phoneNumber,
            mobileOTP: MobileOTP,
            // token: TokenID
        }),
    })
        .done(function (data) {
            if (data.data == "OTP Verified") {
                showSuccessMessage("OTP verified successfully", '/images/twc-logo.png', 90, 40);

                // Add the token to the request headers


                setTimeout(function () {
                    // Redirect to the new page
                    window.location.href = `${RedirectBaseURL}/mutualFund/Screener`;
                }, 5000);
            } else {
                showErrorMessage("Invalid OTP. Please try again.");
            }
        })
        .fail(function () {
            // Handle error
        });
});


$(document).ready(function () {
    // Your existing document ready code

    // Add click event listener to the "Resend OTP" link
    $("#resendOtpLink").click(function (event) {
        event.preventDefault(); // Prevents the default behavior of the link (e.g., navigating to a new page)

        // Disable the link
        $(this).prop('disabled', true);

        // Set the countdown duration in seconds (2 minutes)
        const countdownDuration = 120;

        // Call the ResendOTP function to resend OTP
        ResendOTP(localStorage.getItem("phoneNumber"));

        // Update the countdown timer every second
        let remainingTime = countdownDuration;
        const countdownInterval = setInterval(function () {
            $("#countdownTimer").text(`Resend in ${remainingTime}s`);

            if (remainingTime <= 0) {
                // Enable the link and clear the interval when the countdown reaches zero
                $("#resendOtpLink").prop('disabled', false);
                $("#countdownTimer").text('');
                clearInterval(countdownInterval);
            }

            remainingTime--;
        }, 1000);
    });
});

function ResendOTP(phoneNumber) {
    localStorage.setItem("phoneNumber", phoneNumber);

    $.ajax({
        type: "POST",
        url: `${BaseURL}/api/AgentLogin/UpdateOTP?Mobile=${phoneNumber}`,
        data: {},
    })
        .done(function (data) {
            if (data.data && data.data !== "") {
                showSuccessMessage("OTP sent successfully.", '/images/twc-logo.png', 90, 40);
                setTimeout(function () {
                    //window.location.href = "Login/Login";
                }, 5000);
            } else {
                throw new Error("Something went wrong.");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            showErrorMessage(`Error: ${errorThrown}`);
        });
}

