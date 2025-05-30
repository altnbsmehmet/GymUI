using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IHttpClientService _httpClientService;
    private readonly bool _isCookieSecure;
    private readonly string _cookieSameSiteMode;
    public UserController(IUserService userService, IHttpClientService httpClientService, IConfiguration configuration)
    {
        _userService = userService;
        _httpClientService = httpClientService;
        _isCookieSecure = EnvironmentVariables.IsCookieSecure;
        _cookieSameSiteMode = EnvironmentVariables.CookieSameSiteMode;
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

            var userSignUpResponse = await _httpClientService.PostAsync<ResponseBase>(new RequestModelBase {
                Route = $"user/signup",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "multipart/form-data" },
                },
                Body = formData,
            });
            if (!userSignUpResponse.IsSuccess) {
                viewModel.Message = userSignUpResponse.Message;
                return View("~/Views/UserPath/Home.cshtml", viewModel);                
            }

            viewModel.Message = "Signed up successfully, now you can signin.";
            return View("~/Views/UserPath/Home.cshtml", viewModel);
        } catch (Exception e) {
            viewModel.Message = e.Message;
            return View("~/Views/UserPath/Home.cshtml", viewModel);
        }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInDto signInDto)
    {
        var signInResponse = await _httpClientService.PostAsync<SignInResponse>(new RequestModelBase {
            Route = $"user/signin",
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/json" },
            },
            Body = signInDto,
        });

        if (!signInResponse.IsSuccess) {
            var viewModel = new ViewModelBase();
            viewModel.Message = signInResponse.Message;
            return View("~/Views/UserPath/Home.cshtml", viewModel);
        }

        Response.Cookies.Append("jwt", signInResponse.Token, new CookieOptions {
            HttpOnly = true,
            Secure = _isCookieSecure,
            SameSite = Enum.Parse<SameSiteMode>(_cookieSameSiteMode, true),
            Expires = DateTime.UtcNow.AddHours(1),
        });

        if (signInResponse.Role == "Admin") return RedirectToAction("GetAdminHomePage", "Page");

        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpGet("signout")]
    public async Task<IActionResult> SignOut()
    {
        var signOutResponse = await _httpClientService.GetAsync<ResponseBase>(new RequestModelBase { Route = $"user/signout" });

        Response.Cookies.Delete("jwt");
        
        TempData["Message"] = signOutResponse.Message;
        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(SignUpDto signUpDto)
    {
        var userResponse = await _httpClientService.GetAsync<GetUserResponse>(new RequestModelBase { Route = $"user/getcurrentuser" });

        var userId = userResponse.User.Id;

        signUpDto.Role = userResponse.User.Role;
        
        var userPatchResponse = await _httpClientService.PatchAsync<ResponseBase>(new RequestModelBase {
            Route = $"user/update/{userId}",
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/json" },
            },
            Body = signUpDto,
        });

        TempData["Message"] = userPatchResponse.Message;
        return RedirectToAction("GetProfilePage", "Page");
    }

}