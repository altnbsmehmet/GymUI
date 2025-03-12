using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IHttpClientService _httpClientService;
    public UserController(IUserService userService, IHttpClientService httpClientService)
    {
        _userService = userService;
        _httpClientService = httpClientService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        var viewModel = new ViewModelBase();
        try {
            signUpDto.Role = "Member";

            var formData = new MultipartFormDataContent {
                { new StringContent(signUpDto.UserName), "UserName" },
                { new StringContent(signUpDto.Password), "Password" },
                { new StringContent(signUpDto.FirstName), "FirstName" },
                { new StringContent(signUpDto.LastName), "LastName" },
                { new StringContent(signUpDto.Role), "Role" }
            };

            if (signUpDto.ProfilePhoto != null){
                var fileContent = new StreamContent(signUpDto.ProfilePhoto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(signUpDto.ProfilePhoto.ContentType);
                formData.Add(fileContent, "ProfilePhoto", signUpDto.ProfilePhoto.FileName);
            }

            var userSignUpResponse = await _httpClientService.PostAsync<ResponseBase>("http://localhost:5410/api/user/signup", formData);

            if (!userSignUpResponse.IsSuccess) {
                viewModel.Message = userSignUpResponse.Message;
                return View("~/Views/UserPath/SignUp.cshtml", viewModel);
            }

            return RedirectToAction("GetSignInPage", "Page");
        } catch (Exception e) {
            viewModel.Message = e.Message;
            return View("~/Views/UserPath/SignUp.cshtml", viewModel);
        }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInDto signInDto)
    {
        var signInResponse = await _httpClientService.PostAsync<SignInResponse>("http://localhost:5410/api/user/signin", signInDto);

        if (!signInResponse.IsSuccess) {
            var viewModel = new ViewModelBase();
            viewModel.Message = signInResponse.Message;
            return View("SignIn", viewModel);
        }

        Response.Cookies.Append("jwt", signInResponse.Token, new CookieOptions {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(1),
        });

        if (signInResponse.Role == "Admin") return RedirectToAction("GetAdminHomePage", "Page");

        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpGet("signout")]
    public async Task<IActionResult> SignOut()
    {
        var signOutResponse = await _httpClientService.GetAsync<ResponseBase>("http://localhost:5410/api/user/signout");

        Response.Cookies.Delete("jwt");
        
        TempData["Message"] = signOutResponse.Message;
        return RedirectToAction("GetSignInPage", "Page");
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(SignUpDto signUpDto)
    {
        var userResponse = await _httpClientService.GetAsync<GetUserResponse>("http://localhost:5410/api/user/getcurrentuser");

        var userId = userResponse.User.Id;

        signUpDto.Role = "Member";
        
        var userPatchResponse = await _httpClientService.PatchAsync<ResponseBase>($"http://localhost:5410/api/user/update/{userId}", signUpDto);

        TempData["Message"] = userPatchResponse.Message;
        return RedirectToAction("GetProfilePage", "Page");
    }

}