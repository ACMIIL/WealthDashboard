using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using WealthDashboard.Models;
using WealthDashboard.Models.Login;

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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     
        public async Task<IActionResult> Login(string userid,  string password)
        {
            try
            {
                var client = new HttpClient();
                var query = new { userID = userid, password = password };
                var data = JsonConvert.SerializeObject(query);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://uattwcapi.wealthcompany.in/api/Admin/UserAuthenticationadmin");
                var content = new StringContent(data, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                LoginResponse result = JsonConvert.DeserializeObject<LoginResponse>(jsonString);
                UserData users = JsonConvert.DeserializeObject<UserData>(result.data);
                if (result.statusCode == 200)
                {
                    HttpContext.Session.SetString("userData",  result.data);

                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        public  IActionResult GetSessionData()
        {
            //var data = Context.Session.GetString("userData");
          
            var data = HttpContext.Session.GetString("userData");

          //  var result = JsonConvert.DeserializeObject<WealthDashboard.Models.Login.UserData>(data);
            return Ok(data);
        }
    }
}