using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;

namespace WealthDashboard.Controllers
{
    public class PMSandAFIController : Controller
    {
        public IActionResult PMS()
        {
            return View();
        }
        public IActionResult AFI()
        {
            return View();
        }
        public IActionResult ACEChallengers()
        {
            return View();
        }
        public IActionResult ACELeaders()
        {
            return View();
        }
        public IActionResult ACEPayout()
        {
            return View();
        }
        public IActionResult ACEPrimeEquity()
        {
            return View();
        }
    }
}
