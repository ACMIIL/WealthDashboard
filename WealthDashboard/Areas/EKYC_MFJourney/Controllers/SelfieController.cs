using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Encryption;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SelfieManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class SelfieController : Controller
    {
        #region global variable
        private readonly ISelfieManagerModel _selfiemanager;
        private readonly ILogger<SelfieController> _selfielogger;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        private readonly Appsetting _appsetting;
        #endregion

        #region ctor
        public SelfieController(ISelfieManagerModel selfiemanager, ILogger<SelfieController> selfielogger,
            IOptions<Appsetting> appsetting, IClientRegistrationManager clientRegistrationManager)
        {
            _selfiemanager = selfiemanager;
            _selfielogger = selfielogger;
            _appsetting = appsetting.Value;
            _clientRegistrationManager = clientRegistrationManager;

        }
        #endregion

        public ActionResult SelfieView()
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"].ToString().Replace(' ', '+');
            mUCCTempModel.RegistrationId = Convert.ToInt32(Encryption.Decrypt(mUCCTempModel.EncRegistrationId.Replace(' ', '+')));
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(mUCCTempModel.RegistrationId);
            mUCCTempModel.EncRegistrationId = Encryption.Encrypt(mUCCTempModel.RegistrationId.ToString());
            return View(mUCCTempModel);
        }

        public ActionResult SelfieDigioVerificationView()
        {
            return View();
        }

        public async Task<JsonResult> SelfieDigioWorkTemplate(SelfieTempalteModal mSelfieTempalteModal)
        {
            SelfieRoot mSelfieRoot = new SelfieRoot();

            mSelfieTempalteModal.template_name = "LIVE_DEMO";
            mSelfieTempalteModal.notify_customer = false;
            mSelfieTempalteModal.generate_access_token = true;
            mSelfieTempalteModal.sourceType = "ACMIIL-EKYC";

            mSelfieRoot = await _selfiemanager.SelfieDigioWorkTemplate(mSelfieTempalteModal);

            string Message = await _selfiemanager.SelfieInsertUpdateDigioTemplate(mSelfieTempalteModal.RegistrationId, mSelfieRoot);
            if (Message != "Failed")
            {
                return Json(mSelfieRoot);
            }
            else
            {
                _selfielogger.LogError("Selfie  Request Failed");
                return Json(StaticValues.MobileDetailsInvalid);
            }
        }
        public async Task<JsonResult> SelfieDigioResponseData(int RegistrationId, string InwardNo)
        {

            SelfieResponseModal mSelfieResponseModal = new SelfieResponseModal();
            SelfieStrId mSelfieStrId = new SelfieStrId();
            try
            {
                string strId = await _selfiemanager.GetRequestData(RegistrationId);

                mSelfieStrId.ResponseId = strId;
                mSelfieStrId.sourceType = "ACMIIL-EKYC";

                mSelfieResponseModal = await _selfiemanager.SelfieSaveResponseData(mSelfieStrId);
                string Message = await _selfiemanager.SelfieInsertUpdateDigioTemplate(RegistrationId, mSelfieResponseModal);

                if (Message == "OK")
                {
                    SelfieAction mSelfieAction = new SelfieAction();
                    mSelfieAction = mSelfieResponseModal.data.actions[0];
                    string Filemessage = await _selfiemanager.GetSelfieFileData(mSelfieAction, RegistrationId, InwardNo);
                    if (Filemessage == "OK")
                    {
                        //HttpContext.Session.SetString("ImageURL", "https://web.investmentz.com/imgupload/" + InwardNo + "//"+InwardNo + "_PassportPhoto.png");
                        return Json(Filemessage);
                    }
                    else
                    {
                        _selfielogger.LogError(Filemessage.ToString());
                    }
                }
                else
                {
                    _selfielogger.LogError("Selfie Response Failed");
                    return Json(StaticValues.MobileDetailsInvalid);
                }
            }
            catch (Exception ex)
            {
                _selfielogger.LogError($"Error: {ex.Message}");
            }

            return Json(mSelfieStrId);
        }

        public async Task<JsonResult> InsertorUpdateSelfieData(int RegistrationId, string InwardNo)
        {
            string Msg = await _selfiemanager.SaveSelfieDetails(RegistrationId, InwardNo);
            return Json(Msg);
        }

        public async Task<JsonResult> SaveGeoDetails(int RegistrationId)
        {
            string Msg = await _selfiemanager.GetGeoInfo(RegistrationId);
            return Json(Msg);
        }

        public async Task<JsonResult> CheckSelfieDetails(int RegistrationId)
        {
            string Msg = await _selfiemanager.ChechSelfieDetails(RegistrationId);
            return Json(Msg);
        }

        public async Task<JsonResult> CheckEsignStatus(int RegistrationId)
        {
            string Msg = await _selfiemanager.CheckEsignStatus(RegistrationId);
            return Json(Msg);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
