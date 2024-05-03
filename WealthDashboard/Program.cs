using Serilog;
using WealthDashboard.Areas.EKYC_MFJourney.Models.ClientRegistrationManager;
using WealthDashboard.Areas.EKYC_MFJourney.Models.LoginManager;
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
//Added By MF_journey
builder.Services.AddEkycServices();

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.Configure<Connection>(builder.Configuration.GetSection("ConnectionStrings"));
Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(@"D:\TWCEKYC_CLogs\mylog.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(

      name: "EKYC_MFJourney",
      areaName: "EKYC_MFJourney",
      pattern: "{area:exists}/{controller}/{action}/{id?}"
    );

    //endpoints.MapControllerRoute(
    //  name: "EKYC_MFJourney",
    //  pattern: "{area:exists}/{controller}/{action}/{id?}"
    //);
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");
});



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
