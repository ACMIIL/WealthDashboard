namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class AddressCodeModel
    {
        public string? pinCode { get; set; }
        public string? cityName { get; set; }
        public string? stateName { get; set; }
        public string? countryName { get; set; }
        public int city_Code { get; set; }
        public int state_Code { get; set; }
        public int country_Code { get; set; }
        public int stateId { get; set; }
        public int cityId { get; set; }
    }
}
