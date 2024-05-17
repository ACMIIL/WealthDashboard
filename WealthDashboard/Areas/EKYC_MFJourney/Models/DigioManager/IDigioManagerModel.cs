using WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioManager
{
    public interface IDigioManagerModel
    {
        Task<DigioModel> CentralizeDigioWorkTemplate(CentralizeDigioWorkTemplateRequest centralizeDigioWorkTemplateRequest);
        Task<string> CentralizeInsertUpdateDigioTemplate(int RegistrationId, DigioModel digioModel);
        Task<string> DigioApprovedPANandAAharResponseData(int RegistrationId, CentralizeWorkflowTemplateModel centralizeWorkflowTemplateModel);
        Task<CentralizeWorkflowTemplateModel> CentralizeDigioResponseData(CentralizedigioPrameter centralizedigioPrameter);
        Task<string> CentralizeGetDigiotemplateDetails(int RegistrationId);
        Task<string> InsertPersonalDetails(ClientsPersonalDetailsModel clientsPersonalDetailsModel);
        Task<AddressCodeModel> AddressCodeDetails(string pincode);
        Task<string> InsertOrUpdateClientAddressDetails(InsertOrUpdateClientAddressDetailsModel insertOrUpdateClientAddressDetailsModel);
        Task<string> InsertOrUpdateClientRelationDetails(List<ClientRelationModel> clientRelationModel);
        Task<string> InsertOrUpdateInvestmentAndFatcaDetails(FatcadetailModel fatcadetailModel);

        Task<string> GetDigioFileData(int RegistrationId, string execution_request_id, string ucc);
        Task<DigioStatus> CheckDigioStatus(int RegistrationID);
    }
}
