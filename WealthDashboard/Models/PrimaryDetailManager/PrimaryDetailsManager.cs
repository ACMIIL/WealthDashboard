using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace WealthDashboard.Models.PrimaryDetailManager
{
    public class PrimaryDetailsManager : IPrimaryDetailsManager
    {
        #region Global Variable
        private Connection _connection;
        #endregion

        #region Ctor
        public PrimaryDetailsManager(IOptions<Connection> connection)
        {
            _connection = connection.Value;
        }
        #endregion

        #region Method
        public async Task<ClientDetailsModel> GetPrimaryDetails(string UCC)
        {
            try
            {
                IDbConnection conhercules = new SqlConnection(_connection.conhercules);
                var dp = new DynamicParameters();
                dp.Add("@CCC", UCC);
                var result = await conhercules.QueryFirstOrDefaultAsync<ClientDetailsModel>(
                    sql: "USP_MFgetClientDetails",
                    param: dp,
                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
