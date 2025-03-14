using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;
    public HttpClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? configuration["ApiSettings:LocalhostUrl"];
    }

    private string BuildUrl(string endpoint) => $"{_apiUrl}{endpoint}";

    private void AddAuthorizationHeader()
    {
        var jwtToken = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
        Console.WriteLine($"\n\n[DEBUG] Çerezden Alınan JWT: {jwtToken ?? "YOK"}\n\n");
        if (!string.IsNullOrEmpty(jwtToken)) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    public async Task<T> GetAsync<T>(string endpoint) where T : ResponseBase, new()
    {
        return await MakeRequestAsync<T>(() => _httpClient.GetAsync(BuildUrl(endpoint)));
    }

    public async Task<T> PostAsync<T>(string endpoint, object content) where T : ResponseBase, new()
    {
        HttpContent httpContent = content is MultipartFormDataContent formData ? formData : JsonContent.Create(content);
        return await MakeRequestAsync<T>(() => _httpClient.PostAsync(BuildUrl(endpoint), httpContent));
    }

    public async Task<T> PatchAsync<T>(string endpoint, object content) where T : ResponseBase, new()
    {
        return await MakeRequestAsync<T>(() => _httpClient.PatchAsJsonAsync(BuildUrl(endpoint), content));
    }

    public async Task<T> DeleteAsync<T>(string endpoint) where T : ResponseBase, new()
    {
        return await MakeRequestAsync<T>(() => _httpClient.DeleteAsync(BuildUrl(endpoint)));
    }

    // modular request maker for all type of request methods
    private async Task<T> MakeRequestAsync<T>(Func<Task<HttpResponseMessage>> requestFunc) where T : ResponseBase, new()
    {
        AddAuthorizationHeader();
        Console.WriteLine($"\n\n[DEBUG] Gönderilen Authorization Header: {_httpClient.DefaultRequestHeaders.Authorization?.ToString() ?? "YOK"}\n\n");
        HttpResponseMessage response = null;
        try
        {
            response = await requestFunc();
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (HttpRequestException ex)
        {
            return new T { IsSuccess = response?.IsSuccessStatusCode ?? false, Message = $"HTTP request failed: {response?.StatusCode}. {ex.Message}" };
        }
        catch (Exception ex)
        {
            return new T { IsSuccess = false, Message = $"Unexpected error: {ex.Message}" };
        }
    }
}