using System.Net;

namespace WealthDashboard.Models
{
    public class ResultModel
    {
        public HttpStatusCode Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
