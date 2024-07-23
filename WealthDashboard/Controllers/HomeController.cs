using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using WealthDashboard.Models;
using WealthDashboard.Models.Login;

namespace WealthDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CommanModel _commanModel;
        public HomeController(CommanModel commanModel, ILogger<HomeController> logger)
        {
            _commanModel = commanModel;
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


        public async Task<IActionResult> Login(string userid, string password)
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
                if (result.data == null || result.data == "")
                {
                    return Ok(false);
                }
                else
                {
                    UserData users = JsonConvert.DeserializeObject<UserData>(result.data);
                    if (result.statusCode == 200)
                    {
                        HttpContext.Session.SetString("userData", result.data);

                        return Ok(true);
                    }
                    else
                    {
                        return Ok(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public IActionResult GetSessionData()
        {
            //var data = Context.Session.GetString("userData");

            var data = HttpContext.Session.GetString("userData");

            //  var result = JsonConvert.DeserializeObject<WealthDashboard.Models.Login.UserData>(data);
            return Ok(data);
        }
        [HttpGet]
        public IActionResult RemoveSessionData()
        {
            HttpContext.Session.Remove("userData");

            //  var result = JsonConvert.DeserializeObject<WealthDashboard.Models.Login.UserData>(data);
            return Ok(true);
        }
        #region MainManuSliders
        //_______________________MainManuSliders_____________________________________
        [HttpGet]
        public async Task<IActionResult> MainManu()
        {
            try
            {
                var result = await _commanModel.GetMainSliderValue();
                return Ok(new ResultModel()
                {
                    Code = HttpStatusCode.OK,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MainManu");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = null
                });
            }

        }

        #endregion

        #region MainManuContent1
        //_______________________MainManuContent1_____________________________________
        [HttpGet]
        public async Task<IActionResult> MainManuContent1()
        {
            try
            {
                var result = await _commanModel.GetMainContent1Value();
                return Ok(new ResultModel()
                {
                    Code = HttpStatusCode.OK,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MainManu");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = null
                });
            }

        }

        #endregion

        #region MainManuContent2
        //_______________________MainManuContent2_____________________________________
        [HttpGet]
        public async Task<IActionResult> MainManuContent2()
        {
            try
            {
                var result = await _commanModel.GetMainContent2Value();
                return Ok(new ResultModel()
                {
                    Code = HttpStatusCode.OK,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MainManu");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = null
                });
            }

        }


        #endregion

        #region How_TWC_Helps
        //_______________________How_TWC_Helps_____________________________________
        [HttpGet]
        public async Task<IActionResult> GetDataTWChelp()
        {
            try
            {
                var result = await _commanModel.GetHValue();
                return Ok(new ResultModel()
                {
                    Code = HttpStatusCode.OK,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MainManu");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = null
                });
            }

        }

        #endregion

        #region Range_OF_Products

        //_______________________Range_OF_Products_____________________________________
        [HttpGet]
        public async Task<IActionResult> GetDataProducts()
        {
            try
            {
                var result = await _commanModel.GetDataRangeOfProducts();
                return Ok(new ResultModel()
                {
                    Code = HttpStatusCode.OK,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MainManu");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = null
                });
            }

        }


        #endregion

        #region Meet Our Team
        //_______________________Meet Our Team_____________________________________
        [HttpGet]
        public async Task<IActionResult> GetDataOurTeam()
        {
            try
            {
                var result = await _commanModel.GetDataMeetTeam();
                return Ok(new ResultModel()
                {
                    Code = HttpStatusCode.OK,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MainManu");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = null
                });
            }

        }




        #endregion
    }
}