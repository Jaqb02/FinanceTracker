using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(FinanceDbContext context)
    {
        
        await context.Database.EnsureCreatedAsync();

        
        if (await context.Accounts.AnyAsync())
            return;

        // --- Konta ---
        var mainAccount = new Account { Name = "Konto główne", Balance = 5000m };
        var wallet = new Account { Name = "Portfel", Balance = 200m };
        context.Accounts.AddRange(mainAccount, wallet);

        
        var salary = new Category { Name = "Wynagrodzenie", Type = "Income" };
        var food = new Category { Name = "Jedzenie", Type = "Expense" };
        var transport = new Category { Name = "Transport", Type = "Expense" };
        var entertainment = new Category { Name = "Rozrywka", Type = "Expense" };
        context.Categories.AddRange(salary, food, transport, entertainment);

        await context.SaveChangesAsync();

        // --- Transakcje ---
        var transactions = new List<Transaction>
        {
            // Wpływy
            new() { AccountId = mainAccount.Id, CategoryId = salary.Id, Amount = 5000m, Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), Note = "Pensja" },
            // Wydatki z tego miesiąca
            new() { AccountId = mainAccount.Id, CategoryId = food.Id, Amount = -150m, Date = DateTime.Now.AddDays(-3), Note = "Zakupy w Biedronce" },
            new() { AccountId = wallet.Id, CategoryId = transport.Id, Amount = -30m, Date = DateTime.Now.AddDays(-2), Note = "Bilet miesięczny" },
            new() { AccountId = mainAccount.Id, CategoryId = entertainment.Id, Amount = -60m, Date = DateTime.Now.AddDays(-1), Note = "Kino" },
            // Starsza transakcja (poza bieżącym miesiącem)
            new() { AccountId = wallet.Id, CategoryId = food.Id, Amount = -45m, Date = DateTime.Now.AddMonths(-1), Note = "Obiad na mieście" }
        };
        context.Transactions.AddRange(transactions);

        await context.SaveChangesAsync();
    }
}