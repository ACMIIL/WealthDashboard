namespace WealthDashboard.Areas.EKYC_MFJourney.Models.PDFManager
{
    public class GenarateEsignModel
    {
        public string ucc { get; set; }
        public string firstHolderName { get; set; }
        public string cityName { get; set; }
        public string inwardno { get; set; }
        public int signMode { get; set; }
        public int eSignTypeId { get; set; }
        public int pageNo { get; set; }
    }
}
