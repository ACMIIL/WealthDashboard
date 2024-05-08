using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Encryption;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.NomineeManager;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class NomineeController : Controller
    {
        #region global variable
        private readonly INomineeManager _nomineelogger;
        private readonly ILogger<NomineeController> _logger;
        private readonly Appsetting _appsetting;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        #endregion

        #region
        public NomineeController(INomineeManager nomineeManager, ILogger<NomineeController> nomineelogger, IOptions<Appsetting> appsettings, IClientRegistrationManager clientRegistrationManager)
        {
            _appsetting = appsettings.Value;
            _nomineelogger = nomineeManager;
            _logger = nomineelogger;
            _clientRegistrationManager = clientRegistrationManager;
        }

        #endregion
        public IActionResult Nominee_Details()
        {

            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"].ToString().Replace(' ', '+');
            mUCCTempModel.RegistrationId = Convert.ToInt32(Encryption.Decrypt(mUCCTempModel.EncRegistrationId.Replace(' ', '+')));
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(mUCCTempModel.RegistrationId);
            mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"].ToString().Replace(' ', '+');
            return View(mUCCTempModel);
        }

        public async Task<JsonResult> GetByPincode(string Pincode)
        {
            PincodeMasterModel mpincodeMasterModel = new PincodeMasterModel();
            mpincodeMasterModel = await _nomineelogger.GetPincodeMaster(Pincode);

            return Json(mpincodeMasterModel);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
