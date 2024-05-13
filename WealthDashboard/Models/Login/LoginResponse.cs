namespace WealthDashboard.Models.Login
{
    public class LoginResponse
    {
        public int statusCode { get; set; }
        public string?  message { get; set; } = string.Empty;         

        public bool isError { get; set; }
        public string? data { get; set; }
    }

    public class UserData
    {
        public string AadhaarCardNo { get; set; }
        public string PersonalID { get; set; }
        public string Userid { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string MiddleName { get; set; }
        public object Status_Type { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public object Pancard { get; set; }
        public string Declaration { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string AgentSrno { get; set; }
        public string Mobile { get; set; }
        public string FirstName { get; set; }


    }


}
