using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WealthDashboard.Models.Login;

namespace WealthDashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                //var data = HttpContext.Session.GetString("userData").ToString();
                //var result = JsonConvert.DeserializeObject<UserData>(data);
                //var userdetais=new UserData { FirstName=result.FirstName +" "+result.LastName+" "+ result.MiddleName,UserId= result.UserId};
            }
            catch (Exception ex)
            {

            }
         
            return View();
        }
        public IActionResult LMS()
        {
            return View(_configuration);
        }
    }
}
