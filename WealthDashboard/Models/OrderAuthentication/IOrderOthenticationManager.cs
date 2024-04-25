namespace WealthDashboard.Models.OrderAuthentication
{
    public interface IOrderOthenticationManager
    {
        Task<BSEOrderResult> InsertBSECraeteOrder(InsertBseOrderModel insertBseOrderModel);
        Task<PaymentrequestModel> Paymentrequest(string payeeBankAccountNo, string payeeBankID, string currencyCode, string payeeLoginID, string PayAmount, string MFTransactionID, string RequestSource);
    }
}
