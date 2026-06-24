namespace FinanceTracker.Application.DTOs;

public class DashboardSummaryDto
{
    public decimal TotalBalance { get; set; }
    public int CurrentMonthTransactionsCount { get; set; }
    public List<RecentTransactionDto> RecentTransactions { get; set; } = new();
}

public class RecentTransactionDto
{
    public string AccountName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
}