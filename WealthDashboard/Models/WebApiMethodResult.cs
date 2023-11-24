namespace WealthDashboard.Models
{
    public enum enResultStatuslogin
    {
        NotSet,
        Error,
        Exception,
        Success

    }


    public class WebApiMethodResult
    {
        public enResultStatuslogin Status { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        public void SetStatus(enResultStatuslogin status, object data, string message = null)
        {
            Status = status;
            Data = data;
            Message = message;
        }
    }

    public class Errorlogin
    {
        public string ErrorDescription { get; set; }
        public int? ErrorID { get; set; }
        public object OtherErrorDetails { get; set; }
    }
    public class VerificationModel
    {
        public string EmailId { get; set; }
        public string Mobileno { get; set; }
        public int id { get; set; }
    }

}
    

