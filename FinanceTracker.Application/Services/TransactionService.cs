using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly FinanceDbContext _context;

    public TransactionService(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransactionDto>> GetAllAsync(DateTime? from = null,
        DateTime? to = null, int? categoryId = null, int? accountId = null)
    {
        var query = _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(t => t.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(t => t.Date <= to.Value);
        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);
        if (accountId.HasValue)
            query = query.Where(t => t.AccountId == accountId.Value);

        return await query
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Note = t.Notes,
                Date = t.Date,
                AccountName = t.Account.Name,
                CategoryName = t.Category.Name,
                AccountId = t.AccountId,
                CategoryId = t.CategoryId
            })
            .ToListAsync();
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.Id == id)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Note = t.Notes,
                Date = t.Date,
                AccountName = t.Account.Name,
                CategoryName = t.Category.Name,
                AccountId = t.AccountId,
                CategoryId = t.CategoryId
            })
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(CreateTransactionDto dto)
    {
        // 1. Dodaj transakcję
        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Notes = dto.Note,
            Description = dto.Note ?? string.Empty,
            Date = dto.Date,
            AccountId = dto.AccountId,
            CategoryId = dto.CategoryId,
            Type = dto.Amount >= 0 ? TransactionType.Income : TransactionType.Expense
        };
        _context.Transactions.Add(transaction);

        // 2. Zaktualizuj saldo konta
        var account = await _context.Accounts.FindAsync(dto.AccountId);
        if (account != null)
        {
            account.Balance += dto.Amount; // kwota może być ujemna (wydatek)
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, CreateTransactionDto dto)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
            throw new KeyNotFoundException($"Transakcja o ID {id} nie istnieje.");

        var account = await _context.Accounts.FindAsync(transaction.AccountId);

        // 1. Cofnij wpływ starej transakcji na konto
        if (account != null)
            account.Balance -= transaction.Amount;

        // 2. Nadpisz właściwości
        transaction.Amount = dto.Amount;
        transaction.Notes = dto.Note;
        transaction.Description = dto.Note ?? string.Empty;
        transaction.Date = dto.Date;
        transaction.AccountId = dto.AccountId;
        transaction.CategoryId = dto.CategoryId;
        transaction.Type = dto.Amount >= 0 ? TransactionType.Income : TransactionType.Expense;

        // 3. Zastosuj nowy wpływ (jeśli zmieniło się konto, pobierz nowe)
        var newAccount = await _context.Accounts.FindAsync(dto.AccountId);
        if (newAccount != null)
            newAccount.Balance += dto.Amount;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
            throw new KeyNotFoundException($"Transakcja o ID {id} nie istnieje.");

        // Cofnij wpływ na saldo konta
        var account = await _context.Accounts.FindAsync(transaction.AccountId);
        if (account != null)
            account.Balance -= transaction.Amount;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AccountDto>> GetAccountsAsync()
    {
        return await _context.Accounts
            .Select(a => new AccountDto { Id = a.Id, Name = a.Name })
            .ToListAsync();
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Type = c.Type.ToString() })
            .ToListAsync();
    }
}