using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;


namespace Controllers
{

    [Route("/")]
    public class UIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var client = _httpClientFactory.CreateClient();
            var targetUrl = "http://gym_api:5410/api";

            var response = await client.GetAsync(targetUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string>(responseContent);

            return Ok(result);
        }

    }
}