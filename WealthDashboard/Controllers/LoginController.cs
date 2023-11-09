using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginWithMobile()
        {
            return View();
        }
    }
}
