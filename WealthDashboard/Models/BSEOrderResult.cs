namespace WealthDashboard.Models
{
    public class BSEOrderResult
    {
        public string EmpBaOrdLogId { get; set; }
        public string EmpBaOrdOrderId { get; set; }
    }

    //For Dashboard
    public class Result
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public object data1 { get; set; }
        public object data2 { get; set; }
    }

}

