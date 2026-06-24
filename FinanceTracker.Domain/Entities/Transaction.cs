using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    public string? Note { get; set; }
    
    public DateTime Date { get; set; } = DateTime.Today;
    
    // Foreign keys
    public int AccountId { get; set; }
    public int CategoryId { get; set; }
    
    // Navigation properties
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}