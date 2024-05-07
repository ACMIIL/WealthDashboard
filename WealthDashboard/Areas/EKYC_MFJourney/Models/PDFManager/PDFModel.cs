namespace WealthDashboard.Areas.EKYC_MFJourney.Models.PDFManager
{
    public class MainModel
    {
        public PrimaryDetailsModel PrimaryDetailsModel { get; set; }

        public List<UploadDocDetailsModel>? UploadDocsDetailsModel { get; set; }
        public PersonalDetailsModel? PersonalDetailsModel { get; set; }
        public ClientPermanentAddressModel? ClientPermanentAddressModel { get; set; }

        public ClientCorrespondenceAddressModel ClientCorrespondenceAddressModel { get; set; }
        public ClientBankDetailsModel? ClientBankDetailsModel { get; set; }
        public ClientDerivativesModel? ClientDerivativesModel { get; set; }


        public PennyDropDetailsModel? PennyDropDetailsModel { get; set; }
        public List<ClientsNomineeGuanrdianDetailsModel>? ClientsNomineeGuanrdianDetailsModel { get; set; }
        public ClientDepositoryDetailsModel? ClientDepositoryDetailsModel { get; set; }
        public ClientFatcaDetailsModel? ClientFatcaDetailsModel { get; set; }
        public ClientNomineeDetailsModel? ClientNomineeDetailsModel { get; set; }
        public ClientSecondNomineeDetailsModel? ClientSecondNomineeDetailsModel { get; set; }
        public ClientThirdNomineeDetailsModel? ClientThirdNomineeDetailsModel { get; set; }
        public List<NomineeUploadDocs>? NomineeUploadDocs { get; set; }
        public UploadPhotoDocs? uploadPhotoDocs { get; set; }
        public UploadSignDocs? uploadSignDocs { get; set; }

        public UploadGEOTag uploadGEOTag { get; set; }

        public Notification notification { get; set; }
        public List<DerivativeSegmentModel> DerivativeSegmentModel { get; set; }

        public CivilKRAFetchModel CivilKRAFetchModel { get; set; }

    }
    public class PrimaryDetailsModel
    {
        public int RegistrationId { get; set; }
        public string InwardNo { get; set; }
        public string CommonClientCode { get; set; }
        public int ClientCategoryId { get; set; }
        public string ClientCategoryName { get; set; }
        public string Bacode { get; set; }
        public int NoOfHolders { get; set; }
        public string DeliveryPrice { get; set; }
        public string IntradayAndFuture { get; set; }
        public string OptionPrice { get; set; }

        //Relationship 
        public string EmailRelationShip { get; set; }
        public string MobileRelationShip { get; set; }
        public string ConsentBSDAFacility { get; set; }
        public string ConsentCDSLPOA { get; set; }
        public string ConsentOnlineAcc { get; set; }

    }

    public class WDRegistrationModel
    {
        public int WelcomeRegistrationId { get; set; }
        public int RegistrationId { get; set; }
        public string StageCode { get; set; }
        public string StatusDescription { get; set; }
        public string Flag { get; set; }
        public string IsActive { get; set; }
    }
    public class UploadDocDetailsModel
    {
        public int ClientUploadDataId { get; set; }
        public int UploadDocumentId { get; set; }
        public string DocName { get; set; }
        public int RegistrationId { get; set; }
        public string UploadDocNo { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int OrderByField { get; set; }
    }

    public class PersonalDetailsModel
    {
        public int PersonalDetailsId { get; set; }
        public int RegistrationId { get; set; }
        public string Bacode { get; set; }
        public string ClientFullName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientMiddleName { get; set; }
        public string ClientLastName { get; set; }
        public int ClientHolderId { get; set; }
        public string HolderName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PAN { get; set; }
        public string UID { get; set; }
        public int Gender { get; set; }
        public string GenderText { get; set; }
        public int MaritalStatus { get; set; }
        public string MaritalStatusText { get; set; }
        public int OccupationType { get; set; }
        public string OccupationTypeText { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public bool SameAddress { get; set; }

        //Relation Type
        public string FatherFirstName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherLastName { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherMiddleName { get; set; }
        public string MotherLastName { get; set; }



        //CVL data
        public string CVLKraFlag { get; set; }



    }

    public class ClientPermanentAddressModel
    {
        public string PerAddress1 { get; set; }
        public string PerAddress2 { get; set; }
        public string PerAddress3 { get; set; }
        public string PerCity { get; set; }
        public string PerPincode { get; set; }
        public string PerState { get; set; }
        public string PerstateCode { get; set; }
        public string PerCountry { get; set; }
        public string PerCountryCode { get; set; }
    }

    public class ApprovedPDFModel
    {
        public int RegistrationId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FilePathEsign { get; set; }
        public string UserId { get; set; }
    }

    public class ClientCorrespondenceAddressModel
    {
        public string CorAddress1 { get; set; }
        public string CorAddress2 { get; set; }
        public string CorAddress3 { get; set; }
        public string CorCity { get; set; }
        public string CorPincode { get; set; }
        public string CorState { get; set; }
        public string CorstateCode { get; set; }
        public string CorCountry { get; set; }
        public string CorCountryCode { get; set; }
    }

    public class ClientBankDetailsModel
    {
        public string AccountNo { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public int IFSCMasterId { get; set; }
        public string IFSCCode { get; set; }
        public bool DefaultBank { get; set; }
        public string UpiId { get; set; }
        public string MICRCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BankCity { get; set; }
        public string BankPincode { get; set; }
        public string BankState { get; set; }
        public string BankCountry { get; set; }
        public string Address { get; set; }

    }

    public class ClientDerivativesModel
    {
        public int DerivativeId { get; set; }
        public int RegistrationId { get; set; }
        public int UploadType { get; set; }
        public int ProofType { get; set; }
        public string UploadTypeText { get; set; }
        public string ProofTypeText { get; set; }
        public string ProofNo { get; set; }



    }

    public class GuardianDetailsModel
    {
        public string ProffTypeText { get; set; }
        public string NomineeType { get; set; }
    }

    public class DerivativeSegmentModel
    {
        public int ClientSegmentId { get; set; }
        public int RegistrationId { get; set; }
        public int SegmentMasterId { get; set; }
        public int IsActive { get; set; }

    }

    public class PennyDropDetailsModel
    {
        public int Checkfuzzymatchscore { get; set; }
        public string beneficiary_name_with_bank { get; set; }
    }

    public class ClientsNomineeGuanrdianDetailsModel
    {
        public string NomineeType { get; set; }
        public string NomineeTypeText { get; set; }
        public string GuardianFirstName { get; set; }
        public string GuardianMiddleName { get; set; }
        public string GuardianLastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Pincode { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string GuardianRelationshipType { get; set; }
        public string GuardianRelationshipTypeText { get; set; }

        public string ProffType { get; set; }
        public string ProffTypeText { get; set; }
    }

    public class ClientDepositoryDetailsModel
    {
        public string Depository { get; set; }
        public string DPID { get; set; }
        public string BOID { get; set; }
        public string DepositoryName { get; set; }
        public string CDSLAccountOpeningDate { get; set; }
        public string TariffPlan { get; set; }
        public string PLAN_CODE { get; set; }
        public string DeliveryPrice { get; set; }
        public string IntradayAndFuture { get; set; }
        public string OptionPrice { get; set; }
    }

    public class ClientFatcaDetailsModel
    {
        public string InvestExperience { get; set; }
        public string AnnualIncome { get; set; }
        public string Networth { get; set; }
        public string AnnualIncomeDate { get; set; }
        public string SourceOfWealth { get; set; }
        public string PoliticallyExposePersonText { get; set; }
        public string CountryName { get; set; }
        public string ISYourCountryTAXResidencyOtherThenIndiaText { get; set; }

        public string OptionPrice { get; set; }
        public string DeliveryPrice { get; set; }
        public string IntradayPrice { get; set; }
        public string SettelmentStatus { get; set; }

    }

    public class ClientNomineeDetailsModel
    {

        public string NomineeType { get; set; }
        public string NomineeTypeText { get; set; }
        public string Title { get; set; }
        public string NomineeName { get; set; }

        public string NomineeFirstName { get; set; }
        public string NomineeMiddleName { get; set; }
        public string NomineeLastName { get; set; }

        public DateTime DOBNominee { get; set; }
        public string IsMinor { get; set; }
        public string PanNumber { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string ProffType { get; set; }
        public string ProffTypeText { get; set; }
        public string RelationshipType { get; set; }
        public string RelationshipTypeText { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string NomineeAddress1 { get; set; }
        public string NomineeAddress2 { get; set; }
        public string NomineeAddress3 { get; set; }
        public string NomineePincode { get; set; }
        public string NomineeCity { get; set; }
        public string NomineeState { get; set; }
        public string NomineeCountry { get; set; }
        public string NomineePercentage { get; set; }
        public Boolean IsResidualSecurities { get; set; }
        public string IsResidualSecuritiesText { get; set; }

    }

    public class ClientSecondNomineeDetailsModel
    {

        public string NomineeType { get; set; }
        public string NomineeTypeText { get; set; }
        public string Title { get; set; }
        public string NomineeName { get; set; }

        public string NomineeFirstName { get; set; }
        public string NomineeMiddleName { get; set; }
        public string NomineeLastName { get; set; }

        public DateTime DOBNominee { get; set; }
        public string IsMinor { get; set; }
        public string PanNumber { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string ProffType { get; set; }
        public string ProffTypeText { get; set; }
        public string RelationshipType { get; set; }
        public string RelationshipTypeText { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string NomineeAddress1 { get; set; }
        public string NomineeAddress2 { get; set; }
        public string NomineeAddress3 { get; set; }
        public string NomineePincode { get; set; }
        public string NomineeCity { get; set; }
        public string NomineeState { get; set; }
        public string NomineeCountry { get; set; }
        public string NomineePercentage { get; set; }
        public Boolean IsResidualSecurities { get; set; }
        public string IsResidualSecuritiesText { get; set; }

    }

    public class ClientThirdNomineeDetailsModel
    {

        public string NomineeType { get; set; }
        public string NomineeTypeText { get; set; }
        public string Title { get; set; }
        public string NomineeName { get; set; }

        public string NomineeFirstName { get; set; }
        public string NomineeMiddleName { get; set; }
        public string NomineeLastName { get; set; }

        public DateTime DOBNominee { get; set; }
        public string IsMinor { get; set; }
        public string PanNumber { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string ProffType { get; set; }
        public string ProffTypeText { get; set; }
        public string RelationshipType { get; set; }
        public string RelationshipTypeText { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string NomineeAddress1 { get; set; }
        public string NomineeAddress2 { get; set; }
        public string NomineeAddress3 { get; set; }
        public string NomineePincode { get; set; }
        public string NomineeCity { get; set; }
        public string NomineeState { get; set; }
        public string NomineeCountry { get; set; }
        public string NomineePercentage { get; set; }
        public Boolean IsResidualSecurities { get; set; }
        public string IsResidualSecuritiesText { get; set; }

    }
    public class NomineeUploadDocs
    {
        public int ClientNomineeId { get; set; }
        public int NomineeType { get; set; }
        public string NomineeTypeText { get; set; }
        public int ProffType { get; set; }
        public string ProffTypeText { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileName2 { get; set; }
        public string FilePath2 { get; set; }

    }

    public class UploadPhotoDocs
    {
        public int ClientUploadDataId { get; set; }
        public int UploadDocumentId { get; set; }
        public int RegistrationId { get; set; }
        public string? UploadDocNo { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }


    }

    public class UploadSignDocs
    {
        public int ClientUploadDataId { get; set; }
        public int UploadDocumentId { get; set; }
        public int RegistrationId { get; set; }
        public string? UploadDocNo { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }


    }


    public class UploadGEOTag
    {
        public int GeoTagId { get; set; }
        public string? IPAddress { get; set; }
        public int RegistrationId { get; set; }
        public string? IPType { get; set; }
        public string? ContinentName { get; set; }
        public string? CountryCode { get; set; }
        public string? RegionCode { get; set; }
        public string? RegionName { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime EntryDate { get; set; }


    }

    public class Notification
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public bool IsError { get; set; }
    }

    public class NotificationService
    {
        private readonly List<Notification> _notifications;

        public NotificationService()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(string message, string type)
        {
            _notifications.Add(new Notification { Message = message, Type = type });
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }
    }

    public static class NormalPDFPageModel
    {
        public static int PageCkyc_KraForm = 4;
        //public int PageCkyc_KraFormSecondHolder = 14;
        //public int PageCkyc_KraFormThirdHolder = 16;
        public static int PageCKYC_contact = 5;
        public static int PageCKYC_aaditionaldetails = 6;
        public static int PageCKYC_nomineeimg = 8;
        public static int PageCKYC_nomineeoptoutimg = 9;
        public static int PageCKYC_runningacc = 11;
        public static int PageCKYC_additionalkyc = 11;
        public static int PageCKYC_optionform = 12;
        public static int PageCKYC_optionformdeclaratuion = 13;
        public static int PageCKYC_officeuse = 13;
        public static int PageCKYC_contactOrbis = 3;
        public static int PageCKYC_MFPage = 18;
        public static int LastPageMF = 20;

    }

    public class ResponceEsign
    {
        public int code { get; set; }
        public string? message { get; set; }
        public string? data { get; set; }
        public object esignUrl { get; set; }
        public object pdfdownload { get; set; }
    }
    public static class BankOfIndiaPDFPageModel
    {
        public static int PageCkyc_KraForm = 4;
        public static int PageCKYC_contact = 5;
        public static int PageCKYC_aaditionaldetails = 6;
        public static int PageCKYC_runningaccforboi = 10;
        public static int PageCKYC_officeuseforboi = 12;
    }
    public static class BSEMFModel
    {
        public static int kra = 4;
        public static int ckycKra = 5;
        public static int contact = 5;
        public static int aaditionaldetails = 6;
        public static int nomineeimg = 8;
        public static int runningacc = 10;
        public static int additionalkyc = 11;
        public static int optionform = 12;
        public static int officeuse = 13;
        public static int bsemf = 6;
    }



    public class CivilKRAFetchModel
    {
        public string? AppName { get; set; }
        public string? AppDOB { get; set; }
        public string? AppGen { get; set; }
        public string? AppPerAdd1 { get; set; }
        public string? AppPerAdd2 { get; set; }
        public string? AppPerAdd3 { get; set; }
        public string? RefDate { get; set; }


    }
}
