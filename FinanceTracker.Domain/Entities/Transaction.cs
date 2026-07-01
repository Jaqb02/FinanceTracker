using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Domain.Entities;

public enum TransactionType
{
    Income,
    Expense,
    Transfer
}

public class Transaction
{
    public int Id { get; set; }

    [Required]
    public int AccountId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public string Description { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}