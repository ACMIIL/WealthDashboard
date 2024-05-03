using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using WealthDashboard.Areas.EKYC_MFJourney.Models;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models.LoginManager;
using WealthDashboard.Configuration;
using WealthDashboard.Models;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class LoginController : Controller
    {
        #region global variable
        private readonly ILoginManager _loginManager;
        private readonly ILogger<LoginController> _logger;
        private readonly Appsetting _appsetting;
        #endregion

        #region ctor
        public LoginController(ILoginManager loginManager, ILogger<LoginController> Loginlogger, IOptions<Appsetting> appsetting)
        {
            _loginManager = loginManager;
            _logger = Loginlogger;
            _appsetting = appsetting.Value;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GenerateMobileOTP(string EmailId, string MobileNo)
        {
            GenerateOTPModel mGenerateOTPModel = new GenerateOTPModel();
            OTPMobileDetailsModel mOTPEMailsMobileDetailsModel = new OTPMobileDetailsModel();
            try
            {
                mGenerateOTPModel = await _loginManager.GenerateOtp();
                mOTPEMailsMobileDetailsModel.MobileNo = MobileNo;
                mOTPEMailsMobileDetailsModel.OTPMobile = mGenerateOTPModel.MobileOTP;
                mOTPEMailsMobileDetailsModel = await _loginManager.SaveOtpDetails(mOTPEMailsMobileDetailsModel);
                if (_appsetting.IsLiveEnvironment.ToString() == "Y")
                {
                    SendMobileOTP(mGenerateOTPModel.MobileOTP, MobileNo);
                }
                else
                {
                    mGenerateOTPModel.MobileOTP = "123456";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateMobileOTP" + ex.ToString());
            }
            return Json(mOTPEMailsMobileDetailsModel);
        }

        public void SendMobileOTP(string mobileOTP, string mobileNo)
        {
            try
            {
                string OTPMsg = mobileOTP + " is your OTP. This OTP can be used only once. - www.Investmentz.com";
                if (_appsetting.IsLiveEnvironment.ToString() == "Y")
                {
                    using (var client = new System.Net.WebClient()) //WebClient  
                    {
                        client.Headers.Add("Content-Type:application/json"); //Content-Type  
                        client.Headers.Add("Accept:application/json");
                        var result = client.DownloadString("https://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=acmiil&pass=acmiil&appid=acmiil&subappid=acmiil&contenttype=1&to=" + mobileNo + "&from=ACMIIL&text=" + OTPMsg + "&selfid=true&alert=1&dlrreq=true"); //URI                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SendMobileOTP" + ex.ToString());
            }
        }

        public async Task<JsonResult> save_ucc_temp_details(TempModal tempModal)
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            try
            {
                mUCCTempModel = await _loginManager.save_ucc_temp_details(tempModal);
            }
            catch (Exception ex)
            {
                _logger.LogError("save_ucc_temp_details" + ex.ToString());
            }
            return Json(mUCCTempModel);
        }

        public async Task<JsonResult> CheckMobileDeclarationWithUCC(string MobileNo, string DeclarationType)
        {
            JSONMessageModel mJSONMessageModel = new JSONMessageModel();
            mJSONMessageModel = await _loginManager.CheckMobileDeclarationWithUCC(MobileNo, DeclarationType);
            return Json(mJSONMessageModel);
        }

        public async Task<JsonResult> CheckOTP(string EmailId, string MobileNo)
        {
            OTPMobileDetailsModel mOTPEMailsMobileDetailsModel = new OTPMobileDetailsModel();
            mOTPEMailsMobileDetailsModel = await _loginManager.CheckOTP(EmailId, MobileNo);
            return Json(mOTPEMailsMobileDetailsModel);
        }

        public async Task<JsonResult> CheckOTPemail(string EmailId, string MobileNo, string DeclarationType, int UCC, string enterotp)
        {
            OTPMobileDetailsModel mOTPEMailsMobileDetailsModel = new OTPMobileDetailsModel();
            JSONMessageModel mJSONMessageModel = new JSONMessageModel();

            mOTPEMailsMobileDetailsModel = await _loginManager.CheckOTP(EmailId, MobileNo);
            if (mOTPEMailsMobileDetailsModel.OTPEmail == enterotp)
            {
                mJSONMessageModel = await _loginManager.CheckemailDeclarationWithUCC(EmailId, DeclarationType);
                if (mJSONMessageModel.ResponseCode == "0")
                {
                    await _loginManager.SaveEmaildECLARATION(UCC, DeclarationType, EmailId);
                    // mOTPEMailsMobileDetailsModel = await _loginManager.CheckOTP(EmailId, MobileNo);
                    return Json(mJSONMessageModel);
                }
                else
                {
                    return Json(mJSONMessageModel);
                }
            }
            else
            {
                mJSONMessageModel.ErrorMessage = "Incorrect Otp";
                mJSONMessageModel.ResponseCode = "0";
            }
            return Json(mJSONMessageModel);
        }
        public async Task<JsonResult> LoginLastVisit(string MobileNo)
        {
            LastloginModel mLastloginModel = new LastloginModel();
            mLastloginModel = await _loginManager.LoginLastVisit(MobileNo);
            return Json(mLastloginModel);
        }
        public async Task<JsonResult> SendOtpToEmail(string email, string mobile)
        {
            try
            {
                GenerateOTPModel mGenerateOTPModel = new GenerateOTPModel();
                mGenerateOTPModel = await _loginManager.GenerateOtp();
                SendEmailOTP(mGenerateOTPModel.EmailOTP, email, mobile);
                return Json("Send OTP");

            }
            catch (Exception ex)
            {
                _logger.LogError("CheckemailDeclarationWithUCC" + ex.ToString());
                _logger.LogError("CheckemailDeclarationWithUCC" + ex.StackTrace);
                return Json(ex.ToString());
            }
        }

        public void SendEmailOTP(string StrEmailOTP, string EmailID, string mobile)
        {
            try
            {
                EmailTemplate mEmailTemplate = new EmailTemplate();
                MailMessage msg = new MailMessage();
                string StrFromAddress = _appsetting.FromEmailId;
                string StrSmtpCleint = _appsetting.smtpClientEmail;
                string strLoginId = _appsetting.EmailLoginId;
                string strPassword = _appsetting.EmailPassword;
                msg.From = new MailAddress(StrFromAddress);
                msg.To.Add(EmailID);
                msg.Subject = _appsetting.AccountOpenSubject;
                msg.Body = mEmailTemplate.EmailAccountOpen(StrEmailOTP);
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;
                SmtpClient smt = new SmtpClient(StrSmtpCleint);
                smt.Credentials = new System.Net.NetworkCredential(strLoginId, strPassword);
                smt.Send(msg);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var otp = _loginManager.SaveEmailDetails(EmailID, mobile, "AccountOpen", EmailID, StrFromAddress, msg.Body, StrEmailOTP, "Keyur");
            }
            catch (Exception ex)
            {
                _logger.LogError("SendEmailOTP" + ex.ToString());
                _logger.LogError("SendEmailOTP" + ex.StackTrace);
            }
        }
        


    }
}
