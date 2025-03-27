using Refit;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public HttpClientService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = EnvironmentVariables.ApiUrl;
    }

    // private methods
    private string BuildUrl(string endpoint) => $"{_apiUrl}{endpoint}";

    private (string Controller, string Endpoint, string Id) ParseRoute(string route)
    {
        var routeList = route.Split('/').ToList();
        return (
            routeList.ElementAtOrDefault(0) ?? "", 
            routeList.ElementAtOrDefault(1) ?? "", 
            routeList.ElementAtOrDefault(2) ?? ""
        );
    }

    private IApiService CreateApiService()
    {
        var client = new HttpClient { BaseAddress = new Uri(_apiUrl) };
        return RestService.For<IApiService>(client);
    }

    private string GetAuthorizationHeader()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
    }

    // public methods
    public async Task<T> GetAsync<T>(RequestModelBase Request) where T : ResponseBase, new()
    {
        var jwtToken = GetAuthorizationHeader();
        var authorization = jwtToken != null ? $"Bearer {jwtToken}" : null;

        var (controller, endpoint, id) = ParseRoute(Request.Route);

        var apiService = CreateApiService();
        return await apiService.GetAsync<T>(controller, endpoint, id, authorization, Request.Headers);
    }

    public async Task<T> PostAsync<T>(RequestModelBase Request) where T : ResponseBase, new()
    {
        var jwtToken = GetAuthorizationHeader();
        var authorization = jwtToken != null ? $"Bearer {jwtToken}" : null;

        var (controller, endpoint, id) = ParseRoute(Request.Route);

        var apiService = CreateApiService();
        return await apiService.PostAsync<T>(controller, endpoint, id, Request.Body, authorization, Request.Headers);
    }

    public async Task<T> PatchAsync<T>(RequestModelBase Request) where T : ResponseBase, new()
    {
        var jwtToken = GetAuthorizationHeader();
        var authorization = jwtToken != null ? $"Bearer {jwtToken}" : null;

        var (controller, endpoint, id) = ParseRoute(Request.Route);

        var apiService = CreateApiService();
        return await apiService.PatchAsync<T>(controller, endpoint, id, Request.Body, authorization, Request.Headers);
    }

    public async Task<T> DeleteAsync<T>(RequestModelBase Request) where T : ResponseBase, new()
    {
        var jwtToken = GetAuthorizationHeader();
        var authorization = jwtToken != null ? $"Bearer {jwtToken}" : null;

        var (controller, endpoint, id) = ParseRoute(Request.Route);

        var apiService = CreateApiService();
        return await apiService.DeleteAsync<T>(controller, endpoint, id, authorization, Request.Headers);
    }

}