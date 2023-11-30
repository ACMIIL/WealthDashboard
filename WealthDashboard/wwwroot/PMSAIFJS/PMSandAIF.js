



$("#contsumbit").click(function () {
    const full_Name = $("#FUllName").val();
    const min_Investment = $("#EstimatedInvestmen").val();
    const mobile_No = $("#MobileNumber").val();
    const email_ID = $("#EmailAddress").val();

    // Validation for full_Name
    if (!full_Name.trim()) {
        swal("Full Name cannot be blank.");
        return;
    }

    // Validation for min_Investment (numeric and non-empty)
    if (!min_Investment.trim() || isNaN(min_Investment)) {
        swal("Invalid Investment amount.");
        return;
    }

    // Validation for mobile_No (numeric, 10 digits, non-empty)
    if (!mobile_No.trim() || !/^\d{10}$/.test(mobile_No)) {
        swal("Invalid Mobile Number.");
        return;
    }

    // Validation for email_ID (email format)
    if (!email_ID.trim() || !isValidEmail(email_ID)) {
        swal("Invalid Email Address.");
        return;
    }

    $.ajax({
        method: "POST",
        url: `${BaseURL}api/PMSandAIF/InsertPMS&AIFLeads`,
        contentType: 'application/json',
        data: JSON.stringify({
            full_Name: full_Name,
            min_Investment: min_Investment,
            mobile_No: mobile_No,
            email_ID: email_ID
        }),
    })
        .done(function (data) {
            if (data.data == "OTP Verified") {
                swal.fire("OTP verified successfully");

                // Trigger the Thank-you modal
                $("#Thank-you").modal("show");

                setTimeout(function () {
                    // Redirect to the new page
                    window.location.href = `${RedirectBaseURL}/mutualFund/Screener`;
                }, 5000);
            } else {
                swal("Invalid OTP. Please try again.");
            }
        })
        .fail(function () {
            // Handle error
        });
});

// Function to validate email format
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}