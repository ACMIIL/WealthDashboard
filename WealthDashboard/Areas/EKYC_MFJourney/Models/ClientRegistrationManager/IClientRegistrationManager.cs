using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager
{
    public interface IClientRegistrationManager
    {
        UCCTempModel GetNewClientDetails(string UCC);
        UCCTempModel GetUCCByRegistrationID(int Rid);
    }
}
