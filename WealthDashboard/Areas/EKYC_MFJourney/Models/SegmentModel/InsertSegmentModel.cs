namespace WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentModel
{
    public class InsertSegmentModel
    {
        public string Segmentdta { get; set; }
        public string DPID { get; set; }
        public string BOID { get; set; }
        public string DepositoryName { get; set; }
        public string UserId { get; set; }
        public int OnluMf { get; set; }
        public int RID { get; set; }

        public int tarrifplan { get; set; }
        public int BrokragePlan { get; set; }
        public int Residency { get; set; }
        public int PoliticallyExposePerson { get; set; }
        public int settlementfrequency { get; set; }

        public IFormFile? IncomeProof { get; set; }
        public IFormFile? CMLfile { get; set; }
    }
    public class SegmentData
    {
        public int clientSegmentId { get; set; }
        public int registrationId { get; set; }
        public int segmentMasterId { get; set; }
        public bool isActive { get; set; }
        public string userId { get; set; }
    }
    public class ReqInsertSegment
    {
        public string DPID { get; set; }
        public string BOID { get; set; }
        public string DepositoryName { get; set; }
        public string UserId { get; set; }
        public int OnluMf { get; set; }
        public int RID { get; set; }
        public int Residency { get; set; }
        public int PoliticallyExposePerson { get; set; }
        public int settlementfrequency { get; set; }
    }
    public class SegmentUploadModel
    {
        public int clientUploadDataId { get; set; }
        public int uploadDocumentId { get; set; }
        public string docName { get; set; }
        public int registrationId { get; set; }
        public string uploadDocNo { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public bool isActive { get; set; }
        public string userId { get; set; }
    }

    public class InsertUpdateReqModel
    {
        public int registrationId { get; set; }
        public string accountNo { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public string micrCode { get; set; }
        public int accountTypeId { get; set; }
        public int ifscMasterId { get; set; }
        public string ifscCode { get; set; }
        public bool defaultBank { get; set; }
        public string upiId { get; set; }
        public bool isActive { get; set; }
        public string userId { get; set; }
        public string refId { get; set; }
        public bool verified { get; set; }
        public DateTime verified_at { get; set; }
        public string beneficiary_name_with_bank { get; set; }
        public bool fuzzy_match_result { get; set; }
        public int fuzzy_match_score { get; set; }
    }

    public class brockragedrp
    {
        public int TariffPlanId { get; set; }
        public string PlanDescription { get; set; }
    }
}
