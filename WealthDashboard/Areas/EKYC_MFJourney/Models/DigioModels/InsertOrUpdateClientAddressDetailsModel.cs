namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class InsertOrUpdateClientAddressDetailsModel
    {
        public int registrationId { get; set; }
        public int addressTypeId { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public int cityId { get; set; }
        public int stateId { get; set; }
        public int pinCodeMasterId { get; set; }
        public int countryMasterId { get; set; }
        public string userId { get; set; }
    }
}
