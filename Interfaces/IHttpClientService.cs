public interface IHttpClientService
{
    Task<T> GetAsync<T>(string route, IDictionary<string, string>? customHeaders = null) where T : ResponseBase, new();

    Task<T> PostAsync<T>(string route, object content, IDictionary<string, string>? customHeaders = null) where T : ResponseBase, new();

    Task<T> PatchAsync<T>(string route, object content, IDictionary<string, string>? customHeaders = null) where T : ResponseBase, new();

    Task<T> DeleteAsync<T>(string route, IDictionary<string, string>? customHeaders = null) where T : ResponseBase, new();
}
