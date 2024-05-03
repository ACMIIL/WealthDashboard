using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.LoginManager
{
    public interface ILoginManager
    {
        Task<GenerateOTPModel> GenerateOtp();
        Task<UCCTempModel> save_ucc_temp_details(TempModal tempModal);
        Task<OTPMobileDetailsModel> SaveOtpDetails(OTPMobileDetailsModel mOTPEMailsMobileDetailsModel);

        Task<JSONMessageModel> CheckMobileDeclarationWithUCC(string MobileNo, string DeclarationType);
        Task<JSONMessageModel> CheckemailDeclarationWithUCC(string email, string DeclarationType);
        Task<string> SaveEmaildECLARATION(int UCC, string Declaration, string EmailId);
        Task<OTPMobileDetailsModel> CheckOTP(string EmailId, string MobileNo);
        Task<JSONMessageModel> SaveEmailDetails(string email, string Mobileno, string Emailtype, string sendto,
            string sendfrom, string emailBody, string otpemail, string entryby);

        Task<LastloginModel> LoginLastVisit(string Mobile);
    }
}
