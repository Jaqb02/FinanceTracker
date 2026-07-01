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

        // Seed default categories if they don't exist
        if (!await context.Categories.AnyAsync())
        {
            var defaultCategories = new List<Category>
            {
                new() { Name = "Salary", Type = CategoryType.Income },
                new() { Name = "Freelance", Type = CategoryType.Income },
                new() { Name = "Food & Dining", Type = CategoryType.Expense },
                new() { Name = "Transport", Type = CategoryType.Expense },
                new() { Name = "Housing", Type = CategoryType.Expense },
                new() { Name = "Entertainment", Type = CategoryType.Expense },
                new() { Name = "Health", Type = CategoryType.Expense },
                new() { Name = "Shopping", Type = CategoryType.Expense }
            };
            context.Categories.AddRange(defaultCategories);
            await context.SaveChangesAsync();
        }

        var mainAccount = new Account { Name = "Main Account", Balance = 5000m, Type = AccountType.Checking, Currency = "PLN", IsActive = true };
        var wallet = new Account { Name = "Wallet", Balance = 200m, Type = AccountType.Cash, Currency = "PLN", IsActive = true };
        context.Accounts.AddRange(mainAccount, wallet);
        await context.SaveChangesAsync();

        var salary = await context.Categories.FirstAsync(c => c.Name == "Salary");
        var food = await context.Categories.FirstAsync(c => c.Name == "Food & Dining");
        var transport = await context.Categories.FirstAsync(c => c.Name == "Transport");
        var entertainment = await context.Categories.FirstAsync(c => c.Name == "Entertainment");

        var transactions = new List<Transaction>
        {
            new() { AccountId = mainAccount.Id, CategoryId = salary.Id, Amount = 5000m, Type = TransactionType.Income, Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), Description = "Salary", Notes = "Monthly salary" },
            new() { AccountId = mainAccount.Id, CategoryId = food.Id, Amount = -150m, Type = TransactionType.Expense, Date = DateTime.Now.AddDays(-3), Description = "Groceries", Notes = "Weekly shopping" },
            new() { AccountId = wallet.Id, CategoryId = transport.Id, Amount = -30m, Type = TransactionType.Expense, Date = DateTime.Now.AddDays(-2), Description = "Transit pass", Notes = "Monthly pass" },
            new() { AccountId = mainAccount.Id, CategoryId = entertainment.Id, Amount = -60m, Type = TransactionType.Expense, Date = DateTime.Now.AddDays(-1), Description = "Movie night", Notes = "Cinema" },
            new() { AccountId = wallet.Id, CategoryId = food.Id, Amount = -45m, Type = TransactionType.Expense, Date = DateTime.Now.AddMonths(-1), Description = "Lunch out", Notes = "Previous month" }
        };

        context.Transactions.AddRange(transactions);

        await context.SaveChangesAsync();
    }
}