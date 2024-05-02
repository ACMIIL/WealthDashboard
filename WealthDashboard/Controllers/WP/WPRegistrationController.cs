using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Controllers.WP
{
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
       
        public JsonResult CreateSession( string userid)
        {
            // Set session variable
            HttpContext.Session.SetString("UserId", userid );
            return Json(1);
        }
        public IActionResult PersonalDetails()
        {
            return View(_configuration);
        }
         
    }
}
