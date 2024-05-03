namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class GenerateOTPModel
    {
        public string EmailOTP { get; set; }
        public string MobileOTP { get; set; }
    }
    public class OTPMobileDetailsModel
    {
        public string OTPEmailDetailsID { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string EmailType { get; set; }
        public string SendTo { get; set; }
        public string SendCC { get; set; }
        public string SendBCC { get; set; }
        public string SendFrom { get; set; }
        public string EmailBody { get; set; }
        public string OTPEmail { get; set; }
        public string OTPMobile { get; set; }
        public string EntryBy { get; set; }
    }
    public class LastloginModel
    {
        public string LastloginURL { get; set; }
        public string LastloginID { get; set; }
    }
}
