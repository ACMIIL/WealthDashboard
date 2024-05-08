using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using System.Text.Json;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.SelfieManager
{
    public class SelfieManagerModel : ISelfieManagerModel
    {
        #region Global Variable
        private readonly ILogger<SelfieManagerModel> _selfielogger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region ctor
        public SelfieManagerModel(ILogger<SelfieManagerModel> logger,
                                  IOptions<ConnectionStrings> connectionstring,
                                  IOptions<Appsetting> options, IHttpContextAccessor httpContextAccessor)
        {
            _selfielogger = logger;
            _connectionStrings = connectionstring.Value;
            _appsetting = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion
        public async Task<SelfieRoot> SelfieDigioWorkTemplate(SelfieTempalteModal mSelfieTempalteModal)
        {
            SelfieRoot TempalteModal = new();
            try
            {
                var requestContent = new StringContent
               (
                             JsonSerializer.Serialize(mSelfieTempalteModal),
                             Encoding.UTF8,
                             StaticValues.ApplicationJsonMediaType
               );

                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.BaseAddress = new Uri(_appsetting.DigioBaseUrl);
                HttpResponseMessage response = await client
                                   .PostAsync(StaticValues.SelfieUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    TempalteModal = JsonSerializer.Deserialize<SelfieRoot>(Convert.ToString(objResponse));
                }
                else
                {
                    _selfielogger.LogError(response.IsSuccessStatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _selfielogger.LogError($"Error: {ex.Message}");
            }
            return TempalteModal;
        }

        public async Task<string> SelfieInsertUpdateDigioTemplate(int RegistrationId, SelfieRoot mSelfieRoot)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "USP_InsertUpdateSelfieWorkTemplate";

                    sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                    sqlCmd.Parameters.AddWithValue("@Id", mSelfieRoot.data.id);
                    sqlCmd.Parameters.AddWithValue("@created_at", mSelfieRoot.data.created_at);
                    sqlCmd.Parameters.AddWithValue("@status", mSelfieRoot.data.status);
                    sqlCmd.Parameters.AddWithValue("@customer_identifier", mSelfieRoot.data.customer_identifier);
                    sqlCmd.Parameters.AddWithValue("@reference_id", mSelfieRoot.data.reference_id);
                    sqlCmd.Parameters.AddWithValue("@workflow_name", mSelfieRoot.data.workflow_name);

                    sqlCmd.Parameters.AddWithValue("@transaction_id", mSelfieRoot.data.transaction_id);
                    sqlCmd.Parameters.AddWithValue("@expire_in_days", mSelfieRoot.data.expire_in_days);
                    sqlCmd.Parameters.AddWithValue("@reminder_registered", mSelfieRoot.data.reminder_registered);
                    sqlCmd.Parameters.AddWithValue("@access_token_id", mSelfieRoot.data.access_token.id);
                    sqlCmd.Parameters.AddWithValue("@access_token_entity_Id", mSelfieRoot.data.access_token.entity_id);
                    sqlCmd.Parameters.AddWithValue("@auto_approved", mSelfieRoot.data.auto_approved);
                    sqlCmd.Parameters.AddWithValue("@EntryBy", "CentralizeData");
                    sqlCmd.ExecuteNonQuery();
                    return "OK";



                }
            }
            catch (Exception ex)
            {
                _selfielogger.LogError($"Error: {ex.Message}");
                return "failed";
            }

        }

        public async Task<string> GetRequestData(int RegistrationId)
        {
            string strId = string.Empty;
            using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(StaticValues.GetSelfieWorktemplate, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                    SqlDataAdapter adpt = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adpt.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        strId = dr["id"].ToString();
                    }
                }
                connection.Close();
            }
            return strId;

        }

        public async Task<SelfieResponseModal> SelfieSaveResponseData(SelfieStrId mSelfieStrId)
        {
            SelfieResponseModal mSelfieResponseModal = new SelfieResponseModal();
            try
            {
                var requestContent = new StringContent
                    (
                                  JsonSerializer.Serialize(mSelfieStrId),
                                  Encoding.UTF8,
                                  StaticValues.ApplicationJsonMediaType
                    );
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", "Basic QUlTSkJZSVhDNDdDMktNUlBUU1lETDlMSzlOSEY0Rjk6Nks1VjQ2NVYyNUtRNVNTUzFDNzlUSU1FUEhSOVBMT1M=");
                client.DefaultRequestHeaders.Accept.Clear();
                client.BaseAddress = new Uri(_appsetting.DigioBaseUrl);
                var Url = _appsetting.DigioBaseUrl + StaticValues.SelfieResponseUrl + mSelfieStrId.ResponseId + "&sourceType=" + mSelfieStrId.sourceType;
                HttpResponseMessage response = await client
                                   .PostAsync(Url, requestContent);


                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    mSelfieResponseModal = JsonSerializer.Deserialize<SelfieResponseModal>(Convert.ToString(objResponse));
                }
                else
                {
                    _selfielogger.LogError(response.IsSuccessStatusCode.ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return (mSelfieResponseModal);
        }

        public async Task<string> SelfieInsertUpdateDigioTemplate(int RegistrationId, SelfieResponseModal mSelfieResponseModal)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "USP_SelfieResponseData";

                    sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                    sqlCmd.Parameters.AddWithValue("@ResponseId", mSelfieResponseModal.data.id);
                    sqlCmd.Parameters.AddWithValue("@updated_at", mSelfieResponseModal.data.updated_at);
                    sqlCmd.Parameters.AddWithValue("@created_at", mSelfieResponseModal.data.created_at);
                    sqlCmd.Parameters.AddWithValue("@status", mSelfieResponseModal.data.status);
                    sqlCmd.Parameters.AddWithValue("@customer_identifier", mSelfieResponseModal.data.customer_identifier);

                    sqlCmd.Parameters.AddWithValue("@actions_id", mSelfieResponseModal.data.actions[0].id);
                    sqlCmd.Parameters.AddWithValue("@action_action_ref", mSelfieResponseModal.data.actions[0].action_ref);

                    sqlCmd.Parameters.AddWithValue("@action_type", mSelfieResponseModal.data.actions[0].type);
                    sqlCmd.Parameters.AddWithValue("@action_status", mSelfieResponseModal.data.actions[0].status);
                    sqlCmd.Parameters.AddWithValue("@action_FileId", mSelfieResponseModal.data.actions[0].file_id);
                    sqlCmd.Parameters.AddWithValue("@action_completed_at", mSelfieResponseModal.data.actions[0].completed_at);


                    sqlCmd.Parameters.AddWithValue("@action_face_match_obj_type", mSelfieResponseModal.data.actions[0].face_match_obj_type);
                    sqlCmd.Parameters.AddWithValue("@action_face_match_status", mSelfieResponseModal.data.actions[0].face_match_status);

                    sqlCmd.Parameters.AddWithValue("@action_obj_analysis_status", mSelfieResponseModal.data.actions[0].obj_analysis_status);
                    sqlCmd.Parameters.AddWithValue("@action_method", mSelfieResponseModal.data.actions[0].face_match_status);
                    sqlCmd.Parameters.AddWithValue("@action_processing_done", mSelfieResponseModal.data.actions[0].processing_done);
                    sqlCmd.Parameters.AddWithValue("@action_retry_count", mSelfieResponseModal.data.actions[0].retry_count);

                    sqlCmd.Parameters.AddWithValue("@reference_id", mSelfieResponseModal.data.reference_id);
                    sqlCmd.ExecuteNonQuery();
                    return "OK";



                }
            }
            catch (Exception ex)
            {
                _selfielogger.LogError($"Error: {ex.Message}");
                return "failed";
            }

        }

        public async Task<string> GetSelfieFileData(SelfieAction mSelfieAction, int RegistrationId, string InwardNo)
        {

            string strMsg = "";
            try
            {
                var requestContent = new StringContent
                    (
                                  JsonSerializer.Serialize(mSelfieAction.file_id),
                                  Encoding.UTF8,
                                  StaticValues.ApplicationJsonMediaType
                    );
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", "Basic QUlTSkJZSVhDNDdDMktNUlBUU1lETDlMSzlOSEY0Rjk6Nks1VjQ2NVYyNUtRNVNTUzFDNzlUSU1FUEhSOVBMT1M=");
                client.DefaultRequestHeaders.Accept.Clear();
                client.BaseAddress = new Uri(_appsetting.DigioBaseUrl);
                var Url = _appsetting.DigioBaseUrl + StaticValues.DigioSelfieFileDownload + mSelfieAction.file_id;
                HttpResponseMessage response = await client
                                   .GetAsync(Url);

                SelfieFileDownloadModel mSelfieFileDownloadModel = new SelfieFileDownloadModel();
                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    mSelfieFileDownloadModel = JsonSerializer.Deserialize<SelfieFileDownloadModel>(Convert.ToString(objResponse));
                    byte[] result = mSelfieFileDownloadModel.data;
                    string directoryPath = Path.Combine(_appsetting.ClientFilePath, InwardNo);
                    string filePath = Path.Combine(directoryPath, $"{InwardNo}_PassportPhoto.png");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    File.WriteAllBytes(filePath, result);

                }
                if (response.IsSuccessStatusCode)
                {
                    return strMsg = "OK";
                }
                else
                {
                    _selfielogger.LogError(response.IsSuccessStatusCode.ToString());
                }


            }
            catch (Exception ex)
            {
                _selfielogger.LogError($"Error: {ex.Message}");
                return strMsg = "failed";
            }

            return strMsg;

        }

        public async Task<string> GetGeoInfo(int RegistrationId)
        {
            string Msg = "";
            try
            {
                //I have already created this function under GeoInfoProvider class.
                string ipAddress = GetIPAddress();
                if (ipAddress == "::1")
                {
                    ipAddress = "103.57.141.234";
                }


                var client = new HttpClient();
                string url = string.Format("https://api.ip2location.io/?ip=" + ipAddress + "&key=a3cfce18d5175f6edeafc0371d96e8be");
                HttpResponseMessage response = await client
                                   .GetAsync(url);
                LocationModel location = new LocationModel();
                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    location = JsonSerializer.Deserialize<LocationModel>(Convert.ToString(objResponse));

                }
                if (response.IsSuccessStatusCode)
                {
                    ClientGeoTaggingDetailsModel mClientGeoTaggingDetailsModel = new ClientGeoTaggingDetailsModel();
                    mClientGeoTaggingDetailsModel.IPAddress = location.ip;
                    mClientGeoTaggingDetailsModel.ContinentName = "Asia";
                    mClientGeoTaggingDetailsModel.CountryCode = location.country_code;
                    mClientGeoTaggingDetailsModel.RegionCode = location.region_name;
                    mClientGeoTaggingDetailsModel.RegionName = location.region_name;
                    mClientGeoTaggingDetailsModel.City = location.city_name;
                    mClientGeoTaggingDetailsModel.ZipCode = location.zip_code;
                    mClientGeoTaggingDetailsModel.Latitude = Convert.ToString(location.latitude);
                    mClientGeoTaggingDetailsModel.Longitude = Convert.ToString(location.longitude);
                    string strconhercules = _connectionStrings.EKYCWelcomeDb;

                    using (SqlConnection con = new SqlConnection(strconhercules))
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_InsertOrUpdateClientGeoTrackingDetails", con))
                        {
                            cmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                            cmd.Parameters.AddWithValue("@IPAddress", mClientGeoTaggingDetailsModel.IPAddress);
                            cmd.Parameters.AddWithValue("@IPType", "IPV4");
                            cmd.Parameters.AddWithValue("@ContinentName", mClientGeoTaggingDetailsModel.ContinentName);
                            cmd.Parameters.AddWithValue("@CountryCode", mClientGeoTaggingDetailsModel.CountryCode);

                            cmd.Parameters.AddWithValue("@RegionCode", mClientGeoTaggingDetailsModel.RegionCode);
                            cmd.Parameters.AddWithValue("@RegionName", mClientGeoTaggingDetailsModel.RegionName);
                            cmd.Parameters.AddWithValue("@City", mClientGeoTaggingDetailsModel.City);

                            cmd.Parameters.AddWithValue("@ZipCode", mClientGeoTaggingDetailsModel.ZipCode);
                            cmd.Parameters.AddWithValue("@Latitude", mClientGeoTaggingDetailsModel.Latitude);
                            cmd.Parameters.AddWithValue("@Longitude", mClientGeoTaggingDetailsModel.Longitude);
                            cmd.Parameters.AddWithValue("@EntryBy", "Keyur");

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                sda.Fill(ds);
                            }
                        }
                    }

                    return Msg = "OK";
                }
                else
                {
                    return Msg = "Failed";
                    _selfielogger.LogError(response.IsSuccessStatusCode.ToString());
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                return Msg = ex.ToString();
            }

        }

        private string GetIPAddress()
        {
            try
            {
                var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                // If the request is forwarded, try to get the forwarded IP address
                if (_httpContextAccessor.HttpContext?.Request.Headers.ContainsKey("X-Forwarded-For") == true)
                {
                    ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                }

                return ipAddress;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine(ex.ToString());
                return "";
            }
        }


        public async Task<string> SaveSelfieDetails(int RegistrationId, string InwardNo)
        {
            string Msg = "";
            var FileName = InwardNo + "_PassportPhoto.png";
            var FilePath = "D:\\ekycUploadDocuments\\" + InwardNo + "\\" + InwardNo + "_PassportPhoto.png";
            try
            {

                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "USP_InsertOrUpdateClientsUploadDocDetails";

                    sqlCmd.Parameters.AddWithValue("@UploadDocumentId", 0);
                    sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                    sqlCmd.Parameters.AddWithValue("@UploadDocNo", "1");
                    sqlCmd.Parameters.AddWithValue("@FileName", FileName);
                    sqlCmd.Parameters.AddWithValue("@FilePath", FilePath);
                    sqlCmd.Parameters.AddWithValue("@UserId", "ACMIIL");

                    return Msg = "OK";
                }
            }
            catch (Exception ex)
            {
                Msg = ex.ToString();
                return Msg = "Failed";
            }
        }

        public async Task<string> ChechSelfieDetails(int RegistrationId)
        {
            string strMsg = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.CheckSelfie, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            strMsg = dr["Msg"].ToString();
                        }

                    }

                }
                return strMsg;


            }
            catch (Exception ex)
            {
                strMsg = ex.ToString();
                return strMsg = "Failed";
            }

        }
        public async Task<string> CheckEsignStatus(int RegistrationId)
        {
            string strMsg = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.WhdbEsign))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.CheckEsignStatus, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        adpt.Fill(ds);
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            strMsg = dr["Msg"].ToString();
                        }

                    }

                }
                return strMsg;


            }
            catch (Exception ex)
            {
                strMsg = ex.ToString();
                return strMsg = "Failed";
            }

        }


    }
}
