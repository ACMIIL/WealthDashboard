namespace WealthDashboard.Models
{
    public class SwpOrderModel
    {
        public int UCC { get; set; }
        public string ISIN { get; set; }
        public string Pan { get; set; }
        public string TrnMode { get; set; }
        public decimal Volume { get; set; }
        public decimal Amount { get; set; }
        public string SDate { get; set; }
        public decimal Unit { get; set; }
        public string FolioNo { get; set; }
        public string MinRed { get; set; }
        public string nosip { get; set; }
        public string SchemeName { get; set; }
        public string OrdPlacedBy { get; set; }
        public long Device { get; set; }
    }
}
