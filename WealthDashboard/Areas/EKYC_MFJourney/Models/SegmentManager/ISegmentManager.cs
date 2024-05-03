using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentModel;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager
{
    public interface ISegmentManager
    {
        Task<DepositoryMasterResponse> GetDepositoryMaster(string option, string dpId);
        Task<string> InsertOrupdateClentsegment(List<SegmentData> segmentData);
        Task<string> InsertOrUpdateSegment(ReqInsertSegment reqInsertSegment);
        Task<string> InsertOrUpdateClientUploadData(SegmentUploadModel segmentUploadModel);
        Task<string> UpdateBrokarageplan(int RID, int tarrifplan, int Brockrageplan);
        Task<string> Update_BACode(int RID, string Bacode);
        Task<List<brockragedrp>> Brockarageplan();
    }
}
