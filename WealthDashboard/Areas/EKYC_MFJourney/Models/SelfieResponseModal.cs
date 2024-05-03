namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class SelfieAction
    {
        public string id { get; set; }
        public string action_ref { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string file_id { get; set; }
        public ValidationResult validation_result { get; set; }
        public string completed_at { get; set; }
        public string face_match_obj_type { get; set; }
        public string face_match_status { get; set; }
        public string obj_analysis_status { get; set; }
        public string method { get; set; }
        public bool processing_done { get; set; }
        public int retry_count { get; set; }
        public RulesData rules_data { get; set; }
    }

    public class SelfieResponseData
    {
        public string id { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public string status { get; set; }
        public string customer_identifier { get; set; }
        public List<SelfieAction> actions { get; set; }
        public string reference_id { get; set; }
        public SelfieResponseDetails request_details { get; set; }
        public string transaction_id { get; set; }
        public int expire_in_days { get; set; }
        public bool reminder_registered { get; set; }
        public string workflow_name { get; set; }
        public bool auto_approved { get; set; }
    }

    public class SelfieResponseDetails
    {
    }

    public class SelfieResponseModal
    {
        public int code { get; set; }
        public string message { get; set; }
        public SelfieResponseData data { get; set; }
    }

    public class RulesData
    {
        public List<object> approval_rule { get; set; }
    }

    public class ValidationResult
    {
    }
    public class SelfieFileDownloadModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public byte[] data { get; set; }
    }
}
