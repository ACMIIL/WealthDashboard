using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WealthDashboard.Models.InvestNowManager;
using WealthDashboard.Models.OrderAuthentication;
using WealthDashboard.Models;
using WealthDashboard.Models.PrimaryDetailManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Encryption;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Login;
using Microsoft.Extensions.Options;
using WealthDashboard.Configuration;

namespace WealthDashboard.Controllers
{
    public class OrderAuthenticationController : Controller
    {
        #region Global Variables
        private readonly IOrderOthenticationManager _orderAuthentication;
        private readonly IInvestNowManager _investNowManager;
        private readonly IPrimaryDetailsManager _primaryDetailsManager;
        private readonly Appsetting _appSetting;
        #endregion



        #region Ctor
        public OrderAuthenticationController(IOrderOthenticationManager orderAuthentication,
            IInvestNowManager investNowManager, IPrimaryDetailsManager primaryDetailsManager, IOptions<Appsetting> appSetting)
        {
            _orderAuthentication = orderAuthentication;
            _investNowManager = investNowManager;
            _primaryDetailsManager = primaryDetailsManager;
            _appSetting = appSetting.Value;
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


        [HttpPost]
        public async Task<JsonResult> AuthenticationInsert([FromBody] AuthenticationInsert authenticationInsert)
        {
            var AuthenticationSentOTPDetails = await _orderAuthentication.AuthenticationInsert(authenticationInsert);
            if (authenticationInsert.TransactionType == "Redeem")
            {
                var SentAuthenticationOTP = await _orderAuthentication.SentAuthenticationOTP(new SentAuthenticationOTPModel()
                {
                    UCC = authenticationInsert.UCC,
                    TransactionType = string.Empty,
                    SchemeName = string.Empty,
                    SchemeCode = string.Empty,
                    Option = "1", //for single otp
                    CommonOrderID = AuthenticationSentOTPDetails

                });

                return Json(SentAuthenticationOTP);
            }
            else
            {
                var SentAuthenticationOTP = await _orderAuthentication.SentAuthenticationOTP(new SentAuthenticationOTPModel()
                {
                    UCC = authenticationInsert.UCC,
                    TransactionType = string.Empty,
                    SchemeName = string.Empty,
                    SchemeCode = string.Empty,
                    Option = "1",
                    CommonOrderID = AuthenticationSentOTPDetails

                });

                return Json(SentAuthenticationOTP);
            }


        }

        [HttpPost]
        public async Task<JsonResult> UpdateResendOTP([FromBody] RsendOTP resendotp)
        {
            var getCheckOutsideDPdata = await _orderAuthentication.UpdateResendOTP(resendotp);
            return Json(getCheckOutsideDPdata);

        }
        /// <summary>
        /// for send payment welth link to client
        /// </summary>
        /// <param name="ucc"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> PaymentRequestLink(string ucc)
        {

            try
            {
                var encucc = Encryption.Encrypt(ucc.Replace(' ', '+').Replace(' ', '+'));
                var redirecturl = _appSetting.wealthweburl + "MutualFund/Authentication?encucc=" + encucc;

                await _investNowManager.SendPaymentLinkOnEmail(redirecturl);
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            

        }
        #endregion
    }
}
