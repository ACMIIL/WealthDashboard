namespace WealthDashboard.Models
{
    public class InvestNowDetailInsertModel
    {
        public string UserSrNo { get; set; }
        public string UCC { get; set; }
        public string SchemeCode { get; set; }
        public string SchemeName { get; set; }
        public string TransactionType { get; set; }
        public string TransactionMode { get; set; }
        public decimal? Amount { get; set; }
        public string FolioNo { get; set; }
        public string SIPInstallment { get; set; }
        public string SIPStartDate { get; set; }
        public string FirstOrderToday { get; set; }
        public string Frequency { get; set; }
        public string PaymentMode { get; set; }
        public string OrderBy { get; set; }
        public string ISIN { get; set; }
    }

    public class srModel
    {
        //public HttpStatusCode Code { get; set; }

        public string uDsrno { get; set; }

        public object udPanno_s { get; set; }
    }
}
