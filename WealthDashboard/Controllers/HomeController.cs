using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WealthDashboard.Models;

namespace WealthDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}