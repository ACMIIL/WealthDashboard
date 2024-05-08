namespace WealthDashboard.Models.InvestNowManager
{
    public interface IInvestNowManager
    {
        Task<ResultModel> InvestNowInsert(InvestNowDetailInsertModel investNowDetailInsertModel);
        Task<ResultModel> generatesrno(string ucc);
        Task<ResultModel> SendPaymentLinkOnEmail(string PaymentURL);
    }
}
