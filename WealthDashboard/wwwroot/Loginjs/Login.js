$(document).ready(function () {
    $("#VerifyMobile").val(localStorage.getItem("phoneNumber"));
});

const BaseURL = "https://localhost:7064";
const RedirectBaseURL = "http://localhost:52206";

function getCookie(name) {
    var nameEQ = name + "=";
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1, cookie.length);
        }
        if (cookie.indexOf(nameEQ) === 0) {
            return cookie.substring(nameEQ.length, cookie.length);
        }
    }
    return null;
}
function showSuccessMessage(message, imagePath, imageWidth, imageHeight) {
    Swal.fire({
        position: "center",
        icon: "success",
        title: message,
        showConfirmButton: false,
        width: '350px',
        customClass: {
            heightAuto: false,
            popup: 'custom-popup-class login-form-1'
        },
        timer: 5000,
        timerProgressBar: true,
        imageUrl: imagePath,
        imageWidth: imageWidth,
        imageHeight: imageHeight,
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

// Function to send OTP
function sendOtp() {
    var phoneNumber = $("#phoneNumberInput").val();

    if (!validatePhoneNumber(phoneNumber)) {
        return;
    }
    $.ajax({
        url: '/Login/SendOtp',
        type: 'POST',
        data: { phoneNumber: phoneNumber },
        success: function (response) {
            if (response.success) {
                showSuccessMessageWithDelay(response.message);
                window.location.href = '/Login/OTPVerify';
            } else {
                showErrorMessage("Error sending OTP: " + response.message);
            }
        },
        error: function (error) {
            showErrorMessage("Error sending OTP: " + error.responseText);
        }
    });
}

// Function to verify OTP
$("#Submitbutton").click(function () {
    var otp = $("#EnterOTP").val();
    var mobile = getCookie("phoneNumber");
    $.ajax({
        url: '/Login/VerifyOtp',
        type: 'POST',
        data: { otp: otp, Mobile: mobile },
        success: function (response) {
            if (response.success) {
                showSuccessMessageWithDelay(response.message, 3000);
                window.location.href = 'MutualFund/Screener';
            } else {
                showErrorMessage("Error verifying OTP: " + response.message);
            }
        },
        error: function (error) {
            showErrorMessage("Error verifying OTP: " + error.responseText);
        }
    });
});

// Function to resend OTP (optional)
function resendOtp() {
    var phoneNumber = $("#phoneNumberInput").val();
    $.ajax({
        url: '/Login/SendOtp',
        type: 'POST',
        data: { phoneNumber: phoneNumber },
        success: function (response) {
            // Handle success (if needed)
            showSuccessMessageWithDelay("OTP resent successfully!");
        },
        error: function (error) {
            // Handle error (if needed)
            showErrorMessage("Error resending OTP: " + error.responseText);
        }
    });
}

// You can add more functions or customize the existing ones based on your requirements

$("#SendOTPButton").click(function () {
    sendOtp(); // Corrected the function name to sendOtp
});