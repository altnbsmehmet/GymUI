public class SubscriptionService : ISubscriptionService
{
    private readonly IHttpClientService _httpClientService;
    public SubscriptionService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

}
