namespace FinancialAccounts.Web;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст приложения.
/// </summary>
public sealed class ApplicationContext : DbContext
{
    /// <summary>
    /// Контект приложения. 
    /// </summary>
    /// <param name="options"> Опции контекста. </param>
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Таблица аккаунты.
    /// </summary>
    public DbSet<Account> Accounts { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }
}