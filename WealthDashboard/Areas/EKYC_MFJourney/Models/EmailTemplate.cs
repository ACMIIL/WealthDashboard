using WealthDashboard.Configuration;
using Microsoft.Extensions.Options;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class EmailTemplate
    {
        //private readonly Appsetting _appsetting;

        //public EmailTemplate(IOptions<Appsetting> options)
        //{
        //    _appsetting = options.Value;
        //}
        
        public string EmailAccountOpen(string OTP)
        {
            var PageURL = "https://localhost:44323/";
            string EmailAccountOpen = "";
            return EmailAccountOpen = @"<html><body><div style = 'color:#000;'> Dear User,</div><br><br><div style = 'color:#000;'>Thank you for choosing Investmentz.com for your investment needs.</div><br><div style = 'color:#000;'> Your One Time Password (OTP) for email verification is <b style='text-decoration:underline;'>" + OTP + "</b>.You can use it only once. Please do not share this OTP with anyone for security reasons.</div><br><div style ='color:#000;'> If you did not request the email verification, please ignore the message and report to our customer care on 022- 28584545.</div><br><div style = 'color:#000;'><strong>Thanks and Regards,</strong></div><div style = 'color:#000;'>Asit C.Mehta Investment Interrmediates Ltd.</div><div style = 'color:#000;'><strong><a href = +'" + PageURL + "'+ target = '_blank' style ='text-decoration:none;'> www.investmentz.com </a></strong></div><br><div style = 'color:#000;'>(This is a system - generated email.Please do not reply to this email.)</div></body></html>";
        }
    }
}
