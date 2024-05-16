namespace WealthDashboard.Models
{
    public class InsertRedeemOrderModel
    {
        public string ucc { get; set; }
        public string isin { get; set; }
        public string Volume { get; set; }
        public string TrnMode { get; set; }
        public string pan { get; set; }
        public decimal units { get; set; }
        public decimal amount { get; set; }
        public string folioNo { get; set; }
        public string MinRed { get; set; }
        public string SchemeName { get; set; }
        public string OrdPlacedBy { get; set; }
        public string Device { get; set; }
    }
}
