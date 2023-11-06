using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Controllers
{
    public class MutualFundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("MutualFund/Main")]
        public IActionResult Mutual_Fund()
        {
            return View();
        }
        [Route("MutualFund/Event")]
        public IActionResult Mf_sub_event_mf()
        {
            return View();
        }

        [Route("MutualFund/Screener")]
        public IActionResult Mf_event_mf_screener()
        {
            return View();
        }

        [Route("MutualFund/AdvicedFund")]
        public IActionResult Mf_event_mf_Advised_Fund()
        {
            return View();
        }

        [Route("MutualFund/GlobalSearch")]
        public IActionResult MFGlobalSearch()
        {
            return View();
        }
    }
}
