using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WealthDashboard.Models.InvestNowManager;

namespace WealthDashboard.Models.OrderAuthentication
{
    public class OrderOthenticationManager : IOrderOthenticationManager
    {
        #region Global Variable
        private readonly AppSetting _appSetting;
        #endregion

        #region Ctor
        public OrderOthenticationManager(IOptions<AppSetting> options)
        {
            _appSetting = options.Value;
        }
        #endregion

        public async Task<BSEOrderResult> InsertBSECraeteOrder(InsertBseOrderModel insertBseOrderModel)
        {
            ResultModel getResult = new ResultModel();
            BSEOrderResult Result = new();
            string URL = string.Empty;

            try
            {
                var requestContent = new StringContent
                (
                              JsonConvert.SerializeObject(insertBseOrderModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue(StaticValues.ApplicationJsonMediaType));
                client.BaseAddress = new Uri(_appSetting.MFAPIBaseURL);
                //Sending request to find web api REST service using HttpClient  
                HttpResponseMessage response = await client
                                   .PostAsync(APIUrl.InsertBSECraeteOrder, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    getResult = JsonConvert.DeserializeObject<ResultModel>(Convert.ToString(objResponse));
                    Result = JsonConvert.DeserializeObject<BSEOrderResult>(Convert.ToString(getResult.Data));
                }

                //return Msg;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Result;
        }

        public async Task<PaymentrequestModel> Paymentrequest(string payeeBankAccountNo, string payeeBankID, string currencyCode, string payeeLoginID, string PayAmount, string MFTransactionID, string RequestSource)
        {
            try
            {
                PaymentrequestModel paymentrequestModel = new PaymentrequestModel();
                string mStrUrl = String.Format("https://mfpaymentapi.investmentz.com/api/v1/PayRequestAPI/Create");
                
                    Dictionary<string, string> values9 = new Dictionary<string, string>();
                using (WebClient client2 = new WebClient())
                {    values9["PayRequestsLogId"] = "0";
                    values9["BenificaryID"] = "0";
                    values9["PayeeBankAccountNo"] = payeeBankAccountNo;
                    values9["PayeeBankID"] = payeeBankID;
                    values9["PayAmount"] = PayAmount;
                    values9["CurrencyCode"] = currencyCode;
                    values9["PayeeLoginID"] = payeeLoginID;
                    values9["PayeeCode"] = payeeLoginID;
                    values9["SuccessRU"] = "";
                    values9["FailureRU"] = "";
                    values9["RequestSource"] = RequestSource;
                    values9["RequestSourceName"] = "";
                    values9["TransactionID"] = "";
                    values9["TypeOfTransaction"] = "";
                    values9["MFTransactionID"] = MFTransactionID;
                    string req2 = JsonConvert.SerializeObject(values9);
                    client2.Headers.Add("Content-Type", "application/json");
                    string responseString2 = client2.UploadString(mStrUrl, "POST", req2);
                    paymentrequestModel = JsonConvert.DeserializeObject<PaymentrequestModel>(responseString2);
                    return paymentrequestModel;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<long> AuthenticationInsert(AuthenticationInsert authenticationInsert)
        {
            long CommonId = 0;

            try
            {
                var requestContent = new StringContent
                (
                              JsonConvert.SerializeObject(authenticationInsert),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue(StaticValues.ApplicationJsonMediaType));
                client.BaseAddress = new Uri(_appSetting.MFAPIBaseURL);
                //Sending request to find web api REST service using HttpClient  
                HttpResponseMessage response = await client
                                   .PostAsync(APIUrl.AuthenticationInsert, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    var getResult = JsonConvert.DeserializeObject<ResultModel>(Convert.ToString(objResponse));
                    CommonId = Convert.ToInt64(getResult.Data.ToString());
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return CommonId;
        }
        public async Task<SentAuthenticationOTPDEtailsModel> SentAuthenticationOTP(SentAuthenticationOTPModel sentAuthenticationOTPModel)
        {
            SentAuthenticationOTPDEtailsModel sentAuthenticationOTPDEtailsModel = new();
            try
            {
                var requestContent = new StringContent
                (
                              JsonConvert.SerializeObject(sentAuthenticationOTPModel),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue(StaticValues.ApplicationJsonMediaType));
                client.BaseAddress = new Uri(_appSetting.MFAPIBaseURL);
                //Sending request to find web api REST service using HttpClient  
                HttpResponseMessage response = await client
                                   .PostAsync(APIUrl.SentAuthenticationOTP, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    var getResult = JsonConvert.DeserializeObject<ResultModel>(Convert.ToString(objResponse));
                    sentAuthenticationOTPDEtailsModel = JsonConvert.DeserializeObject<SentAuthenticationOTPDEtailsModel>(Convert.ToString(getResult.Data));
                    return sentAuthenticationOTPDEtailsModel;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return sentAuthenticationOTPDEtailsModel;
        }
        public async Task<ResultResendotp> UpdateResendOTP(RsendOTP resendotp)
        {
            ResultModel getResult = new ResultModel();
            ResultResendotp resultResendotp = new();
            string URL = string.Empty;

            try
            {
                var requestContent = new StringContent
                (
                              JsonConvert.SerializeObject(resendotp),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue(StaticValues.ApplicationJsonMediaType));
                client.BaseAddress = new Uri(_appSetting.MFAPIBaseURL);
                //Sending request to find web api REST service using HttpClient  
                HttpResponseMessage response = await client
                                   .PostAsync(APIUrl.Updateresendotp, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    getResult = JsonConvert.DeserializeObject<ResultModel>(Convert.ToString(objResponse));
                    resultResendotp = JsonConvert.DeserializeObject<ResultResendotp>(Convert.ToString(getResult.Data));
                }

                //return Msg;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return resultResendotp;
        }
    }
}
