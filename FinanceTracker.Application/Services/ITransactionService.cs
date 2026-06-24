using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Services;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetAllAsync(DateTime? from = null, DateTime? to = null,
                                           int? categoryId = null, int? accountId = null);
    Task<TransactionDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateTransactionDto dto);
    Task UpdateAsync(int id, CreateTransactionDto dto);
    Task DeleteAsync(int id);
    Task<List<AccountDto>> GetAccountsAsync();
    Task<List<CategoryDto>> GetCategoriesAsync();
}