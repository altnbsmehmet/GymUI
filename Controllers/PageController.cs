using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

[Route("")]
public class PageController : Controller
{
    private readonly IPageService _pageService;
    private readonly IHttpClientService _httpClientService;
    public PageController(IPageService pageService, IHttpClientService httpClientService)
    {
        _pageService = pageService;
        _httpClientService = httpClientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetHomePage()
    {
        return View("~/Views/UserPath/Home.cshtml");
    }

    [HttpGet("memberships")]
    public async Task<IActionResult> GetMembershipsPage()
    {   
        var membershipsResponse = await _httpClientService.GetAsync<GetMembershipsResponse>($"membership/getall");

        var viewModel = new MembershipsViewModel();
        viewModel.Memberships = membershipsResponse.Memberships;

        string message = TempData["Message"] as string;
        if (!string.IsNullOrEmpty(message)) viewModel.Message = message;
        return View("~/Views/UserPath/Memberships.cshtml", viewModel);
    }

    [HttpGet("memberships/subscribe/{id}")]
    public async Task<IActionResult> GetMembershipPurchasePage(int id)
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");

        if (authorizationResponse.IsSuccess && authorizationResponse.Role == "Member") {
            var membershipResponse = await _httpClientService.GetAsync<GetMembershipResponse>($"membership/getbyid/{id}");

            var viewModel = new MembershipViewModel();
            viewModel.Membership = membershipResponse.Membership;

            return View("~/Views/UserPath/MembershipPurchase.cshtml", viewModel);
        } else if (authorizationResponse.IsSuccess && authorizationResponse.Role != "Member") {
            TempData["Message"] = "You need to be a member to make a subscription.";
            return RedirectToAction("GetMembershipsPage", "Page");
        }

        TempData["Message"] = "Sign In to make a subscription.";
        return RedirectToAction("GetMembershipsPage", "Page");
    }

    [HttpGet("trainers")]
    public async Task<IActionResult> GetTrainersPage()
    {
        var employeeResponse = await _httpClientService.GetAsync<GetEmployeesResponse>($"employee/getall/Trainer");

        var viewModel = new TrainersViewModel();
        viewModel.Trainers = employeeResponse.Employees;

        return View("~/Views/UserPath/Trainers.cshtml", viewModel);
    }

    [HttpGet("gallery")]
    public async Task<IActionResult> GetGalleryPage()
    {
        var viewModel = new GalleryViewModel();
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
        viewModel.Images = Directory.GetFiles(uploadsFolder)
            .Select(Path.GetFileName)
            .ToList();
        return View("~/Views/UserPath/Gallery.cshtml", viewModel);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfilePage()
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");

        var viewModel = new ProfileViewModel();

        if (authorizationResponse.IsSuccess) {
            var userResponse = await _httpClientService.GetAsync<GetUserResponse>($"user/getcurrentuser");

            if (!userResponse.IsSuccess) {
                viewModel.Message = userResponse.Message;
                return View("~/Views/UserPath/Profile.cshtml", viewModel);
            }

            var subscriptionsResponse = await _httpClientService.GetAsync<GetSubscriptionsResponse>($"subscription/getallbymemberid");
            
            viewModel.User = userResponse.User;
            viewModel.Employee = userResponse.Employee;
            viewModel.Member = userResponse.Member;
            viewModel.Subscriptions = subscriptionsResponse.Subscriptions;

            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message)) viewModel.Message = message;

            return View("~/Views/UserPath/Profile.cshtml", viewModel);
        }

        viewModel.Message = "Sign In to see profile information.";
        return View("~/Views/UserPath/Profile.cshtml", viewModel);
    }

    [HttpGet("adminpanel/home")]
    public async Task<IActionResult> GetAdminHomePage()
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");
        Console.WriteLine($"\n\n[DEBUG] authorizationResponse\n{JsonConvert.SerializeObject(authorizationResponse, Formatting.Indented)}\n\n");
        if (authorizationResponse.IsSuccess && authorizationResponse.Role == "Admin") {
            return View("~/Views/AdminPath/AdminHome.cshtml");            
        }

        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpGet("adminpanel/memberships")]
    public async Task<IActionResult> GetAdminMembershipsPage()
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");
        if (authorizationResponse.IsSuccess && authorizationResponse.Role == "Admin") {
            var membershipsResponse = await _httpClientService.GetAsync<GetMembershipsResponse>($"membership/getall");

            var viewModel = new MembershipsViewModel();
            viewModel.Memberships = membershipsResponse.Memberships;

            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message)) viewModel.Message = message;
            return View("~/Views/AdminPath/AdminMemberships.cshtml", viewModel);            
        }

        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpGet("adminpanel/employees")]
    public async Task<IActionResult> GetAdminEmployeesPage()
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");
        if (authorizationResponse.IsSuccess && authorizationResponse.Role == "Admin") {
            var employeesResponse = await _httpClientService.GetAsync<GetEmployeesResponse>($"employee/getall");

            var viewModel = new EmployeesViewModel();
            viewModel.Employees = employeesResponse.Employees;
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message)) viewModel.Message = message;
            return View("~/Views/AdminPath/AdminEmployees.cshtml", viewModel);            
        }

        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpGet("adminpanel/gallery")]
    public async Task<IActionResult> GetAdminGalleryPage()
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");
        if (authorizationResponse.IsSuccess && authorizationResponse.Role == "Admin") {
            var viewModel = new GalleryViewModel();
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            viewModel.Images = Directory.GetFiles(uploadsFolder)
                .Select(Path.GetFileName)
                .ToList();
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message)) viewModel.Message = message;
            return View("~/Views/AdminPath/AdminGallery.cshtml", viewModel);
        }

        return RedirectToAction("GetHomePage", "Page");
    }

    [HttpGet("adminpanel/profile")]
    public async Task<IActionResult> GetAdminProfilePage()
    {
        var authorizationResponse = await _httpClientService.GetAsync<UserAuthorizationResponse>($"user/authorizeuser");
        if (authorizationResponse.IsSuccess && authorizationResponse.Role == "Admin") {
            var userResponse = await _httpClientService.GetAsync<GetUserResponse>($"user/getcurrentuser");

            var viewModel = new ProfileViewModel();

            if (!userResponse.IsSuccess) {
                viewModel.Message = userResponse.Message;
                return View("~/Views/AdminPath/AdminProfile.cshtml", viewModel);
            }

            var subscriptionsResponse = await _httpClientService.GetAsync<GetSubscriptionsResponse>($"subscription/getallbymemberid");
            
            viewModel.User = userResponse.User;
            viewModel.Employee = userResponse.Employee;
            viewModel.Member = userResponse.Member;
            viewModel.Subscriptions = subscriptionsResponse.Subscriptions;

            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message)) viewModel.Message = message;

            return View("~/Views/AdminPath/AdminProfile.cshtml", viewModel);
        }

        return RedirectToAction("GetHomePage", "Page");
    }


}