namespace WealthDashboard.Models.PrimaryDetailManager
{
    public interface IPrimaryDetailsManager
    {
        Task<ClientDetailsModel> GetPrimaryDetails(string UCC);
    }
}
