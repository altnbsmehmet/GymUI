var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.WebHost.UseUrls("http://0.0.0.0:5420");

var apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? builder.Configuration["ApiSettings:LocalhostUrl"];
Console.WriteLine($"\n\n{apiUrl}\n\n");

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