namespace WealthDashboard.Models
{
    public class StaticValues
    {
        public const string ApplicationJsonMediaType = "application/json";
    }

    public class APIUrl
    {
        public const string InvestNowDetailInsert = "MFUsers/UserCartDetailInsert";
        public const string CreateSrNo = "BSEOrderMaster/CreateSrNo";

        public const string AuthenticationInsert = "OrderAuthentication/InsertAuthMaster";
        public const string SentAuthenticationOTP = "OrderAuthentication/SentAuthenticationOTP";
        public const string Updateresendotp = "OrderAuthentication/UpdateResentOTP";
        public const string InsertBSECraeteOrder = "BSEOrderMaster/InsertBSECraeteOrder";
        public const string RedeemInsert = "BSEOrderMaster/InsertRedeemOrder";
        public const string SendSWPOrder = "BSEOrderMaster/SendSwpOrder";
        public const string LoginV1 = "LoginV1";
        public const string SecurityV2 = "SecurityV2";
    }
}
