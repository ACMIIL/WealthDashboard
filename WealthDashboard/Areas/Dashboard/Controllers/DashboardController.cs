using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WealthDashboard.Configuration;
using WealthDashboard.Models;

namespace WealthDashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Appsetting _appSetting;
        public DashboardController(IConfiguration configuration, IOptions<Appsetting> appSetting)
        {
            _configuration = configuration;
            _appSetting = appSetting.Value;
        }
        public IActionResult Index()
        {
            try
            {
                //var data = HttpContext.Session.GetString("userData").ToString();
                //var result = JsonConvert.DeserializeObject<UserData>(data);
                //var userdetais=new UserData { FirstName=result.FirstName +" "+result.LastName+" "+ result.MiddleName,UserId= result.UserId};
            }
            catch (Exception)
            {

            }
            return View(_configuration);
        }
        public IActionResult LMS()
        {
            return View(_configuration);
        }
        [HttpGet]
        public async Task<IActionResult> indexdeta()
        {
            var Condaition = "";
            var data = ""; string apiUrl = _appSetting.URL + "Dashboard/GetWPTotalRevanue"; Result res = new Result();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl); client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                data = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var stuff = JsonConvert.DeserializeObject(data);
                    Condaition = stuff.ToString();



                }
                else
                {
                    Console.WriteLine("API call failed: " + response.StatusCode);
                }
            }
            return Json(Condaition);


        }

    }
}
