using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.WebHost.UseUrls("http://0.0.0.0:5420");

EnvironmentVariables.ApiUrl = Environment.GetEnvironmentVariable("API_URL") ?? builder.Configuration["ApiSettings:LocalhostUrl"] ?? "";
EnvironmentVariables.FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? builder.Configuration["FrontendUrl"] ?? "";
EnvironmentVariables.SeqUrl = Environment.GetEnvironmentVariable("SEQ_URL") ?? builder.Configuration["SeqUrl"] ?? "";
EnvironmentVariables.IsCookieSecure = bool.Parse(Environment.GetEnvironmentVariable("COOKIE_SECURE") ?? builder.Configuration["CookieSettings:Secure"] ?? "false");
EnvironmentVariables.CookieSameSiteMode = Environment.GetEnvironmentVariable("COOKIE_SAME_SITE_MODE") ?? builder.Configuration["CookieSettings:SameSiteMode"] ?? "Lax";
Console.WriteLine($"\n\n\tEnvironmental Variables\n" +
    $"ApiUrl --> {EnvironmentVariables.ApiUrl}\n" +
    $"FrontendUrl --> {EnvironmentVariables.FrontendUrl}\n" +
    $"SeqUrl --> {EnvironmentVariables.SeqUrl}\n" +
    $"isCookieSecure --> {EnvironmentVariables.IsCookieSecure}\n" +
    $"CookieSameSiteMode --> {EnvironmentVariables.CookieSameSiteMode}\n\n");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(EnvironmentVariables.SeqUrl)
    .Enrich.FromLogContext()
    .CreateLogger();

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

// Seq Testing
Log.Information("UI Test Information.");
Log.Warning("UI Test Warning.");
Log.Error("UI Test Error.");

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();