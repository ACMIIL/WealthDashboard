using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using WealthDashboard.Configuration;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.PDFManager
{
    public class PDFManager : IPDFModel
    {
        #region Global Variable
        private readonly ILogger<PDFManager> _logger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;

        #endregion

        #region Ctor
        public PDFManager(ILogger<PDFManager> logger, IOptions<ConnectionStrings> connectionstring, IOptions<Appsetting> options)
        {
            _connectionStrings = connectionstring.Value;
            _appsetting = options.Value;
            _logger = logger;
        }
        #endregion

        #region Method
        public async Task<DataTable> ConvertListToDataTable(List<UploadDocDetailsModel> upload)
        {
            DataTable dataTable = new DataTable();
            try
            {
                foreach (var property in typeof(UploadDocDetailsModel).GetProperties())
                {
                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }
                foreach (var uploaddata in upload)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var property in typeof(UploadDocDetailsModel).GetProperties())
                    {
                        row[property.Name] = property.GetValue(uploaddata);
                    }
                    dataTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "PDF Method : ConvertListToDataTable");
            }
            return dataTable;
        }

        public async Task<DataTable> ConvertListToDataTable(UploadPhotoDocs photo)
        {
            DataTable dataTable = new DataTable();
            try
            {
                foreach (var property in typeof(UploadPhotoDocs).GetProperties())
                {
                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }
                if (photo != null)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var property in typeof(UploadPhotoDocs).GetProperties())
                    {
                        row[property.Name] = property.GetValue(photo);
                    }
                    dataTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "PDF Method : ConvertListToDataTable");
            }
            return dataTable;
        }

        public async Task<DataTable> ConvertListToDataTable(UploadSignDocs sign)
        {
            DataTable dataTable = new DataTable();
            try
            {
                foreach (var property in typeof(UploadSignDocs).GetProperties())
                {
                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }
                if (sign != null)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var property in typeof(UploadSignDocs).GetProperties())
                    {
                        row[property.Name] = property.GetValue(sign);
                    }
                    dataTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "PDF Method : ConvertListToDataTable");
            }
            return dataTable;
        }

        public async Task<DataTable> GetClientPhoto(MainModel model, int RegistraionId)
        {
            DataTable dt = await ConvertListToDataTable(model.uploadPhotoDocs);
            return dt;
        }

        public async Task<DataTable> GetClientSign(MainModel model, int RegistraionId)
        {
            DataTable dt = await ConvertListToDataTable(model.uploadSignDocs);
            return dt;
        }

        public async Task<string> CreateDirectory(int RegistrationId, string BAcode, string PDFSelect, string EsignType, MainModel model)
        {
            string newFileName = "";
            try
            {
                string GenerateFilePath = _appsetting.PDFNewFilePath;

                if (!Directory.Exists(GenerateFilePath + "\"" + model.PrimaryDetailsModel.InwardNo + "\"" + model.PrimaryDetailsModel.InwardNo + ""))
                    Directory.CreateDirectory(GenerateFilePath + "" + model.PrimaryDetailsModel.InwardNo + "");
                if (EsignType == "Yes")
                {
                    if (BAcode == "RC319" && PDFSelect == "BOI")
                    {
                        newFileName = GenerateFilePath + model.PrimaryDetailsModel.InwardNo + @"\" + model.PrimaryDetailsModel.InwardNo + "_BOI.pdf";
                    }
                    else if (PDFSelect == "MF")
                    {
                        newFileName = GenerateFilePath + model.PrimaryDetailsModel.InwardNo + @"\" + model.PrimaryDetailsModel.InwardNo + "_BSESTARMF.pdf";
                    }
                    else
                    {
                        newFileName = GenerateFilePath + model.PrimaryDetailsModel.InwardNo + @"\" + model.PrimaryDetailsModel.InwardNo + ".pdf";
                    }
                    // Check if file exists
                    if (File.Exists(newFileName))
                    {
                        File.Delete(newFileName);
                    }
                }
                else
                {
                    if (BAcode == "RC319" && PDFSelect == "BOI")
                    {
                        newFileName = GenerateFilePath + model.PrimaryDetailsModel.InwardNo + @"\" + model.PrimaryDetailsModel.InwardNo + "Normal_BOI.pdf";
                    }
                    else if (PDFSelect == "MF")
                    {
                        newFileName = GenerateFilePath + model.PrimaryDetailsModel.InwardNo + @"\" + model.PrimaryDetailsModel.InwardNo + "Normal_BSESTARMF.pdf";
                    }
                    else
                    {
                        newFileName = GenerateFilePath + model.PrimaryDetailsModel.InwardNo + @"\" + model.PrimaryDetailsModel.InwardNo + "Normal.pdf";
                    }
                    // Check if file exists
                    if (File.Exists(newFileName))
                    {
                        File.Delete(newFileName);
                    }
                }

            }
            catch (Exception ex)
            {

                _logger.LogError("PDF Method : " + ex.Message);
            }

            return newFileName;
        }

        public async Task<bool> DocUploadDetails(MainModel model, DataTable DtUplaodDetails, PdfReader pdfReader, int addAttchmentPageNo, PdfStamper stamper, PersonalDetailsModel mPersonalDetailsModel, PennyDropDetailsModel mPennyDropDetailsModel, ClientPermanentAddressModel mClientPermanentAddressModel, int RegistrationId, string PDFType, string EsignType, UploadGEOTag uploadGEO)
        {
            bool UploadStatus = false;
            string signatureofauthority = _appsetting.AuthorizedSignPath;
            string EsignStamp = _appsetting.EsignStamp;
            try
            {
                string ESign1stLine2Page = "", ESign2ndLine2Page = "", ESign3rdstLine2Page = "", ESign4thstLine2Page = "";
                BaseFont bfFontI2Page = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (EsignType == "Yes")
                {
                    var pdfContentByteImage22 = stamper.GetOverContent(2);

                    pdfContentByteImage22.SetFontAndSize(bfFontI2Page, 7);

                    ESign1stLine2Page = "Digitally Signed By :";
                    pdfContentByteImage22.ShowTextAligned(0, ESign1stLine2Page, 400, 180, 0);
                    ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                    pdfContentByteImage22.ShowTextAligned(0, ESign2ndLine2Page, 400, 173, 0);
                    ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                    pdfContentByteImage22.ShowTextAligned(0, ESign3rdstLine2Page, 400, 167, 0);
                    ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                    pdfContentByteImage22.ShowTextAligned(0, ESign4thstLine2Page, 400, 160, 0);
                    Image signEsign2Page = Image.GetInstance(EsignStamp);
                    signEsign2Page.SetAbsolutePosition(360, 150);
                    signEsign2Page.ScaleAbsoluteHeight(30);
                    signEsign2Page.ScaleAbsoluteWidth(30);
                    pdfContentByteImage22.AddImage(signEsign2Page);


                    var pdfMITC = stamper.GetOverContent(23);

                    pdfMITC.SetFontAndSize(bfFontI2Page, 7);
                    ESign1stLine2Page = "Digitally Signed By :";
                    pdfMITC.ShowTextAligned(0, ESign1stLine2Page, 130, 80, 0);
                    ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                    pdfMITC.ShowTextAligned(0, ESign2ndLine2Page, 130, 70, 0);
                    ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                    pdfMITC.ShowTextAligned(0, ESign3rdstLine2Page, 130, 60, 0);
                    ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                    pdfMITC.ShowTextAligned(0, ESign4thstLine2Page, 130, 50, 0);
                    Image signEsign1Page = Image.GetInstance(EsignStamp);
                    signEsign1Page.SetAbsolutePosition(100, 40);
                    signEsign1Page.ScaleAbsoluteHeight(30);
                    signEsign1Page.ScaleAbsoluteWidth(30);
                    pdfMITC.AddImage(signEsign1Page);
                }

                if (PDFType != "BOI" && EsignType == "Yes")
                {
                    var pdfContentByteImageEsigb22 = stamper.GetOverContent(21);
                    pdfContentByteImageEsigb22.SetFontAndSize(bfFontI2Page, 7);

                    //1 st sign
                    Image signEsign22Page = Image.GetInstance(EsignStamp);
                    ESign1stLine2Page = "Digitally Signed By :";
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign1stLine2Page, 390, 595, 0);
                    ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign2ndLine2Page, 390, 585, 0);
                    ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign3rdstLine2Page, 390, 575, 0);
                    ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign4thstLine2Page, 390, 565, 0);

                    signEsign22Page = Image.GetInstance(EsignStamp);
                    signEsign22Page.SetAbsolutePosition(360, 560);
                    signEsign22Page.ScaleAbsoluteHeight(30);
                    signEsign22Page.ScaleAbsoluteWidth(30);
                    pdfContentByteImageEsigb22.AddImage(signEsign22Page);

                    //2nd Sign                   
                    ESign1stLine2Page = "Digitally Signed By :";
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign1stLine2Page, 390, 540, 0);
                    ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign2ndLine2Page, 390, 530, 0);
                    ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign3rdstLine2Page, 390, 520, 0);
                    ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign4thstLine2Page, 390, 510, 0);

                    signEsign22Page = Image.GetInstance(EsignStamp);
                    signEsign22Page.SetAbsolutePosition(360, 515);
                    signEsign22Page.ScaleAbsoluteHeight(30);
                    signEsign22Page.ScaleAbsoluteWidth(30);
                    pdfContentByteImageEsigb22.AddImage(signEsign22Page);
                    //3rd sign

                    ESign1stLine2Page = "Digitally Signed By :";
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign1stLine2Page, 390, 420, 0);
                    ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign2ndLine2Page, 390, 410, 0);
                    ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign3rdstLine2Page, 390, 400, 0);
                    ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign4thstLine2Page, 390, 390, 0);

                    signEsign22Page = Image.GetInstance(EsignStamp);
                    signEsign22Page.SetAbsolutePosition(360, 385);
                    signEsign22Page.ScaleAbsoluteHeight(30);
                    signEsign22Page.ScaleAbsoluteWidth(30);
                    pdfContentByteImageEsigb22.AddImage(signEsign22Page);

                    //4 sign
                    ESign1stLine2Page = "Digitally Signed By :";
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign1stLine2Page, 390, 380, 0);
                    ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign2ndLine2Page, 390, 370, 0);
                    ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign3rdstLine2Page, 390, 360, 0);
                    ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                    pdfContentByteImageEsigb22.ShowTextAligned(0, ESign4thstLine2Page, 390, 350, 0);

                    signEsign22Page = Image.GetInstance(EsignStamp);
                    signEsign22Page.SetAbsolutePosition(360, 355);
                    signEsign22Page.ScaleAbsoluteHeight(30);
                    signEsign22Page.ScaleAbsoluteWidth(30);
                    pdfContentByteImageEsigb22.AddImage(signEsign22Page);

                    if (model.DerivativeSegmentModel != null)
                    {
                        foreach (var item in model.DerivativeSegmentModel)
                        {
                            if (item.SegmentMasterId == 8)
                            {
                                var pdfContentByteImageEsigbMTF = stamper.GetOverContent(22);
                                pdfContentByteImageEsigbMTF.SetFontAndSize(bfFontI2Page, 7);
                                Image signEsignMTFPage = Image.GetInstance(EsignStamp);
                                ESign1stLine2Page = "Digitally Signed By :";
                                pdfContentByteImageEsigbMTF.ShowTextAligned(0, ESign1stLine2Page, 400, 165, 0);
                                ESign2ndLine2Page = "Name :" + mPersonalDetailsModel.ClientFullName;
                                pdfContentByteImageEsigbMTF.ShowTextAligned(0, ESign2ndLine2Page, 400, 155, 0);
                                ESign3rdstLine2Page = "Location :" + mClientPermanentAddressModel.PerCity;
                                pdfContentByteImageEsigbMTF.ShowTextAligned(0, ESign3rdstLine2Page, 400, 145, 0);
                                ESign4thstLine2Page = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                                pdfContentByteImageEsigbMTF.ShowTextAligned(0, ESign4thstLine2Page, 400, 135, 0);
                                signEsignMTFPage = Image.GetInstance(EsignStamp);
                                signEsignMTFPage.SetAbsolutePosition(350, 120);
                                signEsignMTFPage.ScaleAbsoluteHeight(30);
                                signEsignMTFPage.ScaleAbsoluteWidth(30);
                                pdfContentByteImageEsigbMTF.AddImage(signEsignMTFPage);
                            }
                        }
                    }
                }
                string StrDocName = "";
                string MaskFilePath = "";
                //string MaskFilePathBack = "";
                if (DtUplaodDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < DtUplaodDetails.Rows.Count; i++)
                    {
                        StrDocName = DtUplaodDetails.Rows[i]["DocName"].ToString();
                        var rectangle = pdfReader.GetPageSize(1);
                        string fileName = Path.GetFileName(DtUplaodDetails.Rows[i]["FilePath"].ToString());
                        string strext = Path.GetExtension(fileName);
                        if (strext != ".pdf")
                        {
                            addAttchmentPageNo = addAttchmentPageNo + 1;
                        }
                        stamper.InsertPage(addAttchmentPageNo, rectangle);
                        var pdfUploadDoc = stamper.GetOverContent(addAttchmentPageNo);
                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        pdfUploadDoc.SetColorFill(BaseColor.BLACK);
                        pdfUploadDoc.SetFontAndSize(bf, 12);
                        pdfUploadDoc.BeginText();
                        pdfUploadDoc.ShowTextAligned(100, StrDocName, 60, 820, 0);
                        if (StrDocName == "AadharCardFront")
                        {
                            MaskFilePath = DtUplaodDetails.Rows[i]["FilePath"].ToString().Replace("_AadharCardFront", "_AadharCardFront_mask");

                            if (File.Exists(MaskFilePath))
                            {
                                var UploadDoc = Image.GetInstance(MaskFilePath);
                                UploadDoc.SetAbsolutePosition(40, 300);
                                UploadDoc.ScaleAbsolute(63, 78);
                                UploadDoc.ScaleAbsoluteHeight(300);
                                UploadDoc.ScaleAbsoluteWidth(500);
                                pdfUploadDoc.AddImage(UploadDoc);
                            }
                            else
                            {
                                var UploadDoc = Image.GetInstance(DtUplaodDetails.Rows[i]["FilePath"].ToString());
                                UploadDoc.SetAbsolutePosition(100, 300);
                                UploadDoc.ScaleAbsolute(63, 78);
                                UploadDoc.ScaleAbsoluteHeight(450);
                                UploadDoc.ScaleAbsoluteWidth(400);
                                pdfUploadDoc.AddImage(UploadDoc);
                            }
                        }
                        else if (StrDocName == "PanCard")
                        {
                            var UploadDoc = Image.GetInstance(DtUplaodDetails.Rows[i]["FilePath"].ToString());
                            UploadDoc.SetAbsolutePosition(100, 300);
                            UploadDoc.ScaleAbsolute(63, 78);
                            UploadDoc.ScaleAbsoluteHeight(450);
                            UploadDoc.ScaleAbsoluteWidth(400);
                            pdfUploadDoc.AddImage(UploadDoc);

                        }
                        else
                        {
                            var UploadDoc = iTextSharp.text.Image.GetInstance(DtUplaodDetails.Rows[i]["FilePath"].ToString());
                            UploadDoc.SetAbsolutePosition(40, 300);
                            UploadDoc.ScaleAbsolute(63, 78);
                            UploadDoc.ScaleAbsoluteHeight(300);
                            UploadDoc.ScaleAbsoluteWidth(500);
                            pdfUploadDoc.AddImage(UploadDoc);
                        }

                        if (EsignType == "Yes")
                        {

                            BaseFont bfFontI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            pdfUploadDoc.SetFontAndSize(bfFontI, 7);

                            string ESign1stLine = "Digitally Signed By :";
                            pdfUploadDoc.ShowTextAligned(0, ESign1stLine, 470, 56, 0);
                            string ESign2ndLine = "Name :" + mPersonalDetailsModel.ClientFullName;
                            pdfUploadDoc.ShowTextAligned(0, ESign2ndLine, 470, 49, 0);
                            string ESign3rdstLine = "Location :" + mClientPermanentAddressModel.PerCity;
                            pdfUploadDoc.ShowTextAligned(0, ESign3rdstLine, 470, 42, 0);
                            string ESign4thstLine = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                            pdfUploadDoc.ShowTextAligned(0, ESign4thstLine, 470, 35, 0);

                            Image signEsign = Image.GetInstance(EsignStamp);
                            signEsign.SetAbsolutePosition(440, 20);
                            signEsign.ScaleAbsoluteHeight(30);
                            signEsign.ScaleAbsoluteWidth(30);
                            pdfUploadDoc.AddImage(signEsign);

                            Image signinsidestamp2 = Image.GetInstance(signatureofauthority);
                            signinsidestamp2.SetAbsolutePosition(144, 24);
                            signinsidestamp2.ScaleAbsoluteHeight(115);
                            signinsidestamp2.ScaleAbsoluteWidth(200);
                            pdfUploadDoc.AddImage(signinsidestamp2);
                        }

                        if (StrDocName == "CancelCheque")
                        {
                            if (mPennyDropDetailsModel.Checkfuzzymatchscore >= 70)
                            {
                                pdfUploadDoc.ShowTextAligned(0, "Beneficiary Name : " + mPennyDropDetailsModel.beneficiary_name_with_bank, 170, 356, 0);
                            }
                        }



                        if (StrDocName == "AadharCardFront")
                        {
                            //string GeoLine1 = "Geo Location Details :";
                            //pdfUploadDoc.ShowTextAligned(0, GeoLine1, 20, 90, 0);
                            //string Longitude = "Longitude :" + uploadGEO.Longitude;
                            //pdfUploadDoc.ShowTextAligned(0, Longitude, 20, 75, 0);
                            //string Latitude = "Latitude :" + uploadGEO.Latitude;
                            //pdfUploadDoc.ShowTextAligned(0, Latitude, 20, 60, 0);
                            //string City = "City :" + uploadGEO.City;
                            //pdfUploadDoc.ShowTextAligned(0, City, 20, 45, 0);
                            //string ZipCode = "ZipCode :" + uploadGEO.ZipCode;
                            //pdfUploadDoc.ShowTextAligned(0, ZipCode, 20, 30, 0);
                            //string GeoDate = "Date :" + uploadGEO.EntryDate.ToString();
                            //pdfUploadDoc.ShowTextAligned(0, GeoDate, 20, 15, 0);

                        }

                        PdfContentByte canvas = stamper.GetOverContent(addAttchmentPageNo);
                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(DateTime.Now.ToString("dd-MM-yyyy")), 175, 45, 0);
                    }
                }
                UploadStatus = true;
            }
            catch (Exception ex)
            {
                //mControllerErrorLog.Controllerwritelog("Proof not set So pdf not print with Data" + ex.ToString());
                UploadStatus = false;
                _logger.LogError("PDF_DocUploadDetails " + ex.Message);
                _logger.LogError("PDF_DocUploadDetails " + ex.StackTrace);
            }
            return UploadStatus;
        }

        public async Task<MainModel> GetDataFromDatabase(int RegistrationId)
        {
            string ErrorMessage = string.Empty;
            MainModel mModel = new MainModel();
            Notification notification = new Notification();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "[dbo].[sp_Pdf_Generate_UAT]";
                    sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    var count = ds.Tables[21].Rows.Count;

                    if (count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[21].Rows)
                        {
                            ErrorMessage = dr["Message"].ToString();

                            if (ErrorMessage != string.Empty)
                            {
                                notification.IsError = true;
                                notification.Message = ErrorMessage;
                                notification.Type = "Error";

                                _logger.LogError(ErrorMessage);

                                mModel.notification = notification;

                                return mModel;

                            }
                        }
                    }
                    else
                    {
                        notification.IsError = false;
                        notification.Message = "";
                        notification.Type = "Information";

                        mModel.notification = notification;

                        // ---======================= Primary details  ======================================== 
                        #region Primary Details

                        PrimaryDetailsModel primaryDetailsModel = new PrimaryDetailsModel();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            primaryDetailsModel.NoOfHolders = Convert.ToInt32(dr["NoOfHolders"]);
                            primaryDetailsModel.RegistrationId = Convert.ToInt32(dr["RegistrationId"]);
                            primaryDetailsModel.InwardNo = Convert.IsDBNull(dr["InwardNo"]) ? "" : dr["InwardNo"].ToString();
                            primaryDetailsModel.CommonClientCode = Convert.IsDBNull(dr["CommonClientCode"]) ? "" : dr["CommonClientCode"].ToString();
                            primaryDetailsModel.ClientCategoryId = Convert.IsDBNull(dr["ClientCategoryId"]) ? 0 : Convert.ToInt32(dr["ClientCategoryId"]);
                            primaryDetailsModel.ClientCategoryName = Convert.IsDBNull(dr["ClientCategoryName"].ToString()) ? "" : dr["ClientCategoryName"].ToString();
                            primaryDetailsModel.Bacode = Convert.IsDBNull(dr["Bacode"]) ? "" : dr["Bacode"].ToString();
                            // mModel.PrimaryDetailsModel.NoOfHolders = Convert.IsDBNull(dr["NoOfHolders"]) ? 0 : Convert.ToInt32(dr["NoOfHolders"]);
                            primaryDetailsModel.EmailRelationShip = Convert.IsDBNull(dr["Email_relation"]) ? "" : dr["Email_relation"].ToString();
                            primaryDetailsModel.MobileRelationShip = Convert.IsDBNull(dr["Mobile_relation"]) ? "" : dr["Mobile_relation"].ToString();
                            primaryDetailsModel.ConsentBSDAFacility = Convert.IsDBNull(dr["ConsentBSDAFacility"]) ? "" : dr["ConsentBSDAFacility"].ToString();
                            primaryDetailsModel.ConsentCDSLPOA = Convert.IsDBNull(dr["ConsentCDSLPOA"]) ? "" : dr["ConsentCDSLPOA"].ToString();
                            primaryDetailsModel.ConsentOnlineAcc = Convert.IsDBNull(dr["ConsentOnlineAcc"]) ? "" : dr["ConsentOnlineAcc"].ToString();

                        }
                        mModel.PrimaryDetailsModel = primaryDetailsModel;

                        #endregion


                        // ---======================= Next Upload documents ========================================  

                        #region Upload Documents details

                        List<UploadDocDetailsModel> upload = new List<UploadDocDetailsModel>();

                        DataTable dtdoc = ds.Tables[1];

                        foreach (DataRow dr in dtdoc.Rows)
                        {
                            upload.Add(new UploadDocDetailsModel()
                            {

                                ClientUploadDataId = Convert.IsDBNull(dr["ClientUploadDataId"]) ? 0 : Convert.ToInt32(dr["ClientUploadDataId"]),
                                RegistrationId = Convert.ToInt32(dr["RegistrationId"]),
                                DocName = Convert.IsDBNull(dr["DocName"]) ? "" : dr["DocName"].ToString(),
                                UploadDocumentId = Convert.IsDBNull(dr["UploadDocumentId"]) ? 0 : Convert.ToInt32(dr["UploadDocumentId"]),
                                UploadDocNo = Convert.IsDBNull(dr["UploadDocNo"]) ? "" : dr["UploadDocNo"].ToString(),
                                FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString(),
                                FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString()

                            });
                        }


                        mModel.UploadDocsDetailsModel = upload;

                        #endregion


                        // ---======================= Next Get Personal details sp ========================================  

                        #region Personal Details

                        PersonalDetailsModel personal = new PersonalDetailsModel();

                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            personal.PersonalDetailsId = Convert.IsDBNull(dr["PersonalDetailsId"]) ? 0 : Convert.ToInt32(dr["PersonalDetailsId"]);
                            personal.RegistrationId = Convert.ToInt32(dr["RegistrationId"]);
                            personal.Bacode = Convert.IsDBNull(dr["Bacode"]) ? "" : dr["Bacode"].ToString();
                            personal.ClientFullName = Convert.IsDBNull(dr["ClientFullName"]) ? "" : dr["ClientFullName"].ToString();
                            personal.ClientFirstName = Convert.IsDBNull(dr["ClientFirstName"]) ? "" : dr["ClientFirstName"].ToString();
                            personal.ClientMiddleName = Convert.IsDBNull(dr["ClientMiddleName"]) ? "" : dr["ClientMiddleName"].ToString();
                            personal.ClientLastName = Convert.IsDBNull(dr["ClientLastName"]) ? "" : dr["ClientLastName"].ToString();
                            personal.FatherFirstName = Convert.IsDBNull(dr["FatherFirstName"]) ? "" : dr["FatherFirstName"].ToString();
                            personal.FatherMiddleName = Convert.IsDBNull(dr["FatherMiddleName"]) ? "" : dr["FatherMiddleName"].ToString();
                            personal.FatherLastName = Convert.IsDBNull(dr["FatherLastName"]) ? "" : dr["FatherLastName"].ToString();
                            personal.MotherFirstName = Convert.IsDBNull(dr["MotherFirstName"]) ? "" : dr["MotherFirstName"].ToString();
                            personal.MotherMiddleName = Convert.IsDBNull(dr["MotherMiddleName"]) ? "" : dr["MotherMiddleName"].ToString();
                            personal.MotherLastName = Convert.IsDBNull(dr["MotherLastName"]) ? "" : dr["MotherLastName"].ToString();
                            personal.ClientHolderId = Convert.IsDBNull(dr["ClientHolderId"]) ? 0 : Convert.ToInt32(dr["ClientHolderId"]);
                            personal.HolderName = Convert.IsDBNull(dr["HolderName"]) ? "" : dr["HolderName"].ToString();
                            personal.DateOfBirth = Convert.IsDBNull(dr["DateOfBirth"]) ? DateTime.Now : Convert.ToDateTime(dr["DateOfBirth"]);
                            personal.PAN = Convert.IsDBNull(dr["PAN"]) ? "" : dr["PAN"].ToString();
                            personal.Gender = Convert.IsDBNull(dr["Gender"]) ? 0 : Convert.ToInt32(dr["Gender"]);
                            personal.GenderText = Convert.IsDBNull(dr["GenderText"]) ? "" : dr["GenderText"].ToString();
                            personal.MaritalStatus = Convert.IsDBNull(dr["MaritalStatus"]) ? 0 : Convert.ToInt32(dr["MaritalStatus"]);
                            personal.MaritalStatusText = Convert.IsDBNull(dr["MaritalStatusText"]) ? "" : dr["MaritalStatusText"].ToString();
                            personal.OccupationType = Convert.IsDBNull(dr["OccupationType"]) ? 0 : Convert.ToInt32(dr["OccupationType"]);
                            personal.OccupationTypeText = Convert.IsDBNull(dr["OccupationTypeText"]) ? "" : dr["OccupationTypeText"].ToString();
                            personal.Telephone1 = Convert.IsDBNull(dr["Telephone1"]) ? "" : dr["Telephone1"].ToString();
                            personal.Telephone2 = Convert.IsDBNull(dr["Telephone2"]) ? "" : dr["Telephone2"].ToString();
                            personal.EmailId = Convert.IsDBNull(dr["EmailId"]) ? "" : dr["EmailId"].ToString();
                            personal.MobileNo = Convert.IsDBNull(dr["MobileNo"]) ? "" : dr["MobileNo"].ToString();
                            personal.CVLKraFlag = Convert.IsDBNull(dr["CVLKraFlag"]) ? "" : dr["CVLKraFlag"].ToString();
                            personal.SameAddress = Convert.IsDBNull(dr["SameAddress"]) ? false : Convert.ToBoolean(dr["SameAddress"]);
                        }
                        mModel.PersonalDetailsModel = personal;

                        #endregion


                        ///---====================================== Next Client permanent address details =============================================

                        #region Permanent Address

                        ClientPermanentAddressModel addressModel = new ClientPermanentAddressModel();


                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            addressModel.PerAddress1 = Convert.IsDBNull(dr["PerAddress1"]) ? "" : dr["PerAddress1"].ToString();
                            addressModel.PerAddress2 = Convert.IsDBNull(dr["PerAddress2"]) ? "" : dr["PerAddress2"].ToString();
                            addressModel.PerAddress3 = Convert.IsDBNull(dr["PerAddress3"]) ? "" : dr["PerAddress3"].ToString();
                            addressModel.PerCity = Convert.IsDBNull(dr["PerCity"]) ? "" : dr["PerCity"].ToString();
                            addressModel.PerPincode = Convert.IsDBNull(dr["PerPincode"]) ? "" : dr["PerPincode"].ToString();
                            addressModel.PerState = Convert.IsDBNull(dr["PerState"]) ? "" : dr["PerState"].ToString();
                            addressModel.PerstateCode = Convert.IsDBNull(dr["PerstateCode"]) ? "" : dr["PerstateCode"].ToString();
                            addressModel.PerCountry = Convert.IsDBNull(dr["PerCountry"]) ? "" : dr["PerCountry"].ToString();
                            addressModel.PerCountryCode = Convert.IsDBNull(dr["PerCountryCode"]) ? "" : dr["PerCountryCode"].ToString();
                        }


                        mModel.ClientPermanentAddressModel = addressModel;

                        #endregion

                        ///====================================== Next Client Communication address details =============================================  


                        #region Correspondance Address

                        ClientCorrespondenceAddressModel mClientCorrespondenceAddressModel = new ClientCorrespondenceAddressModel();

                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            mClientCorrespondenceAddressModel.CorAddress1 = Convert.IsDBNull(dr["CorAddress1"]) ? "" : dr["CorAddress1"].ToString();
                            mClientCorrespondenceAddressModel.CorAddress2 = Convert.IsDBNull(dr["CorAddress2"]) ? "" : dr["CorAddress2"].ToString();
                            mClientCorrespondenceAddressModel.CorAddress3 = Convert.IsDBNull(dr["CorAddress3"]) ? "" : dr["CorAddress3"].ToString();
                            mClientCorrespondenceAddressModel.CorCity = Convert.IsDBNull(dr["CorCity"]) ? "" : dr["CorCity"].ToString();
                            mClientCorrespondenceAddressModel.CorPincode = Convert.IsDBNull(dr["CorPincode"]) ? "" : dr["CorPincode"].ToString();
                            mClientCorrespondenceAddressModel.CorState = Convert.IsDBNull(dr["CorState"]) ? "" : dr["CorState"].ToString();
                            mClientCorrespondenceAddressModel.CorstateCode = Convert.IsDBNull(dr["CorstateCode"]) ? "" : dr["CorstateCode"].ToString();
                            mClientCorrespondenceAddressModel.CorCountry = Convert.IsDBNull(dr["CorCountry"]) ? "" : dr["CorCountry"].ToString();
                            mClientCorrespondenceAddressModel.CorCountryCode = Convert.IsDBNull(dr["CorCountryCode"]) ? "" : dr["CorCountryCode"].ToString();
                        }

                        mModel.ClientCorrespondenceAddressModel = mClientCorrespondenceAddressModel;


                        #endregion

                        //====================================== Next Client bank details =============================================  


                        #region Bank Details

                        ClientBankDetailsModel mClientBankDetailsModel = new ClientBankDetailsModel();

                        foreach (DataRow dr in ds.Tables[5].Rows)
                        {

                            mClientBankDetailsModel.AccountNo = Convert.IsDBNull(dr["AccountNo"]) ? "" : dr["AccountNo"].ToString();
                            mClientBankDetailsModel.AccountTypeId = Convert.IsDBNull(dr["AccountTypeId"]) ? 0 : Convert.ToInt32(dr["AccountTypeId"]);
                            mClientBankDetailsModel.AccountType = Convert.IsDBNull(dr["AccountType"]) ? "" : dr["AccountType"].ToString();
                            mClientBankDetailsModel.IFSCMasterId = Convert.IsDBNull(dr["IFSCMasterId"]) ? 0 : Convert.ToInt32(dr["IFSCMasterId"]);
                            mClientBankDetailsModel.IFSCCode = Convert.IsDBNull(dr["IFSCCode"]) ? "" : dr["IFSCCode"].ToString();
                            mClientBankDetailsModel.DefaultBank = Convert.IsDBNull(dr["DefaultBank"]) ? false : Convert.ToBoolean(dr["DefaultBank"]);
                            mClientBankDetailsModel.UpiId = Convert.IsDBNull(dr["UpiId"]) ? "" : dr["UpiId"].ToString();
                            mClientBankDetailsModel.MICRCode = Convert.IsDBNull(dr["MICRCode"]) ? "" : dr["MICRCode"].ToString();

                            mClientBankDetailsModel.BankName = Convert.IsDBNull(dr["BankName"]) ? "" : dr["BankName"].ToString();
                            mClientBankDetailsModel.BankCity = Convert.IsDBNull(dr["BankCity"]) ? "" : dr["BankCity"].ToString();
                            mClientBankDetailsModel.BankCountry = Convert.IsDBNull(dr["BankCountry"]) ? "" : dr["BankCountry"].ToString();

                            mClientBankDetailsModel.BankState = Convert.IsDBNull(dr["BankState"]) ? "" : dr["BankState"].ToString();
                            mClientBankDetailsModel.BranchName = Convert.IsDBNull(dr["BranchName"]) ? "" : dr["BranchName"].ToString();
                            mClientBankDetailsModel.BankPincode = Convert.IsDBNull(dr["BankPincode"]) ? "" : dr["BankPincode"].ToString();
                            mClientBankDetailsModel.Address = Convert.IsDBNull(dr["Address"]) ? "" : dr["Address"].ToString();
                        }

                        mModel.ClientBankDetailsModel = mClientBankDetailsModel;


                        #endregion
                        //====================================== Next Client Depository details ============================================= 


                        #region Depository Details

                        ClientDepositoryDetailsModel mClientDepositoryDetailsModel = new ClientDepositoryDetailsModel();

                        foreach (DataRow dr in ds.Tables[6].Rows)
                        {

                            mClientDepositoryDetailsModel.Depository = Convert.IsDBNull(dr["Depository"]) ? "" : dr["Depository"].ToString();
                            mClientDepositoryDetailsModel.DPID = Convert.IsDBNull(dr["DPID"]) ? "" : dr["DPID"].ToString();
                            mClientDepositoryDetailsModel.BOID = Convert.IsDBNull(dr["BOID"]) ? "" : dr["BOID"].ToString();
                            mClientDepositoryDetailsModel.DepositoryName = Convert.IsDBNull(dr["DepositoryName"]) ? "" : dr["DepositoryName"].ToString();
                            mClientDepositoryDetailsModel.CDSLAccountOpeningDate = Convert.IsDBNull(dr["CDSLAccountOpeningDate"]) ? "" : dr["CDSLAccountOpeningDate"].ToString();
                            mClientDepositoryDetailsModel.TariffPlan = Convert.IsDBNull(dr["TariffPlan"]) ? "" : dr["TariffPlan"].ToString();
                            mClientDepositoryDetailsModel.PLAN_CODE = Convert.IsDBNull(dr["PLAN_CODE"]) ? "" : dr["PLAN_CODE"].ToString();
                            mClientDepositoryDetailsModel.DeliveryPrice = Convert.IsDBNull(dr["DeliveryPrice"]) ? "" : dr["DeliveryPrice"].ToString();
                            mClientDepositoryDetailsModel.IntradayAndFuture = Convert.IsDBNull(dr["IntradayAndFuture"]) ? "" : dr["IntradayAndFuture"].ToString();
                            mClientDepositoryDetailsModel.OptionPrice = Convert.IsDBNull(dr["OptionPrice"]) ? "" : dr["OptionPrice"].ToString();


                        }

                        mModel.ClientDepositoryDetailsModel = mClientDepositoryDetailsModel;

                        #endregion

                        //====================================== Next Client fatca details ============================================= 

                        #region Fatca Details

                        ClientFatcaDetailsModel mClientFatcaDetailsModel = new ClientFatcaDetailsModel();

                        foreach (DataRow dr in ds.Tables[7].Rows)
                        {
                            mClientFatcaDetailsModel.InvestExperience = Convert.IsDBNull(dr["InvestExperience"]) ? "" : dr["InvestExperience"].ToString();
                            mClientFatcaDetailsModel.AnnualIncome = Convert.IsDBNull(dr["AnnualIncome"]) ? "" : dr["AnnualIncome"].ToString();
                            mClientFatcaDetailsModel.Networth = Convert.IsDBNull(dr["Networth"]) ? "" : dr["Networth"].ToString();
                            mClientFatcaDetailsModel.AnnualIncomeDate = Convert.IsDBNull(dr["AnnualIncomeDate"]) ? "" : dr["AnnualIncomeDate"].ToString();
                            mClientFatcaDetailsModel.SourceOfWealth = Convert.IsDBNull(dr["SourceOfWealth"]) ? "" : dr["SourceOfWealth"].ToString();
                            mClientFatcaDetailsModel.PoliticallyExposePersonText = Convert.IsDBNull(dr["PoliticallyExposePersonText"]) ? "" : dr["PoliticallyExposePersonText"].ToString();
                            mClientFatcaDetailsModel.CountryName = Convert.IsDBNull(dr["CountryName"]) ? "" : dr["CountryName"].ToString();
                            mClientFatcaDetailsModel.ISYourCountryTAXResidencyOtherThenIndiaText = Convert.IsDBNull(dr["ISYourCountryTAXResidencyOtherThenIndiaText"]) ? "" : dr["ISYourCountryTAXResidencyOtherThenIndiaText"].ToString();
                            mClientFatcaDetailsModel.OptionPrice = Convert.IsDBNull(dr["OptionPrice"]) ? "" : dr["OptionPrice"].ToString();
                            mClientFatcaDetailsModel.DeliveryPrice = Convert.IsDBNull(dr["DeliveryPrice"]) ? "" : dr["DeliveryPrice"].ToString();
                            mClientFatcaDetailsModel.IntradayPrice = Convert.IsDBNull(dr["IntradayPrice"]) ? "" : dr["IntradayPrice"].ToString();
                            mClientFatcaDetailsModel.SettelmentStatus = Convert.IsDBNull(dr["settleStatus"]) ? "" : dr["settleStatus"].ToString();
                        }

                        mModel.ClientFatcaDetailsModel = mClientFatcaDetailsModel;

                        #endregion

                        ///====================================== Next Client first nominee details =============================================  

                        #region First Nominee Details

                        ClientNomineeDetailsModel mClientNomineeDetailsModel = new ClientNomineeDetailsModel();

                        foreach (DataRow dr in ds.Tables[8].Rows)
                        {
                            mClientNomineeDetailsModel.NomineeTypeText = Convert.IsDBNull(dr["NomineeTypeText"]) ? "" : dr["NomineeTypeText"].ToString();
                            mClientNomineeDetailsModel.Title = Convert.IsDBNull(dr["Title"]) ? "" : dr["Title"].ToString();
                            mClientNomineeDetailsModel.NomineeFirstName = Convert.IsDBNull(dr["NomineeFirstName"]) ? "" : dr["NomineeFirstName"].ToString();
                            mClientNomineeDetailsModel.NomineeMiddleName = Convert.IsDBNull(dr["NomineeMiddleName"]) ? "" : dr["NomineeMiddleName"].ToString();
                            mClientNomineeDetailsModel.NomineeLastName = Convert.IsDBNull(dr["NomineeLastName"]) ? "" : dr["NomineeLastName"].ToString();
                            //mClientNomineeDetailsModel.DOBNominee = dr["DOBNominee"].ToString();
                            mClientNomineeDetailsModel.DOBNominee = Convert.IsDBNull(dr["DOBNominee"]) ? DateTime.Now : Convert.ToDateTime(dr["DOBNominee"]);
                            mClientNomineeDetailsModel.PanNumber = Convert.IsDBNull(dr["PanNumber"]) ? "" : dr["PanNumber"].ToString();
                            mClientNomineeDetailsModel.EmailId = Convert.IsDBNull(dr["EmailId"]) ? "" : dr["EmailId"].ToString();
                            mClientNomineeDetailsModel.MobileNo = Convert.IsDBNull(dr["MobileNo"]) ? "" : dr["MobileNo"].ToString();
                            mClientNomineeDetailsModel.ProffType = Convert.IsDBNull(dr["ProffType"]) ? "" : dr["ProffType"].ToString();
                            mClientNomineeDetailsModel.ProffTypeText = Convert.IsDBNull(dr["ProffTypeText"]) ? "" : dr["ProffTypeText"].ToString();
                            mClientNomineeDetailsModel.RelationshipTypeText = Convert.IsDBNull(dr["RelationshipTypeText"]) ? "" : dr["RelationshipTypeText"].ToString();
                            mClientNomineeDetailsModel.FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString();
                            mClientNomineeDetailsModel.FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString();
                            mClientNomineeDetailsModel.NomineeAddress1 = Convert.IsDBNull(dr["NomineeAddress1"]) ? "" : dr["NomineeAddress1"].ToString();
                            mClientNomineeDetailsModel.NomineeAddress2 = Convert.IsDBNull(dr["NomineeAddress2"]) ? "" : dr["NomineeAddress2"].ToString();
                            mClientNomineeDetailsModel.NomineeAddress3 = Convert.IsDBNull(dr["NomineeAddress3"]) ? "" : dr["NomineeAddress3"].ToString();
                            mClientNomineeDetailsModel.NomineePincode = Convert.IsDBNull(dr["NomineePincode"]) ? "" : dr["NomineePincode"].ToString();
                            mClientNomineeDetailsModel.NomineeCity = Convert.IsDBNull(dr["NomineeCity"]) ? "" : dr["NomineeCity"].ToString();
                            mClientNomineeDetailsModel.NomineeState = Convert.IsDBNull(dr["NomineeState"]) ? "" : dr["NomineeState"].ToString();
                            mClientNomineeDetailsModel.NomineeCountry = Convert.IsDBNull(dr["NomineeCountry"]) ? "" : dr["NomineeCountry"].ToString();
                            mClientNomineeDetailsModel.IsResidualSecurities = Convert.IsDBNull(dr["IsResidualSecurities"]) ? false : Convert.ToBoolean(dr["IsResidualSecurities"]);
                            mClientNomineeDetailsModel.IsResidualSecuritiesText = Convert.IsDBNull(dr["IsResidualSecuritiesText"]) ? "" : dr["IsResidualSecuritiesText"].ToString();
                            mClientNomineeDetailsModel.NomineePercentage = Convert.IsDBNull(dr["NomineePercentage"]) ? "" : dr["NomineePercentage"].ToString();
                        }

                        mModel.ClientNomineeDetailsModel = mClientNomineeDetailsModel;

                        #endregion

                        /// ====================================== Next Client second nominee details =============================================

                        #region Second nominee Details

                        ClientSecondNomineeDetailsModel secondNominee = new ClientSecondNomineeDetailsModel();

                        foreach (DataRow dr in ds.Tables[9].Rows)
                        {
                            secondNominee.NomineeTypeText = Convert.IsDBNull(dr["NomineeTypeText"]) ? "" : dr["NomineeTypeText"].ToString();
                            secondNominee.Title = Convert.IsDBNull(dr["Title"]) ? "" : dr["Title"].ToString();
                            secondNominee.NomineeFirstName = Convert.IsDBNull(dr["NomineeFirstName"]) ? "" : dr["NomineeFirstName"].ToString();
                            secondNominee.NomineeMiddleName = Convert.IsDBNull(dr["NomineeMiddleName"]) ? "" : dr["NomineeMiddleName"].ToString();
                            secondNominee.NomineeLastName = Convert.IsDBNull(dr["NomineeLastName"]) ? "" : dr["NomineeLastName"].ToString();
                            //secondNominee.DOBNominee = dr["DOBNominee"].ToString();
                            secondNominee.DOBNominee = Convert.IsDBNull(dr["DOBNominee"]) ? DateTime.Now : Convert.ToDateTime(dr["DOBNominee"]);
                            secondNominee.PanNumber = Convert.IsDBNull(dr["PanNumber"]) ? "" : dr["PanNumber"].ToString();
                            secondNominee.EmailId = Convert.IsDBNull(dr["EmailId"]) ? "" : dr["EmailId"].ToString();
                            secondNominee.MobileNo = Convert.IsDBNull(dr["MobileNo"]) ? "" : dr["MobileNo"].ToString();
                            secondNominee.ProffType = Convert.IsDBNull(dr["ProffType"]) ? "" : dr["ProffType"].ToString();
                            secondNominee.ProffTypeText = Convert.IsDBNull(dr["ProffTypeText"]) ? "" : dr["ProffTypeText"].ToString();
                            secondNominee.RelationshipTypeText = Convert.IsDBNull(dr["RelationshipTypeText"]) ? "" : dr["RelationshipTypeText"].ToString();
                            secondNominee.FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString();
                            secondNominee.FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString();
                            secondNominee.NomineeAddress1 = Convert.IsDBNull(dr["NomineeAddress1"]) ? "" : dr["NomineeAddress1"].ToString();
                            secondNominee.NomineeAddress2 = Convert.IsDBNull(dr["NomineeAddress2"]) ? "" : dr["NomineeAddress2"].ToString();
                            secondNominee.NomineeAddress3 = Convert.IsDBNull(dr["NomineeAddress3"]) ? "" : dr["NomineeAddress3"].ToString();
                            secondNominee.NomineePincode = Convert.IsDBNull(dr["NomineePincode"]) ? "" : dr["NomineePincode"].ToString();
                            secondNominee.NomineeCity = Convert.IsDBNull(dr["NomineeCity"]) ? "" : dr["NomineeCity"].ToString();
                            secondNominee.NomineeState = Convert.IsDBNull(dr["NomineeState"]) ? "" : dr["NomineeState"].ToString();
                            secondNominee.NomineeCountry = Convert.IsDBNull(dr["NomineeCountry"]) ? "" : dr["NomineeCountry"].ToString();
                            secondNominee.IsResidualSecurities = Convert.IsDBNull(dr["IsResidualSecurities"]) ? false : Convert.ToBoolean(dr["IsResidualSecurities"]);
                            secondNominee.IsResidualSecuritiesText = Convert.IsDBNull(dr["IsResidualSecuritiesText"]) ? "" : dr["IsResidualSecuritiesText"].ToString();
                            secondNominee.NomineePercentage = Convert.IsDBNull(dr["NomineePercentage"]) ? "" : dr["NomineePercentage"].ToString();
                        }

                        mModel.ClientSecondNomineeDetailsModel = secondNominee;

                        #endregion

                        /// ====================================== Next Client third nominee details =============================================

                        #region Third Nominee Details

                        ClientThirdNomineeDetailsModel thirdNominee = new ClientThirdNomineeDetailsModel();

                        foreach (DataRow dr in ds.Tables[10].Rows)
                        {
                            thirdNominee.NomineeTypeText = Convert.IsDBNull(dr["NomineeTypeText"]) ? "" : dr["NomineeTypeText"].ToString();
                            thirdNominee.Title = Convert.IsDBNull(dr["Title"]) ? "" : dr["Title"].ToString();
                            thirdNominee.NomineeFirstName = Convert.IsDBNull(dr["NomineeFirstName"]) ? "" : dr["NomineeFirstName"].ToString();
                            thirdNominee.NomineeMiddleName = Convert.IsDBNull(dr["NomineeMiddleName"]) ? "" : dr["NomineeMiddleName"].ToString();
                            thirdNominee.NomineeLastName = Convert.IsDBNull(dr["NomineeLastName"]) ? "" : dr["NomineeLastName"].ToString();
                            //thirdNominee.DOBNominee = dr["DOBNominee"].ToString();
                            thirdNominee.DOBNominee = Convert.IsDBNull(dr["DOBNominee"]) ? DateTime.Now : Convert.ToDateTime(dr["DOBNominee"]);
                            thirdNominee.PanNumber = Convert.IsDBNull(dr["PanNumber"]) ? "" : dr["PanNumber"].ToString();
                            thirdNominee.EmailId = Convert.IsDBNull(dr["EmailId"]) ? "" : dr["EmailId"].ToString();
                            thirdNominee.MobileNo = Convert.IsDBNull(dr["MobileNo"]) ? "" : dr["MobileNo"].ToString();
                            thirdNominee.ProffType = Convert.IsDBNull(dr["ProffType"]) ? "" : dr["ProffType"].ToString();
                            thirdNominee.ProffTypeText = Convert.IsDBNull(dr["ProffTypeText"]) ? "" : dr["ProffTypeText"].ToString();
                            thirdNominee.RelationshipTypeText = Convert.IsDBNull(dr["RelationshipTypeText"]) ? "" : dr["RelationshipTypeText"].ToString();
                            thirdNominee.FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString();
                            thirdNominee.FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString();
                            thirdNominee.NomineeAddress1 = Convert.IsDBNull(dr["NomineeAddress1"]) ? "" : dr["NomineeAddress1"].ToString();
                            thirdNominee.NomineeAddress2 = Convert.IsDBNull(dr["NomineeAddress2"]) ? "" : dr["NomineeAddress2"].ToString();
                            thirdNominee.NomineeAddress3 = Convert.IsDBNull(dr["NomineeAddress3"]) ? "" : dr["NomineeAddress3"].ToString();
                            thirdNominee.NomineePincode = Convert.IsDBNull(dr["NomineePincode"]) ? "" : dr["NomineePincode"].ToString();
                            thirdNominee.NomineeCity = Convert.IsDBNull(dr["NomineeCity"]) ? "" : dr["NomineeCity"].ToString();
                            thirdNominee.NomineeState = Convert.IsDBNull(dr["NomineeState"]) ? "" : dr["NomineeState"].ToString();
                            thirdNominee.NomineeCountry = Convert.IsDBNull(dr["NomineeCountry"]) ? "" : dr["NomineeCountry"].ToString();
                            thirdNominee.IsResidualSecurities = Convert.IsDBNull(dr["IsResidualSecurities"]) ? false : Convert.ToBoolean(dr["IsResidualSecurities"]);
                            thirdNominee.IsResidualSecuritiesText = Convert.IsDBNull(dr["IsResidualSecuritiesText"]) ? "" : dr["IsResidualSecuritiesText"].ToString();
                            thirdNominee.NomineePercentage = Convert.IsDBNull(dr["NomineePercentage"]) ? "" : dr["NomineePercentage"].ToString();
                        }

                        mModel.ClientThirdNomineeDetailsModel = thirdNominee;

                        #endregion

                        ////====================================== Next Client nominee guardian details ============================================= 

                        #region Nominee Guardian Details

                        List<ClientsNomineeGuanrdianDetailsModel> mClientsNomineeGuanrdianDetailsModel = new List<ClientsNomineeGuanrdianDetailsModel>();

                        DataTable dataTable = ds.Tables[11];

                        foreach (DataRow dr in dataTable.Rows)
                        {
                            mClientsNomineeGuanrdianDetailsModel.Add(new ClientsNomineeGuanrdianDetailsModel
                            {
                                NomineeType = dr["NomineeType"].ToString(),
                                NomineeTypeText = dr["NomineeTypeText"].ToString(),
                                GuardianFirstName = dr["GuardianFirstName"].ToString(),
                                GuardianMiddleName = dr["GuardianMiddleName"].ToString(),
                                GuardianLastName = dr["GuardianLastName"].ToString(),
                                Address1 = dr["Address1"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                Address3 = dr["Address3"].ToString(),
                                FileName = dr["FileName"].ToString(),
                                FilePath = dr["FilePath"].ToString(),
                                Pincode = dr["Pincode"].ToString(),
                                CityName = dr["CityName"].ToString(),
                                StateName = dr["StateName"].ToString(),
                                CountryCode = dr["CountryCode"].ToString(),
                                GuardianRelationshipType = dr["GuardianRelationshipType"].ToString(),
                                GuardianRelationshipTypeText = dr["GuardianRelationshipTypeText"].ToString(),
                                ProffType = dr["ProffType"].ToString(),
                                ProffTypeText = dr["ProffTypeText"].ToString(),
                            });
                        }

                        mModel.ClientsNomineeGuanrdianDetailsModel = mClientsNomineeGuanrdianDetailsModel;

                        #endregion

                        ///====================================== Next Client derivative details =============================================  

                        #region Client Derivative Model


                        ClientDerivativesModel clientDerivativesModel = new ClientDerivativesModel();

                        foreach (DataRow dr in ds.Tables[12].Rows)
                        {
                            clientDerivativesModel.ProofTypeText = Convert.IsDBNull(dr["ProofTypeText"]) ? "" : dr["ProofTypeText"].ToString();
                        }


                        mModel.ClientDerivativesModel = clientDerivativesModel;

                        #endregion



                        ///====================================== Next Client derivative segment details =============================================  


                        List<DerivativeSegmentModel> clienSegment = new List<DerivativeSegmentModel>();

                        if (mModel.DerivativeSegmentModel == null)
                        {
                            DataTable dtsegment = ds.Tables[13];

                            foreach (DataRow dr in dtsegment.Rows)
                            {

                                clienSegment.Add(new DerivativeSegmentModel()
                                {
                                    ClientSegmentId = Convert.IsDBNull(dr["ClientSegmentId"]) ? 0 : Convert.ToInt32(dr["ClientSegmentId"]),
                                    IsActive = Convert.IsDBNull(dr["IsActive"]) ? 0 : Convert.ToInt32(dr["RegistrationId"]),
                                    RegistrationId = Convert.IsDBNull(dr["RegistrationId"]) ? 0 : Convert.ToInt32(dr["RegistrationId"]),
                                    SegmentMasterId = Convert.IsDBNull(dr["SegmentMasterId"]) ? 0 : Convert.ToInt32(dr["SegmentMasterId"])
                                });
                            }
                        }

                        mModel.DerivativeSegmentModel = clienSegment;



                        //====================================== Next Client penny drop details =============================================


                        #region Pennydrop Data


                        PennyDropDetailsModel pennyDropDetailsModel = new PennyDropDetailsModel();

                        foreach (DataRow dr in ds.Tables[14].Rows)
                        {
                            pennyDropDetailsModel.Checkfuzzymatchscore = Convert.IsDBNull(dr["fuzzy_match_score"]) ? 0 : Convert.ToInt32(dr["fuzzy_match_score"]);
                            pennyDropDetailsModel.beneficiary_name_with_bank = Convert.IsDBNull(dr["beneficiary_name_with_bank"]) ? "" : dr["beneficiary_name_with_bank"].ToString();
                        }


                        mModel.PennyDropDetailsModel = pennyDropDetailsModel;


                        #endregion

                        //====================================== Next Client upload docs without passport photo details =============================================  


                        #region Upload Documents details


                        //List<UploadDocDetailsModel> uploadDoc = new List<UploadDocDetailsModel>();

                        //DataTable table = ds.Tables[15];

                        //foreach (DataRow dr in table.Rows)
                        //{
                        //    uploadDoc.Add(new UploadDocDetailsModel
                        //    {
                        //        ClientUploadDataId = Convert.IsDBNull(dr["ClientUploadDataId"]) ? 0 : Convert.ToInt32(dr["ClientUploadDataId"]),
                        //        UploadDocNo = Convert.IsDBNull(dr["UploadDocNo"]) ? "" : dr["UploadDocNo"].ToString(),
                        //        RegistrationId = Convert.IsDBNull(dr["RegistrationId"]) ? 0 : Convert.ToInt32(dr["RegistrationId"]),
                        //        UploadDocumentId = Convert.IsDBNull(dr["UploadDocumentId"]) ? 0 : Convert.ToInt32(dr["UploadDocumentId"]),
                        //        DocName = Convert.IsDBNull(dr["DocName"]) ? "" : dr["DocName"].ToString(),
                        //        FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString(),
                        //        FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString(),
                        //        OrderByField = Convert.IsDBNull(dr["OrderByField"]) ? 0 : Convert.ToInt32(dr["OrderByField"])
                        //    });


                        //}


                        //mModel.UploadDocsDetailsModel = uploadDoc;

                        #endregion

                        ///====================================== Next Client nominee upload docs details =============================================  

                        #region Nominee Documents Uploaded Data

                        List<NomineeUploadDocs> uploadDocs = new List<NomineeUploadDocs>();

                        DataTable dt = ds.Tables[16];

                        foreach (DataRow dr in dt.Rows)
                        {
                            uploadDocs.Add(new NomineeUploadDocs
                            {
                                ClientNomineeId = Convert.IsDBNull(dr["ClientNomineeId"]) ? 0 : Convert.ToInt32(dr["ClientNomineeId"]),
                                FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString(),
                                FileName2 = Convert.IsDBNull(dr["FileName2"]) ? "" : dr["FileName2"].ToString(),
                                FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString(),
                                FilePath2 = Convert.IsDBNull(dr["FilePath2"]) ? "" : dr["FilePath2"].ToString(),
                                NomineeType = Convert.IsDBNull(dr["NomineeType"]) ? 0 : Convert.ToInt32(dr["NomineeType"]),
                                NomineeTypeText = Convert.IsDBNull(dr["NomineeTypeText"]) ? "" : dr["NomineeTypeText"].ToString(),
                                ProffType = Convert.IsDBNull(dr["ProffType"]) ? 0 : Convert.ToInt32(dr["ProffType"]),
                                ProffTypeText = Convert.IsDBNull(dr["ProffTypeText"]) ? "" : dr["ProffTypeText"].ToString()
                            });


                        }
                        mModel.NomineeUploadDocs = uploadDocs;

                        #endregion

                        // ====================================== Next Client upload photo details =============================================

                        #region Uploaded Photos data

                        UploadPhotoDocs uploadPhotoDocs = new UploadPhotoDocs();


                        foreach (DataRow dr in ds.Tables[17].Rows)
                        {
                            uploadPhotoDocs.FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString();
                            uploadPhotoDocs.FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString();
                            uploadPhotoDocs.UploadDocumentId = Convert.IsDBNull(dr["UploadDocumentId"]) ? 0 : Convert.ToInt32(dr["UploadDocumentId"]);
                            uploadPhotoDocs.ClientUploadDataId = Convert.IsDBNull(dr["ClientUploadDataId"]) ? 0 : Convert.ToInt32(dr["ClientUploadDataId"]);
                            uploadPhotoDocs.RegistrationId = Convert.IsDBNull(dr["RegistrationId"]) ? 0 : Convert.ToInt32(dr["RegistrationId"]);
                            uploadPhotoDocs.UploadDocNo = Convert.IsDBNull(dr["UploadDocNo"]) ? "" : dr["UploadDocNo"].ToString();


                        }

                        mModel.uploadPhotoDocs = uploadPhotoDocs;


                        #endregion

                        /// ====================================== Next Client upload sighn docs details =============================================

                        #region Uploaded Signature data

                        UploadSignDocs uploadSignDocs = new UploadSignDocs();


                        foreach (DataRow dr in ds.Tables[18].Rows)
                        {
                            uploadSignDocs.FileName = Convert.IsDBNull(dr["FileName"]) ? "" : dr["FileName"].ToString();
                            uploadSignDocs.FilePath = Convert.IsDBNull(dr["FilePath"]) ? "" : dr["FilePath"].ToString();
                            uploadSignDocs.UploadDocumentId = Convert.IsDBNull(dr["UploadDocumentId"]) ? 0 : Convert.ToInt32(dr["UploadDocumentId"]);
                            uploadSignDocs.ClientUploadDataId = Convert.IsDBNull(dr["ClientUploadDataId"]) ? 0 : Convert.ToInt32(dr["ClientUploadDataId"]);
                            uploadSignDocs.RegistrationId = Convert.IsDBNull(dr["RegistrationId"]) ? 0 : Convert.ToInt32(dr["RegistrationId"]);
                            uploadSignDocs.UploadDocNo = Convert.IsDBNull(dr["UploadDocNo"]) ? "" : dr["UploadDocNo"].ToString();


                        }

                        mModel.uploadSignDocs = uploadSignDocs;

                        #endregion

                        ///---====================================== Next Client Geo Tag details =============================================  
                        ///

                        #region Clients GEO TAG data

                        UploadGEOTag GEOTag = new UploadGEOTag();


                        foreach (DataRow dr in ds.Tables[19].Rows)
                        {
                            GEOTag.Longitude = Convert.IsDBNull(dr["Longitude"]) ? "" : dr["Longitude"].ToString();
                            GEOTag.Latitude = Convert.IsDBNull(dr["Latitude"]) ? "" : dr["Latitude"].ToString();
                            GEOTag.RegistrationId = Convert.IsDBNull(dr["RegistrationId"]) ? 0 : Convert.ToInt32(dr["RegistrationId"]);
                            GEOTag.CountryCode = Convert.IsDBNull(dr["CountryCode"]) ? "" : dr["CountryCode"].ToString();
                            GEOTag.City = Convert.IsDBNull(dr["City"]) ? "" : dr["City"].ToString();
                            GEOTag.ZipCode = Convert.IsDBNull(dr["ZipCode"]) ? "" : dr["ZipCode"].ToString();
                            GEOTag.ContinentName = Convert.IsDBNull(dr["ContinentName"]) ? "" : dr["ContinentName"].ToString();
                            GEOTag.EntryDate = Convert.IsDBNull(dr["EntryDate"]) ? DateTime.Now : Convert.ToDateTime(dr["EntryDate"]);
                            GEOTag.GeoTagId = Convert.IsDBNull(dr["GeoTagId"]) ? 0 : Convert.ToInt32(dr["GeoTagId"]);
                            GEOTag.IPAddress = Convert.IsDBNull(dr["IPAddress"]) ? "" : dr["IPAddress"].ToString();
                            GEOTag.IPType = Convert.IsDBNull(dr["IPType"]) ? "" : dr["IPType"].ToString();
                            GEOTag.RegionCode = Convert.IsDBNull(dr["RegionCode"]) ? "" : dr["RegionCode"].ToString();
                            GEOTag.RegionName = Convert.IsDBNull(dr["RegionName"]) ? "" : dr["RegionName"].ToString();


                        }

                        mModel.uploadGEOTag = GEOTag;

                        #endregion


                        ///---====================================== Next Civil KRA Fetch Model details =============================================  

                        #region

                        CivilKRAFetchModel CivilKRA = new CivilKRAFetchModel();
                        foreach (DataRow dr in ds.Tables[20].Rows)
                        {
                            CivilKRA.AppDOB = Convert.IsDBNull(dr["AppDOB"]) ? "" : dr["AppDOB"].ToString();
                            CivilKRA.AppGen = Convert.IsDBNull(dr["AppGen"]) ? "" : dr["AppGen"].ToString();
                            CivilKRA.AppName = Convert.IsDBNull(dr["AppNAme"]) ? "" : dr["AppNAme"].ToString();
                            CivilKRA.AppPerAdd1 = Convert.IsDBNull(dr["AppPerAdd1"]) ? "" : dr["AppPerAdd1"].ToString();
                            CivilKRA.AppPerAdd2 = Convert.IsDBNull(dr["AppPerAdd2"]) ? "" : dr["AppPerAdd2"].ToString();
                            CivilKRA.AppPerAdd3 = Convert.IsDBNull(dr["AppPerAdd3"]) ? "" : dr["AppPerAdd3"].ToString();
                            CivilKRA.RefDate = Convert.IsDBNull(dr["RefDatae"]) ? "" : dr["RefDatae"].ToString();

                        }

                        mModel.CivilKRAFetchModel = CivilKRA;

                        #endregion
                    }



                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PDF_GetDataFromDatabase " + ex.Message);
                _logger.LogError("PDF_GetDataFromDatabase " + ex.StackTrace);
            }
            return mModel;
        }

        public async Task<bool> ClientPhoto(MainModel model, int PageAddedCount, PdfReader pdfReader, PdfStamper stamper, string PDFType)
        {
            bool UploadStatus = false;
            try
            {
                DataTable DtClientPhoto = await ConvertListToDataTable(model.uploadPhotoDocs);
                if (DtClientPhoto.Rows.Count > 0)
                {
                    if (DtClientPhoto.Rows[0]["FileName"].ToString() != "" && DtClientPhoto.Rows[0]["FileName"].ToString() != null)
                    {
                        string StrFilePath = DtClientPhoto.Rows[0]["FilePath"].ToString();
                        string rootPath = "D:\\ekycUploadDocuments\\" + model.PrimaryDetailsModel.InwardNo + "\\";
                        string fileName = DtClientPhoto.Rows[0]["FileName"].ToString();

                        using (System.Net.WebClient client = new System.Net.WebClient())
                        {
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            client.DownloadFile(new Uri(StrFilePath), fileName);
                        }



                        var pdfContentByteImage = stamper.GetOverContent(PageAddedCount + NormalPDFPageModel.PageCkyc_KraForm);
                        var ClientPassPhoto = Image.GetInstance(fileName);
                        ClientPassPhoto.SetAbsolutePosition(511, 588);
                        ClientPassPhoto.ScaleAbsolute(63, 78);
                        pdfContentByteImage.Rectangle(5, 5, 5, 5);
                        pdfContentByteImage.AddImage(ClientPassPhoto);
                        UploadStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ClientPhoto" + ex.ToString());
                _logger.LogError("ClientPhoto" + ex.StackTrace);
                UploadStatus = false;
            }
            return UploadStatus;
        }

        public async Task<bool> ClientSignature(MainModel model, DataTable DtClientSign, int PageNo, PdfStamper stamper, string PDFType)
        {
            bool UploadStatus = false;
            try
            {
                if (DtClientSign.Rows.Count > 0)
                {
                    if (PDFType == "MF")
                    {
                        Image signimg4 = Image.GetInstance(DtClientSign.Rows[0]["FilePath"].ToString());
                        var pdfContentByteImageEsigb19 = stamper.GetOverContent(PageNo);
                        signimg4.SetAbsolutePosition(55, 535);
                        signimg4.ScaleAbsoluteHeight(50);
                        signimg4.ScaleAbsoluteWidth(100);
                        pdfContentByteImageEsigb19.AddImage(signimg4);

                        UploadStatus = true;
                    }

                    if (PDFType == "Normal")
                    {
                        Image signimg4 = Image.GetInstance(DtClientSign.Rows[0]["FilePath"].ToString());

                        var pdfpg4 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCkyc_KraForm);
                        signimg4.SetAbsolutePosition(520, 550);
                        signimg4.ScaleAbsoluteHeight(25);
                        signimg4.ScaleAbsoluteWidth(50);
                        pdfpg4.AddImage(signimg4);

                        var pdfpg5 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_contact);
                        signimg4.SetAbsolutePosition(430, 310);
                        signimg4.SetAbsolutePosition(430, 310);
                        signimg4.ScaleAbsoluteHeight(0);
                        signimg4.ScaleAbsoluteWidth(0);
                        pdfpg5.AddImage(signimg4);

                        //Segment Sign
                        var pdfpg6 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_aaditionaldetails);
                        signimg4.SetAbsolutePosition(200, 538);
                        signimg4.ScaleAbsoluteHeight(35);
                        signimg4.ScaleAbsoluteWidth(45);
                        pdfpg6.AddImage(signimg4);

                        var pdfpg09 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_optionformdeclaratuion);
                        signimg4.SetAbsolutePosition(80, 70);
                        signimg4.ScaleAbsoluteHeight(35);
                        signimg4.ScaleAbsoluteWidth(45);
                        pdfpg09.AddImage(signimg4);

                        //derivatives selection
                        if (model.DerivativeSegmentModel != null)
                        {
                            foreach (var item in model.DerivativeSegmentModel)
                            {
                                if (item.SegmentMasterId == 5)
                                {
                                    var pdfpg7 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_aaditionaldetails);
                                    signimg4.SetAbsolutePosition(200, 500);
                                    signimg4.ScaleAbsoluteHeight(30);
                                    signimg4.ScaleAbsoluteWidth(45);
                                    pdfpg7.AddImage(signimg4);
                                }
                                else if (item.SegmentMasterId == 3)
                                {
                                    var pdfpg9 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_aaditionaldetails);
                                    signimg4.SetAbsolutePosition(200, 465);
                                    signimg4.ScaleAbsoluteHeight(30);
                                    signimg4.ScaleAbsoluteWidth(45);
                                    pdfpg9.AddImage(signimg4);
                                }
                                else if (item.SegmentMasterId == 5 && item.SegmentMasterId == 3)
                                {
                                    var pdfpg7 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_aaditionaldetails);
                                    signimg4.SetAbsolutePosition(200, 500);
                                    signimg4.ScaleAbsoluteHeight(30);
                                    signimg4.ScaleAbsoluteWidth(45);
                                    pdfpg7.AddImage(signimg4);

                                    var pdfpg9 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_aaditionaldetails);
                                    signimg4.SetAbsolutePosition(200, 465);
                                    signimg4.ScaleAbsoluteHeight(30);
                                    signimg4.ScaleAbsoluteWidth(45);
                                    pdfpg9.AddImage(signimg4);
                                }
                                else if (item.SegmentMasterId == 7)
                                {
                                    var pdfpg9 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_aaditionaldetails);
                                    signimg4.SetAbsolutePosition(200, 430);
                                    signimg4.ScaleAbsoluteHeight(30);
                                    signimg4.ScaleAbsoluteWidth(45);
                                    pdfpg9.AddImage(signimg4);
                                }
                            }
                        }


                        var pdfpg8 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_runningacc);
                        signimg4.SetAbsolutePosition(80, 65);
                        signimg4.ScaleAbsoluteHeight(40);
                        signimg4.ScaleAbsoluteWidth(80);
                        pdfpg8.AddImage(signimg4);

                        var pdfpg11forBoi1 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_optionform);
                        signimg4.SetAbsolutePosition(80, 50);
                        signimg4.ScaleAbsoluteHeight(40);
                        signimg4.ScaleAbsoluteWidth(80);
                        pdfpg11forBoi1.AddImage(signimg4);

                        //Page 20
                        var pdfContentByteImageEsigb19 = stamper.GetOverContent(PageNo + NormalPDFPageModel.LastPageMF);
                        signimg4.SetAbsolutePosition(55, 535);
                        signimg4.ScaleAbsoluteHeight(50);
                        signimg4.ScaleAbsoluteWidth(100);
                        pdfContentByteImageEsigb19.AddImage(signimg4);

                        UploadStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PDF_ClientSignature " + ex.Message);
                _logger.LogError("PDF_ClientSignature " + ex.StackTrace);
                return UploadStatus = false;
            }
            return UploadStatus;
        }

        public async Task<bool> AuthEmpSign(int PageNo, PdfStamper stamper, string PDFType)
        {
            bool AuthEmpSignStatus = false;

            try
            {
                string signatureofauthority1 = _appsetting.AuthorizedSameer;
                string signatureofauthority = _appsetting.AuthorizedEmpSignPath;

                if (PDFType == "Normal")
                {
                    var pdfpg5 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_contact);
                    Image signofauthorityipv = Image.GetInstance(signatureofauthority);
                    signofauthorityipv.SetAbsolutePosition(100, 15);
                    signofauthorityipv.ScaleAbsoluteHeight(27);
                    signofauthorityipv.ScaleAbsoluteWidth(60);
                    pdfpg5.AddImage(signofauthorityipv);

                    signofauthorityipv.SetAbsolutePosition(100, 112);
                    signofauthorityipv.ScaleAbsoluteHeight(27);
                    signofauthorityipv.ScaleAbsoluteWidth(60);
                    pdfpg5.AddImage(signofauthorityipv);

                    var pdfpg6 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_MFPage);
                    Image signofauthorityipv6 = Image.GetInstance(signatureofauthority);
                    signofauthorityipv6.SetAbsolutePosition(160, 15);
                    signofauthorityipv6.ScaleAbsoluteHeight(27);
                    signofauthorityipv6.ScaleAbsoluteWidth(60);
                    pdfpg6.AddImage(signofauthorityipv6);

                    signofauthorityipv6.SetAbsolutePosition(180, 580);
                    signofauthorityipv6.ScaleAbsoluteHeight(27);
                    signofauthorityipv6.ScaleAbsoluteWidth(60);
                    pdfpg6.AddImage(signofauthorityipv6);

                    var pdfpg7 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_MFPage);
                    Image signofauthorityipv7 = Image.GetInstance(signatureofauthority);
                    signofauthorityipv7.SetAbsolutePosition(320, 580);
                    signofauthorityipv7.ScaleAbsoluteHeight(27);
                    signofauthorityipv7.ScaleAbsoluteWidth(60);
                    pdfpg7.AddImage(signofauthorityipv7);

                    signofauthorityipv7.SetAbsolutePosition(450, 580);
                    signofauthorityipv7.ScaleAbsoluteHeight(27);
                    signofauthorityipv7.ScaleAbsoluteWidth(60);
                    pdfpg7.AddImage(signofauthorityipv7);

                    var pdfpg26 = stamper.GetOverContent(PageNo + NormalPDFPageModel.PageCKYC_MFPage);
                    Image signofauthorityipv26 = Image.GetInstance(signatureofauthority1);
                    signofauthorityipv26.SetAbsolutePosition(75, 485);
                    signofauthorityipv26.ScaleAbsoluteHeight(27);
                    signofauthorityipv26.ScaleAbsoluteWidth(60);
                    pdfpg26.AddImage(signofauthorityipv26);
                }
                else if (PDFType == "BOI")
                {
                    var pdfpg5 = stamper.GetOverContent(PageNo + BankOfIndiaPDFPageModel.PageCKYC_contact);
                    Image signofauthorityipv = Image.GetInstance(signatureofauthority);
                    signofauthorityipv.SetAbsolutePosition(100, 15);
                    signofauthorityipv.ScaleAbsoluteHeight(27);
                    signofauthorityipv.ScaleAbsoluteWidth(60);
                    pdfpg5.AddImage(signofauthorityipv);

                    signofauthorityipv.SetAbsolutePosition(100, 112);
                    signofauthorityipv.ScaleAbsoluteHeight(27);
                    signofauthorityipv.ScaleAbsoluteWidth(60);
                    pdfpg5.AddImage(signofauthorityipv);

                    var pdfpg6 = stamper.GetOverContent(PageNo + BankOfIndiaPDFPageModel.PageCKYC_officeuseforboi);
                    Image signofauthorityipv6 = Image.GetInstance(signatureofauthority);
                    signofauthorityipv6.SetAbsolutePosition(180, 38);
                    signofauthorityipv6.ScaleAbsoluteHeight(27);
                    signofauthorityipv6.ScaleAbsoluteWidth(60);
                    pdfpg6.AddImage(signofauthorityipv6);

                    signofauthorityipv6.SetAbsolutePosition(180, 580);
                    signofauthorityipv6.ScaleAbsoluteHeight(27);
                    signofauthorityipv6.ScaleAbsoluteWidth(60);
                    pdfpg6.AddImage(signofauthorityipv6);

                    var pdfpg7 = stamper.GetOverContent(PageNo + BankOfIndiaPDFPageModel.PageCKYC_officeuseforboi);
                    Image signofauthorityipv7 = Image.GetInstance(signatureofauthority);
                    signofauthorityipv7.SetAbsolutePosition(320, 580);
                    signofauthorityipv7.ScaleAbsoluteHeight(27);
                    signofauthorityipv7.ScaleAbsoluteWidth(60);
                    pdfpg7.AddImage(signofauthorityipv7);

                    signofauthorityipv7.SetAbsolutePosition(450, 580);
                    signofauthorityipv7.ScaleAbsoluteHeight(27);
                    signofauthorityipv7.ScaleAbsoluteWidth(60);
                    pdfpg7.AddImage(signofauthorityipv7);

                    var pdfpg26 = stamper.GetOverContent(PageNo + BankOfIndiaPDFPageModel.PageCKYC_officeuseforboi);
                    Image signofauthorityipv26 = Image.GetInstance(signatureofauthority1);
                    signofauthorityipv26.SetAbsolutePosition(75, 485);
                    signofauthorityipv26.ScaleAbsoluteHeight(27);
                    signofauthorityipv26.ScaleAbsoluteWidth(60);
                    pdfpg26.AddImage(signofauthorityipv26);
                }
                else if (PDFType == "MF")
                {
                    var pdfpg5 = stamper.GetOverContent(PageNo + BSEMFModel.bsemf);
                    Image signofauthorityipv = Image.GetInstance(signatureofauthority);
                    signofauthorityipv.SetAbsolutePosition(100, 15);
                    signofauthorityipv.ScaleAbsoluteHeight(27);
                    signofauthorityipv.ScaleAbsoluteWidth(60);
                    pdfpg5.AddImage(signofauthorityipv);

                    signofauthorityipv.SetAbsolutePosition(100, 112);
                    signofauthorityipv.ScaleAbsoluteHeight(27);
                    signofauthorityipv.ScaleAbsoluteWidth(60);
                    pdfpg5.AddImage(signofauthorityipv);
                }

                AuthEmpSignStatus = true;
            }
            catch (Exception ex)
            {
                AuthEmpSignStatus = false;

                _logger.LogError(ex.ToString());
                throw;
            }
            return AuthEmpSignStatus;


        }

        public async Task<bool> ClientNomineeEsign_BOI(PersonalDetailsModel mPersonalDetailsModel, ClientPermanentAddressModel mClientPermanentAddressModel,
                   int PageAddedCount, PdfStamper stamper, string PDFType, string EsignType)
        {
            bool ClientEsign = false;
            string EsignStamp = _appsetting.EsignStamp;
            try
            {
                if (EsignType == "Yes")
                {
                    if (PDFType == "BOI" || PDFType == "Normal")
                    {
                        ///11052023
                        var pdfpg15 = stamper.GetOverContent(PageAddedCount + NormalPDFPageModel.PageCKYC_nomineeimg);
                        pdfpg15.BeginText();
                        BaseFont bfFontI1 = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        pdfpg15.SetFontAndSize(bfFontI1, 7);
                        string ESign1stLine1 = "Digitally Signed By :";
                        pdfpg15.ShowTextAligned(0, ESign1stLine1, 70, 380, 0);
                        string ESign2ndLine1 = "Name :" + mPersonalDetailsModel.ClientFullName;
                        pdfpg15.ShowTextAligned(0, ESign2ndLine1, 70, 373, 0);
                        string ESign3rdstLine1 = "Location :" + mClientPermanentAddressModel.PerCity;
                        pdfpg15.ShowTextAligned(0, ESign3rdstLine1, 70, 367, 0);
                        string ESign4thstLine1 = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                        pdfpg15.ShowTextAligned(0, ESign4thstLine1, 70, 361, 0);
                        pdfpg15.EndText();

                        Image signEsign = Image.GetInstance(EsignStamp);
                        signEsign.SetAbsolutePosition(40, 350);
                        signEsign.ScaleAbsoluteHeight(30);
                        signEsign.ScaleAbsoluteWidth(30);
                        pdfpg15.AddImage(signEsign);

                        ClientEsign = true;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PDF Manager : Method Name :  ClientNomineeEsign Error : " + ex.Message);
                ClientEsign = false;
            }
            return ClientEsign;
        }


        public async Task<bool> ClientNomineeEsign(MainModel model, int PageAddedCount, PdfStamper stamper, string PDFType, string EsignType)
        {
            bool ClientEsign = false;
            string EsignStamp = _appsetting.EsignStamp;
            var nominee = model.ClientNomineeDetailsModel.NomineeFirstName;
            try
            {
                if (nominee == null)   // if nominee data are not present 
                {
                    if (EsignType == "Yes")
                    {
                        var pdfUploadDoc = stamper.GetOverContent(PageAddedCount - 1 + NormalPDFPageModel.PageCKYC_nomineeoptoutimg);
                        //Esign Attached
                        BaseFont bfFontI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        pdfUploadDoc.SetFontAndSize(bfFontI, 7);

                        string ESign1stLine = "Digitally Signed By :";
                        pdfUploadDoc.ShowTextAligned(0, ESign1stLine, 360, 261, 0);
                        string ESign2ndLine = "Name :" + model.PersonalDetailsModel.ClientFullName;
                        pdfUploadDoc.ShowTextAligned(0, ESign2ndLine, 360, 254, 0);
                        string ESign3rdstLine = "Location :" + model.ClientPermanentAddressModel.PerCity;
                        pdfUploadDoc.ShowTextAligned(0, ESign3rdstLine, 360, 247, 0);
                        string ESign4thstLine = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                        pdfUploadDoc.ShowTextAligned(0, ESign4thstLine, 360, 240, 0);

                        Image signEsign = Image.GetInstance(EsignStamp);
                        signEsign.SetAbsolutePosition(390, 210);
                        signEsign.ScaleAbsoluteHeight(30);
                        signEsign.ScaleAbsoluteWidth(30);
                        pdfUploadDoc.AddImage(signEsign);
                    }
                }
                else
                {
                    if (EsignType == "Yes")    // if nominee data is present
                    {

                        var pdfpg15 = stamper.GetOverContent(PageAddedCount - 1 + NormalPDFPageModel.PageCKYC_nomineeimg);
                        pdfpg15.BeginText();
                        BaseFont bfFontI1 = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        pdfpg15.SetFontAndSize(bfFontI1, 7);
                        string ESign1stLine1 = "Digitally Signed By :";
                        pdfpg15.ShowTextAligned(0, ESign1stLine1, 70, 380, 0);
                        string ESign2ndLine1 = "Name :" + model.PersonalDetailsModel.ClientFullName;
                        pdfpg15.ShowTextAligned(0, ESign2ndLine1, 70, 373, 0);
                        string ESign3rdstLine1 = "Location :" + model.ClientPermanentAddressModel.PerCity;
                        pdfpg15.ShowTextAligned(0, ESign3rdstLine1, 70, 367, 0);
                        string ESign4thstLine1 = "Date: " + System.DateTime.Now.ToString("ddd, MMM dd HH’:’mm’:’ss ‘IST’ yyyy");
                        pdfpg15.ShowTextAligned(0, ESign4thstLine1, 70, 361, 0);
                        pdfpg15.EndText();

                        Image signEsign = Image.GetInstance(EsignStamp);
                        signEsign.SetAbsolutePosition(40, 350);
                        signEsign.ScaleAbsoluteHeight(30);
                        signEsign.ScaleAbsoluteWidth(30);
                        pdfpg15.AddImage(signEsign);


                        ClientEsign = true;

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PDF Manager : Method Name :  ClientNomineeEsign Error : " + ex.Message);
                ClientEsign = false;
            }
            return ClientEsign;
        }

        #endregion
    }
}
