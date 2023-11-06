using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Controllers
{
    public class MutualFundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Mutual_Fund()
        {
            return View();
        }
        public IActionResult Mf_sub_event_mf()
        {
            return View();
        }
        public IActionResult Mf_event_mf_screener()
        {
            return View();
        }
        public IActionResult Mf_event_mf_Advised_Fund()
        {
            return View();
        }
    }
}
