namespace WealthDashboard.Areas.EKYC_MFJourney.Models.NomineeManager
{
    public interface INomineeManager
    {
        Task<PincodeMasterModel> GetPincodeMaster(string Pincode);
    }
}
