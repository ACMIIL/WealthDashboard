namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class FatcadetailModel
    {
        public int clientFatcaId { get; set; }
        public int registrationId { get; set; }
        public int investmentExperienceId { get; set; }
        public int annualIncomeId { get; set; }
        public string networth { get; set; }
        public int tarrifPlan { get; set; }
        public int brokragePlan { get; set; }
        public int dpStatus { get; set; }
        public int dpSubstatus { get; set; }
        public int modeOfholdingId { get; set; }
        public int sourceOfWealthId { get; set; }
        public bool politicallyExposePerson { get; set; }
        public int countryMasterId { get; set; }
        public bool isYourCountryTAXResidencyOtherThenIndia { get; set; }
        public bool consentForCollateralLimitMargin { get; set; }
        public string userId { get; set; }
    }
}
