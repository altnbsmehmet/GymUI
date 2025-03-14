var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.WebHost.UseUrls("http://0.0.0.0:5420");

var _apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? builder.Configuration["ApiSettings:LocalhostUrl"];
var _isCookieSecure = bool.Parse(Environment.GetEnvironmentVariable("COOKIE_SECURE") ?? builder.Configuration["CookieSettings:Secure"]);
var _cookieSameSiteMode = Environment.GetEnvironmentVariable("COOKIE_SAME_SITE_MODE") ?? builder.Configuration["CookieSettings:SameSiteMode"];
Console.WriteLine($"\n\n\tEnvironmental Variables\nApiUrl --> {_apiUrl}\nisCookieSecure --> {_isCookieSecure}\n CookieSameSiteMode --> {_cookieSameSiteMode}\n\n");


builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

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