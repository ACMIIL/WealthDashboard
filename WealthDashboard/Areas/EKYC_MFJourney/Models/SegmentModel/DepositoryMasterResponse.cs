namespace WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentModel
{
    public class DepositoryMasterResponse
    {
        public List<DPIDInfo> DPID { get; set; }
    }
    public class DPIDInfo
    {
        public string Depository { get; set; }
        public string DPID { get; set; }
        public string DPName { get; set; }
    }


}
