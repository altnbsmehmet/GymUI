using System.Net.Http.Headers;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

[Route("subscription")]
public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IHttpClientService _httpClientService;
    public SubscriptionController(ISubscriptionService subscriptionService, IHttpClientService httpClientService)
    {
        _subscriptionService = subscriptionService;
        _httpClientService = httpClientService;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(int membershipId, int membershipDuration)
    {
        var subscriptionDto = new SubscriptionDto {
            MembershipId = membershipId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(membershipDuration),
            Status = "Active"
        };

        var subscriptionResponse = await _httpClientService.PostAsync<ResponseBase>(new RequestModelBase {
            Route = $"subscription/create/{membershipId}",
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/json" }
            },
            Body = subscriptionDto,
        });

        if (subscriptionResponse.IsSuccess) {
            return RedirectToAction("GetProfilePage", "Page");
        }

        var viewModel = new ViewModelBase();
        viewModel.Message = subscriptionResponse.Message;
        return View("~/Views/UserPath/MembershipPurchase.cshtml", viewModel);
    }

    [HttpGet("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var subscriptionDeletionRespons = await _httpClientService.DeleteAsync<ResponseBase>(new RequestModelBase { Route = $"subscription/delete/{id}" });

        TempData["Message"] = subscriptionDeletionRespons.Message;
        return RedirectToAction("GetProfilePage", "Page");
    }
}