namespace WealthDashboard.Models
{
    public class AuthenticationInsert
    {
        public string UCC { get; set; }
        public string TransactionType { get; set; }
    }

    public class SentAuthenticationOTPModel
    {
        public string UCC { get; set; }
        public string TransactionType { get; set; }
        public string SchemeName { get; set; }
        public string SchemeCode { get; set; }
        public string Option { get; set; }
        public Int64? CommonOrderID { get; set; }
    }

    public class SentAuthenticationOTPDEtailsModel
    {
        public string UCC { get; set; }
        public string FholdOTP { get; set; }
        public string FIsValidOTP { get; set; }
        public string SHoldOTP { get; set; }
        public string SIsValidOTP { get; set; }
        public string THoldOTP { get; set; }
        public string TIsValidOTP { get; set; }
        public string BSEOrdType { get; set; }
        public string SchemeCode { get; set; }
        public string SchemeName { get; set; }
        public string SIPLumpum { get; set; }
        public string FMobileNumber { get; set; }
        public string SMobileNumber { get; set; }
        public string TMobileNumber { get; set; }
        public string FEmailID { get; set; }
        public string SEmailID { get; set; }
        public string TEmailID { get; set; }
        public Int64 CommonOrderID { get; set; }
        public Int16 Holders { get; set; }
    }
    public class RsendOTP
    {
        public string CommonOrderID { get; set; }
        public string Holders { get; set; }
        public string TransactionType { get; set; }
        public string UCC { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
    }
    public class ResultResendotp
    {
        public string holder { get; set; }
        public string holdOTP { get; set; }
    }

    public class ENCUCCM
    {
        public string? ucc { get; set; }
    }
}
