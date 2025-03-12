public interface IHttpClientService
{
    Task<T> GetAsync<T>(string url) where T : ResponseBase, new();
    Task<T> PostAsync<T>(string url, object content) where T : ResponseBase, new();
    Task<T> PatchAsync<T>(string url, object content) where T : ResponseBase, new();
    Task<T> DeleteAsync<T>(string url) where T : ResponseBase, new();
}