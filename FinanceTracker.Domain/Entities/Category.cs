namespace FinanceTracker.Domain.Entities;

public enum CategoryType
{
    Income,
    Expense
}

public class Category
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public string Color { get; set; } = "#6366f1";
    public string Icon { get; set; } = "ti-tag";
    public bool IsSystem { get; set; } = false;

    public ApplicationUser? User { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}