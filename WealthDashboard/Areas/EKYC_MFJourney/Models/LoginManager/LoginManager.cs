using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using WealthDashboard.Models;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using StaticValues = WealthDashboard.Areas.EKYC_MFJourney.Models.Common.StaticValues;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.LoginManager
{
    public class LoginManager : ILoginManager
    {
        #region global variable
        private readonly ILogger<LoginManager> _logger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;
        #endregion
        #region 
        public LoginManager(IOptions<ConnectionStrings> connectionStrings, IOptions<Appsetting> appsetting, ILogger<LoginManager> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _appsetting = appsetting.Value;
            _logger = logger;

        }
        #endregion
        public async Task<GenerateOTPModel> GenerateOtp()
        {
            GenerateOTPModel mGenerateOTPModel = new GenerateOTPModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.WHDB1))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(StaticValues.GenerateOTP, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);
                        if (st.Tables.Count > 0)
                        {
                            DataTable dt = st.Tables[0];
                            foreach (DataRow dr in dt.Rows)
                            {
                                mGenerateOTPModel.EmailOTP = dr["EmailOTP"].ToString();
                                mGenerateOTPModel.MobileOTP = dr["MobileOTP"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return mGenerateOTPModel;
        }

        public async Task<UCCTempModel> save_ucc_temp_details(TempModal tempModal)
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            try
            {
                if (tempModal.BACode == "" || tempModal.BACode == null)
                {
                    tempModal.BACode = _appsetting.DirectClient;
                }
                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "USP_InsertUCCTempData_TWC";
                    sqlCmd.Parameters.AddWithValue("@Email_Id", " ");
                    sqlCmd.Parameters.AddWithValue("@MobileNumber", tempModal.MobileNo);
                    sqlCmd.Parameters.AddWithValue("@BAcode", tempModal.BACode);
                    sqlCmd.Parameters.AddWithValue("@SourceType", " ");
                    sqlCmd.Parameters.AddWithValue("@EmailRelation", " ");
                    sqlCmd.Parameters.AddWithValue("@MobileRelation", tempModal.MobileRelation);
                    sqlCmd.Parameters.AddWithValue("@RelationshipWithEmailUCC", " ");
                    sqlCmd.Parameters.AddWithValue("@RelationshipWithMobileUCC", " ");
                    sqlCmd.Parameters.AddWithValue("@EmployeeReferralCode", tempModal.EmployeeRef);


                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mUCCTempModel.UCC = reader["UCC_Tmp_No"].ToString();
                            mUCCTempModel.Email_Id = reader["Email_Id"].ToString();
                            mUCCTempModel.MobileNumber = reader["MobileNumber"].ToString();
                            mUCCTempModel.BACode = reader["BaCode"].ToString();
                            mUCCTempModel.SourceType = reader["SourceType"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return mUCCTempModel;
        }

        public async Task<OTPMobileDetailsModel> SaveOtpDetails(OTPMobileDetailsModel mOTPEMailsMobileDetailsModel)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "USP_InsertOrSelectOTPEMailsMobileDetails";
                    sqlCmd.Parameters.AddWithValue("@EmailId", " ");
                    sqlCmd.Parameters.AddWithValue("@MobileNo", mOTPEMailsMobileDetailsModel.MobileNo);
                    sqlCmd.Parameters.AddWithValue("@EmailType", " ");
                    sqlCmd.Parameters.AddWithValue("@SendTo", " ");
                    sqlCmd.Parameters.AddWithValue("@SendCC", " ");
                    sqlCmd.Parameters.AddWithValue("@SendBCC", " ");
                    sqlCmd.Parameters.AddWithValue("@SendFrom", " ");
                    sqlCmd.Parameters.AddWithValue("@EmailBody", " ");
                    sqlCmd.Parameters.AddWithValue("@OTPEmail", " ");
                    sqlCmd.Parameters.AddWithValue("@OTPMobile", mOTPEMailsMobileDetailsModel.OTPMobile);
                    sqlCmd.Parameters.AddWithValue("@EntryBy", "EKYCtwc");

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mOTPEMailsMobileDetailsModel.OTPEmailDetailsID = reader["OTPEmailDetailsID"].ToString();
                            mOTPEMailsMobileDetailsModel.EmailId = reader["EmailId"].ToString();
                            mOTPEMailsMobileDetailsModel.MobileNo = reader["MobileNo"].ToString();
                            mOTPEMailsMobileDetailsModel.OTPEmail = reader["OTPEmail"].ToString();
                            mOTPEMailsMobileDetailsModel.OTPMobile = reader["OTPMobile"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return mOTPEMailsMobileDetailsModel;
        }
        public async Task<JSONMessageModel> CheckMobileDeclarationWithUCC(string MobileNo, string DeclarationType)
        {
            JSONMessageModel mJSONMessageModel = new JSONMessageModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(StaticValues.CheckMobileClientDeclaration, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MobileNo", MobileNo);
                        command.Parameters.AddWithValue("@DeclarationType", DeclarationType);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);

                        if (st.Tables.Count > 0)
                        {
                            DataTable dt = st.Tables[0];
                            foreach (DataRow dr in dt.Rows)
                            {
                                mJSONMessageModel.ErrorMessage = dr["Msg"].ToString();
                                mJSONMessageModel.ResponseCode = dr["MsgCode"].ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return (mJSONMessageModel);
        }
        public async Task<JSONMessageModel> CheckemailDeclarationWithUCC(string email, string DeclarationType)
        {
            JSONMessageModel mJSONMessageModel = new JSONMessageModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(StaticValues.USP_CheckClientsEMailIdDeclaration, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmailId", email);
                        command.Parameters.AddWithValue("@DeclarationType", DeclarationType);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);

                        DataTable dt = st.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            mJSONMessageModel.ErrorMessage = dr["Msg"].ToString();
                            mJSONMessageModel.ResponseCode = dr["MsgCode"].ToString();

                        }
                    }
                }



            }
            catch
            {

            }
            return (mJSONMessageModel);
        }

        public async Task<JSONMessageModel> SaveEmailDetails(string email, string Mobileno, string Emailtype, string sendto,
            string sendfrom, string emailBody, string otpemail, string entryby)
        {
            JSONMessageModel mJSONMessageModel = new JSONMessageModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(StaticValues.USP_UpdateOTPEMailsDetails, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmailId", email);
                        command.Parameters.AddWithValue("@MobileNo", Mobileno);
                        command.Parameters.AddWithValue("@EmailType", Emailtype);
                        command.Parameters.AddWithValue("@SendTo", sendto);
                        command.Parameters.AddWithValue("@SendFrom", sendfrom);
                        command.Parameters.AddWithValue("@EmailBody", emailBody);
                        command.Parameters.AddWithValue("@OTPEmail", otpemail);
                        command.Parameters.AddWithValue("@EntryBy", entryby);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }



            }
            catch
            {

            }
            return (mJSONMessageModel);
        }

        public async Task<OTPMobileDetailsModel> CheckOTP(string EmailId, string MobileNo)
        {
            OTPMobileDetailsModel mOTPEMailsMobileDetailsModel = new OTPMobileDetailsModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.GetOTPById, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MobileNo", MobileNo);
                        command.Parameters.AddWithValue("@EmailId", EmailId);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);
                        DataTable dt = st.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            mOTPEMailsMobileDetailsModel.OTPEmail = dr["OTPEmail"].ToString();
                            mOTPEMailsMobileDetailsModel.OTPMobile = dr["OTPMobile"].ToString();
                        }
                    }
                }
                if (_appsetting.IsLiveEnvironment.ToString() == "Y")
                {

                }
                else
                {
                    mOTPEMailsMobileDetailsModel.OTPEmail = "123456";
                    mOTPEMailsMobileDetailsModel.OTPMobile = "123456";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return (mOTPEMailsMobileDetailsModel);
        }

        public async Task<string> SaveEmaildECLARATION(int UCC, string Declaration, string EmailId)
        {
            string message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(StaticValues.USP_UpdateUCCTempData, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email_relation", Declaration);
                        command.Parameters.AddWithValue("@UCC", UCC);
                        command.Parameters.AddWithValue("@EmailId", EmailId);
                        command.ExecuteNonQuery();
                        connection.Close();
                        message = "Ok";
                    }
                }



            }
            catch
            {
                message = "Fail";
            }
            return (message);
        }
        public async Task<LastloginModel> LoginLastVisit(string MobileNo)
        {
            LastloginModel mLastloginModel = new LastloginModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.LoginLastVisit, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MobileNo", MobileNo);
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);
                        DataTable dt = st.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            mLastloginModel.LastloginURL = dr["PageVisitURL"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return (mLastloginModel);
        }
    }
}
