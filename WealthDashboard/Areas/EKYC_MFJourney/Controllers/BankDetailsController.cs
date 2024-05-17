using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class BankDetailsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BankDetailsController> _logger;

        public BankDetailsController(IConfiguration configuration, ILogger<BankDetailsController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BankDetail()
        {
            string outputValue = HttpContext.Request.Query["output"];

            if (!string.IsNullOrEmpty(outputValue))
            {
                try
                {
                    string apiResponse = await GetQRData(outputValue);
                    ApiResponseModel responseData = JsonConvert.DeserializeObject<ApiResponseModel>(apiResponse);

                    // Access the required data
                    string jsonString = responseData.data;
                    CamspayResponse camspayResponse = JsonConvert.DeserializeObject<CamspayResponse>(jsonString);

                    // Access the extracted values
                    string trxnid = camspayResponse.TransactionId;
                    string camspayrefno = camspayResponse.CamspayRefNo;

                    string refundResponse = await Refund(trxnid, camspayrefno);
                    RefundResponseModel RefundresponseData = JsonConvert.DeserializeObject<RefundResponseModel>(apiResponse);

                    string RefundjsonString = responseData.data;
                    Root refundCamspayResponse = JsonConvert.DeserializeObject<Root>(jsonString);

                    string custid = refundCamspayResponse.Req.Custid;
                    string message = refundCamspayResponse.Message;

                    if (message == "SUCCESS" && refundCamspayResponse.Resc == "RC111")
                    {
                        // Call the GetBankData API
                        string getBankDataApi = await GetBankData(custid);
                        GetBankDetailApiResponseModel bankDataResponse = JsonConvert.DeserializeObject<GetBankDetailApiResponseModel>(getBankDataApi);

                        // Access the bank data directly from the intermediate object
                        BankDetailsModel bankData = bankDataResponse.data;

                        // Set values in HTML elements using ViewData
                        ViewData["AccountNumber"] = bankData.accountNo;
                        ViewData["IFSCCode"] = bankData.ifsC_Code;
                        ViewData["AccountHolderName"] = bankData.accountHolderName;
                        ViewData["BankName"] = bankData.bankName;
                        ViewData["BranchName"] = bankData.branch;
                        ViewData["BankAddress"] = bankData.address;
                        ViewData["BankCity"] = bankData.city;
                        ViewData["BankState"] = bankData.state;
                    }
                    else
                    {
                        // Show message or handle accordingly
                        ViewData["ErrorMessage"] = "Refund was not successful. Message: " + message;
                    }

                    return View();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred while calling the API.");

                    // Return a more detailed error message to the client
                    return StatusCode(500, $"An error occurred while calling the API: {ex.Message}");
                }
            }
            else
            {
                return View();
            }
        }

        // Private method to call the other API asynchronously
        private async Task<string> GetQRData(string outputValue)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrlQR = _configuration["CamspayApiUrls:GetQRBankData"];
                string apiRequestUrl = $"{apiUrlQR}?data={outputValue}";

                // Make a GET request to the other API asynchronously
                HttpResponseMessage response = await client.GetAsync(apiRequestUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle the case when the API request fails
                    return $"API request failed with status code: {response.StatusCode}";
                }
            }
        }

        private async Task<string> Refund(string trxnid, string camspayrefno)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrlData = _configuration["CamspayApiUrls:GetRefund"];
                string apiRequestUrl = $"{apiUrlData}Refund?comsparinfo={camspayrefno}&trno={trxnid}";



                // Make a GET request to the other API asynchronously
                HttpResponseMessage response = await client.GetAsync(apiRequestUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle the case when the API request fails
                    return $"API request failed with status code: {response.StatusCode}";
                }
            }
        }
        private async Task<string> GetBankData(string outputValue)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrlQR = _configuration["CamspayApiUrls:GetBankDetails"];
                string apiRequestUrl = $"{apiUrlQR}?RegistrationID={outputValue}";

                // Make a GET request to the other API asynchronously
                HttpResponseMessage response = await client.GetAsync(apiRequestUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    //return await response.Content.ReadAsStringAsync();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    // Handle the case when the API request fails
                    return $"API request failed with status code: {response.StatusCode}";
                }
            }
        }

        private class ApiResponseModel
        {
            public int code { get; set; }
            public string message { get; set; }
            public string data { get; set; }
            public string trxnid { get; set; }
            public string camspayrefno { get; set; }
        }

        public class GetBankDetailApiResponseModel
        {
            public int code { get; set; }
            public string message { get; set; }
            public BankDetailsModel data { get; set; }
            public string trxnid { get; set; }
            public string camspayrefno { get; set; }
        }


        public class CamspayResponse
        {
            [JsonProperty("trxnid")]
            public string TransactionId { get; set; }

            [JsonProperty("camspayrefno")]
            public string CamspayRefNo { get; set; }
        }
    }

    public class RefundResponseModel
    {
        public string resc { get; set; }
        public string msg { get; set; }
        public string trxnid { get; set; }
        public decimal amount { get; set; }
        public string msgdesc { get; set; }
        public string linkid { get; set; }
        public string camspayrefno { get; set; }
        public DateTime trxndate { get; set; }
    }

    public class Req
    {
        [JsonProperty("custid")]
        public string Custid { get; set; }
    }

    public class Root
    {
        [JsonProperty("resc")]
        public string Resc { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("req")]
        public Req Req { get; set; }
    }
    public class BankDetailsModel
    {

        public int registrationId { get; set; }
        public string accountHolderName { get; set; }
        public string accountNo { get; set; }
        public string ifsC_Code { get; set; }
        public int ifscMasterId { get; set; }
        public string bankName { get; set; }
        public string micrcode { get; set; }
        public string branch { get; set; }
        public string address { get; set; }
        public string contactNo { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string state { get; set; }

    }
}
