namespace FinanceTracker.Domain.Entities;

public class RecurringTransaction
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Frequency { get; set; } = "Monthly"; // Monthly, Weekly, Yearly
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int AccountId { get; set; }
    public int CategoryId { get; set; }

    // Navigation
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
