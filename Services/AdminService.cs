public class AdminService : IAdminService
{
    private readonly IHttpClientService _httpClientService;
    public AdminService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

}
