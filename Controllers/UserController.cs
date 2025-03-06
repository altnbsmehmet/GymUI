using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
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

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5410/api/user/signup", signUpDto);
        var result = await response.Content.ReadFromJsonAsync<ResponseBase>();

        var viewModel = new ViewModelBase();

        if (!result.IsSuccess) {
            viewModel.Message = result.Message;
            return View("~/Views/UserPath/SignUp.cshtml", viewModel);
        }

        return RedirectToAction("GetSignInPage", "Page");
    }

    /*[HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInDto signInDto)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5410/api/user/signin", signInDto);
        var result = await response.Content.ReadFromJsonAsync<ResponseBase>();

        if (!result.IsSuccess) {
            var viewModel = new ViewModelBase();
            viewModel.Message = result.Message;
            return View("~/Views/UserPath/SignUp.cshtml", viewModel);
        }

        return RedirectToAction("GetHomePage", "Page");
    }*/

    [HttpPost("update")]
    public async Task<IActionResult> Update(SignUpDto signUpDto)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["jwt"]);

        var userResponse = await _httpClient.GetAsync("http://localhost:5410/api/user/getcurrentuser");
        var userResult = await userResponse.Content.ReadFromJsonAsync<GetUserResponse>();

        Console.WriteLine($"\nUserResult\n{JsonConvert.SerializeObject(userResult, Formatting.Indented)}\n");
        var userId = userResult.User.Id;

        var response = await _httpClient.PatchAsJsonAsync($"http://localhost:5410/api/user/update/{userId}", signUpDto);
        var result = await response.Content.ReadFromJsonAsync<ResponseBase>();
        Console.WriteLine($"\nUserResult\n{JsonConvert.SerializeObject(result, Formatting.Indented)}\n");

        TempData["Message"] = result.Message;
        return RedirectToAction("GetProfilePage", "Page");
    }

}