namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class CentralizeWorkflowTemplateModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public ResponseData data { get; set; }
    }
    public class Aadhaar
    {
        public string id_number { get; set; }
        public string document_type { get; set; }
        public string id_proof_type { get; set; }
        public string gender { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string current_address { get; set; }
        public string permanent_address { get; set; }
        public CurrentAddressDetails current_address_details { get; set; }
        public PermanentAddressDetails permanent_address_details { get; set; }
    }

    public class ResponseAction
    {
        public string id { get; set; }
        public string action_ref { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string execution_request_id { get; set; }
        public Details details { get; set; }
        public ValidationResult validation_result { get; set; }
        public string completed_at { get; set; }
        public string face_match_obj_type { get; set; }
        public string face_match_status { get; set; }
        public string obj_analysis_status { get; set; }
        public bool processing_done { get; set; }
        public int retry_count { get; set; }
        public RulesData rules_data { get; set; }
    }

    public class CurrentAddressDetails
    {
        public string address { get; set; }
        public string locality_or_post_office { get; set; }
        public string district_or_city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
    }

    public class ResponseData
    {
        public string id { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public string status { get; set; }
        public string customer_identifier { get; set; }
        public List<ResponseAction> actions { get; set; }
        public string reference_id { get; set; }
        public RequestDetails request_details { get; set; }
        public string transaction_id { get; set; }
        public int expire_in_days { get; set; }
        public bool reminder_registered { get; set; }
        public string workflow_name { get; set; }
        public bool auto_approved { get; set; }
    }

    public class Details
    {
        public Aadhaar aadhaar { get; set; }
        public Pan pan { get; set; }
    }

    public class Pan
    {
        public string id_number { get; set; }
        public string document_type { get; set; }
        public string id_proof_type { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
    }

    public class PermanentAddressDetails
    {
        public string address { get; set; }
        public string locality_or_post_office { get; set; }
        public string district_or_city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
    }

    public class ResponseRequestDetails
    {
    }

    public class RulesData
    {
        public List<object> approval_rule { get; set; }
    }

    public class ValidationResult
    {
    }
}
