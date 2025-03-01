using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("user")]
public class UserController : Controller
{
    private readonly HttpClient _httpClient;

    public UserController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        signUpDto.Role = "Member";
        Console.WriteLine($"\nSignUpDto --> {JsonConvert.SerializeObject(signUpDto)}\n");
        var jsonContent = new StringContent(JsonConvert.SerializeObject(signUpDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5410/api/user/signup", jsonContent);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseBase>(responseContent);

        Console.WriteLine($"\nResult --> {JsonConvert.SerializeObject(result)}\n");

        if (!result.IsSuccess) {
            var viewModel = new ViewModelBase();
            viewModel.Message = result.Message;
            return View("~/Views/UserPath/SignUp.cshtml", viewModel);
        }

        return View("SignIn");
    }

}