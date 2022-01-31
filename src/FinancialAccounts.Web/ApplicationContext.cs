namespace FinancialAccounts.Web
{
    using Microsoft.EntityFrameworkCore;

    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}