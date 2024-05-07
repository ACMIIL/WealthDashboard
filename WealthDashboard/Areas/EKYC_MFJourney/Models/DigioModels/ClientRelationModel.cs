namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class ClientRelationModel
    {
        public int relationTypeId { get; set; }
        public int registrationId { get; set; }
        public string relationFirstName { get; set; }
        public string relationMiddleName { get; set; }
        public string relationLastName { get; set; }
        public string userId { get; set; }
    }
}
