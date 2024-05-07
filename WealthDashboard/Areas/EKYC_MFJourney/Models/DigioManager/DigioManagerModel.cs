using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels;
using System.Text.Json;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioManager
{
    public class DigioManagerModel : IDigioManagerModel
    {
        #region Global Variable
        private readonly ILogger<DigioManagerModel> _logger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;
        #endregion
        #region Ctor
        public DigioManagerModel(IOptions<ConnectionStrings> connectionStrings,
                                 IOptions<Appsetting> options,
                                 ILogger<DigioManagerModel> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _appsetting = options.Value;
            _logger = logger;
        }
        #endregion

        #region Method

        public async Task<DigioModel> CentralizeDigioWorkTemplate(CentralizeDigioWorkTemplateRequest centralizeDigioWorkTemplateRequest)
        {
            DigioModel digioModel = new();
            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(centralizeDigioWorkTemplateRequest),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.DigioBaseUrl);
                //Sending request to find web api REST service using HttpClient  

                HttpResponseMessage response = await client
                                   .PostAsync(StaticValues.DigioUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    digioModel = JsonSerializer.Deserialize<DigioModel>(Convert.ToString(objResponse));
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
                ex.ToString();
            }
            return digioModel;
        }
        public async Task<string> CentralizeInsertUpdateDigioTemplate(int RegistrationId, DigioModel digioModel)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "USP_InsertUpdateDigioWorkTemplate";
                    sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                    sqlCmd.Parameters.AddWithValue("@Id", digioModel.data.id);
                    sqlCmd.Parameters.AddWithValue("@created_at", digioModel.data.created_at);
                    sqlCmd.Parameters.AddWithValue("@status", digioModel.data.status);
                    sqlCmd.Parameters.AddWithValue("@customer_identifier", digioModel.data.customer_identifier);
                    sqlCmd.Parameters.AddWithValue("@reference_id", digioModel.data.reference_id);
                    sqlCmd.Parameters.AddWithValue("@workflow_name", digioModel.data.workflow_name);
                    sqlCmd.Parameters.AddWithValue("@transaction_id", digioModel.data.transaction_id);
                    sqlCmd.Parameters.AddWithValue("@expire_in_days", digioModel.data.expire_in_days);
                    sqlCmd.Parameters.AddWithValue("@reminder_registered", digioModel.data.reminder_registered);
                    sqlCmd.Parameters.AddWithValue("@access_token_id", digioModel.data.access_token.id);
                    sqlCmd.Parameters.AddWithValue("@access_token_entity_Id", digioModel.data.access_token.entity_id);
                    sqlCmd.Parameters.AddWithValue("@auto_approved", digioModel.data.auto_approved);
                    sqlCmd.Parameters.AddWithValue("@EntryBy", "CentralizeData");
                    sqlCmd.ExecuteNonQuery();
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Digio details Saving Issue ");
                return "Failed";
            }
        }

        public async Task<string> CentralizeGetDigiotemplateDetails(int RegistrationId)
        {
            string strId = string.Empty;
            try
            {

                SqlConnection con = new SqlConnection(_connectionStrings.EKYCWelcomeDb);
                con.Open();
                SqlCommand cmd = new SqlCommand(StaticValues.GetDegioWorktemplate, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    strId = dr["id"].ToString();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return strId;
        }

        public async Task<CentralizeWorkflowTemplateModel> CentralizeDigioResponseData(CentralizedigioPrameter centralizedigioPrameter)
        {

            CentralizeWorkflowTemplateModel centralizeWorkflowTemplateModel = new();
            try
            {

                var requestContent = new StringContent
            (
                          JsonSerializer.Serialize(centralizedigioPrameter),
                          Encoding.UTF8,
                          StaticValues.ApplicationJsonMediaType
            );
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", _appsetting.DigioAuthorization);
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();


                client.BaseAddress = new Uri(_appsetting.CntrlzDigiSuccRespoBaseurl);
                //Sending request to find web api REST service using HttpClient  
                var centdgourl = StaticValues.CntrlzDigiSuccRespoUrl + centralizedigioPrameter.ResponseId + "&sourceType=ACMIIL-EKYC";
                HttpResponseMessage response = await client
                                   .PostAsync(centdgourl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    centralizeWorkflowTemplateModel = JsonSerializer.Deserialize<CentralizeWorkflowTemplateModel>(Convert.ToString(objResponse));
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
                ex.ToString();
            }
            return centralizeWorkflowTemplateModel;
        }
        public async Task<string> DigioApprovedPANandAAharResponseData(int RegistrationId, CentralizeWorkflowTemplateModel centralizeWorkflowTemplateModel)
        {
            try
            {
                ResponseData mResponseData = new ResponseData();
                mResponseData = centralizeWorkflowTemplateModel.data;
                ResponseAction mResponseAction = new ResponseAction();
                if (mResponseData != null)
                {
                    mResponseAction = centralizeWorkflowTemplateModel.data.actions[0];
                    using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                    {
                        sqlConn.Open();
                        SqlCommand sqlCmd = new SqlCommand();
                        sqlCmd.Connection = sqlConn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "USP_InsertDigioActionDetails";
                        sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                        sqlCmd.Parameters.AddWithValue("@id", mResponseData.id);
                        sqlCmd.Parameters.AddWithValue("@action_ref", mResponseData.reference_id);
                        sqlCmd.Parameters.AddWithValue("@type", "digilocker-1");
                        sqlCmd.Parameters.AddWithValue("@status", mResponseData.status);
                        sqlCmd.Parameters.AddWithValue("@execution_request_id", mResponseAction.execution_request_id);
                        sqlCmd.Parameters.AddWithValue("@completed_at", mResponseAction.completed_at);
                        sqlCmd.Parameters.AddWithValue("@aadhaar", true);
                        sqlCmd.Parameters.AddWithValue("@pan", true);

                        sqlCmd.Parameters.AddWithValue("@PAN_id_number", mResponseAction.details.pan.id_number);
                        sqlCmd.Parameters.AddWithValue("@PAN_document_type", mResponseAction.details.pan.document_type);
                        sqlCmd.Parameters.AddWithValue("@PAN_id_proof_type", mResponseAction.details.pan.id_proof_type);
                        sqlCmd.Parameters.AddWithValue("@PAN_gender", mResponseAction.details.pan.gender);
                        sqlCmd.Parameters.AddWithValue("@PAN_name", mResponseAction.details.pan.name);
                        sqlCmd.Parameters.AddWithValue("@PAN_dob", mResponseAction.details.pan.dob);

                        sqlCmd.Parameters.AddWithValue("@id_number", mResponseAction.details.aadhaar.id_number);
                        sqlCmd.Parameters.AddWithValue("@document_type", mResponseAction.details.aadhaar.document_type);
                        sqlCmd.Parameters.AddWithValue("@id_proof_type", mResponseAction.details.aadhaar.id_proof_type);
                        sqlCmd.Parameters.AddWithValue("@gender", mResponseAction.details.aadhaar.gender);
                        sqlCmd.Parameters.AddWithValue("@image", mResponseAction.details.aadhaar.image);
                        sqlCmd.Parameters.AddWithValue("@name", mResponseAction.details.aadhaar.name);

                        sqlCmd.Parameters.AddWithValue("@dob", mResponseAction.details.aadhaar.dob);
                        sqlCmd.Parameters.AddWithValue("@current_address", mResponseAction.details.aadhaar.current_address);
                        sqlCmd.Parameters.AddWithValue("@permanent_address", mResponseAction.details.aadhaar.permanent_address);
                        sqlCmd.Parameters.AddWithValue("@Curaddress", mResponseAction.details.aadhaar.current_address_details.address);
                        sqlCmd.Parameters.AddWithValue("@Curlocality_or_post_office", mResponseAction.details.aadhaar.current_address_details.locality_or_post_office);
                        sqlCmd.Parameters.AddWithValue("@Curdistrict_or_city", mResponseAction.details.aadhaar.current_address_details.district_or_city);
                        sqlCmd.Parameters.AddWithValue("@Curstate", mResponseAction.details.aadhaar.current_address_details.state);
                        sqlCmd.Parameters.AddWithValue("@Curpincode", mResponseAction.details.aadhaar.current_address_details.pincode);
                        sqlCmd.Parameters.AddWithValue("@peraddress", mResponseAction.details.aadhaar.permanent_address_details.address);
                        sqlCmd.Parameters.AddWithValue("@Perlocality_or_post_office", mResponseAction.details.aadhaar.permanent_address_details.locality_or_post_office);
                        sqlCmd.Parameters.AddWithValue("@Perdistrict_or_city", mResponseAction.details.aadhaar.permanent_address_details.district_or_city);
                        sqlCmd.Parameters.AddWithValue("@Perstate", mResponseAction.details.aadhaar.permanent_address_details.state);
                        sqlCmd.Parameters.AddWithValue("@Perpincode", mResponseAction.details.aadhaar.permanent_address_details.pincode);
                        sqlCmd.ExecuteNonQuery();
                        //mControllerErrorLog.Controllerwritelog("InsertUpdateDigio Worktemplate");
                        return "OK";
                    }
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Digio Worktemplate" + ex.ToString());
                _logger.LogError("Digio Worktemplate" + ex.StackTrace);
                //mControllerErrorLog.Controllerwritelog("Digio Worktemplate" + ex.ToString());
                // mControllerErrorLog.Controllerwritelog("Digio Worktemplate" + ex.StackTrace);
                return "Failed";
            }
            //return null;
        }

        public async Task<string> GetDigioFileData(int RegistrationId, string execution_request_id, string ucc)
        {
            CentralizeWorkflowTemplateModel centralizeWorkflowTemplateModel = new();
            string strMsg = string.Empty;
            try
            {

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", _appsetting.DigioAuthorization);
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();


                client.BaseAddress = new Uri(_appsetting.CntrlzDigiSuccRespoBaseurl);
                //Sending request to find web api REST service using HttpClient  
                var url = _appsetting.DigioFileDownload + execution_request_id;

                HttpResponseMessage response = await client
                .GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    var getdata = JsonSerializer.Deserialize<XMLData>(Convert.ToString(objResponse));
                    string getxmldata = getdata.data.xmlContent;
                    string xmlFilePath = _appsetting.PDFNewFilePath + "20" + ucc;
                    Directory.CreateDirectory(xmlFilePath);
                    string fullPath = Path.Combine(xmlFilePath, "DigioAadhar_XMLFile.xml");
                    System.IO.File.WriteAllText(fullPath, getxmldata);
                    strMsg = "OK";
                }
                else
                {
                    _logger.LogError(response.IsSuccessStatusCode.ToString());

                }
                return strMsg;
            }
            catch (Exception ex)
            {
                _logger.LogError("Digio" + ex.ToString());
                _logger.LogError("Digio" + ex.StackTrace);
                return ex.ToString();
            }
            // return null;
        }
        public async Task<string> InsertPersonalDetails(ClientsPersonalDetailsModel clientsPersonalDetailsModel)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(clientsPersonalDetailsModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  

                HttpResponseMessage response = await client
                                   .PostAsync(StaticValues.personalDetailurl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    message = "Ok";
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
                return ex.ToString();
            }
            return message;
        }
        public async Task<AddressCodeModel> AddressCodeDetails(string pincode)
        {
            AddressCodeModel addressCodeModel = new AddressCodeModel();
            try
            {
                //var requestContent = new StringContent
                //(
                //              JsonSerializer.Serialize(clientsPersonalDetailsModel),
                //              Encoding.UTF8,
                //              StaticValues.ApplicationJsonMediaType
                //);
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  

                var urlget = _appsetting.EKYC_apiBaseUrl + StaticValues.PinCodeMasterDetails + pincode;

                HttpResponseMessage response = await client.GetAsync(urlget);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    //message = "Ok";
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

            }
            return addressCodeModel;
        }
        public async Task<string> InsertOrUpdateClientAddressDetails(InsertOrUpdateClientAddressDetailsModel insertOrUpdateClientAddressDetailsModel)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(insertOrUpdateClientAddressDetailsModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  


                HttpResponseMessage response = await client.PostAsync(StaticValues.AddressDetailurl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    //var objResponse = await response.Content.ReadAsStringAsync();
                    //addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    message = "Ok";
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

            }
            return message;
        }
        public async Task<string> InsertOrUpdateClientRelationDetails(List<ClientRelationModel> clientRelationModel)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(clientRelationModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  


                HttpResponseMessage response = await client.PostAsync(StaticValues.ReleationDetailurl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    //var objResponse = await response.Content.ReadAsStringAsync();
                    //addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    message = "Ok";
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

            }
            return message;
        }
        public async Task<string> InsertOrUpdateInvestmentAndFatcaDetails(FatcadetailModel fatcadetailModel)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(fatcadetailModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  


                HttpResponseMessage response = await client.PostAsync(StaticValues.fatcaUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    //var objResponse = await response.Content.ReadAsStringAsync();
                    //addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    message = "Ok";
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

            }
            return message;
        }


        public async Task<DigioStatus> CheckDigioStatus(int RegistrationID)
        {
            DigioStatus mDigioStatus = new DigioStatus();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.CheckDigioStatus, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationId", RegistrationID);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);
                        DataTable dt = st.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            mDigioStatus.pan = dr["pan"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return (mDigioStatus);
        }

        #endregion
    }
}
