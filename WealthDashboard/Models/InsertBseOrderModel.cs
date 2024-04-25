namespace WealthDashboard.Models
{
    public class InsertBseOrderModel
    {
        public string ucc { get; set; }
        public string paymentmode { get; set; }
        public string siplumpsum { get; set; }
        public string schemecode { get; set; }
        public string buySell { get; set; }
        public string dematPhy { get; set; }
        public double ordLumpsumValue { get; set; }
        public string sipStartDate { get; set; }
        public double sipAmount { get; set; }
        public int sipInstallMents { get; set; }
        public string empBaLoginType { get; set; }
        public string loginId { get; set; }
        public long fundIntoSrNo { get; set; }
        public string isFirstOrder { get; set; }
        public string foliono { get; set; }
        public string Mandate_SrNo { get; set; }
        public string Mandate_Flag { get; set; }
        public string orderplaced { get; set; }
        public string CommonOrderId { get; set; }
        public string Frequency { get; set; }
    }
}
