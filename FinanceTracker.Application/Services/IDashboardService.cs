using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
}