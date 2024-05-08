using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager
{   
    public class ClientRegistrationManager : IClientRegistrationManager
    {
        #region Global Variable
        private readonly ILogger<ClientRegistrationManager> _logger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;
        private readonly IClientRegistrationManager _clientRegistrationManager;
        #endregion
       
        #region Ctor
        public ClientRegistrationManager(IOptions<ConnectionStrings> connectionStrings,
                                 IOptions<Appsetting> options,
                                 ILogger<ClientRegistrationManager> logger
                                 )
        {
            _connectionStrings = connectionStrings.Value;
            _appsetting = options.Value;
            _logger = logger;
        }
        #endregion

        #region Method
        public UCCTempModel GetNewClientDetails(string UCC)
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.GetPrimaryDetailsByUCC, connection))
                    {
                        command.Parameters.AddWithValue("@CommonClientCode", UCC);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);
                        if (st.Tables.Count > 0)
                        {
                            DataTable dt = st.Tables[0];
                            foreach (DataRow dr in dt.Rows)
                            {
                                mUCCTempModel.RegistrationId = Convert.ToInt16(dt.Rows[0]["RegistrationId"].ToString());
                                mUCCTempModel.UCC = dt.Rows[0]["CommonClientCode"].ToString();
                                mUCCTempModel.MobileNumber = dt.Rows[0]["MobileNumber"].ToString();
                                mUCCTempModel.Email_Id = dt.Rows[0]["EmailId"].ToString();
                                mUCCTempModel.BACode = dt.Rows[0]["Bacode"].ToString();
                            }
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
        public UCCTempModel GetUCCByRegistrationID(int RID)
        {
            UCCTempModel mUCCTempModel = new UCCTempModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(StaticValues.GetPrimaryDetailsByRegistrationId, connection))
                    {
                        command.Parameters.AddWithValue("@RegistrationId", RID);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);
                        if (st.Tables.Count > 0)
                        {
                            DataTable dt = st.Tables[0];
                            foreach (DataRow dr in dt.Rows)
                            {
                                mUCCTempModel.RegistrationId = Convert.ToInt16(dt.Rows[0]["RegistrationId"].ToString());
                                mUCCTempModel.UCC = dt.Rows[0]["CommonClientCode"].ToString();
                                mUCCTempModel.MobileNumber = dt.Rows[0]["MobileNumber"].ToString();
                                mUCCTempModel.Email_Id = dt.Rows[0]["EmailId"].ToString();
                                mUCCTempModel.BACode = dt.Rows[0]["Bacode"].ToString();
                                mUCCTempModel.ClientName = dt.Rows[0]["ClientFullName"].ToString();
                                mUCCTempModel.CityName = dt.Rows[0]["CityName"].ToString();
                                mUCCTempModel.FileName = dt.Rows[0]["FileName"].ToString();
                                mUCCTempModel.FilePath = dt.Rows[0]["FilePath"].ToString();
                            }
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
        #endregion
    }
}
