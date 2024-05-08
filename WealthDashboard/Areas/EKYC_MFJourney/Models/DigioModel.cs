//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class DigioModel
    {
        public int code { get; set; }
        public string? message { get; set; }
        public Data data { get; set; }
    }
    public class AccessToken
    {
        public string? created_at { get; set; }
        public string? id { get; set; }
        public string? entity_id { get; set; }
        public string? valid_till { get; set; }
    }

    public class Data
    {
        public string? id { get; set; }
        public string? created_at { get; set; }
        public string? status { get; set; }
        public string? customer_identifier { get; set; }
        public string? reference_id { get; set; }
        public RequestDetails? request_details { get; set; }
        public string? transaction_id { get; set; }
        public int expire_in_days { get; set; }
        public bool reminder_registered { get; set; }
        public AccessToken? access_token { get; set; }
        public string? workflow_name { get; set; }
        public bool auto_approved { get; set; }
    }

    public class RequestDetails
    {
    }
    public class CentralizeDigioWorkTemplateRequest
    {
        public string customer_identifier { get; set; }
        public string template_name { get; set; }
        public int RegistrationId { get; set; }
        public bool notify_customer { get; set; }
        public bool generate_access_token { get; set; }
        public string sourceType { get; set; }     
    }
    public class CeDigioResDataPrameter
    {
        public int RegistrationId { get; set; }
        public string ucc { get; set; }
    }
    public class CentralizedigioPrameter
    {
        public string ResponseId { get; set; }
        public string sourceType { get; set; }
    }
}
