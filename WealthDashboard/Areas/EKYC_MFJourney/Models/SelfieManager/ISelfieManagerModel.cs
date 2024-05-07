namespace WealthDashboard.Areas.EKYC_MFJourney.Models.SelfieManager
{
    public interface ISelfieManagerModel
    {
        Task<SelfieRoot> SelfieDigioWorkTemplate(SelfieTempalteModal mSelfieTempalteModal);
        Task<string> SelfieInsertUpdateDigioTemplate(int RegistrationId, SelfieRoot mSelfieRoot);

        Task<string> GetRequestData(int RegistrationId);
        Task<SelfieResponseModal> SelfieSaveResponseData(SelfieStrId mSelfieStrId);

        Task<string> SelfieInsertUpdateDigioTemplate(int RegistrationId, SelfieResponseModal mSelfieResponseModal);

        Task<string> GetSelfieFileData(SelfieAction mSelfieAction, int RegistrationId, string InwardNo);
        Task<string> GetGeoInfo(int RegistrationId);
        Task<string> SaveSelfieDetails(int RegistrationId, string InwardNo);

        Task<string> ChechSelfieDetails(int RegistrationId);
        Task<string> CheckEsignStatus(int RegistrationId);
    }
}
