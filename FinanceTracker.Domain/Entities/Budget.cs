namespace FinanceTracker.Domain.Entities;

public class Budget
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal LimitAmount { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
