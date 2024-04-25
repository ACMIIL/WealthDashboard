namespace WealthDashboard.Models
{
    public class PaymentrequestModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string URL { get; set; }
        public string TransactionNumber { get; set; }
    }
}
