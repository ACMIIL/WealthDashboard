using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Encryption;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models;
using WealthDashboard.Areas.EKYC_MFJourney.Models.DigioManager;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class DigioController : Controller
    {
        #region Global Variable
        private readonly IDigioManagerModel _digioManagerModel;
        private readonly ILogger<DigioController> _logger;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        private readonly Appsetting _appsetting;
        #endregion

        #region Ctor
        public DigioController(IDigioManagerModel digioManagerModel,
            ILogger<DigioController> logger,
            IOptions<Appsetting> options,
            IClientRegistrationManager clientRegistrationManager)
        {
            _digioManagerModel = digioManagerModel;
            _logger = logger;
            _appsetting = options.Value;
            _clientRegistrationManager = clientRegistrationManager;

        }
        #endregion

        #region Method
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DigiLockerView()
        {
            return View();
        }
        public IActionResult UploadDocCropperView()
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            mUCCTempModel.EncRegistrationId = Request.Query["EncRegistrationId"];
            mUCCTempModel.RegistrationId = Convert.ToInt32(Encryption.Decrypt(mUCCTempModel.EncRegistrationId.ToString().Replace(" ", "+")));
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(mUCCTempModel.RegistrationId);
            return View(mUCCTempModel);
        }
        public async Task<JsonResult> CentralizeDigioWorkTemplate(CentralizeDigioWorkTemplateRequest centralizeDigioWorkTemplateRequest)
        {
            DigioModel digioModel = new DigioModel();
            centralizeDigioWorkTemplateRequest.notify_customer = true;
            centralizeDigioWorkTemplateRequest.generate_access_token = true;
            centralizeDigioWorkTemplateRequest.sourceType = "ACMIIL-EKYC";
            digioModel = await _digioManagerModel.CentralizeDigioWorkTemplate(centralizeDigioWorkTemplateRequest);
            string Message = await _digioManagerModel.CentralizeInsertUpdateDigioTemplate(centralizeDigioWorkTemplateRequest.RegistrationId, digioModel);
            if (Message != "Failed")
            {
                return Json(digioModel);
            }
            else
            {
                _logger.LogError("Failed Mobile Details Invalid");
                return Json(StaticValues.MobileDetailsInvalid);
            }
        }

        public async Task<JsonResult> CentralizeDigioResponseData(CeDigioResDataPrameter ceDigioResDataPrameter)
        {
            string filemessage = string.Empty;
            CentralizeWorkflowTemplateModel centralizeWorkflowTemplateModel = new CentralizeWorkflowTemplateModel();
            CentralizedigioPrameter centralizedigioPrameter = new CentralizedigioPrameter();
            try
            {
                string StrId = await _digioManagerModel.CentralizeGetDigiotemplateDetails(ceDigioResDataPrameter.RegistrationId);

                #region paramerer Add CentralizeDigioResponseData method
                centralizedigioPrameter.ResponseId = StrId;
                centralizedigioPrameter.sourceType = _appsetting.sourceType;
                #endregion

                centralizeWorkflowTemplateModel = await _digioManagerModel.CentralizeDigioResponseData(centralizedigioPrameter);
                if (centralizeWorkflowTemplateModel.data.id != null)
                {
                    var message = await _digioManagerModel.DigioApprovedPANandAAharResponseData(ceDigioResDataPrameter.RegistrationId, centralizeWorkflowTemplateModel);
                    if (message == "OK")
                    {
                        ResponseAction mResponseAction = new ResponseAction();
                        mResponseAction = centralizeWorkflowTemplateModel.data.actions[0];
                        filemessage = await _digioManagerModel.GetDigioFileData(ceDigioResDataPrameter.RegistrationId, mResponseAction.execution_request_id, ceDigioResDataPrameter.ucc);
                        if (filemessage == "OK")
                        {
                            return Json(filemessage);
                        }
                        else
                        {
                            return Json("fail");
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    _logger.LogError("https://digio.investmentz.com/api/DigioKYC/DigioKYCResponse?ResponseId= not get id ");
                    return Json(filemessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return Json(filemessage);
        }
        [HttpPost]
        public async Task<JsonResult> InsertPersonalDetails(InsertPersonalDetailModel insertPersonalDetailModel)
        {
            ClientsPersonalDetailsModel clientsPersonalDetailsModel = new ClientsPersonalDetailsModel();
            AddressCodeModel addressCodeModel = new AddressCodeModel();
            List<ClientRelationModel> clientRelationModel = new List<ClientRelationModel>();
            InsertOrUpdateClientAddressDetailsModel insertOrUpdateClientAddressDetailsModel = new InsertOrUpdateClientAddressDetailsModel();
            FatcadetailModel fatcadetailModel = new FatcadetailModel();
            try
            {
                #region Set Data on clientsPersonalDetailsModel
                clientsPersonalDetailsModel.PersonalDetailsId = insertPersonalDetailModel.PersonalDetailsId;
                clientsPersonalDetailsModel.RegistrationId = insertPersonalDetailModel.RegistrationId;
                clientsPersonalDetailsModel.Title = insertPersonalDetailModel.Title;
                clientsPersonalDetailsModel.Bacode = "";//insertPersonalDetailModel.Bacode;
                clientsPersonalDetailsModel.ClientFullName = insertPersonalDetailModel.ClientFullName;
                clientsPersonalDetailsModel.ClientFirstName = insertPersonalDetailModel.ClientFirstName;
                clientsPersonalDetailsModel.ClientMiddleName = insertPersonalDetailModel.ClientMiddleName;
                clientsPersonalDetailsModel.ClientLastName = insertPersonalDetailModel.ClientLastName;
                clientsPersonalDetailsModel.ClientMotherName = "";// insertPersonalDetailModel.ClientMotherName;
                clientsPersonalDetailsModel.ClientCategoryId = insertPersonalDetailModel.ClientCategoryId;
                clientsPersonalDetailsModel.ClientHolderId = insertPersonalDetailModel.ClientHolderId;
                clientsPersonalDetailsModel.DateOfBirth = insertPersonalDetailModel.DateOfBirth;
                clientsPersonalDetailsModel.PAN = insertPersonalDetailModel.PAN;
                clientsPersonalDetailsModel.UID = insertPersonalDetailModel.UID;
                clientsPersonalDetailsModel.Gender = insertPersonalDetailModel.Gender;
                clientsPersonalDetailsModel.Education = insertPersonalDetailModel.Education;
                clientsPersonalDetailsModel.ClientsRelationId = insertPersonalDetailModel.ClientsRelationId;
                clientsPersonalDetailsModel.MaritalStatus = insertPersonalDetailModel.MaritalStatus;
                clientsPersonalDetailsModel.OccupationType = insertPersonalDetailModel.OccupationType;
                clientsPersonalDetailsModel.Telephone1 = insertPersonalDetailModel.Telephone1;
                clientsPersonalDetailsModel.Telephone2 = insertPersonalDetailModel.Telephone2;
                clientsPersonalDetailsModel.EmailId = insertPersonalDetailModel.EmailId;
                clientsPersonalDetailsModel.MobileNo = insertPersonalDetailModel.MobileNo;
                clientsPersonalDetailsModel.SameAddress = insertPersonalDetailModel.SameAddress;
                clientsPersonalDetailsModel.UserId = insertPersonalDetailModel.UserId;
                #endregion



                var PersonalMessage = await _digioManagerModel.InsertPersonalDetails(clientsPersonalDetailsModel);


                //for get address Id
                addressCodeModel = await _digioManagerModel.AddressCodeDetails(insertPersonalDetailModel.Pincode.ToString());


                int charPerPart = 50; // Number of characters per part

                // Extract three parts of  50 characters each

                string address1 = "";
                string address2 = "";
                string address3 = "";

                if (insertPersonalDetailModel.Address.Length > 50)
                {
                    address1 = insertPersonalDetailModel.Address.Substring(0, Math.Min(charPerPart, insertPersonalDetailModel.Address.Length));
                    address2 = insertPersonalDetailModel.Address.Substring(charPerPart, Math.Min(charPerPart, insertPersonalDetailModel.Address.Length - charPerPart));
                }
                if (insertPersonalDetailModel.Address.Length > 100)
                {
                    address1 = insertPersonalDetailModel.Address.Substring(0, Math.Min(charPerPart, insertPersonalDetailModel.Address.Length));
                    address2 = insertPersonalDetailModel.Address.Substring(charPerPart, Math.Min(charPerPart, insertPersonalDetailModel.Address.Length - charPerPart));
                    address3 = insertPersonalDetailModel.Address.Substring(charPerPart * 2, Math.Min(charPerPart, insertPersonalDetailModel.Address.Length - charPerPart * 2));
                }
                if (insertPersonalDetailModel.Address.Length > 5)
                {
                    address1 = insertPersonalDetailModel.Address.Substring(0, Math.Min(charPerPart, insertPersonalDetailModel.Address.Length));
                }
                var addressmsg = "";
                var relationMessage = "";
                var FatcaMessage = "";
                if (PersonalMessage == "Ok")
                {
                    int[] addressTId = { 33, 34 };

                    foreach (int AdresstypeId in addressTId)
                    {
                        #region Set Values on insertOrUpdateClientAddressDetailsModel obj

                        insertOrUpdateClientAddressDetailsModel.registrationId = insertPersonalDetailModel.RegistrationId;
                        insertOrUpdateClientAddressDetailsModel.addressTypeId = AdresstypeId;
                        insertOrUpdateClientAddressDetailsModel.address1 = address1;
                        insertOrUpdateClientAddressDetailsModel.address2 = address2;
                        insertOrUpdateClientAddressDetailsModel.address3 = address3;
                        insertOrUpdateClientAddressDetailsModel.cityId = addressCodeModel.cityId;
                        insertOrUpdateClientAddressDetailsModel.stateId = addressCodeModel.stateId;
                        insertOrUpdateClientAddressDetailsModel.pinCodeMasterId = Convert.ToInt32(addressCodeModel.pinCode);
                        insertOrUpdateClientAddressDetailsModel.countryMasterId = addressCodeModel.country_Code;
                        insertOrUpdateClientAddressDetailsModel.userId = insertPersonalDetailModel.UserId;
                        #endregion

                        addressmsg = await _digioManagerModel.InsertOrUpdateClientAddressDetails(insertOrUpdateClientAddressDetailsModel);
                    }
                }
                if (addressmsg == "Ok")
                {
                    #region Add Value On clientRelationModel obj 
                    clientRelationModel.Add(new ClientRelationModel()
                    {
                        relationTypeId = 38,
                        registrationId = insertPersonalDetailModel.RegistrationId,
                        relationFirstName = insertPersonalDetailModel.fatherfirstname,
                        relationMiddleName = insertPersonalDetailModel.fathermiddlename,
                        relationLastName = insertPersonalDetailModel.fatherlastname,
                        userId = insertPersonalDetailModel.UserId
                    });

                    #endregion

                    relationMessage = await _digioManagerModel.InsertOrUpdateClientRelationDetails(clientRelationModel);
                }
                if (relationMessage == "Ok")
                {
                    #region Add value on fatcadetailModel  Obj
                    fatcadetailModel.clientFatcaId = 0;
                    fatcadetailModel.registrationId = insertPersonalDetailModel.RegistrationId;
                    fatcadetailModel.investmentExperienceId = insertPersonalDetailModel.investmentExperienceId;
                    fatcadetailModel.annualIncomeId = insertPersonalDetailModel.annualIncomeId;
                    fatcadetailModel.networth = insertPersonalDetailModel.networth;
                    fatcadetailModel.tarrifPlan = 1;
                    fatcadetailModel.brokragePlan = 1;
                    fatcadetailModel.dpStatus = 1;
                    fatcadetailModel.dpSubstatus = 1;
                    fatcadetailModel.modeOfholdingId = 1;
                    fatcadetailModel.sourceOfWealthId = insertPersonalDetailModel.OccupationType;
                    fatcadetailModel.politicallyExposePerson = false;
                    fatcadetailModel.countryMasterId = 1;
                    fatcadetailModel.isYourCountryTAXResidencyOtherThenIndia = false;
                    fatcadetailModel.consentForCollateralLimitMargin = false;
                    fatcadetailModel.userId = insertPersonalDetailModel.UserId;
                    #endregion

                    FatcaMessage = await _digioManagerModel.InsertOrUpdateInvestmentAndFatcaDetails(fatcadetailModel);

                }
                if (FatcaMessage == "Ok")
                {

                }
                else
                {
                    FatcaMessage = "Fail";
                }
                return Json(FatcaMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Json(ex.ToString());
            }

        }
        public async Task<JsonResult> CheckDigioStatus(int RegistrationID)
        {
            DigioStatus mDigioStatus = new DigioStatus();
            mDigioStatus = await _digioManagerModel.CheckDigioStatus(RegistrationID);
            return Json(mDigioStatus);

        }
        #endregion
    }
}
