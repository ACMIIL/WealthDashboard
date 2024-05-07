using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentModel;
using WealthDashboard.Configuration;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class SegmentController : Controller
    {
        #region Global Variable
        private readonly ISegmentManager _segmentManager;
        private readonly Appsetting _appsetting;
        private readonly ILogger<SegmentController> _logger;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        #endregion

        #region Ctor
        public SegmentController(ISegmentManager segmentManager, IClientRegistrationManager clientRegistrationManager,
            IOptions<Appsetting> appsetting, ILogger<SegmentController> logger)
        {
            _segmentManager = segmentManager;
            _appsetting = appsetting.Value;
            _logger = logger;
            _clientRegistrationManager = clientRegistrationManager;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDepositoryMaster([FromQuery] string option, [FromQuery] string dpId)
        {
            try
            {
                var result = await _segmentManager.GetDepositoryMaster(option, dpId);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<JsonResult> InsertOrUpdateSegment(InsertSegmentModel insertSegmentModel)
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            List<SegmentData> segmentData = new List<SegmentData>();
            ReqInsertSegment reqInsertSegment = new ReqInsertSegment();
            SegmentUploadModel segmentUploadModel = new SegmentUploadModel();
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(insertSegmentModel.RID);

            #region Add Data on reqInsertSegment obj 
            reqInsertSegment.DPID = insertSegmentModel.DPID;
            reqInsertSegment.BOID = insertSegmentModel.BOID;
            reqInsertSegment.DepositoryName = insertSegmentModel.DepositoryName;
            reqInsertSegment.UserId = insertSegmentModel.UserId;
            reqInsertSegment.OnluMf = insertSegmentModel.OnluMf;
            reqInsertSegment.RID = insertSegmentModel.RID;
            reqInsertSegment.Residency = insertSegmentModel.Residency;
            reqInsertSegment.PoliticallyExposePerson = insertSegmentModel.PoliticallyExposePerson;
            reqInsertSegment.settlementfrequency = insertSegmentModel.settlementfrequency;
            #endregion

            string result = string.Empty;
            string filePath = string.Empty;
            string fileName = string.Empty;
            int uploadDocumentId = 0;
            #region File Save
            if (insertSegmentModel.IncomeProof != null)
            {
                var path = _appsetting.Incomeprooffilepath + "20" + mUCCTempModel.UCC.ToString();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileExtension = Path.GetExtension(insertSegmentModel.IncomeProof.FileName);
                fileName = $"{"20" + mUCCTempModel.UCC.ToString()}_{"Derivative1" + fileExtension}";
                filePath = Path.Combine(path, fileName);
                uploadDocumentId = 7; // for drivative 1
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await insertSegmentModel.IncomeProof.CopyToAsync(stream);
                }
            }
            else if (insertSegmentModel.CMLfile != null)
            {

                var path = _appsetting.Incomeprooffilepath + "20" + mUCCTempModel.UCC.ToString();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileExtension = Path.GetExtension(insertSegmentModel.CMLfile.FileName);
                if (fileExtension != ".pdf")
                {
                    fileExtension = ".png";
                }

                fileName = $"{"20" + mUCCTempModel.UCC.ToString()}_{"DP1" + fileExtension}";
                filePath = Path.Combine(path, fileName);
                uploadDocumentId = 12; //for DP1 
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await insertSegmentModel.CMLfile.CopyToAsync(stream);
                }
            }
            else
            {
                filePath = " ";
                fileName = "";
            }

            #endregion

            #region Set Data on segmentUploadModel obj
            segmentUploadModel.clientUploadDataId = 0;
            segmentUploadModel.uploadDocumentId = uploadDocumentId;
            segmentUploadModel.docName = "";
            segmentUploadModel.registrationId = insertSegmentModel.RID;
            segmentUploadModel.uploadDocNo = "0";
            segmentUploadModel.fileName = fileName;
            segmentUploadModel.filePath = filePath;
            segmentUploadModel.isActive = true;
            segmentUploadModel.userId = insertSegmentModel.UserId;
            #endregion

            try
            {
                segmentData = JsonConvert.DeserializeObject<List<SegmentData>>(insertSegmentModel.Segmentdta);

                result = await _segmentManager.InsertOrupdateClentsegment(segmentData);
                result = await _segmentManager.InsertOrUpdateSegment(reqInsertSegment);
                if (insertSegmentModel.IncomeProof != null)
                {
                    result = await _segmentManager.InsertOrUpdateClientUploadData(segmentUploadModel);
                }
                else if (insertSegmentModel.CMLfile != null)
                {
                    result = await _segmentManager.InsertOrUpdateClientUploadData(segmentUploadModel);
                }

                result = await _segmentManager.UpdateBrokarageplan(insertSegmentModel.RID, insertSegmentModel.tarrifplan, insertSegmentModel.BrokragePlan);
                result = await _segmentManager.Update_BACode(insertSegmentModel.RID, insertSegmentModel.UserId);

                if (result != null)
                {
                    return Json(result);
                }
                else
                {
                    return Json("Segment Insert time error");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                _logger.LogError(ex.ToString());
                return Json(500, "Internal server error");
            }
            return Json(result);
        }

        public bool CheckPDFPassWordProtected(InsertSegmentModel insertSegmentModel)
        {
            byte[] pdfData = convertintobyte(insertSegmentModel.IncomeProof);
            try
            {
                using (MemoryStream stream = new MemoryStream(pdfData))
                using (PdfReader pdfReader = new PdfReader(stream))
                {
                    return pdfReader.IsEncrypted();
                }
            }
            catch (BadPasswordException)
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return true;
            }
        }

        public byte[] convertintobyte(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    file.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }

        public async Task<JsonResult> Brockarageplan()
        {
            List<brockragedrp> brockragedrps = new List<brockragedrp>();
            brockragedrps = await _segmentManager.Brockarageplan();
            return Json(brockragedrps);
        }
    }
}
