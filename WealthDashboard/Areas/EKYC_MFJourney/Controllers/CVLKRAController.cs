using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WealthDashboard.Areas.EKYC_MFJourney.Models;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using WealthDashboard.Areas.EKYC_MFJourney.Models.CVLKRAManager;
using WealthDashboard.Configuration;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class CVLKRAController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICVLKRADetailsManager _cvLKRADetailsManager;
        private readonly Appsetting _appsetting;

        public CVLKRAController(ILogger<HomeController> logger, ICVLKRADetailsManager cVLKRADetailsManager, IOptions<Appsetting> appsetting)
        {
            _logger = logger;
            _cvLKRADetailsManager = cVLKRADetailsManager;
            _appsetting = appsetting.Value;
        }

        public async Task<JsonResult> GetCVLDATA(string RegistrationId, string PAN, string DOB)
        {
            CVLKRAReqModel mCVLKRAReqModel = new CVLKRAReqModel();
            mCVLKRAReqModel.RegistrationId = RegistrationId;
            mCVLKRAReqModel.PAN = PAN;
            mCVLKRAReqModel.DOB = DOB;
            string StrMsg = await _cvLKRADetailsManager.GetCVLDATA(mCVLKRAReqModel);
            CVLKRAResponsexmlDataModel mCVLKRAResponsexmlDataModel = new CVLKRAResponsexmlDataModel();
            if (StrMsg == "OK")
            {
                mCVLKRAResponsexmlDataModel = await _cvLKRADetailsManager.GetResponseCVLXMLData(RegistrationId);
                if (mCVLKRAResponsexmlDataModel.Fullname == "" || mCVLKRAResponsexmlDataModel.Fullname == null)
                {
                    return Json(StaticValues.MobileDetailsInvalid);
                }
                return Json(mCVLKRAResponsexmlDataModel);
            }
            else
            {
                _logger.LogError("Failed Mobile Details Invalid");
                return Json(StaticValues.MobileDetailsInvalid);
            }
        }
    }
}
