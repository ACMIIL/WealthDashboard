namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class XMLData
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string result { get; set; }
        public string xmlContent { get; set; }
    }
    public class DigioStatus
    {
        public string pan { get; set; }

    }
}
