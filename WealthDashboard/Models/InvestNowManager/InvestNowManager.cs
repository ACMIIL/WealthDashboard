using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;
using WealthDashboard.Configuration;

namespace WealthDashboard.Models.InvestNowManager
{
    public class InvestNowManager : IInvestNowManager
    {
        #region Global Variables
        private readonly Appsetting _appSetting;
        #endregion

        #region Ctor
        public InvestNowManager(IOptions<Appsetting> appSetting)
        {
            _appSetting = appSetting.Value;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestNowDetailInsertModel"></param>
        /// <returns></returns>
        public async Task<ResultModel> InvestNowInsert(InvestNowDetailInsertModel investNowDetailInsertModel)
        {
            ResultModel getResult = new ResultModel();
            string URL = string.Empty;

            try
            {
                var requestContent = new StringContent
                (
                              JsonConvert.SerializeObject(investNowDetailInsertModel),
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
                                   .PostAsync(APIUrl.InvestNowDetailInsert, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    getResult = JsonConvert.DeserializeObject<ResultModel>(Convert.ToString(objResponse));

                }

                //return Msg;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return getResult;
        }


        public async Task<ResultModel> generatesrno(string ucc)
        {
            ResultModel getResult = new ResultModel();
            string URL = string.Empty;

            try
            {
                var requestContent = new StringContent
                (
                              JsonConvert.SerializeObject(ucc),
                              Encoding.UTF8,
                              StaticValues.ApplicationJsonMediaType
                );

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue(StaticValues.ApplicationJsonMediaType));
                client.BaseAddress = new Uri(_appSetting.MFAPIBaseURL);
                //Sending request to find web api REST service using HttpClient  
                string url = _appSetting.MFAPIBaseURL + "api/BSEOrderMaster/CreateSrNo?UCC=" + ucc + "";
                HttpResponseMessage response = await client
                                   .GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var objResponse = await response.Content.ReadAsStringAsync();
                    getResult = JsonConvert.DeserializeObject<ResultModel>(Convert.ToString(objResponse));

                }

                //return Msg;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return getResult;
        }
        public async Task<ResultModel> SendPaymentLinkOnEmail(string PaymentURL)
        {
            var html = "";
            string subject = "";
            try
            {
                html = $"Click here to payment {PaymentURL}";
                subject = "Payment Request.";
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("donotreply@acm.co.in");
                //msg.To.Add("sameer.nalawade.acm.co.in");
                //msg.To.Add(PanstatusModel.EmailId);
                msg.To.Add("sheikhtalha.ms@gmail.com");
                msg.CC.Add("pravin.patil@acm.co.in");
                msg.Subject = subject;
                msg.Body = html;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //if (!string.IsNullOrEmpty(attachmentFilePath) && File.Exists(attachmentFilePath))
                //{
                //    Attachment attachment = new Attachment(attachmentFilePath);
                //    msg.Attachments.Add(attachment);
                //}

                SmtpClient smt = new SmtpClient("smtpin.falconide.com");
                smt.Credentials = new System.Net.NetworkCredential("acmmails", "cCnc!nC");
                smt.Send(msg);


                // await InsertEmailService(PanstatusModel, msg.Bcc.ToString(), msg.CC.ToString(), msg.Subject.ToString(), html);
                return new ResultModel()
                {

                    Code = 0,
                    Message = string.Empty,
                    Data = string.Empty
                };

            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Code = 0,
                    Message = ex.Message,
                    Data = string.Empty
                };
            }
        }
    }
}
