namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class CVLKRA_ResponseData
    {
        public int code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

    public class CVLKRAResponseDataModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public CVLKRA_ResponseData data { get; set; }
    }

}
