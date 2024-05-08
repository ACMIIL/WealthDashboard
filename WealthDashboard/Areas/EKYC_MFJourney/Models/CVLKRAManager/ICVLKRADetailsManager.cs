namespace WealthDashboard.Areas.EKYC_MFJourney.Models.CVLKRAManager
{
    public interface ICVLKRADetailsManager
    {

        Task<string> GetCVLDATA(CVLKRAReqModel mCVLKRAReqModel);

        Task<CVLKRAResponsexmlDataModel> GetResponseCVLXMLData(string RegistrationId);
    }
}
