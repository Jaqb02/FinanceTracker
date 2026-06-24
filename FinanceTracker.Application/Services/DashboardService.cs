using FinanceTracker.Application.DTOs;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly FinanceDbContext _context;

    public DashboardService(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        // Saldo wszystkich kont
        var totalBalance = await _context.Accounts.SumAsync(a => a.Balance);

        // Transakcje w bieżącym miesiącu
        var now = DateTime.Now;
        var currentMonthTransactions = await _context.Transactions
            .CountAsync(t => t.Date.Year == now.Year && t.Date.Month == now.Month);

        // Ostatnie 5 transakcji z nazwą konta i kategorii
        var recentTransactions = await _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date)
            .Take(5)
            .Select(t => new RecentTransactionDto
            {
                AccountName = t.Account.Name,
                CategoryName = t.Category.Name,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note
            })
            .ToListAsync();

        return new DashboardSummaryDto
        {
            TotalBalance = totalBalance,
            CurrentMonthTransactionsCount = currentMonthTransactions,
            RecentTransactions = recentTransactions
        };
    }
}