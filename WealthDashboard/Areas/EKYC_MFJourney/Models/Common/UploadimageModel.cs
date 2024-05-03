namespace WealthDashboard.Areas.EKYC_MFJourney.Models.Common
{
    public class UploadimageModel
    {
        public IFormFile PanImage { get; set; }
        public IFormFile CheckImage { get; set; }
        public IFormFile SignImage { get; set; }
        public int registrationId { get; set; }
    }
}
