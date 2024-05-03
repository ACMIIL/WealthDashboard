namespace WealthDashboard.Areas.EKYC_MFJourney.Models.Common
{
    public class JSONMessageModel
    {
        public string ErrorMessage { get; set; }
        public string ResponseCode { get; set; }

        public List<AssociateLoginData> Data { get; set; }
    }
    public class AssociateLoginData
    {
        public string CommonClientCode { get; set; }
        public string InwardNo { get; set; }
        public string ClientName { get; set; }
    }
}
