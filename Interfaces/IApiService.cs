using Refit;

public interface IApiService
{
    [Get("/{controller}/{endpoint}/{id}")]
    Task<T> GetAsync<T>(string controller, string endpoint, string id, [Header("Authorization")] string? authorization = null, [HeaderCollection] IDictionary<string, string>? customHeaders = null);

    [Post("/{controller}/{endpoint}")]
    Task<T> PostAsync<T>(string controller, string endpoint, string id, [Body] object content, [Header("Authorization")] string? authorization = null, [HeaderCollection] IDictionary<string, string>? customHeaders = null);

    [Patch("/{controller}/{endpoint}")]
    Task<T> PatchAsync<T>(string controller, string endpoint, string id, [Body] object content, [Header("Authorization")] string? authorization = null, [HeaderCollection] IDictionary<string, string>? customHeaders = null);

    [Delete("/{controller}/{endpoint}")]
    Task<T> DeleteAsync<T>(string controller, string endpoint, string id, [Header("Authorization")] string? authorization = null, [HeaderCollection] IDictionary<string, string>? customHeaders = null);
}