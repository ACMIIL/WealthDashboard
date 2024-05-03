using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.LoginManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager;

namespace WealthDashboard.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddEkycServices(this IServiceCollection services)
        {
            services.AddTransient<ILoginManager, LoginManager>();
            services.AddTransient<IClientRegistrationManager, ClientRegistrationManager>();
            services.AddTransient<ISegmentManager, SegmentManager>();

            //services.AddTransient<IDigioManagerModel, DigioManagerModel>();

            //services.AddTransient<ISelfieManagerModel, SelfieManagerModel>();
            //services.AddTransient<IDigioManagerModel, DigioManagerModel>();
            //services.AddTransient<INomineeManager, NomineeManager>();

            //services.AddTransient<IPDFModel, PDFManager>();         

            //services.AddTransient<ICVLKRADetailsManager, CVLKRADetailsManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
