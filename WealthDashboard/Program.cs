using WealthDashboard.Models;
using WealthDashboard.Models.InvestNowManager;
using WealthDashboard.Models.OrderAuthentication;
using WealthDashboard.Models.PrimaryDetailManager;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
});
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".WP.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IInvestNowManager, InvestNowManager>();
builder.Services.AddTransient<IOrderOthenticationManager, OrderOthenticationManager>();
builder.Services.AddTransient<IPrimaryDetailsManager, PrimaryDetailsManager>();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.Configure<Connection>(builder.Configuration.GetSection("ConnectionStrings"));  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
