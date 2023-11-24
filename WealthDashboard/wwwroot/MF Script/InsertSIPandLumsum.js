$(document).ready(function () {
    // Initially hide the additional content
    $("#additionalContent").hide();

    // Handle radio button change event
    $('input[name="inlineRadioOptions"]').change(function () {
        if ($(this).val() === 'option2') {
            // Lumpsum is selected, show the additional content
            $("#additionalContent").show();
        } else {
            // SIP is selected, hide the additional content and its children
            $("#additionalContent").hide();
            // Optionally, hide specific elements within the additional content
            $("#additionalContent").find(".specific-element-to-hide").hide();
        }
    });
});
