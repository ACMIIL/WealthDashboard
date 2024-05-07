using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using WealthDashboard.Configuration;
using System.Text.Json;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.CVLKRAManager
{
    public class CVLKRADetailsManager : ICVLKRADetailsManager
    {
        #region Global Variable
        private readonly ILogger<CVLKRADetailsManager> _logger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;
        #endregion

        #region Ctor
        public CVLKRADetailsManager(IOptions<ConnectionStrings> connectionStrings,
                                 IOptions<Appsetting> options,
                                 ILogger<CVLKRADetailsManager> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _appsetting = options.Value;
            _logger = logger;
        }
        #endregion

        #region Method
        public async Task<string> GetCVLDATA(CVLKRAReqModel mCVLKRAReqModel)
        {

            string strMsg = "";
            CVLKRAResponseDataModel mCVLKRAResponseDataModel = new();
            mCVLKRAReqModel.DOB = Convert.ToDateTime(mCVLKRAReqModel.DOB).ToString("dd-MM-yyyy");
            try
            {   
                var requestContent = new StringContent
                (
                              JsonSerializer.Serialize(mCVLKRAReqModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.BaseAddress = new Uri("https://devdigio.investmentz.com/");
                var url = "https://devdigio.investmentz.com" + StaticValues.CVLKRAUrl + "?RegistrationId=" + mCVLKRAReqModel.RegistrationId + "&Pan_No=" + mCVLKRAReqModel.PAN + "&DOB=" + mCVLKRAReqModel.DOB;
                HttpResponseMessage response = await client.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    mCVLKRAResponseDataModel = JsonSerializer.Deserialize<CVLKRAResponseDataModel>(Convert.ToString(objResponse));
                    strMsg = await SaveCVLKRAXmlData(Convert.ToInt32(mCVLKRAReqModel.RegistrationId), mCVLKRAResponseDataModel.data.data);
                }
                else
                {
                    _logger.LogError("Failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CVLKRA" + ex.ToString());
                _logger.LogError("CVLKRA" + ex.StackTrace);
                ex.ToString();
            }
            return strMsg;
        }

        public async Task<string> SaveCVLKRAXmlData(int RegistrationId, string Str)
        {
            try
            {
                if (Str != null || Str != "")
                {
                    using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                    {
                        sqlConn.Open();
                        SqlCommand sqlCmd = new SqlCommand();
                        sqlCmd.Connection = sqlConn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "USP_INSERTCVLKRA_ResponseData";
                        sqlCmd.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                        sqlCmd.Parameters.AddWithValue("@XmlData", Str);
                        sqlCmd.ExecuteNonQuery();
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
                return "Failed";
            }
        }

        public async Task<CVLKRAResponsexmlDataModel> GetResponseCVLXMLData(string RegistrationId)
        {

            CVLKRAResponsexmlDataModel mCVLKRAResponsexmlDataModel = new();
            try
            {
                if (RegistrationId != "")
                {
                    using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                    {
                        using (SqlCommand command = new SqlCommand(StaticValues.GetResponseXMLData, sqlConn))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                            SqlDataAdapter adpt = new SqlDataAdapter(command);
                            DataSet st = new DataSet();
                            adpt.Fill(st);
                            if (st.Tables.Count > 0)
                            {
                                DataTable dt = st.Tables[0];
                                if (st.Tables.Count > 0)
                                {
                                    mCVLKRAResponsexmlDataModel.panno = dt.Rows[0]["panno"].ToString();
                                    mCVLKRAResponsexmlDataModel.Fullname = dt.Rows[0]["Fullname"].ToString();
                                    mCVLKRAResponsexmlDataModel.gender = dt.Rows[0]["gender"].ToString();
                                    mCVLKRAResponsexmlDataModel.dob = dt.Rows[0]["dob"].ToString();
                                    mCVLKRAResponsexmlDataModel.adharno = dt.Rows[0]["adharno"].ToString();
                                    mCVLKRAResponsexmlDataModel.Per_address1 = dt.Rows[0]["Per_address1"].ToString();
                                    mCVLKRAResponsexmlDataModel.Per_address2 = dt.Rows[0]["Per_address2"].ToString();
                                    mCVLKRAResponsexmlDataModel.Per_address3 = dt.Rows[0]["Per_address3"].ToString();
                                    mCVLKRAResponsexmlDataModel.Per_distorcity = dt.Rows[0]["Per_distorcity"].ToString();
                                    mCVLKRAResponsexmlDataModel.Per_state = dt.Rows[0]["Per_state"].ToString();
                                    mCVLKRAResponsexmlDataModel.Per_pincode = dt.Rows[0]["Per_pincode"].ToString();
                                    mCVLKRAResponsexmlDataModel.mobile = dt.Rows[0]["mobile"].ToString();
                                    mCVLKRAResponsexmlDataModel.networth = dt.Rows[0]["networth"].ToString();
                                    mCVLKRAResponsexmlDataModel.Fatherspouce = dt.Rows[0]["Fatherspouce"].ToString();
                                    mCVLKRAResponsexmlDataModel.Emailid = dt.Rows[0]["Emailid"].ToString();
                                }
                            }
                        }
                    }
                    return mCVLKRAResponsexmlDataModel;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Response XML Data" + ex.ToString());
                _logger.LogError("Response XML Data" + ex.StackTrace);
            }
            return mCVLKRAResponsexmlDataModel;
        }
        #endregion
    }
}
