namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class SelfieTempalteModal
    {
        public string customer_identifier { get; set; }
        public string template_name { get; set; }
        public int RegistrationId { get; set; }
        public bool notify_customer { get; set; }
        public bool generate_access_token { get; set; }
        public string sourceType { get; set; }

    }
    public class SelfieAccessToken
    {
        public string created_at { get; set; }
        public string id { get; set; }
        public string entity_id { get; set; }
        public string valid_till { get; set; }
    }

    public class SelfieData
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string status { get; set; }
        public string customer_identifier { get; set; }
        public string reference_id { get; set; }
        public SelfieRequestDetails request_details { get; set; }
        public string transaction_id { get; set; }
        public int expire_in_days { get; set; }
        public bool reminder_registered { get; set; }
        public SelfieAccessToken access_token { get; set; }
        public string workflow_name { get; set; }
        public bool auto_approved { get; set; }
    }

    public class SelfieRequestDetails
    {
    }

    public class SelfieRoot
    {
        public int code { get; set; }
        public string message { get; set; }
        public SelfieData data { get; set; }
    }

    public class SelfieStrId
    {
        public string ResponseId { get; set; }
        public string sourceType { get; set; }
    }
}
