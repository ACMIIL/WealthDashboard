using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WealthDashboard.Models.InvestNowManager;
using WealthDashboard.Models.OrderAuthentication;
using WealthDashboard.Models;
using WealthDashboard.Models.PrimaryDetailManager;

namespace WealthDashboard.Controllers
{
    public class OrderAuthenticationController : Controller
    {
        #region Global Variables
        private readonly IOrderOthenticationManager _orderAuthentication;
        private readonly IInvestNowManager _investNowManager;
        private readonly IPrimaryDetailsManager _primaryDetailsManager;
        #endregion



        #region Ctor
        public OrderAuthenticationController(IOrderOthenticationManager orderAuthentication,
            IInvestNowManager investNowManager, IPrimaryDetailsManager primaryDetailsManager)
        {
            _orderAuthentication = orderAuthentication;
            _investNowManager = investNowManager;
            _primaryDetailsManager = primaryDetailsManager;
        }
        #endregion
        #region Method
        [HttpPost]
        public async Task<JsonResult> InsertBSECraeteOrder([FromBody] InsertBseOrderModel insertBseOrderModel)
        {
            if (insertBseOrderModel.siplumpsum == "SIP")
            {
                var generatesrno = await _investNowManager.generatesrno(insertBseOrderModel.ucc);
                var getResult = JsonConvert.DeserializeObject<srModel>(Convert.ToString(generatesrno.Data));
                insertBseOrderModel.Mandate_SrNo = getResult.uDsrno;
            }
            var getCheckOutsideDPdata = await _orderAuthentication.InsertBSECraeteOrder(insertBseOrderModel);
            return Json(getCheckOutsideDPdata);
        }

        [HttpPost]
        public async Task<JsonResult> Paymentrequest(string payeeBankAccountNo, string payeeBankID, string currencyCode, string payeeLoginID, string PayAmount, string MFTransactionID, string RequestSource)
        {
            var paymentrequestModel = await _orderAuthentication.Paymentrequest(payeeBankAccountNo, payeeBankID, currencyCode, payeeLoginID, PayAmount, MFTransactionID, RequestSource);
            if (paymentrequestModel.data.URL != null)
            {
                await _investNowManager.SendPaymentLinkOnEmail(paymentrequestModel.data.URL);
            }
            return Json(paymentrequestModel);
        }

        public async Task<JsonResult> GetPrimaryDetails(string UCC)
        {
            try
            {
                var result = await _primaryDetailsManager.GetPrimaryDetails(UCC);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion
    }
}
