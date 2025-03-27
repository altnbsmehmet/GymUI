public interface IHttpClientService
{
    Task<T> GetAsync<T>(RequestModelBase Request) where T : ResponseBase, new();

    Task<T> PostAsync<T>(RequestModelBase Request) where T : ResponseBase, new();

    Task<T> PatchAsync<T>(RequestModelBase Request) where T : ResponseBase, new();

    Task<T> DeleteAsync<T>(RequestModelBase Request) where T : ResponseBase, new();
}
