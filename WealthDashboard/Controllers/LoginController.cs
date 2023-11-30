using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult OTPVerify()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
