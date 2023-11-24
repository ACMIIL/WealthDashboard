using Microsoft.Extensions.Configuration;

namespace WealthDashboard.Models
{
    public class GlobalVariable
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        public static string DbConnHercules { get { return Configuration.GetConnectionString("conhercules"); } }
    }
}
