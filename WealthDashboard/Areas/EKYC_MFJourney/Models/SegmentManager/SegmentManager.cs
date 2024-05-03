using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentModel;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using System.Text.Json;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager
{
    public class SegmentManager : ISegmentManager
    {
        #region Global Variable
        private readonly Appsetting _appsetting;
        private readonly ConnectionStrings _connectionStrings;
        #endregion

        #region Ctor
        public SegmentManager(IOptions<Appsetting> options, IOptions<ConnectionStrings> connectionstring)
        {
            _appsetting = options.Value;
            _connectionStrings = connectionstring.Value;
        }
        #endregion

        public async Task<DepositoryMasterResponse> GetDepositoryMaster(string option, string dpId)
        {
            DepositoryMasterResponse depositoryMasterResponse = new DepositoryMasterResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.ApplicationJsonMediaType));
                    client.BaseAddress = new Uri(_appsetting.GetDPBaseURL);
                    string requestUri = _appsetting.GetDPDataURL.Replace("#Option#", option).Replace("#DPID#", dpId);
                    HttpResponseMessage response = await client.GetAsync(requestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        depositoryMasterResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositoryMasterResponse>(jsonResponse);
                        
                        if (depositoryMasterResponse != null && depositoryMasterResponse.DPID != null && depositoryMasterResponse.DPID.Count > 0)
                        {   
                            return depositoryMasterResponse;
                        }
                        else
                        {
                            Console.WriteLine($"Unexpected response format: {jsonResponse}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions here
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return depositoryMasterResponse;
        }

        public async Task<string> InsertOrupdateClentsegment(List<SegmentData> segmentData)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                    JsonSerializer.Serialize(segmentData),
                    Encoding.UTF8,
                    StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  


                HttpResponseMessage response = await client.PostAsync(StaticValues.InsertOrUpdateClientSegment, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    //var objResponse = await response.Content.ReadAsStringAsync();
                    //addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    message = "Ok";
                }
                else
                {
                    // _logger.LogError("Failed Mobile Details Invalid");

                }

            }
            catch (Exception ex)
            {
                //_logger.LogError("Digio" + ex.ToString());
                //_logger.LogError("Digio" + ex.StackTrace);

            }
            return message;
        }

        public async Task<string> InsertOrUpdateSegment(ReqInsertSegment reqInsertSegment)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                    JsonSerializer.Serialize(reqInsertSegment),
                    Encoding.UTF8,
                    StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  


                HttpResponseMessage response = await client.PostAsync(StaticValues.InsertOrUpdateSegment, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    //var objResponse = await response.Content.ReadAsStringAsync();
                    //addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    message = "Ok";
                }
                else
                {
                    // _logger.LogError("Failed Mobile Details Invalid");

                }

            }
            catch (Exception ex)
            {
                //_logger.LogError("Digio" + ex.ToString());
                //_logger.LogError("Digio" + ex.StackTrace);

            }
            return message;
        }
        public async Task<string> InsertOrUpdateClientUploadData(SegmentUploadModel segmentUploadModel)
        {
            string message = string.Empty;
            try
            {
                var requestContent = new StringContent
                (
                    JsonSerializer.Serialize(segmentUploadModel),
                    Encoding.UTF8,
                    StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.BaseAddress = new Uri(_appsetting.EKYC_apiBaseUrl);
                //Sending request to find web api REST service using HttpClient  


                HttpResponseMessage response = await client.PostAsync(StaticValues.InsertOrUpdateClientUploadData, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    //var objResponse = await response.Content.ReadAsStringAsync();
                    //addressCodeModel = JsonSerializer.Deserialize<AddressCodeModel>(objResponse);

                    message = "Ok";
                }
                else
                {
                    // _logger.LogError("Failed Mobile Details Invalid");

                }

            }
            catch (Exception ex)
            {
                //_logger.LogError("Digio" + ex.ToString());
                //_logger.LogError("Digio" + ex.StackTrace);

            }
            return message;
        }
        public async Task<string> UpdateBrokarageplan(int RID, int tarrifplan, int BrokragePlan)
        {
            try
            {
                SqlConnection con = new SqlConnection(_connectionStrings.EKYCWelcomeDb);
                SqlCommand cmd = new SqlCommand("USP_BOIBrokragePlan", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RegistraionID", RID);
                cmd.Parameters.AddWithValue("@BrokragePlan", BrokragePlan);
                cmd.Parameters.AddWithValue("@TarrifPlan", tarrifplan);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<string> Update_BACode(int RID, string Bacode)
        {
            try
            {
                SqlConnection con = new SqlConnection(_connectionStrings.EKYCWelcomeDb);
                SqlCommand cmd = new SqlCommand("SP_UPDATE_BACODE", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Registration", RID);
                cmd.Parameters.AddWithValue("@Bacode", Bacode);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<List<brockragedrp>> Brockarageplan()
        {
            List<brockragedrp> brockragedrp = new List<brockragedrp>();
            try
            {
                SqlConnection con = new SqlConnection(_connectionStrings.EKYCWelcomeDb);
                SqlCommand cmd = new SqlCommand("USP_GetTariffPlanDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet dS = new DataSet();
                adp.Fill(dS);

                DataTable dt = dS.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    brockragedrp.Add(new SegmentModel.brockragedrp
                    {
                        TariffPlanId = Convert.ToInt32(dr["TariffPlanId"]),
                        PlanDescription = dr["PlanDescription"].ToString()
                    });


                }

                con.Close();
                return brockragedrp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
