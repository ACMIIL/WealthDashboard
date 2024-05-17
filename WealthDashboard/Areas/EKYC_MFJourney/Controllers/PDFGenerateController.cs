using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Drawing.Printing;
using System.Reflection.PortableExecutable;
using System.Text;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.PDFManager;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.Json;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;

namespace WealthDashboard.Areas.EKYC_MFJourney.Controllers
{
    [Area("EKYC_MFJourney")]
    public class PDFGenerateController : Controller
    {
        #region global variable
        private readonly IPDFModel _pdfManager;
        private readonly ILogger<PDFGenerateController> _logger;
        private readonly Appsetting _appsetting;
        private readonly ConnectionStrings _connectionStrings;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        #endregion

        public PDFGenerateController(IOptions<ConnectionStrings> connectionstring, IPDFModel pdfManager,
            ILogger<PDFGenerateController> PDFlogger, IOptions<Appsetting> appsetting,
            IClientRegistrationManager clientRegistrationManager)
        {
            _pdfManager = pdfManager;
            _logger = PDFlogger;
            _appsetting = appsetting.Value;
            _connectionStrings = connectionstring.Value;
            _clientRegistrationManager = clientRegistrationManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GeneratePDF(int RegistrationId, string PDFSelect, string EsignType)
        {
            string message = string.Empty;

            string Bacode = _appsetting.BaCode;
            int addAttchmentPageNo = Convert.ToInt32(_appsetting.addAttchmentPageNo);

            MainModel model = new MainModel();

            model = await _pdfManager.GetDataFromDatabase(RegistrationId);
            if (model != null)
            {
                if (model.notification.IsError == true)
                {
                    message = model.notification.Message;
                    return Json(message);
                }
                else
                {
                    string Blank_File = _appsetting.PDFBlank_Normal;

                    string normalFileName = "";
                    normalFileName = await _pdfManager.CreateDirectory(RegistrationId, Bacode, PDFSelect, EsignType, model);


                    iTextSharp.text.Document xdocument = null;

                    using (xdocument = new iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10))
                    using (MemoryStream Memorystream = new MemoryStream())

                    using (var Blank_FileStream = new FileStream(Blank_File, FileMode.Open))
                    {
                        try
                        {
                            Blank_FileStream.Position = 0;

                            PdfReader pdfReader = new PdfReader(Blank_FileStream);

                            Blank_FileStream.Close();
                            xdocument.Open();
                            var filestream = new FileStream(normalFileName, FileMode.Create);
                            using (var pdfStamper = new PdfStamper(pdfReader, filestream))
                            {


                                List<UploadDocDetailsModel> lstUpload = model.UploadDocsDetailsModel.ToList();

                                DataTable dt = await _pdfManager.ConvertListToDataTable(lstUpload);

                                DataTable DtClientPhoto = await _pdfManager.GetClientPhoto(model, RegistrationId);
                                DataTable DtClientSign = await _pdfManager.GetClientSign(model, RegistrationId);


                                //bool DocUploadStatus = await _pdfManager.DocUploadDetails(model, dt, pdfReader, addAttchmentPageNo, pdfStamper, model.PersonalDetailsModel, model.PennyDropDetailsModel, model.ClientPermanentAddressModel, RegistrationId, "Normal", EsignType, model.uploadGEOTag);

                                //bool clientphoto = await _pdfManager.ClientPhoto(model, dt.Rows.Count, pdfReader, pdfStamper, "Normal");

                                bool ClientSignatureStatus = await _pdfManager.ClientSignature(model, DtClientSign, dt.Rows.Count, pdfStamper, "MF");

                                //For Authority Signature
                                if (EsignType == "Yes")
                                {

                                    //int Count = dt.Rows.Count - 2;

                                    //bool ClientAuthSignatureStatus = await _pdfManager.AuthEmpSign(addAttchmentPageNo + Count - 1, pdfStamper, "Normal");

                                    //bool PDFClientEsignStatus = await _pdfManager.ClientNomineeEsign(model, addAttchmentPageNo + Count, pdfStamper, "Normal", EsignType);
                                }


                                AcroFields FieldsValue = pdfStamper.AcroFields;

                                if (dt.Rows.Count > 0)
                                {
                                    var DocUploadStatus = true;
                                    if (DocUploadStatus == true)
                                    {
                                        IDictionary<String, AcroFields.Item> fields = FieldsValue.Fields;
                                        foreach (String name in fields.Keys)
                                        {
                                            FieldsValue.SetFieldProperty(name, "textcolor", BaseColor.BLUE, null);
                                            FieldsValue.SetFieldProperty(name, "textfont", Font.BOLD, null);
                                        }
                                        //Blank 1st Page

                                        //BSEStar MF Page
                                        FieldsValue.SetField("Name of the First Applicant_MF", model.PersonalDetailsModel.ClientFullName);
                                        FieldsValue.SetField("PAN Number_MF", model.PersonalDetailsModel.PAN);
                                        FieldsValue.SetField("applicationtype_MF", "1", true);
                                        FieldsValue.SetField("Date of Birth_MF", model.PersonalDetailsModel.DateOfBirth.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("City_MF", model.ClientPermanentAddressModel.PerCity);
                                        FieldsValue.SetField("State_MF", model.ClientPermanentAddressModel.PerState);
                                        FieldsValue.SetField("Country_MF", "India");
                                        FieldsValue.SetField("Pincode_MF", model.ClientPermanentAddressModel.PerPincode);
                                        FieldsValue.SetField("Mother Name_MF", model.PersonalDetailsModel.MotherFirstName + ' ' + model.PersonalDetailsModel.MotherMiddleName + ' ' + model.PersonalDetailsModel.MotherLastName);
                                        FieldsValue.SetField("Contact Address_MF", model.ClientPermanentAddressModel.PerAddress1 + " " + model.ClientPermanentAddressModel.PerAddress2 + " " + model.ClientPermanentAddressModel.PerAddress3);
                                        FieldsValue.SetField("Father Name_MF", model.PersonalDetailsModel.FatherFirstName + ' ' + model.PersonalDetailsModel.FatherMiddleName + ' ' + model.PersonalDetailsModel.FatherLastName);
                                        FieldsValue.SetField("Email_MF", model.PersonalDetailsModel.EmailId);
                                        FieldsValue.SetField("Mobile_MF", model.PersonalDetailsModel.MobileNo);
                                        FieldsValue.SetField("Fax_MF", "");
                                        FieldsValue.SetField("Fax Res_MF", "");
                                        FieldsValue.SetField("Occupaon_MF", model.PersonalDetailsModel.OccupationTypeText);
                                        FieldsValue.SetField("Occupaon Details_MF", model.PersonalDetailsModel.OccupationTypeText);
                                        //FieldsValue.SetField("Income Tax Slab Networth_MF", model.ClientFatcaDetailsModel.AnnualIncome);
                                        FieldsValue.SetField("cr1", "");
                                        FieldsValue.SetField("tr1", "");
                                        FieldsValue.SetField("Place of Birth_MF", "India");

                                        if (model.ClientsNomineeGuanrdianDetailsModel.Count > 0)
                                        {
                                            foreach (var itemGuardianDetails in model.ClientsNomineeGuanrdianDetailsModel)
                                            {
                                                if (itemGuardianDetails.NomineeType == "76")
                                                {
                                                    FieldsValue.SetField("Guard Identity_other_cp1", itemGuardianDetails.ProffTypeText);
                                                }
                                                if (itemGuardianDetails.NomineeType == "77")
                                                {
                                                    FieldsValue.SetField("Guard Identity_other_cp2", itemGuardianDetails.ProffTypeText);
                                                }
                                                if (itemGuardianDetails.NomineeType == "78")
                                                {
                                                    FieldsValue.SetField("Guard Identity_other_cp3", itemGuardianDetails.ProffTypeText);
                                                }
                                            }
                                        }

                                        //if (model.ClientFatcaDetailsModel.PoliticallyExposePersonText == "Yes")
                                        //{
                                        //    FieldsValue.SetField("Check Box1_MF", "Yes", true);
                                        //}
                                        //else
                                        //{

                                        FieldsValue.SetField("Check Box1_MF", "NO", true);
                                        //}
                                        FieldsValue.SetField("o1", model.PersonalDetailsModel.OccupationTypeText);

                                        //if (model.ClientDepositoryDetailsModel.DPID != null || model.ClientDepositoryDetailsModel.DPID != "")
                                        //{
                                        //    if (model.ClientDepositoryDetailsModel.DPID.Contains("IN"))
                                        //    {
                                        FieldsValue.SetField("Name of Bank_MF5", model.ClientBankDetailsModel.BankName);
                                        FieldsValue.SetField("Branch_MF5", model.ClientBankDetailsModel.BranchName);
                                        FieldsValue.SetField("AC No_MF5", model.ClientBankDetailsModel.AccountNo);
                                        FieldsValue.SetField("Ac Type_MF5", model.ClientBankDetailsModel.AccountType);
                                        FieldsValue.SetField("IFSC Code_MF5", model.ClientBankDetailsModel.IFSCCode.ToString().ToUpper());
                                        FieldsValue.SetField("Bank Address_MF5", model.ClientBankDetailsModel.Address);
                                        FieldsValue.SetField("City_3_MF5", model.ClientBankDetailsModel.BankCity);
                                        FieldsValue.SetField("Pincode_3_MF5", model.ClientBankDetailsModel.BankPincode);
                                        FieldsValue.SetField("State_2_MF5", model.ClientBankDetailsModel.BankState);
                                        FieldsValue.SetField("Country_3_MF5", "India");
                                        //    }
                                        //    else
                                        //    {
                                        //        FieldsValue.SetField("Name of Bank_MF4", model.ClientBankDetailsModel.BankName);
                                        //        FieldsValue.SetField("Branch_MF4", model.ClientBankDetailsModel.BranchName);
                                        //        FieldsValue.SetField("AC No_MF4", model.ClientBankDetailsModel.AccountNo);
                                        //        FieldsValue.SetField("Ac Type_MF4", model.ClientBankDetailsModel.AccountType);
                                        //        FieldsValue.SetField("IFSC Code_MF4", model.ClientBankDetailsModel.IFSCCode.ToString().ToUpper());
                                        //        FieldsValue.SetField("Bank Address_MF4", model.ClientBankDetailsModel.Address);
                                        //        FieldsValue.SetField("City_3_MF4", model.ClientBankDetailsModel.BankCity);
                                        //        FieldsValue.SetField("Pincode_3_MF4", model.ClientBankDetailsModel.BankPincode);
                                        //        FieldsValue.SetField("State_2_MF4", model.ClientBankDetailsModel.BankState);
                                        //        FieldsValue.SetField("Country_3_MF4", "India");
                                        //    }
                                        //}
                                        // else
                                        // {
                                        FieldsValue.SetField("Name of Bank_MF4", model.ClientBankDetailsModel.BankName);
                                        FieldsValue.SetField("Branch_MF4", model.ClientBankDetailsModel.BranchName);
                                        FieldsValue.SetField("AC No_MF4", model.ClientBankDetailsModel.AccountNo);
                                        FieldsValue.SetField("Ac Type_MF4", model.ClientBankDetailsModel.AccountType);
                                        FieldsValue.SetField("IFSC Code_MF4", model.ClientBankDetailsModel.IFSCCode.ToString().ToUpper());
                                        FieldsValue.SetField("Bank Address_MF4", model.ClientBankDetailsModel.Address);
                                        FieldsValue.SetField("City_3_MF4", model.ClientBankDetailsModel.BankCity);
                                        FieldsValue.SetField("Pincode_3_MF4", model.ClientBankDetailsModel.BankPincode);
                                        FieldsValue.SetField("State_2_MF4", model.ClientBankDetailsModel.BankState);
                                        FieldsValue.SetField("Country_3_MF4", "India");
                                        // }

                                        // model.ClientNomineeDetailsModel = model.GetClientNomineeDetails(RegistrationId);

                                        FieldsValue.SetField("Nominee Name_MF", model.ClientNomineeDetailsModel.NomineeName + ' ' + model.ClientNomineeDetailsModel.NomineeLastName);
                                        FieldsValue.SetField("Relaonship_MF", model.ClientNomineeDetailsModel.RelationshipTypeText);

                                        FieldsValue.SetField("Nominee Address_MF", model.ClientNomineeDetailsModel.NomineeAddress1 + " " + model.ClientNomineeDetailsModel.NomineeAddress2);
                                        FieldsValue.SetField("City_4_MF", model.ClientNomineeDetailsModel.NomineeCity);
                                        FieldsValue.SetField("Pincode_4_MF", model.ClientNomineeDetailsModel.NomineePincode);
                                        FieldsValue.SetField("State_3_MF", model.ClientNomineeDetailsModel.NomineeState);
                                        FieldsValue.SetField("Date Row1", Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy")));
                                        FieldsValue.SetField("Place Row1", model.ClientNomineeDetailsModel.NomineeCity);

                                        if (model.CivilKRAFetchModel != null)
                                        {
                                            if (model.CivilKRAFetchModel.AppName != "")
                                            {
                                                FieldsValue.SetField("Name_CVL", model.CivilKRAFetchModel.AppName);
                                                FieldsValue.SetField("DOB_CVL", model.CivilKRAFetchModel.AppDOB ?? "");
                                                FieldsValue.SetField("gender_CVL", model.CivilKRAFetchModel.AppGen ?? "");
                                                FieldsValue.SetField("Address_CVL", model.CivilKRAFetchModel.AppPerAdd1 + " " + model.CivilKRAFetchModel.AppPerAdd2 + " " + model.CivilKRAFetchModel.AppPerAdd3 ?? "");
                                                FieldsValue.SetField("Add Proof_CVL", "Aadhar");
                                                FieldsValue.SetField("ID proof_CVL", "PAN");
                                                FieldsValue.SetField("Generated_CVL", Convert.ToDateTime(model.CivilKRAFetchModel.RefDate).ToString("dd-MMM-yyyy"));
                                            }
                                        }
                                        //Guardian Details
                                        if (model.ClientsNomineeGuanrdianDetailsModel.Count > 0)
                                        {
                                            foreach (var itemGuardian in model.ClientsNomineeGuanrdianDetailsModel)
                                            {
                                                if (itemGuardian.NomineeType == "76")
                                                {
                                                    FieldsValue.SetField("First _nominee1", itemGuardian.GuardianFirstName ?? "");
                                                    FieldsValue.SetField("Middle_nominee_1", itemGuardian.GuardianMiddleName ?? "");
                                                    FieldsValue.SetField("Last_nominee_1", itemGuardian.GuardianLastName ?? "");
                                                    FieldsValue.SetField("Last_nominee_1", itemGuardian.GuardianLastName ?? "");
                                                    FieldsValue.SetField("Guardian_nominee1", itemGuardian.GuardianFirstName + " " + itemGuardian.GuardianMiddleName + " " + itemGuardian.GuardianLastName);

                                                    FieldsValue.SetField("Address_Guard_nominee_1", itemGuardian.Address1 + "," + itemGuardian.Address2 ?? "");
                                                    FieldsValue.SetField("city_Guard1", itemGuardian.CityName ?? "");
                                                    FieldsValue.SetField("state_Guard1", itemGuardian.StateName ?? "");
                                                    FieldsValue.SetField("Country_Guard1", itemGuardian.CountryCode ?? "");
                                                    FieldsValue.SetField("PIN_Guard1", itemGuardian.Pincode ?? "");
                                                    FieldsValue.SetField("Percentage1_cp", model.ClientNomineeDetailsModel.NomineePercentage ?? "");

                                                    FieldsValue.SetField("Sole First Holder", model.PersonalDetailsModel.ClientFullName ?? "");
                                                    FieldsValue.SetField("Rel_Guard1", itemGuardian.GuardianRelationshipTypeText);
                                                    //MF Page
                                                    FieldsValue.SetField("Guardian Name If Nominee is Minor_MF", itemGuardian.GuardianFirstName + ' ' + itemGuardian.GuardianMiddleName + ' ' + itemGuardian.GuardianLastName);
                                                    FieldsValue.SetField("Nominee Name_MF", model.ClientNomineeDetailsModel.NomineeFirstName + ' ' + model.ClientNomineeDetailsModel.NomineeLastName);
                                                    FieldsValue.SetField("Relaonship_MF", itemGuardian.GuardianRelationshipTypeText);

                                                    FieldsValue.SetField("Nominee Address_MF", itemGuardian.Address1 + " " + itemGuardian.Address2);
                                                    FieldsValue.SetField("City_4_MF", itemGuardian.CityName);
                                                    FieldsValue.SetField("Pincode_4_MF", itemGuardian.Pincode);
                                                    FieldsValue.SetField("State_3_MF", itemGuardian.StateName);
                                                    FieldsValue.SetField("Date Row1", DateTime.Now.ToString("dd-MM-yyyy"));
                                                    FieldsValue.SetField("Place Row1", itemGuardian.CityName);

                                                    //Added Proof type
                                                    if (itemGuardian.ProffTypeText == "Photograph And Signature")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity", "photo", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "PAN")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity", "PAN", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "AADHAR")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity", "UID", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Saving bank account number")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity", "SB", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Proof of Identity")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity", "IP", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Demat Account Id")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity", "DMat", "true", true);
                                                    }
                                                    FieldsValue.SetField("proof_guard Nominee 1", itemGuardian.ProffTypeText);


                                                }
                                                if (itemGuardian.NomineeType == "77")
                                                {
                                                    FieldsValue.SetField("First _nominee2", itemGuardian.GuardianFirstName ?? "");
                                                    FieldsValue.SetField("Middle_nominee_2", itemGuardian.GuardianMiddleName ?? "");
                                                    FieldsValue.SetField("Last_nominee_2", itemGuardian.GuardianLastName ?? "");
                                                    FieldsValue.SetField("Guardian_nominee2", itemGuardian.GuardianFirstName + " " + itemGuardian.GuardianMiddleName + " " + itemGuardian.GuardianLastName);
                                                    FieldsValue.SetField("Address_Guard_nominee_2", itemGuardian.Address1 + "," + itemGuardian.Address2 ?? "");
                                                    FieldsValue.SetField("city_Guard2", itemGuardian.CityName ?? "");
                                                    FieldsValue.SetField("state_Guard2", itemGuardian.StateName ?? "");
                                                    FieldsValue.SetField("Country_Guard2", itemGuardian.CountryCode ?? "");
                                                    FieldsValue.SetField("PIN_Guard2", itemGuardian.Pincode ?? "");
                                                    FieldsValue.SetField("Rel_Guard2", itemGuardian.GuardianRelationshipTypeText);

                                                    if (itemGuardian.ProffTypeText == "Photograph And Signature")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity1", "photo", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "PAN")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity1", "PAN", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "AADHAR")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity1", "UID", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Saving bank account number")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity1", "SB", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Proof of Identity")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity1", "IP", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Demat Account Id")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity1", "DMat", "true", true);
                                                    }
                                                    FieldsValue.SetField("proof_guard Nominee 2", itemGuardian.ProffTypeText);

                                                }
                                                if (itemGuardian.NomineeType == "78")
                                                {
                                                    FieldsValue.SetField("First _nominee3", itemGuardian.GuardianFirstName ?? "");
                                                    FieldsValue.SetField("Middle_nominee_3", itemGuardian.GuardianMiddleName ?? "");
                                                    FieldsValue.SetField("Last_nominee_3", itemGuardian.GuardianLastName ?? "");
                                                    FieldsValue.SetField("Guardian_nominee3", itemGuardian.GuardianFirstName + " " + itemGuardian.GuardianMiddleName + " " + itemGuardian.GuardianLastName);

                                                    FieldsValue.SetField("Address_Guard_nominee_3", itemGuardian.Address1 + "," + itemGuardian.Address2 ?? "");
                                                    FieldsValue.SetField("city_Guard3", itemGuardian.CityName ?? "");
                                                    FieldsValue.SetField("state_Guard3", itemGuardian.StateName ?? "");
                                                    FieldsValue.SetField("Country_Guard3", itemGuardian.CountryCode ?? "");
                                                    FieldsValue.SetField("PIN_Guard3", itemGuardian.Pincode ?? "");
                                                    FieldsValue.SetField("Rel_Guard3", itemGuardian.GuardianRelationshipTypeText);

                                                    if (itemGuardian.ProffTypeText == "Photograph And Signature")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity2", "photo", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "PAN")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity2", "PAN", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "AADHAR")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity2", "UID", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Saving bank account number")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity2", "SB", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Proof of Identity")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity2", "IP", "true", true);
                                                    }
                                                    else if (itemGuardian.ProffTypeText == "Demat Account Id")
                                                    {
                                                        FieldsValue.SetField("Guard_Identity2", "DMat", "true", true);
                                                    }
                                                    FieldsValue.SetField("proof_guard Nominee 3", itemGuardian.ProffTypeText);
                                                }
                                            }
                                        }
                                        message = "PDF has been generated successfully!";
                                    }
                                    else
                                    {
                                        _logger.LogError("Upload Document issue");
                                        // mControllerErrorLog.Controllerwritelog("BOI PDF Error :" + model.PrimaryDetailsModel.InwardNo + "Above Error is there.");
                                        pdfStamper.Close();
                                        pdfReader.Close();
                                        xdocument.Close();
                                        Blank_FileStream.Dispose();
                                        Blank_FileStream.Close();
                                        message = "MF PDF Error : " + model.PrimaryDetailsModel.InwardNo + " Documents not found.";
                                        _logger.LogError(message);
                                        return Json(message);
                                    }
                                    pdfStamper.FormFlattening = true;
                                    pdfStamper.PartialFormFlattening("field1");
                                    pdfStamper.Close();
                                    pdfReader.Close();
                                    filestream.Dispose();
                                    filestream.Close();
                                    Blank_FileStream.Dispose();
                                    Blank_FileStream.Close();
                                    xdocument.Close();
                                }
                                else
                                {
                                    pdfStamper.FormFlattening = true;
                                    pdfStamper.PartialFormFlattening("field1");
                                    pdfStamper.Close();
                                    pdfReader.Close();
                                    filestream.Dispose();
                                    filestream.Close();
                                    Blank_FileStream.Dispose();
                                    Blank_FileStream.Close();
                                    xdocument.Close();
                                    message = "MF  PDF Error :Proof not found on server";
                                }
                            }
                            pdfReader.Close();
                            filestream.Dispose();
                            filestream.Close();
                            Blank_FileStream.Dispose();
                            Blank_FileStream.Close();
                            xdocument.Close();

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("PDF Issue " + ex.ToString());
                            _logger.LogError(ex.StackTrace);
                            Blank_FileStream.Dispose();
                            Blank_FileStream.Close();
                            xdocument.Close();
                            message = "Fail Error : " + ex.Message.ToString();
                            return Json(message);
                        }
                    }
                    xdocument.Close();
                    //MerGEPDFData(RegistrationId, EsignType);
                    return Json(message);
                }
            }
            else
            {
                _logger.LogError("Data Not Found");
                message = "Error: Data Not Found";
                return Json(message);
            }
        }

        public async Task<JsonResult> GeneratePDF_BOI(int RegistrationId, string PDFSelect, string EsignType)
        {
            string message = string.Empty;
            string Bacode = _appsetting.BaCode;
            int addAttchmentPageNo = Convert.ToInt32(_appsetting.addAttchmentPageNo);

            MainModel model = new MainModel();

            model = await _pdfManager.GetDataFromDatabase(RegistrationId);
            if (model != null)
            {

                if (model.notification == null)
                {
                    message = model.notification.Message;
                    return Json(message);
                }
                else
                {
                    string Blank_File = _appsetting.PDFBlank_BOI;
                    string newBOIFileName = "";
                    newBOIFileName = await _pdfManager.CreateDirectory(RegistrationId, Bacode, PDFSelect, EsignType, model);
                    iTextSharp.text.Document xdocument = null;
                    PdfReader pdfReader = null;

                    using (xdocument = new iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10))
                    using (MemoryStream Memorystream = new MemoryStream())
                    using (var Blank_FileStream = new FileStream(Blank_File, FileMode.Open))
                    using (var newFileStream = new FileStream(newBOIFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    {
                        try
                        {
                            pdfReader = new PdfReader(Blank_FileStream);
                            xdocument.Open();
                            var stamper = new PdfStamper(pdfReader, newFileStream);
                            stamper.FreeTextFlattening = true;
                            var FieldsValue = stamper.AcroFields;
                            var TotalPDFPages = pdfReader.NumberOfPages;

                            List<UploadDocDetailsModel> lstUpload = model.UploadDocsDetailsModel.ToList();
                            DataTable DtUplaodDetails = await _pdfManager.ConvertListToDataTable(lstUpload);
                            DataTable DtClientPhoto = await _pdfManager.GetClientPhoto(model, RegistrationId);
                            DataTable DtClientSign = await _pdfManager.GetClientSign(model, RegistrationId);
                            try
                            {
                                if (DtUplaodDetails.Rows.Count > 0)
                                {
                                    bool DocUplaodStatus = await _pdfManager.DocUploadDetails(model, DtUplaodDetails, pdfReader, addAttchmentPageNo, stamper, model.PersonalDetailsModel, model.PennyDropDetailsModel, model.ClientPermanentAddressModel, RegistrationId, "BOI", EsignType, model.uploadGEOTag);

                                    int PageAddedCount = DtUplaodDetails.Rows.Count;
                                    //for Photo 
                                    bool ClientPhotoStatus = await _pdfManager.ClientPhoto(model, PageAddedCount, pdfReader, stamper, "BOI");
                                    //for Signature
                                    bool ClientSignatureStatus = await _pdfManager.ClientSignature(model, DtClientSign, PageAddedCount, stamper, "BOI");
                                    //For Authority Signature
                                    bool ClientAuthSignatureStatus = await _pdfManager.AuthEmpSign(PageAddedCount, stamper, "BOI");
                                    //For Client Esign Manual --
                                    //bool PDFClientEsignStatus = await _pdfManager.ClientNomineeEsign_BOI(model.PersonalDetailsModel, model.ClientPermanentAddressModel, PageAddedCount, stamper, "BOI", EsignType);

                                    if (DocUplaodStatus == true)
                                    {
                                        IDictionary<String, AcroFields.Item> fields = FieldsValue.Fields;
                                        foreach (String name in fields.Keys)
                                        {
                                            FieldsValue.SetFieldProperty(name, "textcolor", BaseColor.BLUE, null);
                                            FieldsValue.SetFieldProperty(name, "textfont", Font.BOLD, null);
                                        }
                                        //Blank 1st Page
                                        FieldsValue.SetField("formno", model.PrimaryDetailsModel.InwardNo);
                                        FieldsValue.SetField("boid", _appsetting.BOID);
                                        FieldsValue.SetField("aodate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("invardno", model.PrimaryDetailsModel.InwardNo);
                                        FieldsValue.SetField("bacode", model.PrimaryDetailsModel.Bacode);
                                        FieldsValue.SetField("Name of the client _index", model.PersonalDetailsModel.ClientFullName);

                                        FieldsValue.SetField("Text4", model.PersonalDetailsModel.ClientFullName);


                                        //Blank 8th Page
                                        FieldsValue.SetField("dmatfirstname", model.PersonalDetailsModel.ClientFullName);
                                        FieldsValue.SetField("dmatfirstpan", model.PersonalDetailsModel.PAN);
                                        FieldsValue.SetField("dmatfirstaadhar", model.PersonalDetailsModel.UID);
                                        FieldsValue.SetField("UCC", model.PrimaryDetailsModel.CommonClientCode);
                                        FieldsValue.SetField("dmatdate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("dmatfirstpan", model.PersonalDetailsModel.PAN);

                                        FieldsValue.SetField("dmatautoemailaddress", model.PersonalDetailsModel.EmailId ?? "");
                                        FieldsValue.SetField("dmat-autocredit", "YES", "true", true);
                                        FieldsValue.SetField("dmat-autocredit-2", "YES", "true", true);
                                        FieldsValue.SetField("dmat-autoemail", "YES", "true", true);
                                        FieldsValue.SetField("dmat-autoemailrta", "YES", "true", true);
                                        FieldsValue.SetField("dmat-dividend", "YES", "true", true);
                                        FieldsValue.SetField("dmat-acc-statement", "Monthly", "true", true);
                                        FieldsValue.SetField("dmat-annualreport", "ELECTRONIC", "true", true);
                                        FieldsValue.SetField("firstholderfour", model.PersonalDetailsModel.ClientFullName);
                                        FieldsValue.SetField("dptariffplan1", model.ClientDepositoryDetailsModel.PLAN_CODE ?? "", "true", true);

                                        //Blank 4th Page
                                        FieldsValue.SetField("applicationtype", "1", true);
                                        FieldsValue.SetField("kyctype", "true", "true", true);
                                        FieldsValue.SetField("panno", model.PersonalDetailsModel.PAN);
                                        FieldsValue.SetField("name", model.PersonalDetailsModel.ClientFullName);
                                        //FieldsValue.SetField("maidenname", model.PersonalDetailsModel.ClientMiddleName);
                                        //FieldsValue.SetField("fathersposename", model.PersonalDetailsModel.ClientMiddleName);
                                        FieldsValue.SetField("fathersposename", model.PersonalDetailsModel.FatherFirstName + " " + model.PersonalDetailsModel.FatherMiddleName + " " + model.PersonalDetailsModel.FatherLastName);
                                        // PDF Page 11
                                        FieldsValue.SetField("documentvery1", "Neetu Singh");
                                        FieldsValue.SetField("clientinterview1", "Neetu Singh");
                                        FieldsValue.SetField("inpersonverif1", "Neetu Singh");
                                        FieldsValue.SetField("clientdocdesgn", "Senior Executive");
                                        FieldsValue.SetField("clientinterviewdegsn", "Senior Executive");
                                        FieldsValue.SetField("inpersonverifcodedegsn", "Senior Executive");
                                        FieldsValue.SetField("Approvedbydesignationdetail", "Senior Executive");
                                        FieldsValue.SetField("EmploRCcode", model.PrimaryDetailsModel.Bacode);
                                        FieldsValue.SetField("clientinterviewcode", model.PrimaryDetailsModel.Bacode);
                                        FieldsValue.SetField("inpersonverifcode", model.PrimaryDetailsModel.Bacode);
                                        FieldsValue.SetField("documentverydate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("clientinterviewdate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("inpersonverifdate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("verificationdate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("Approvedbynamedetail", "Neetu Singh");
                                        FieldsValue.SetField("chequebankname", model.ClientBankDetailsModel.BankName);

                                        //FieldsValue.SetField("mothername", model.PersonalDetailsModel.MotherFirstName);
                                        FieldsValue.SetField("mothername", model.PersonalDetailsModel.MotherFirstName + " " + model.PersonalDetailsModel.MotherMiddleName + " " + model.PersonalDetailsModel.MotherLastName);
                                        FieldsValue.SetField("dob", model.PersonalDetailsModel.DateOfBirth.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("gender", model.PersonalDetailsModel.GenderText ?? "", "true", true);
                                        FieldsValue.SetField("maritalstatus", model.PersonalDetailsModel.MaritalStatusText ?? "", "true", true);

                                        FieldsValue.SetField("residentialstatus", "INDIAN", "true", true);
                                        FieldsValue.SetField("citizenship", "Indian", "true", true);
                                        // FieldsValue.SetField("residentialstatus", model.PersonalDetailsModel. ?? "", "true", true);

                                        //Set Occupation
                                        if (model.PersonalDetailsModel.OccupationTypeText == "SERVICE")
                                        {
                                            FieldsValue.SetField("occupationtype", "Service", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "OTHERS")
                                        {
                                            FieldsValue.SetField("occupationtype", "OTHERS", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "BUSINESS")
                                        {
                                            FieldsValue.SetField("occupationtype", "BUSINESS", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "SERVICE(PRIVATE COMPANY)")
                                        {
                                            FieldsValue.SetField("occupationtype", "SERVICE(PRIVATE COMPANY)", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "PROFESSIONAL")
                                        {
                                            FieldsValue.SetField("occupationtype", "PROFESSION", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "SERVICE(PUBLIC LTD. COMP.)")
                                        {
                                            FieldsValue.SetField("occupationtype", "SERVICE(PUBLIC LTD. COMP.)", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "GOVERNMENT SERVICES")
                                        {
                                            FieldsValue.SetField("occupationtype", "Governmentsector", true);

                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "RETIRED")
                                        {
                                            FieldsValue.SetField("occupationtype", "RETIRED", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "HOUSEWIFE")
                                        {
                                            FieldsValue.SetField("occupationtype", "HOUSEWIFE", true);
                                        }
                                        else if (model.PersonalDetailsModel.OccupationTypeText == "STUDENT")
                                        {
                                            FieldsValue.SetField("occupationtype", "STUDENT", true);
                                        }

                                        if (model.PrimaryDetailsModel.Bacode == "RC319")
                                        {
                                            FieldsValue.SetField("brokerage-cash-delivery", "0.5");
                                            FieldsValue.SetField("brokerage-cash-intra", "0.03");
                                        }

                                        //POI
                                        FieldsValue.SetField("poi", model.UploadDocsDetailsModel[0].DocName == "" ? "$18$" : model.UploadDocsDetailsModel[0].DocName, "true", true);
                                        FieldsValue.SetField("poi", "$01$", "true", true);
                                        FieldsValue.SetField("poi-pan-card", model.PersonalDetailsModel.PAN == "" ? model.PersonalDetailsModel.PAN : model.PersonalDetailsModel.PAN);

                                        //Per Address
                                        FieldsValue.SetField("poa-address-line1", model.ClientPermanentAddressModel.PerAddress1 ?? "");
                                        FieldsValue.SetField("poa-address-line2", model.ClientPermanentAddressModel.PerAddress2 + "," + model.ClientPermanentAddressModel.PerAddress3 ?? "");
                                        FieldsValue.SetField("poa-address-line3", model.ClientPermanentAddressModel.PerAddress3 ?? "");
                                        FieldsValue.SetField("poa-address-city", model.ClientPermanentAddressModel.PerCity ?? "");
                                        FieldsValue.SetField("poa-address-district", model.ClientPermanentAddressModel.PerCity ?? "");
                                        FieldsValue.SetField("poa-address-pincode", model.ClientPermanentAddressModel.PerPincode ?? "");
                                        FieldsValue.SetField("poa-address-state", model.ClientPermanentAddressModel.PerState ?? "");
                                        FieldsValue.SetField("poa-address-country", model.ClientPermanentAddressModel.PerCountry ?? "");
                                        FieldsValue.SetField("poa-address-ccode", "IN");//model.ClientPermanentAddressModel.PerCountryCode ?? "");
                                        FieldsValue.SetField("poa-address-scode", model.ClientPermanentAddressModel.PerstateCode ?? "");
                                        FieldsValue.SetField("poa-type", "R", "true", true);
                                        FieldsValue.SetField("poa-doc", "$18$", "true", true);

                                        FieldsValue.SetField("poa-corres-line1", model.ClientCorrespondenceAddressModel.CorAddress1 ?? "");
                                        FieldsValue.SetField("poa-corres-line2", model.ClientCorrespondenceAddressModel.CorAddress2 + " " + model.ClientCorrespondenceAddressModel.CorAddress3);
                                        FieldsValue.SetField("poa-corres-line3", model.ClientCorrespondenceAddressModel.CorAddress3 ?? "");
                                        FieldsValue.SetField("poa-corres-city", model.ClientCorrespondenceAddressModel.CorCity ?? "");
                                        FieldsValue.SetField("poa-corres-district", model.ClientCorrespondenceAddressModel.CorCity ?? "");
                                        FieldsValue.SetField("poa-corres-pincode", model.ClientCorrespondenceAddressModel.CorPincode ?? "");
                                        FieldsValue.SetField("poa-corres-state", model.ClientCorrespondenceAddressModel.CorState ?? "");
                                        FieldsValue.SetField("poa-corres-country", model.ClientCorrespondenceAddressModel.CorCountry ?? "");
                                        FieldsValue.SetField("poa-corres-ccode", "IN");//model.ClientCorrespondenceAddressModel.CorCountryCode ?? "");
                                        FieldsValue.SetField("poa-corres-scode", "");// model.ClientCorrespondenceAddressModel.CorstateCode ?? "");

                                        //Page 2
                                        FieldsValue.SetField("emailaddr", model.PersonalDetailsModel.EmailId ?? "");
                                        FieldsValue.SetField("mobile", model.PersonalDetailsModel.MobileNo ?? "");
                                        FieldsValue.SetField("telres", model.PersonalDetailsModel.Telephone2 ?? "");
                                        FieldsValue.SetField("country-jor", model.ClientPermanentAddressModel.PerCountry ?? "");
                                        FieldsValue.SetField("country-jor-code", "IN");// model.ClientPermanentAddressModel.PerCountryCode ?? "");
                                        FieldsValue.SetField("fatca-country-birth", model.ClientPermanentAddressModel.PerCountry ?? "");
                                        FieldsValue.SetField("facta-ccode", "IN");// model.ClientPermanentAddressModel.PerCountryCode ?? "");

                                        //FieldsValue.SetField("realated", "R", "true", true);
                                        //FieldsValue.SetField("realated-type", "AOR", "true", true);
                                        // FieldsValue.SetField("poi-type", "$18$", "true", true);
                                        FieldsValue.SetField("applicationdate", DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("applicationplace", model.ClientPermanentAddressModel.PerCity);
                                        //Employee Details
                                        FieldsValue.SetField("kyc_v_date", DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("kyc_v_name", _appsetting.EmpFullName);
                                        FieldsValue.SetField("kyc_v_code", _appsetting.EmpACMCode);
                                        FieldsValue.SetField("kyc_v_desig", _appsetting.EmpDesignation);
                                        FieldsValue.SetField("ipv_v_date", DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("ipv_v_name", _appsetting.EmpFullName);
                                        FieldsValue.SetField("ipv_v_code", _appsetting.EmpACMCode);
                                        FieldsValue.SetField("ipv_v_desig", _appsetting.EmpDesignation);
                                        //Company Details
                                        FieldsValue.SetField("ins_v_name", _appsetting.CompName);
                                        FieldsValue.SetField("ins_v_code", _appsetting.CompCode);
                                        FieldsValue.SetField("ins_v_branch", _appsetting.CompBranch);
                                        FieldsValue.SetField("ipv_ins_v_name", _appsetting.CompName);
                                        FieldsValue.SetField("ipv_ins_v_code", _appsetting.CompCode);
                                        FieldsValue.SetField("ipv_ins_v_branch", _appsetting.CompBranch);
                                        //Bank Details
                                        FieldsValue.SetField("bank-micr", model.ClientBankDetailsModel.MICRCode ?? "");
                                        FieldsValue.SetField("bank-ifsc", model.ClientBankDetailsModel.IFSCCode ?? "");
                                        FieldsValue.SetField("bank-acc-no", model.ClientBankDetailsModel.AccountNo ?? "");
                                        FieldsValue.SetField("bank-name", model.ClientBankDetailsModel.BankName ?? "");
                                        FieldsValue.SetField("bank-branch", model.ClientBankDetailsModel.BranchName ?? "");
                                        FieldsValue.SetField("bank-branch-address", model.ClientBankDetailsModel.BranchAddress ?? "");
                                        FieldsValue.SetField("bank-branch-city", model.ClientBankDetailsModel.BankCity ?? "");
                                        FieldsValue.SetField("bank-branch-pincode", model.ClientBankDetailsModel.BankPincode ?? "");
                                        FieldsValue.SetField("bank-branch-state", model.ClientBankDetailsModel.BankState ?? "");
                                        FieldsValue.SetField("bank-branch-country", model.ClientBankDetailsModel.BankCountry ?? model.ClientBankDetailsModel.BankCountry);
                                        //FieldsValue.SetField("bank-address", model.ClientBankDetailsModel.BranchAddress ?? "");
                                        FieldsValue.SetField("bank-address", model.ClientBankDetailsModel.Address);
                                        if (model.ClientBankDetailsModel.AccountTypeId == 35)
                                        {
                                            FieldsValue.SetField("acc-type", "SAVINGS BANK A/C", "true", true);
                                        }
                                        else if (model.ClientBankDetailsModel.AccountTypeId == 36)
                                        {
                                            FieldsValue.SetField("acc-type", "CURRENT A/C", "true", true);
                                        }
                                        //Depository Details
                                        FieldsValue.SetField("depository-type", model.ClientDepositoryDetailsModel.Depository ?? "", "true", true);
                                        FieldsValue.SetField("dp-name", model.ClientDepositoryDetailsModel.DepositoryName ?? model.ClientDepositoryDetailsModel.DepositoryName);
                                        FieldsValue.SetField("dp-id", model.ClientDepositoryDetailsModel.DPID ?? "");
                                        FieldsValue.SetField("dp-client-id", model.ClientDepositoryDetailsModel.BOID ?? model.ClientDepositoryDetailsModel.BOID);

                                        FieldsValue.SetField("additional-contract", "EMAIL", "true", true);
                                        FieldsValue.SetField("additional-internet", "YES", "true", true);
                                        FieldsValue.SetField("additional-document", "YES", "true", true);
                                        FieldsValue.SetField("additional-alert", "YES", "true", true);
                                        FieldsValue.SetField("additional-alert-type", "BOTH", "true", true);
                                        // FieldsValue.SetField("additional-alert-relation", "Self", "true", true);
                                        //Fatca Details
                                        if (model.ClientFatcaDetailsModel.AnnualIncome != "" || model.ClientFatcaDetailsModel.AnnualIncome != null)
                                        {
                                            FieldsValue.SetField("income-range1", model.ClientFatcaDetailsModel.AnnualIncome.Trim(), "true", true);
                                            FieldsValue.SetField("income-range2", model.ClientFatcaDetailsModel.AnnualIncome.Trim(), "true", true);
                                            FieldsValue.SetField("income-range3", model.ClientFatcaDetailsModel.AnnualIncome.Trim(), "true", true);
                                            FieldsValue.SetField("income-range4", model.ClientFatcaDetailsModel.AnnualIncome.Trim(), "true", true);
                                            FieldsValue.SetField("income-range5", model.ClientFatcaDetailsModel.AnnualIncome.Trim(), "true", true);
                                        }
                                        FieldsValue.SetField("otherincome", "");
                                        //date found
                                        FieldsValue.SetField("otherincomedate", model.ClientFatcaDetailsModel.AnnualIncomeDate.Replace("/", string.Empty) ?? "");
                                        //FieldsValue.SetField("political-type", model.ClientFatcaDetailsModel.PoliticallyExposePersonText, "true", true);
                                        if (model.ClientFatcaDetailsModel.PoliticallyExposePersonText == "Yes")
                                        {
                                            FieldsValue.SetField("political-type", "PEP", "true", true);
                                        }
                                        else if (model.ClientFatcaDetailsModel.PoliticallyExposePersonText == "No")
                                        {
                                            FieldsValue.SetField("political-type", "NA", "true", true);
                                        }


                                        FieldsValue.SetField("First Name_nominee1", model.PersonalDetailsModel.ClientFullName);




                                        if (model.ClientNomineeDetailsModel.NomineeTypeText == "FirstNominee")
                                        {
                                            //Nominee Details
                                            FieldsValue.SetField("Form No", model.PrimaryDetailsModel.InwardNo);
                                            FieldsValue.SetField("UCC", model.PrimaryDetailsModel.CommonClientCode);
                                            FieldsValue.SetField("place1_cp", model.ClientPermanentAddressModel.PerCity);
                                            FieldsValue.SetField("date_1_cp", DateTime.Now.ToString("dd-MM-yyyy"));
                                            FieldsValue.SetField("Sole First Holder", model.PersonalDetailsModel.ClientFullName ?? "");
                                            FieldsValue.SetField("First Name_nominee1", model.ClientNomineeDetailsModel.NomineeFirstName);
                                            FieldsValue.SetField("Middle_nominee1", model.ClientNomineeDetailsModel.NomineeMiddleName);
                                            FieldsValue.SetField("Last_nominee1", model.ClientNomineeDetailsModel.NomineeLastName);
                                            FieldsValue.SetField("lastdname_Nominee1", model.ClientNomineeDetailsModel.NomineeLastName);
                                            FieldsValue.SetField("address_nominee1", model.ClientNomineeDetailsModel.NomineeAddress1 + "," + model.ClientNomineeDetailsModel.NomineeAddress2 + " " + model.ClientNomineeDetailsModel.NomineeAddress3);
                                            FieldsValue.SetField("city_nominee1", model.ClientNomineeDetailsModel.NomineeCity ?? "");
                                            FieldsValue.SetField("state_nominee1", model.ClientNomineeDetailsModel.NomineeState ?? "");
                                            FieldsValue.SetField("PIN_nominee1", model.ClientNomineeDetailsModel.NomineePincode ?? "");
                                            FieldsValue.SetField("Country_nominee1", model.ClientNomineeDetailsModel.NomineeCountry ?? "");
                                            FieldsValue.SetField("tel_nominee1", model.ClientNomineeDetailsModel.MobileNo ?? "");
                                            FieldsValue.SetField("email_nominee1", model.ClientNomineeDetailsModel.EmailId ?? "");
                                            FieldsValue.SetField("pan_nominee1", model.ClientNomineeDetailsModel.PanNumber ?? "");
                                            FieldsValue.SetField("rel_nominee1", model.ClientNomineeDetailsModel.RelationshipTypeText ?? "");
                                            FieldsValue.SetField("Dob_nominee1", model.ClientNomineeDetailsModel.DOBNominee.ToString("dd-MM-yyyy") ?? "");
                                            FieldsValue.SetField("Percentage1", model.ClientNomineeDetailsModel.NomineePercentage ?? "");

                                            if (model.ClientNomineeDetailsModel.IsResidualSecuritiesText == "Yes")
                                            {
                                                FieldsValue.SetField("RS_1", "Yes", "true", true);
                                            }
                                        }
                                        if (model.ClientSecondNomineeDetailsModel.NomineeTypeText == "SecondNominee")
                                        {
                                            FieldsValue.SetField("First Name_nominee2", model.ClientSecondNomineeDetailsModel.NomineeFirstName);
                                            FieldsValue.SetField("Middle_nominee2", model.ClientSecondNomineeDetailsModel.NomineeMiddleName);
                                            FieldsValue.SetField("Last_nominee2", model.ClientSecondNomineeDetailsModel.NomineeLastName);
                                            // FieldsValue.SetField("lastdname_Nominee1", model.ClientSecondNomineeDetailsModel.NomineeLastName);
                                            FieldsValue.SetField("address_nominee2", model.ClientSecondNomineeDetailsModel.NomineeAddress1 + "," + model.ClientSecondNomineeDetailsModel.NomineeAddress2 + " " + model.ClientNomineeDetailsModel.NomineeAddress3);
                                            FieldsValue.SetField("city_nominee2", model.ClientSecondNomineeDetailsModel.NomineeCity ?? "");
                                            FieldsValue.SetField("state_nominee2", model.ClientSecondNomineeDetailsModel.NomineeState ?? "");
                                            FieldsValue.SetField("PIN_nominee2", model.ClientSecondNomineeDetailsModel.NomineePincode ?? "");
                                            FieldsValue.SetField("Country_nominee2", model.ClientSecondNomineeDetailsModel.NomineeCountry ?? "");
                                            FieldsValue.SetField("tel_nominee2", model.ClientSecondNomineeDetailsModel.MobileNo ?? "");
                                            FieldsValue.SetField("email_nominee2", model.ClientSecondNomineeDetailsModel.EmailId ?? "");
                                            FieldsValue.SetField("pan_nominee2", model.ClientSecondNomineeDetailsModel.PanNumber ?? "");
                                            FieldsValue.SetField("rel_nominee2", model.ClientSecondNomineeDetailsModel.RelationshipTypeText ?? "");
                                            FieldsValue.SetField("Dob_nominee2", model.ClientSecondNomineeDetailsModel.DOBNominee.ToString("dd-MM-yyyy") ?? "");
                                            FieldsValue.SetField("Percentage2", model.ClientSecondNomineeDetailsModel.NomineePercentage ?? "");

                                            if (model.ClientSecondNomineeDetailsModel.IsResidualSecuritiesText == "Yes")
                                            {
                                                FieldsValue.SetField("RS_3", "Yes", "true", true);
                                            }
                                        }
                                        if (model.ClientThirdNomineeDetailsModel.NomineeTypeText == "ThirdNominee")
                                        {
                                            FieldsValue.SetField("First Name_nominee3", model.ClientThirdNomineeDetailsModel.NomineeFirstName);
                                            FieldsValue.SetField("Middle_nominee3", model.ClientThirdNomineeDetailsModel.NomineeMiddleName);
                                            FieldsValue.SetField("Last_nominee3", model.ClientThirdNomineeDetailsModel.NomineeLastName);
                                            //  FieldsValue.SetField("lastdname_Nominee1", model.ClientThirdNomineeDetailsModel.NomineeLastName);
                                            FieldsValue.SetField("address_nominee3", model.ClientThirdNomineeDetailsModel.NomineeAddress1 + "," + model.ClientThirdNomineeDetailsModel.NomineeAddress2 + " " + model.ClientNomineeDetailsModel.NomineeAddress3);
                                            FieldsValue.SetField("city_nominee3", model.ClientThirdNomineeDetailsModel.NomineeCity ?? "");
                                            FieldsValue.SetField("state_nominee3", model.ClientThirdNomineeDetailsModel.NomineeState ?? "");
                                            FieldsValue.SetField("PIN_nominee3", model.ClientThirdNomineeDetailsModel.NomineePincode ?? "");
                                            FieldsValue.SetField("Country_nominee3", model.ClientThirdNomineeDetailsModel.NomineeCountry ?? "");
                                            FieldsValue.SetField("tel_nominee3", model.ClientThirdNomineeDetailsModel.MobileNo ?? "");
                                            FieldsValue.SetField("email_nominee3", model.ClientThirdNomineeDetailsModel.EmailId ?? "");
                                            FieldsValue.SetField("pan_nominee3", model.ClientThirdNomineeDetailsModel.PanNumber ?? "");
                                            FieldsValue.SetField("rel_nominee3", model.ClientThirdNomineeDetailsModel.RelationshipTypeText ?? "");
                                            FieldsValue.SetField("Dob_nominee3", model.ClientThirdNomineeDetailsModel.DOBNominee.ToString("dd-MM-yyyy") ?? "");
                                            FieldsValue.SetField("Percentage3", model.ClientThirdNomineeDetailsModel.NomineePercentage ?? "");

                                            if (model.ClientThirdNomineeDetailsModel.IsResidualSecuritiesText == "Yes")
                                            {
                                                FieldsValue.SetField("RS_4", "Yes", "true", true);
                                            }
                                        }

                                        //// Derivative Details
                                        if (model.ClientDerivativesModel.ProofTypeText != "")
                                        {
                                            switch (model.ClientDerivativesModel.ProofTypeText)
                                            {
                                                case "ITR ACKNOWLEDGEMENT":
                                                    FieldsValue.SetField("trading-segment1", "ITR ACKNOWLEDGEMENT", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "ITR ACKNOWLEDGEMENT");
                                                    break;
                                                case "FORM 16":
                                                    FieldsValue.SetField("trading-segment2", "FORM 16", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "FORM 16");
                                                    break;
                                                case "NETWORTH CERTIFICATE":
                                                    FieldsValue.SetField("trading-segment3", "NETWORTH CERTIFICATE", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "NETWORTH CERTIFICATE");
                                                    break;
                                                case "ANNUAL ACCOUNTS":
                                                    FieldsValue.SetField("trading-segment4", "ANNUAL ACCOUNTS", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "ANNUAL ACCOUNTS");
                                                    break;
                                                case "SALARY SLIPS":
                                                    FieldsValue.SetField("trading-segment5", "SALARY SLIPS", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "SALARY SLIPS");
                                                    break;
                                                case "BANK A/C STATEMENTS":
                                                    FieldsValue.SetField("trading-segment6", "BANK A/C STATEMENTS", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "BANK A/C STATEMENTS");
                                                    break;
                                                case "DP HOLDING STATEMENT":
                                                    FieldsValue.SetField("trading-segment7", "DP HOLDING STATEMENT", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "DP HOLDING STATEMENT");
                                                    break;
                                                case "SELF DECLARATION":
                                                    FieldsValue.SetField("trading-segment8", "SELF DECLARATION", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "SELF DECLARATION");
                                                    break;
                                                case "OWNERSHIP OF ASSETS":
                                                    FieldsValue.SetField("trading-segment9", "OWNERSHIP OF ASSETS", "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", "OWNERSHIP OF ASSETS");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch (model.ClientDerivativesModel.ProofTypeText)
                                            {
                                                case "ITR ACKNOWLEDGEMENT":
                                                    FieldsValue.SetField("trading-segment1", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "FORM 16":
                                                    FieldsValue.SetField("trading-segment2", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "NETWORTH CERTIFICATE":
                                                    FieldsValue.SetField("trading-segment3", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "ANNUAL ACCOUNTS":
                                                    FieldsValue.SetField("trading-segment4", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "SALARY SLIPS":
                                                    FieldsValue.SetField("trading-segment5", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "BANK A/C STATEMENTS":
                                                    FieldsValue.SetField("trading-segment6", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "DP HOLDING STATEMENT":
                                                    FieldsValue.SetField("trading-segment7", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "SELF DECLARATION":
                                                    FieldsValue.SetField("trading-segment8", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                                case "OWNERSHIP OF ASSETS":
                                                    FieldsValue.SetField("trading-segment9", model.ClientDerivativesModel.ProofTypeText, "true", true);
                                                    FieldsValue.SetField("derivatives_attachement_proof", model.ClientDerivativesModel.ProofTypeText);
                                                    break;
                                            }
                                        }

                                        ////Quarterly Set
                                        FieldsValue.SetField("authoritydate", System.DateTime.Now.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("authoritybasis", "quarterly");
                                        FieldsValue.SetField("firstholderthree", model.PersonalDetailsModel.ClientFullName);

                                        //Page 6
                                        FieldsValue.SetField("additional-email", model.PersonalDetailsModel.EmailId ?? "");
                                        FieldsValue.SetField("additional-alert-name", model.PersonalDetailsModel.ClientFullName);

                                        //Page No 8



                                        //Page No 9
                                        FieldsValue.SetField("booklet-option", "ONE", "true", true);

                                        //BSEStar MF Page
                                        FieldsValue.SetField("Name of the First Applicant_MF", model.PersonalDetailsModel.ClientFullName);
                                        FieldsValue.SetField("PAN Number_MF", model.PersonalDetailsModel.PAN);
                                        FieldsValue.SetField("applicationtype_MF", "1", true);
                                        FieldsValue.SetField("Date of Birth_MF", model.PersonalDetailsModel.DateOfBirth.ToString("dd-MM-yyyy"));
                                        FieldsValue.SetField("City_MF", model.ClientPermanentAddressModel.PerCity);
                                        FieldsValue.SetField("State_MF", model.ClientPermanentAddressModel.PerState);
                                        FieldsValue.SetField("Country_MF", "India");
                                        FieldsValue.SetField("Pincode_MF", model.ClientPermanentAddressModel.PerPincode);
                                        FieldsValue.SetField("Mother Name_MF", model.PersonalDetailsModel.MotherFirstName + ' ' + model.PersonalDetailsModel.MotherMiddleName + ' ' + model.PersonalDetailsModel.MotherLastName);
                                        FieldsValue.SetField("Contact Address_MF", model.ClientPermanentAddressModel.PerAddress1 + " " + model.ClientPermanentAddressModel.PerAddress2 + " " + model.ClientPermanentAddressModel.PerAddress3);
                                        FieldsValue.SetField("Father Name_MF", model.PersonalDetailsModel.FatherFirstName + ' ' + model.PersonalDetailsModel.FatherMiddleName + ' ' + model.PersonalDetailsModel.FatherLastName);
                                        FieldsValue.SetField("Email_MF", model.PersonalDetailsModel.EmailId);
                                        FieldsValue.SetField("Mobile_MF", model.PersonalDetailsModel.MobileNo);
                                        FieldsValue.SetField("Fax_MF", "");
                                        FieldsValue.SetField("Fax Res_MF", "");
                                        FieldsValue.SetField("Occupaon_MF", model.PersonalDetailsModel.OccupationTypeText);
                                        FieldsValue.SetField("Occupaon Details_MF", model.PersonalDetailsModel.OccupationTypeText);
                                        FieldsValue.SetField("Income Tax Slab Networth_MF", model.ClientFatcaDetailsModel.AnnualIncome);
                                        FieldsValue.SetField("cr1", "");
                                        FieldsValue.SetField("tr1", "");
                                        if (model.ClientFatcaDetailsModel.PoliticallyExposePersonText == "Yes")
                                        {
                                            FieldsValue.SetField("Check Box1_MF", "Yes", true);
                                        }
                                        else
                                        {

                                            FieldsValue.SetField("Check Box1_MF", "NO", true);
                                        }
                                        FieldsValue.SetField("o1", model.PersonalDetailsModel.OccupationTypeText);
                                        if (model.ClientDepositoryDetailsModel.DPID.Contains("IN"))
                                        {
                                            FieldsValue.SetField("Name of Bank_MF5", model.ClientBankDetailsModel.BankName);
                                            FieldsValue.SetField("Branch_MF5", model.ClientBankDetailsModel.BranchName);
                                            FieldsValue.SetField("AC No_MF5", model.ClientBankDetailsModel.AccountNo);
                                            FieldsValue.SetField("Ac Type_MF5", model.ClientBankDetailsModel.AccountType);
                                            FieldsValue.SetField("IFSC Code_MF5", model.ClientBankDetailsModel.IFSCCode.ToString().ToUpper());
                                            FieldsValue.SetField("Bank Address_MF5", model.ClientBankDetailsModel.Address);
                                            FieldsValue.SetField("City_3_MF5", model.ClientBankDetailsModel.BankCity);
                                            FieldsValue.SetField("Pincode_3_MF5", model.ClientBankDetailsModel.BankPincode);
                                            FieldsValue.SetField("State_2_MF5", model.ClientBankDetailsModel.BankState);
                                            FieldsValue.SetField("Country_3_MF5", "India");
                                        }
                                        else
                                        {
                                            FieldsValue.SetField("Name of Bank_MF4", model.ClientBankDetailsModel.BankName);
                                            FieldsValue.SetField("Branch_MF4", model.ClientBankDetailsModel.BranchName);
                                            FieldsValue.SetField("AC No_MF4", model.ClientBankDetailsModel.AccountNo);
                                            FieldsValue.SetField("Ac Type_MF4", model.ClientBankDetailsModel.AccountType);
                                            FieldsValue.SetField("IFSC Code_MF4", model.ClientBankDetailsModel.IFSCCode.ToString().ToUpper());
                                            FieldsValue.SetField("Bank Address_MF4", model.ClientBankDetailsModel.Address);
                                            FieldsValue.SetField("City_3_MF4", model.ClientBankDetailsModel.BankCity);
                                            FieldsValue.SetField("Pincode_3_MF4", model.ClientBankDetailsModel.BankPincode);
                                            FieldsValue.SetField("State_2_MF4", model.ClientBankDetailsModel.BankState);
                                            FieldsValue.SetField("Country_3_MF4", "India");
                                        }


                                        FieldsValue.SetField("Nominee Name_MF", model.ClientNomineeDetailsModel.NomineeName + ' ' + model.ClientNomineeDetailsModel.NomineeLastName);
                                        FieldsValue.SetField("Relaonship_MF", model.ClientNomineeDetailsModel.RelationshipTypeText);

                                        FieldsValue.SetField("Nominee Address_MF", model.ClientNomineeDetailsModel.NomineeAddress1 + " " + model.ClientNomineeDetailsModel.NomineeAddress2);
                                        FieldsValue.SetField("City_4_MF", model.ClientNomineeDetailsModel.NomineeCity);
                                        FieldsValue.SetField("Pincode_4_MF", model.ClientNomineeDetailsModel.NomineePincode);
                                        FieldsValue.SetField("State_3_MF", model.ClientNomineeDetailsModel.NomineeState);
                                        FieldsValue.SetField("Date Row1", Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy")));
                                        FieldsValue.SetField("Place Row1", model.ClientNomineeDetailsModel.NomineeCity);

                                        message = "PDF has been generated successfully!";
                                        stamper.Close();
                                        pdfReader.Close();
                                        xdocument.Close();
                                        newFileStream.Dispose();
                                        newFileStream.Close();
                                        Blank_FileStream.Dispose();
                                        Blank_FileStream.Close();
                                        xdocument.Close();
                                    }
                                    else
                                    {
                                        //mControllerErrorLog.Controllerwritelog("BOI PDF Error :" + model.PrimaryDetailsModel.InwardNo + "Above Error is there.");
                                        _logger.LogError("BOI PDF Error : " + model.PrimaryDetailsModel.InwardNo + " Proof not found on server.");
                                        stamper.Close();
                                        pdfReader.Close();
                                        xdocument.Close();
                                        newFileStream.Dispose();
                                        newFileStream.Close();
                                        Blank_FileStream.Dispose();
                                        Blank_FileStream.Close();
                                        xdocument.Close();
                                        return Json("Failed");
                                    }
                                }
                                else
                                {
                                    //mControllerErrorLog.Controllerwritelog("BOI  PDF Error :Proof not found on server");
                                    _logger.LogError("BOI  PDF Error :Proof not found on server");
                                    stamper.Close();
                                    pdfReader.Close();
                                    xdocument.Close();
                                    newFileStream.Dispose();
                                    newFileStream.Close();
                                    Blank_FileStream.Dispose();
                                    Blank_FileStream.Close();
                                    xdocument.Close();
                                    return Json("BOI  PDF Error :Proof not found on server");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("Bank Of India PDF" + ex.ToString());
                                _logger.LogError("Bank Of India PDF" + ex.StackTrace);
                                stamper.Close();
                                pdfReader.Close();
                                xdocument.Close();
                                newFileStream.Dispose();
                                newFileStream.Close();
                                Blank_FileStream.Dispose();
                                Blank_FileStream.Close();
                                xdocument.Close();
                                return Json(ex.Message);
                            }
                            stamper.FormFlattening = true;
                            stamper.Close();
                            pdfReader.Close();
                            xdocument.Close();
                            newFileStream.Dispose();
                            newFileStream.Close();
                            Blank_FileStream.Dispose();
                            Blank_FileStream.Close();
                            xdocument.Close();


                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Bank Of India PDF Last 2 catch" + ex.ToString());
                            _logger.LogError("Bank Of India PDF Last 2 catch" + ex.StackTrace);

                            pdfReader.Close();
                            xdocument.Close();
                            newFileStream.Dispose();
                            newFileStream.Close();
                            Blank_FileStream.Dispose();
                            Blank_FileStream.Close();
                            xdocument.Close();

                            return Json("Failed : " + ex.Message);
                        }
                    }
                    MerGEPDFData(RegistrationId, EsignType);
                    return Json(message);
                }
            }
            else
            {
                _logger.LogError("Data Not Found");
                message = "Data Not Found";
                return Json(message);
            }
        }

        public async Task<JsonResult> GenarateEsign(int RegistrationId)
        {
            string message = string.Empty;
            UCCTempModel mUCCTempModel = new UCCTempModel();
            GenarateEsignModel genarateEsignModel = new GenarateEsignModel();
            ResponceEsign responceEsign = new ResponceEsign();
            mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(RegistrationId);
            int DerPAgeCnt = 0;
            if (mUCCTempModel.FilePath != "")
            {
                string ext = Path.GetExtension(mUCCTempModel.FileName);

                if (ext == ".pdf")
                {
                    PdfReader pdfReaderDer = new PdfReader(mUCCTempModel.FilePath);
                    DerPAgeCnt = pdfReaderDer.NumberOfPages;
                }
            }
            string docPathCP = "";
            string docPathCP1 = "";
            int PageCount = 0;
            if (mUCCTempModel.BACode == "RC319")
            {
                docPathCP = _appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\20" + mUCCTempModel.UCC + "_BOI.pdf";
                docPathCP1 = @"20" + mUCCTempModel.UCC + "_BOI.pdf";
                PageCount = Convert.ToInt16(_appsetting.PagestaticBOICount) + DerPAgeCnt;
            }
            else
            {
                docPathCP = _appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\20" + mUCCTempModel.UCC + ".pdf";
                docPathCP1 = @"20" + mUCCTempModel.UCC + ".pdf";
                PageCount = Convert.ToInt16(_appsetting.PagestaticCount) + DerPAgeCnt;
            }

            PdfReader pdfReader = new PdfReader(docPathCP);
            int pageno = pdfReader.NumberOfPages;

            int count = pageno - PageCount;
            genarateEsignModel.firstHolderName = mUCCTempModel.ClientName;
            genarateEsignModel.ucc = mUCCTempModel.UCC;
            genarateEsignModel.cityName = mUCCTempModel.CityName;
            genarateEsignModel.inwardno = mUCCTempModel.BACode;
            genarateEsignModel.signMode = 1;
            genarateEsignModel.eSignTypeId = 1;
            genarateEsignModel.pageNo = count;

            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(genarateEsignModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EsignBaseurl);
                //Sending request to find web api REST service using HttpClient  

                HttpResponseMessage response = await client
                                   .PostAsync(StaticValues.CreateEsign, requestContent);

                if (response.IsSuccessStatusCode)

                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    responceEsign = JsonSerializer.Deserialize<ResponceEsign>(objResponse);
                }
                else
                {
                    _logger.LogError("Failed Mobile Details Invalid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Digio" + ex.ToString());
                _logger.LogError("Digio" + ex.StackTrace);
                return Json(ex.ToString());
            }
            return Json(responceEsign);
        }

        public void MerGEPDFData(int RegistrationId, string EsignType)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            try
            {
                string message = string.Empty;
                UCCTempModel mUCCTempModel = new UCCTempModel();
                GenarateEsignModel genarateEsignModel = new GenarateEsignModel();
                ResponceEsign responceEsign = new ResponceEsign();
                mUCCTempModel = _clientRegistrationManager.GetUCCByRegistrationID(RegistrationId);
                string targetPath = _appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\PdfFiles";
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                string ApprovePDF = "";

                if (mUCCTempModel.FilePath == "")
                {
                    return;
                }

                if (mUCCTempModel.BACode == "RC319" && EsignType == "Yes")
                {
                    ApprovePDF = _appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\20" + mUCCTempModel.UCC + "_BOI.pdf";
                }
                else if (mUCCTempModel.BACode == "RC319" && EsignType == "No")
                {
                    return;
                    //ApprovePDF = _appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\20" + mUCCTempModel.UCC + "Normal_BOI.pdf";
                }
                else
                {
                    ApprovePDF = _appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\20" + mUCCTempModel.UCC + ".pdf";
                }

                string fileName = Path.GetFileName(mUCCTempModel.FilePath);
                string ApproveFileName = Path.GetFileName(ApprovePDF);
                string FileNameext = Path.GetFileName(fileName);

                string strext = Path.GetExtension(FileNameext);
                if (strext == ".pdf")
                {
                    System.IO.File.Copy(mUCCTempModel.FilePath, targetPath + "\\" + fileName, true);
                    System.IO.File.Copy(ApprovePDF, targetPath + "\\" + ApproveFileName, true);


                    string[] filenames = System.IO.Directory.GetFiles(targetPath);
                    string outputFileName = Path.GetFileName(ApprovePDF);// "20" + mUCCTempModel.UCC + ".pdf";
                    string outputPath = ApprovePDF;//_appsetting.PDFNewFilePath + "20" + mUCCTempModel.UCC + "\\20" + mUCCTempModel.UCC + ".pdf";

                    PdfCopy writer = new PdfCopy(doc, new FileStream(outputPath, FileMode.Create));
                    if (writer == null)
                    {
                        return;
                    }
                    doc.Open();
                    foreach (string filename in filenames)
                    {
                        PdfReader reader = new PdfReader(filename);
                        reader.ConsolidateNamedDestinations();
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = writer.GetImportedPage(reader, i);
                            writer.AddPage(page);
                        }
                        reader.Close();
                    }
                    writer.Close();
                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("MergePDF Error" + ex.ToString());
                _logger.LogError("MERGEPDF" + ex.StackTrace);
                doc.Close();
            }
        }


    }
}
