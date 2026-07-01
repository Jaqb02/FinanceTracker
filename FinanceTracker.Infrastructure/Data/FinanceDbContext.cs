using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Data;

public class FinanceDbContext : IdentityDbContext<ApplicationUser>
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Account>()
            .Ignore(a => a.UserId)
            .Ignore(a => a.Type)
            .Ignore(a => a.Currency)
            .Ignore(a => a.IsActive)
            .Ignore(a => a.CreatedAt);

        // Make the shadow foreign key nullable
        modelBuilder.Entity<Account>()
            .Property<string>("UserId1")
            .IsRequired(false);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Notes)
            .HasColumnName("Note");

        modelBuilder.Entity<Transaction>()
            .Ignore(t => t.Description)
            .Ignore(t => t.Type)
            .Ignore(t => t.CreatedAt);

        modelBuilder.Entity<Category>()
            .Property(c => c.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Category>()
            .Ignore(c => c.UserId)
            .Ignore(c => c.Color)
            .Ignore(c => c.Icon)
            .Ignore(c => c.IsSystem);

        // Make the shadow foreign key nullable
        modelBuilder.Entity<Category>()
            .Property<string>("UserId1")
            .IsRequired(false);

        modelBuilder.Entity<Budget>()
            .Property(b => b.LimitAmount)
            .HasColumnName("Limit");

        modelBuilder.Entity<Budget>()
            .Ignore(b => b.UserId);

        // Make the shadow foreign key nullable
        modelBuilder.Entity<Budget>()
            .Property<string>("UserId1")
            .IsRequired(false);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}