using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class KeepAliveService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<KeepAliveService> _logger;
    private string FrontendUrl = EnvironmentVariables.FrontendUrl;
    private string ApiUrl = $"{EnvironmentVariables.ApiUrl}membership/getall";
    public KeepAliveService(HttpClient httpClient, ILogger<KeepAliveService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested){
            try {
                var uiResponse = await _httpClient.GetAsync(FrontendUrl, stoppingToken);
                Console.WriteLine($"\n\n[KeepAlive] UI Ping - Status: {uiResponse.StatusCode} - {DateTime.UtcNow}\n\n");

                var apiResponse = await _httpClient.GetAsync(ApiUrl, stoppingToken);
                Console.WriteLine($"\n\n[KeepAlive] UI Ping - Status: {apiResponse.StatusCode} - {DateTime.UtcNow}\n\n");
            } catch (Exception ex) {
                Console.WriteLine($"\n\n[KeepAlive] Error: {ex.Message} - {DateTime.UtcNow}\n\n");
            }

            await Task.Delay(TimeSpan.FromMinutes(12), stoppingToken);
        }
    }
}
