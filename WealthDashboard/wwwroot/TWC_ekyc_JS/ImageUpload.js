
//var CommonPageURL = "https://devekyc.investmentz.com";
//var CommonPageURL = "https://localhost:7244";


$("#DocumentUpload").click(function () {
    var isvalid = bankvalidation()
    if (isvalid != false) {
        var RegistrationID = $("#RegistrationId").val();
        var EncRegistrationId = $("#EncRegistrationId").val();

        //var PanProof = document.getElementById('panUpload').files[0];
        var PanProof = undefined;
        var SignProof = document.getElementById('signatureUpload').files[0];
        var CheckProof = document.getElementById('chequeUpload').files[0];
        var uploadimageModel = new FormData();
        uploadimageModel.append('registrationId', parseInt(RegistrationID));
        uploadimageModel.append('PanImage', PanProof);
        uploadimageModel.append('SignImage', SignProof);
        uploadimageModel.append('CheckImage', CheckProof);

        $.ajax({
            url: CommonPageURL + "Home/ImageUpload",
            type: "POST",
            cache: false,
            contentType: false,
            processData: false,
            data: uploadimageModel,
            success: function (data) {
                if (data == 'Ok') {
                    SavePageDetails(RegistrationID, CommonPageURL + "Nominee/Nominee_Details" + '?encregistrationId=' + EncRegistrationId + "");
                    window.location.assign(CommonPageURL + "Nominee/Nominee_Details" + '?encregistrationId=' + EncRegistrationId + "");
                }
                else {
                    console.log("Segment/InsertOrUpdateSegment insert time error");
                }
            },
            error: function (data) {

            }
        });
    }
    



});



function bankvalidation() {
    var bankno = $("#AccountNumber").val();
    var IFSCCode = $("#IFSCCode").val();
    var pennyverify = $("#pennyverify").val();
    var sign = document.getElementById('signatureUpload').files[0];
    //var pan = document.getElementById('panUpload').files[0];
    var chequeUpload = document.getElementById('chequeUpload').files[0];

    if (bankno == "") {
        showMessage('Please Enter Account No.', 4000, 'red');
        return false;
    }
    if (IFSCCode == "") {
        showMessage('Please Enter IFSC Code.', 4000, 'red');
        return false;
    }
    if (sign == undefined) {
        showMessage('Please Upload Document.', 4000, 'red');
        return false;
    }
    //if ($("#hiddpanstatus").val() != 'True') {
    //    if (pan == undefined) {
    //        showMessage('Please Upload Document.', 4000, 'red');
    //        return false;
    //    }
    //}
    
    if (pennyverify != "verified") {
        if (chequeUpload == undefined ) {
            showMessage('Please Upload Document.', 4000, 'red');
            return false;
        }
        
    }
}




function CommonFilevaldate() {

    var sign = document.getElementById('signatureUpload').files[0];
    var pan = document.getElementById('panUpload').files[0];
    var chequeUpload = document.getElementById('chequeUpload').files[0];
    if (pan != undefined) {
        document.getElementById('panUploadmsg').style.display = "block";
    }

    if (sign != undefined) {
        document.getElementById('signaturemsg').style.display = "block";
        //$("#signaturemsg").show();
    }
    if (chequeUpload != undefined) {
        document.getElementById('chequeUploadmsg').style.display = "block";

        //$("#chequeUploadmsg").show();
    }
    else {
        
    }
}

function signcheck() {
    var sign = document.getElementById('signatureUpload').files[0];
    if (sign != undefined) {
        var type = sign.type;
        var size = sign.size / 1024;
        if (type == 'image/png' || type == 'image/jpg' || type == 'image/jpeg') {
            if (size > 5120) {
                alert('Uploaded document size should not exceed 5mb.');
                $("#signatureUpload").val('');
                return false;
            } else {
                document.getElementById('signaturemsg').style.display = "block";
                return true;
            }
        } else {
            alert('Uploaded document has an incorrect format.');
            document.getElementById('signaturemsg').style.display = "none";
            $("#signatureUpload").val('');
            return false;
        }
    }
}

//function pancheck() {
//    var pan = document.getElementById('panUpload').files[0];
    

//    if (pan != undefined) {
//        var type = pan.type;
//        var size = pan.size / 1024;
//        if (type == 'image/png' || type == 'image/jpg' || type == 'image/jpeg') {
//            if (size > 5120) {
//                alert('Uploaded document size should not exceed 5mb.');
//                $("#panUpload").val('');
//                return false;
//            } else {
//                document.getElementById('panUploadmsg').style.display = "block";
//                return true;
//            }
//        } else {
//            alert('Uploaded document has an incorrect format.');
//            $("#panUpload").val('');
//            document.getElementById('panUploadmsg').style.display = "none";
//            return false;
//        }
//    }
    
//}

function chequecheck() {

    var chequeUpload = document.getElementById('chequeUpload').files[0];
    if (chequeUpload != undefined) {
        var type = chequeUpload.type;
        var size = chequeUpload.size / 1024;
        if (type == 'image/png' || type == 'image/jpg' || type == 'image/jpeg') {
            if (size > 5120) {
                alert('Uploaded document size should not exceed 5mb.');
                $("#chequeUpload").val('');
                return false;
            } else {
                document.getElementById('chequeUploadmsg').style.display = "block";
                return true;
            }
        } else {
            alert('Uploaded document has an incorrect format.');
            $("#chequeUpload").val('');
            document.getElementById('chequeUploadmsg').style.display = "none";
            return false;
        }
    }

}