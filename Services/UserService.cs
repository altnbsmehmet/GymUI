public class UserService : IUserService
{
    private readonly IHttpClientService _httpClientService;
    public UserService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

}
