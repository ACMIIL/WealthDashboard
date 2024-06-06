namespace WealthDashboard.Areas.EKYC_MFJourney.Models.Login
{
    public class UCCTempModel
    {
        public string Email_Id { get; set; }
        public string MobileNumber { get; set; }
        public string UCC { get; set; }
        public string InwardNo { get; set; }
        public string EncUCC { get; set; }


        public string BACode { get; set; }
        public string EncBACode { get; set; }
        public string SourceType { get; set; }

        public int RegistrationId { get; set; }
        public string OCRToken { get; set; }
        public string EncRegistrationId { get; set; }

        // Personal Details
        public string PANNumber { get; set; }

        public string ClientName { get; set; }
        public string CityName { get; set; }
        public string AadharNo { get; set; }

        public DateTime PANDOB { get; set; }


        public string sToken { get; set; }
        public string SignzyId { get; set; }
        public string TTL { get; set; }
        public string ImageURL { get; set; }

        //Files
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
