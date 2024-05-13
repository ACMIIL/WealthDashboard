using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WealthDashboard.Models.Login;

namespace WealthDashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
    {

       // [Route("dashboar/home")]
        public IActionResult Index()
        {
          var data=   HttpContext.Session.GetString("userData" ).ToString();
          var result = JsonConvert.DeserializeObject<UserData>(data);
           //var userdetais=new UserData { FirstName=result.FirstName +" "+result.LastName+" "+ result.MiddleName,UserId= result.UserId};
            return View();
        }
    }
}
