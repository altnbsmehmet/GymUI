using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("")]
public class PageController : Controller
{
    private readonly HttpClient _httpClient;

    public PageController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("signin")]
    public async Task<IActionResult> GetSignInPage()
    {
        var viewModel = new ViewModelBase();
        return View("SignIn", viewModel);
    }

    [HttpGet("signup")]
    public async Task<IActionResult> GetSignUpPage()
    {
        var viewModel = new ViewModelBase();
        return View("~/Views/UserPath/SignUp.cshtml", viewModel);
    }

    [HttpGet("home")]
    public async Task<IActionResult> GetHomePage()
    {
        return View("~/Views/UserPath/Home.cshtml");
    }

    [HttpGet("memberships")]
    public async Task<IActionResult> GetMembershipsPage()
    {
        return View("~/Views/UserPath/Memberships.cshtml");
    }

    [HttpGet("trainers")]
    public async Task<IActionResult> GetTrainersPage()
    {
        var response = await _httpClient.GetAsync("http://localhost:5410/api/employee/getall/Trainer");
        var result = await response.Content.ReadFromJsonAsync<GetEmployeesResponse>();

        var viewModel = new TrainersViewModel();
        viewModel.Trainers = result.Employees;

        return View("~/Views/UserPath/Trainers.cshtml", viewModel);
    }

    [HttpGet("gallery")]
    public async Task<IActionResult> GetGalleryPage()
    {
        return View("~/Views/UserPath/Gallery.cshtml");
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfilePage()
    {
        var jwt = HttpContext.Request.Cookies["jwt"];
        var viewModel = new UserInfoViewModel();

        if (!string.IsNullOrEmpty(jwt)) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var userResponse = await _httpClient.GetAsync("http://localhost:5410/api/user/getcurrentuser");
            var userResult = await userResponse.Content.ReadFromJsonAsync<GetUserResponse>();
            
            viewModel.User = userResult.User;
            viewModel.Employee = userResult.Employee;

            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message)) viewModel.Message = message;            

            return View("~/Views/UserPath/Profile.cshtml", viewModel);
        }

        return View("~/Views/UserPath/Profile.cshtml", viewModel);
    }

}