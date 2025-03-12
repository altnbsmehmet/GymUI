using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void AddAuthorizationHeader()
    {
        var jwtToken = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
        if (!string.IsNullOrEmpty(jwtToken)) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    public async Task<T> GetAsync<T>(string url) where T : ResponseBase, new()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = null;
        try {
            response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        } catch (HttpRequestException ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = success, Message = $"HTTP request failed. Status code: {response?.StatusCode}. Message: {ex.Message}" };
        } catch (Exception ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = false,Message = $"An unexpected error occurred. IsSuccessStatusCode: {response?.IsSuccessStatusCode}. Message: {ex.Message}" };
        }
    }

    public async Task<T> PostAsync<T>(string url, object content) where T : ResponseBase, new()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = null;
        try {
            HttpContent httpContent;
            if (content is MultipartFormDataContent multipartFormDataContent) httpContent = multipartFormDataContent;
            else httpContent = JsonContent.Create(content);
            
            response = await _httpClient.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        } catch (HttpRequestException ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = success, Message = $"HTTP request failed. Status code: {response?.StatusCode}. Message: {ex.Message}" };
        } catch (Exception ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = success, Message = $"An unexpected error occurred. IsSuccessStatusCode: {response?.IsSuccessStatusCode}. Message: {ex.Message}" };
        }
    }

    public async Task<T> PatchAsync<T>(string url, object content) where T : ResponseBase, new()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = null;
        try {
            response = await _httpClient.PatchAsJsonAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        } catch (HttpRequestException ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = success, Message = $"HTTP request failed. Status code: {response?.StatusCode}. Message: {ex.Message}" };
        } catch (Exception ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = false,Message = $"An unexpected error occurred. IsSuccessStatusCode: {response?.IsSuccessStatusCode}. Message: {ex.Message}" };
        }
    }

    public async Task<T> DeleteAsync<T>(string url) where T : ResponseBase, new()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = null;
        try {
            response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        } catch (HttpRequestException ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = success, Message = $"HTTP request failed. Status code: {response?.StatusCode}. Message: {ex.Message}" };
        } catch (Exception ex) {
            var success = response?.IsSuccessStatusCode ?? false;
            return new T { IsSuccess = false,Message = $"An unexpected error occurred. IsSuccessStatusCode: {response?.IsSuccessStatusCode}. Message: {ex.Message}" };
        }
    }
}
