using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using WealthDashboard.Configuration;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Common;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.NomineeManager
{
    public class NomineeManager : INomineeManager
    {
        #region global variable
        private readonly ILogger<NomineeManager> _nomineelogger;
        private readonly ConnectionStrings _connectionStrings;
        private readonly Appsetting _appsetting;
        #endregion
        #region 
        public NomineeManager(IOptions<ConnectionStrings> connectionStrings, IOptions<Appsetting> appsetting, ILogger<NomineeManager> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _appsetting = appsetting.Value;
            _nomineelogger = logger;
        }
        #endregion

        public async Task<PincodeMasterModel> GetPincodeMaster(string Pincode)
        {

            PincodeMasterModel mpincodeMasterModel = new PincodeMasterModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.EKYCWelcomeDb))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(StaticValues.GetCityState_Pincode, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PINCode", Pincode);

                        SqlDataAdapter adpt = new SqlDataAdapter(command);
                        DataSet st = new DataSet();
                        adpt.Fill(st);

                        DataTable dt = st.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            mpincodeMasterModel.Pin_code = dr["PinCode"].ToString();
                            mpincodeMasterModel.City_Name = dr["CityName"].ToString();
                            mpincodeMasterModel.StateName = dr["StateName"].ToString();
                            mpincodeMasterModel.CountryName = dr["CountryName"].ToString();
                            mpincodeMasterModel.City_id = Convert.ToInt32(dr["City_Code"]);
                            mpincodeMasterModel.State_Code = Convert.ToInt32(dr["State_Code"]);
                            mpincodeMasterModel.Country_Code = Convert.ToInt32(dr["Country_Code"]);


                        }
                    }
                }
            }
            catch (Exception ex) { }
            return (mpincodeMasterModel);

        }
    }
}
