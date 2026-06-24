namespace FinanceTracker.Application.DTOs;

// To, co wyświetlamy na liście
public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }

    public string AccountName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;

    // Potrzebne do edycji (żeby ustawić wybrane w dropdownie)
    public int AccountId { get; set; }
    public int CategoryId { get; set; }
}

// To, co wysyłamy z formularza tworzenia/edycji
public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public int AccountId { get; set; }
    public int CategoryId { get; set; }
}

// Pomocnicze DTO dla dropdownów kont i kategorii
public class AccountDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Income / Expense – przyda się w UI
}