using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.CVLKRAManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.DigioManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.LoginManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.NomineeManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.PDFManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SegmentManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.SelfieManager;

namespace WealthDashboard.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddEkycServices(this IServiceCollection services)
        {
            services.AddTransient<ILoginManager, LoginManager>();
            services.AddTransient<IClientRegistrationManager, ClientRegistrationManager>();
            services.AddTransient<ISegmentManager, SegmentManager>();
            services.AddTransient<IDigioManagerModel, DigioManagerModel>();
            services.AddTransient<ICVLKRADetailsManager, CVLKRADetailsManager>();
            services.AddTransient<INomineeManager, NomineeManager>();
            services.AddTransient<IPDFModel, PDFManager>();
            services.AddTransient<ISelfieManagerModel, SelfieManagerModel>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
