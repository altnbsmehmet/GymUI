using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("")]
public class PageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PageController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("signin")]
    public async Task<IActionResult> GetSignInPage()
    {
        return View("SignIn");
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
        return View("~/Views/UserPath/Trainers.cshtml");
    }

    [HttpGet("gallery")]
    public async Task<IActionResult> GetGalleryPage()
    {
        return View("~/Views/UserPath/Gallery.cshtml");
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfilePage()
    {
        return View("~/Views/UserPath/Profile.cshtml");
    }

}