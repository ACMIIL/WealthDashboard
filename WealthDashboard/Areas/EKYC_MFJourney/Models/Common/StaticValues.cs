namespace WealthDashboard.Areas.EKYC_MFJourney.Models.Common
{
    public class StaticValues
    {
        public const string ApplicationJsonMediaType = "application/json";
        public const string DigioUrl = "/api/DigioKYC/DigioWorkTemplate";
        public const string CntrlzDigiSuccRespoUrl = "/api/DigioKYC/DigioKYCResponse?ResponseId=";
        public const string InsertUpdateBank = "/api/ClientBankDetails/InsertOrUpdateClientBankDetails";
        public const string personalDetailurl = "/api/ClientPersonalDetails/InsertOrUpdateClientpersonalDetails";
        public const string AddressDetailurl = "/api/ClientPersonalDetails/InsertOrUpdateClientAddressDetails";
        public const string ReleationDetailurl = "/api/ClientPersonalDetails/InsertOrUpdateClientRelationDetails";
        public const string PinCodeMasterDetails = "/api/EKYCMasters/GetClientPinCodeMasterDetails?Pincode=";
        public const string fatcaUrl = "/api/ClientInvestmentFatcaDetails/InsertOrUpdateInvestmentAndFatcaDetails";
        public const string InsertOrUpdateClientSegment = "/api/PrimaryDetails/InsertOrUpdateClientSegment";
        public const string InsertOrUpdateSegment = "/api/PrimaryDetails/InsertOrUpdateSegment";
        public const string InsertOrUpdateClientUploadData = "/api/UploadDocuments/InsertOrUpdateClientUploadDocuments";
        public const string CreateEsign = "/api/Esign/CreateAccountEsign";

        ///CVL KRA API
        public const string CVLKRAUrl = "/api/Camspay/PANDetailsFetchKRA";
        public const string GetResponseXMLData = "USP_GetCVLKRAXmlData";

        public const string MobileDetailsInvalid = "Failed Mobile Details Invalid";


        //selfie
        public const string SelfieUrl = "/api/DigioSelfie/SelfieWorkTemplate";
        public const string SelfieResponseUrl = "/api/DigioSelfie/SelfieResponse?ResponseId=";
        public const string DigioSelfieFileDownload = "/api/DigioSelfie/GetselfieMedia?FileId=";


        //Stored Procedure
        public const string GenerateOTP = "USP_GetOTP";
        public const string CheckMobileClientDeclaration = "USP_CheckClientsMobileDeclaration";
        public const string USP_CheckClientsEMailIdDeclaration = "USP_CheckClientsEMailIdDeclaration";
        public const string USP_UpdateOTPEMailsDetails = "USP_UpdateOTPEMailsDetails";
        public const string USP_UpdateUCCTempData = "USP_UpdateUCCTempData";
        public const string GetDegioWorktemplate = "USP_GetDigioWorktemplateDetailsByRegistrationId";
        public const string GetSelfieWorktemplate = "USP_GetSelfieWorktemplateDetailsByRegistrationId";
        public const string GetOTPById = "USP_GetOTPByEmailIdMobile";
        public const string CheckSelfie = "USP_CheckSekfieExistsOrNot";
        public const string LoginLastVisit = "USP_LastLoginURL";
        public const string GetCityState_Pincode = "USP_GetCityState_Pincode";
        public const string CheckEsignStatus = "USP_CheckEsignStatus";
        public const string CheckDigioStatus = "USP_GetDigioFileStatus";


        //CommonClient Details
        public const string GetPrimaryDetailsByUCC = "USP_GetPrimaryDetailsByUCC";

        public const string GetPrimaryDetailsByRegistrationId = "USP_GetClientBasicDetailsByRegistrationId";
    }
}
