using Microsoft.Extensions.FileProviders;
using Serilog;
using WealthDashboard.Configuration;
using WealthDashboard.Models;
using WealthDashboard.Models.InvestNowManager;
using WealthDashboard.Models.OrderAuthentication;
using WealthDashboard.Models.PrimaryDetailManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IInvestNowManager, InvestNowManager>();
builder.Services.AddTransient<IOrderOthenticationManager, OrderOthenticationManager>();
builder.Services.AddTransient<IPrimaryDetailsManager, PrimaryDetailsManager>();
builder.Services.AddTransient<CommanModel>();
//Added By MF_journey
builder.Services.AddEkycServices();
builder.Services.Configure<Appsetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.Configure<Connection>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<Appsetting>(builder.Configuration.GetSection("DashboardAPI"));
Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(@"D:\TWCEKYC_CLogs\mylog.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".WP.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"D:\LandingPageImages\Sliders"),
    RequestPath = "/LandingPageImages/Sliders"
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
      name: "EKYC_MFJourney",
      areaName: "EKYC_MFJourney",
      pattern: "{area:exists}/{controller}/{action}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


    endpoints.MapAreaControllerRoute(
           name: "WP_Registration",
           areaName: "WP_Registration",
           pattern: "{area:exists}/{controller}/{action}/{id?}"
         );
    endpoints.MapAreaControllerRoute(
          name: "Dashboard",
          areaName: "Dashboard",
          pattern: "{area:exists}/{controller}/{action}/{id?}"
        );
});
//

app.Run();
