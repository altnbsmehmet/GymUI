using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


[Route("admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly IHttpClientService _httpClientService;
    public AdminController(IAdminService adminService, IHttpClientService httpClientService)
    {
        _adminService = adminService;
        _httpClientService = httpClientService;
    }

    [HttpPost("membership/create")]
    public async Task<IActionResult> CreateMembership()
    {
        return RedirectToAction("GetAdminMembershipsPage", "Page");
    }

    [HttpPost("membership/update")]
    public async Task<IActionResult> UpdateMembership()
    {
        return RedirectToAction("GetAdminMembershipsPage", "Page");
    }

    [HttpGet("membership/toggleactivation/{id}")]
    public async Task<IActionResult> DeactivateMembership(int id)
    {
        var deactivationResponse = await _httpClientService.GetAsync<ResponseBase>($"http://localhost:5410/api/membership/toggleactivation/{id}");
        TempData["Message"] = deactivationResponse.Message;
        return RedirectToAction("GetAdminMembershipsPage", "Page");
    }

    [HttpPost("employee/create")]
    public async Task<IActionResult> CreateEmployee(SignUpDto signUpDto)
    {
        try {
            signUpDto.Role = "Employee";

            var formData = new MultipartFormDataContent {
                { new StringContent(signUpDto.UserName), "UserName" },
                { new StringContent(signUpDto.Password), "Password" },
                { new StringContent(signUpDto.FirstName), "FirstName" },
                { new StringContent(signUpDto.LastName), "LastName" },
                { new StringContent(signUpDto.Role), "Role" },
                { new StringContent(signUpDto.Position), "Position" },
                { new StringContent(signUpDto.Salary.ToString()), "Salary" }
            };

            if (signUpDto.ProfilePhoto != null){
                var fileContent = new StreamContent(signUpDto.ProfilePhoto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(signUpDto.ProfilePhoto.ContentType);
                formData.Add(fileContent, "ProfilePhoto", signUpDto.ProfilePhoto.FileName);
            }

            var userSignUpResponse = await _httpClientService.PostAsync<ResponseBase>("http://localhost:5410/api/user/signup", formData);

            TempData["Message"] = userSignUpResponse.Message;
            return RedirectToAction("GetAdminEmployeesPage", "Page");
        } catch (Exception e) {
            TempData["Message"] = e.Message;
            return RedirectToAction("GetAdminEmployeesPage", "Page");
        }
    }

    [HttpPost("employee/update")]
    public async Task<IActionResult> UpdateEmployee(SignUpDto signUpDto, string userId)
    {
        signUpDto.Role = "Employee";
        var userPatchResponse = await _httpClientService.PatchAsync<ResponseBase>($"http://localhost:5410/api/user/update/{userId}", signUpDto);

        TempData["Message"] = userPatchResponse.Message;
        return RedirectToAction("GetAdminEmployeesPage", "Page");
    }
    
    [HttpPost("employee/delete")]
    public async Task<IActionResult> DeleteEmployee(string userId)
    {
        var userDeletionResponse = await _httpClientService.DeleteAsync<ResponseBase>($"http://localhost:5410/api/user/delete/{userId}");

        TempData["Message"] = userDeletionResponse.Message;
        return RedirectToAction("GetAdminEmployeesPage", "Page");
    }

    [HttpPost("gallery/create")]
    public async Task<IActionResult> CreateGalleryImage(IFormFile image)
    {
        if (image != null && image.Length  != 0) {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await image.CopyToAsync(stream);
            }
            TempData["Message"] = "Image added to gallery.";
            return RedirectToAction("GetAdminGalleryPage", "Page");
        }

        TempData["Message"] = "Choose an image!";
        return RedirectToAction("GetAdminGalleryPage", "Page");
    }

    [HttpPost("gallery/delete")]
    public IActionResult DeleteGalleryImage(string imageName)
    {
        if (string.IsNullOrEmpty(imageName)) {
            TempData["Message"] = "Image name is not valid!";
            return RedirectToAction("GetAdminGalleryPage", "Page");
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", imageName);

        if (System.IO.File.Exists(filePath)) {
            System.IO.File.Delete(filePath);
        }

        TempData["Message"] = "Image deleted from gallery.";
        return RedirectToAction("GetAdminGalleryPage", "Page");
    }

}