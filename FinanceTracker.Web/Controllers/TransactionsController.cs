using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Services;

namespace FinanceTracker.Web.Controllers;

public class TransactionsController : Controller
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // GET: Transactions
    public async Task<IActionResult> Index(DateTime? from, DateTime? to, int? categoryId, int? accountId)
    {
        var transactions = await _transactionService.GetAllAsync(from, to, categoryId, accountId);
        await LoadDropdownsAsync(categoryId, accountId);
        return View(transactions);
    }

    // GET: Transactions/Create
    public async Task<IActionResult> Create()
    {
        await LoadDropdownsAsync();
        return View(new CreateTransactionDto());
    }

    // POST: Transactions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(dto.CategoryId, dto.AccountId);
            return View(dto);
        }

        await _transactionService.CreateAsync(dto);
        TempData["Message"] = "Transakcja została dodana.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Transactions/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
        if (transaction == null)
            return NotFound();

        var editDto = new CreateTransactionDto
        {
            Amount = transaction.Amount,
            Note = transaction.Note,
            Date = transaction.Date,
            AccountId = transaction.AccountId,
            CategoryId = transaction.CategoryId
        };
        await LoadDropdownsAsync(transaction.CategoryId, transaction.AccountId);
        return View(editDto);
    }

    // POST: Transactions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateTransactionDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(dto.CategoryId, dto.AccountId);
            return View(dto);
        }

        try
        {
            await _transactionService.UpdateAsync(id, dto);
            TempData["Message"] = "Transakcja została zaktualizowana.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // GET: Transactions/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
        if (transaction == null)
            return NotFound();

        return View(transaction);
    }

    // POST: Transactions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _transactionService.DeleteAsync(id);
            TempData["Message"] = "Transakcja została usunięta.";
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    // Pomocnicza metoda do ładowania list dla dropdownów
    private async Task LoadDropdownsAsync(int? selectedCategoryId = null, int? selectedAccountId = null)
    {
        var accounts = await _transactionService.GetAccountsAsync();
        var categories = await _transactionService.GetCategoriesAsync();

        ViewBag.Accounts = new SelectList(accounts, "Id", "Name", selectedAccountId);
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
    }
}