using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WealthDashboard.Areas.EKYC_MFJourney.Models;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Encryption;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentModel;
using WealthDashboard.Configuration;
using WealthDashboard.Models;
using ErrorViewModel = WealthDashboard.Areas.EKYC_MFJourney.Models.ErrorViewModel;
using StaticValues = WealthDashboard.Areas.EKYC_MFJourney.Models.Common.StaticValues;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        private readonly Appsetting _appsetting;
        private readonly ISegmentManager _segmentManager;

        public HomeController(ILogger<HomeController> logger, IClientRegistrationManager clientRegistrationManager, IOptions<Appsetting> appsetting,
            ISegmentManager segmentManager)
        {
            _logger = logger;
            _clientRegistrationManager = clientRegistrationManager;
            _appsetting = appsetting.Value;
            _segmentManager = segmentManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginView()
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            var sourceType = HttpContext.Request.Query["sourceType"];
            var encWPCode = HttpContext.Request.Query["WPCode"];
            var encMobNo = HttpContext.Request.Query["MobNo"];
            var encBACode = HttpContext.Request.Query["rc_code"];
            mUCCTempModel.SourceType = sourceType;

            _logger.LogError("sourceType" + sourceType + " encencWPCode" + encWPCode + " encMobNo" + encMobNo + "Encrypted LMS Data");

            if (sourceType == "LMS")
            {
                mUCCTempModel.EncWPCode = encWPCode;
                mUCCTempModel.EncMobNo = encMobNo;

                if (!string.IsNullOrEmpty(encWPCode))
                {
                    mUCCTempModel.WPCode = Encryption.Decrypt(mUCCTempModel.EncWPCode.Replace(' ', '+'));
                }
                if (!string.IsNullOrEmpty(encMobNo))
                {
                    mUCCTempModel.MobNo = Encryption.Decrypt(mUCCTempModel.EncMobNo.Replace(' ', '+'));
                }
            }
            mUCCTempModel.EncBACode = string.IsNullOrEmpty(encBACode) ? "0fG9SSKKfEI=" : encBACode;
            mUCCTempModel.BACode = Encryption.Decrypt(mUCCTempModel.EncBACode.Replace(' ', '+'));
            mUCCTempModel.MobileNumber = mUCCTempModel.MobNo;
            return View(mUCCTempModel);
        }

        public IActionResult PAN_Details()
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel.EncUCC = HttpContext.Request.Query["encucc"].ToString().Replace(' ', '+');
            mUCCTempModel.EncBACode = HttpContext.Request.Query["rc_code"];
            if (HttpContext.Request.Query["rc_code"].ToString() == "")
            {
                mUCCTempModel.EncBACode = "0fG9SSKKfEI=";
            }
            mUCCTempModel.BACode = Encryption.Decrypt(mUCCTempModel.EncBACode.Replace(' ', '+').Replace(' ', '+'));
            mUCCTempModel.UCC = Encryption.Decrypt(mUCCTempModel.EncUCC.Replace(' ', '+'));
            mUCCTempModel = _clientRegistrationManager.GetNewClientDetails(mUCCTempModel.UCC);
            mUCCTempModel.EncBACode = Encryption.Encrypt(mUCCTempModel.BACode.ToString().Replace('+', ' ').Replace('+', ' '));
            mUCCTempModel.EncRegistrationId = Encryption.Encrypt(mUCCTempModel.RegistrationId.ToString().Replace('+', ' ').Replace('+', ' '));
            mUCCTempModel.EncUCC = HttpContext.Request.Query["encucc"].ToString().Replace(' ', '+');
            return View(mUCCTempModel);
        }

        public IActionResult Bank_Details()
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            if (HttpContext.Request.Query["encregistrationId"].ToString() != "")
            {
                mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"];
                mUCCTempModel.RegistrationId = Convert.ToInt32(Encryption.Decrypt(mUCCTempModel.EncRegistrationId.Replace(' ', '+')));
                HttpContext.Session.SetString("RegistrationId", Convert.ToString(mUCCTempModel.RegistrationId));
                mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(mUCCTempModel.RegistrationId);
                mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"];
                _logger.LogError("Get Registration" + HttpContext.Session.GetString("RegistrationId").Replace(' ', '+'));
                return View(mUCCTempModel);
            }
            else
            {
                _logger.LogError("Qrresponce" + HttpContext.Session.GetString("RegistrationId").Replace(' ', '+'));
                mUCCTempModel.RegistrationId = Convert.ToInt32(HttpContext.Session.GetString("RegistrationId"));
                mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(mUCCTempModel.RegistrationId);
                mUCCTempModel.EncRegistrationId = Encryption.Encrypt(mUCCTempModel.RegistrationId.ToString());
                return View(mUCCTempModel);
            }
        }

        public IActionResult SegmentSelection()
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"].ToString().Replace(' ', '+');
            mUCCTempModel.RegistrationId = Convert.ToInt32(Encryption.Decrypt(mUCCTempModel.EncRegistrationId.Replace(' ', '+').Replace(' ', '+')));
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(mUCCTempModel.RegistrationId);
            mUCCTempModel.EncRegistrationId = HttpContext.Request.Query["encregistrationId"].ToString().Replace(' ', '+');
            return View(mUCCTempModel);
        }

        [HttpGet]
        public JsonResult EncryptedUCC(string UCC)
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel.UCC = UCC;
            mUCCTempModel = _clientRegistrationManager.GetNewClientDetails(mUCCTempModel.UCC);
            mUCCTempModel.EncUCC = Encryption.Encrypt(UCC.ToString().Replace(" ", "+").Replace(" ", "+"));
            mUCCTempModel.EncBACode = Encryption.Encrypt(mUCCTempModel.BACode.ToString().Replace(" ", "+").Replace(" ", "+"));
            mUCCTempModel.EncUCC = mUCCTempModel.EncUCC.ToString().Replace(" ", "+");
            mUCCTempModel.EncBACode = mUCCTempModel.EncBACode.ToString().Replace(" ", "+");
            return Json(mUCCTempModel);
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


        [HttpPost]
        public async Task<JsonResult> ImageUpload(UploadimageModel uploadimageModel)
        {
            string fileName1 = string.Empty;
            string filePath1 = string.Empty;
            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(uploadimageModel.registrationId);
            SegmentUploadModel segmentUploadModel = new SegmentUploadModel();
            string result = string.Empty;
            if (uploadimageModel.PanImage != null)
            {
                var path1 = _appsetting.UplodDocument + "20" + mUCCTempModel.UCC.ToString();
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                fileName1 = $"{"20" + mUCCTempModel.UCC.ToString()}_{"PanCard.png"}";
                filePath1 = Path.Combine(path1, fileName1);
                using (var stream1 = new FileStream(filePath1, FileMode.Create))
                {

                    await uploadimageModel.PanImage.CopyToAsync(stream1);

                }
                #region Set Data on segmentUploadModel obj
                segmentUploadModel.clientUploadDataId = 0;
                segmentUploadModel.uploadDocumentId = 2;
                segmentUploadModel.docName = "";
                segmentUploadModel.registrationId = uploadimageModel.registrationId;
                segmentUploadModel.uploadDocNo = "0";
                segmentUploadModel.fileName = fileName1;
                segmentUploadModel.filePath = filePath1;
                segmentUploadModel.isActive = true;
                segmentUploadModel.userId = "";
                #endregion
                result = await _segmentManager.InsertOrUpdateClientUploadData(segmentUploadModel);
            }

            if (uploadimageModel.SignImage != null)
            {
                var path1 = _appsetting.UplodDocument + "20" + mUCCTempModel.UCC.ToString();
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                fileName1 = $"{"20" + mUCCTempModel.UCC.ToString()}_{"Signature.png"}";
                filePath1 = Path.Combine(path1, fileName1);
                using (var stream2 = new FileStream(filePath1, FileMode.Create))
                {

                    await uploadimageModel.SignImage.CopyToAsync(stream2);

                }
                #region Set Data on segmentUploadModel obj
                segmentUploadModel.clientUploadDataId = 0;
                segmentUploadModel.uploadDocumentId = 6;
                segmentUploadModel.docName = "";
                segmentUploadModel.registrationId = uploadimageModel.registrationId;
                segmentUploadModel.uploadDocNo = "0";
                segmentUploadModel.fileName = fileName1;
                segmentUploadModel.filePath = filePath1;
                segmentUploadModel.isActive = true;
                segmentUploadModel.userId = "";
                #endregion
                result = await _segmentManager.InsertOrUpdateClientUploadData(segmentUploadModel);
            }

            if (uploadimageModel.CheckImage != null)
            {
                var path1 = _appsetting.UplodDocument + "20" + mUCCTempModel.UCC.ToString();
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                fileName1 = $"{"20" + mUCCTempModel.UCC.ToString()}_{"CancelCheque.png"}";
                filePath1 = Path.Combine(path1, fileName1);
                using (var stream3 = new FileStream(filePath1, FileMode.Create))
                {
                    await uploadimageModel.CheckImage.CopyToAsync(stream3);
                }

                #region Set Data on segmentUploadModel obj
                segmentUploadModel.clientUploadDataId = 0;
                segmentUploadModel.uploadDocumentId = 5;
                segmentUploadModel.docName = "";
                segmentUploadModel.registrationId = uploadimageModel.registrationId;
                segmentUploadModel.uploadDocNo = "0";
                segmentUploadModel.fileName = fileName1;
                segmentUploadModel.filePath = filePath1;
                segmentUploadModel.isActive = true;
                segmentUploadModel.userId = "";
                #endregion
                result = await _segmentManager.InsertOrUpdateClientUploadData(segmentUploadModel);

            }
            return Json(result);
        }
        public async Task<string> InsertUpdatedBankDetails(InsertUpdateReqModel insertUpdateReqModel)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(insertUpdateReqModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);


                HttpResponseMessage response = await client
                                   .PostAsync(StaticValues.InsertUpdateBank, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    message = "Ok";
                }
                else
                {
                    _logger.LogError("Bank Details Not Inserted");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Digio" + ex.ToString());
                _logger.LogError("Digio" + ex.StackTrace);
                return ex.ToString();
            }
            return message;
        }

       
    }
}
