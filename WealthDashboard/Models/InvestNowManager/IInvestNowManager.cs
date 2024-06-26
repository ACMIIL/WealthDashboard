﻿namespace WealthDashboard.Models.InvestNowManager
{
    public interface IInvestNowManager
    {
        Task<ResultModel> InvestNowInsert(InvestNowDetailInsertModel investNowDetailInsertModel);
        Task<ResultModel> RedeemInsert(InsertRedeemOrderModel insertRedeemOrderModel);
        Task<ResultModel> SWPInsert(SwpOrderModel swpOrderModel);
        Task<ResultModel> generatesrno(string ucc);
        Task<ResultModel> SendPaymentLinkOnEmail(string PaymentURL);
    }
}
