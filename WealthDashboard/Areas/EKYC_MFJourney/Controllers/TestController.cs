using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
