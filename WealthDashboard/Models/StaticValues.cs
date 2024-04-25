namespace WealthDashboard.Models
{
    public class StaticValues
    {
        public const string ApplicationJsonMediaType = "application/json";
    }

    public class APIUrl
    {
        public const string InvestNowDetailInsert = "api/MFUsers/UserCartDetailInsert";
        public const string CreateSrNo = "api/BSEOrderMaster/CreateSrNo";

        public const string AuthenticationInsert = "api/OrderAuthentication/InsertAuthMaster";
        public const string SentAuthenticationOTP = "api/OrderAuthentication/SentAuthenticationOTP";
        public const string Updateresendotp = "api/OrderAuthentication/UpdateResentOTP";
        public const string InsertBSECraeteOrder = "api/BSEOrderMaster/InsertBSECraeteOrder";
        public const string RedeemInsert = "api/BSEOrderMaster/InsertRedeemOrder";
        public const string SendSWPOrder = "api/BSEOrderMaster/SendSwpOrder";
        public const string LoginV1 = "api/LoginV1";
        public const string SecurityV2 = "api/SecurityV2";
    }
}
