using Microsoft.AspNetCore.Mvc;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager;

namespace WealthDashboard.Areas.WP_Registration.Controllers
{
    [Area("WP_Registration")]
    public class WPRegistrationController : Controller
    {

        private readonly IConfiguration _configuration;

        public WPRegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View(_configuration);
        }
        public IActionResult VerifyOTP()
        {
            return View(_configuration);
        }

        public IActionResult PanVerification()
        {
            return View(_configuration);
        }

        public IActionResult ARNdetails()
        {
            return View(_configuration);
        }
        public IActionResult DigiLocker()
        {
            return View(_configuration);
        }
        public IActionResult QRBankVerification()
        {
            return View(_configuration);
        }
        public IActionResult UploadChequeBankVerification()
        {
            return View(_configuration);
        }

        public IActionResult SelfieVerification()
        {
            return View(_configuration);
        }

        public IActionResult SignatureVerification()
        {
            return View(_configuration);
        }

        public IActionResult QRBankDetails()
        {
            return View(_configuration);
        }

        public JsonResult CreateSession(string userid)
        {
            // Set session variable
            HttpContext.Session.SetString("UserId", userid);
            return Json(1);
        }

        public IActionResult PersonalDetails()
        {
            return View(_configuration);
        }

        public IActionResult Thankyou()
        {
            return View(_configuration);
        }
        public IActionResult QRFail()
        {
            return View(_configuration);
        }
        public IActionResult PanyDrop()
        { return View(_configuration); }

        public IActionResult UploadPersonalData()
        { 
            return View(_configuration); 
        }

    }
}
