var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.WebHost.UseUrls("http://0.0.0.0:5420");

EnvironmentVariables.ApiUrl = Environment.GetEnvironmentVariable("API_URL") ?? builder.Configuration["ApiSettings:LocalhostUrl"] ?? "";
EnvironmentVariables.FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? builder.Configuration["FrontendUrl"] ?? "";
EnvironmentVariables.IsCookieSecure = bool.Parse(Environment.GetEnvironmentVariable("COOKIE_SECURE") ?? builder.Configuration["CookieSettings:Secure"] ?? "false");
EnvironmentVariables.CookieSameSiteMode = Environment.GetEnvironmentVariable("COOKIE_SAME_SITE_MODE") ?? builder.Configuration["CookieSettings:SameSiteMode"] ?? "Lax";
Console.WriteLine($"\n\n\tEnvironmental Variables\n" +
    $"ApiUrl --> {EnvironmentVariables.ApiUrl}\n" +
    $"isCookieSecure --> {EnvironmentVariables.IsCookieSecure}\n" +
    $"CookieSameSiteMode --> {EnvironmentVariables.CookieSameSiteMode}\n\n");


builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddHostedService<KeepAliveService>();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();