using Microsoft.AspNetCore.Mvc;
using FinanceTracker.Application.Services;

namespace FinanceTracker.Web.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var summary = await _dashboardService.GetDashboardSummaryAsync();
        return View(summary);
    }
}