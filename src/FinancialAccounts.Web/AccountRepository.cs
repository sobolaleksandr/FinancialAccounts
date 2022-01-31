namespace FinancialAccounts.Web;

using Microsoft.EntityFrameworkCore;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationContext _context;

    public AccountRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<decimal> CheckSum(int id)
    {
        var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
        return account?.Sum ?? default;
    }

    public async Task<bool> Delete(int id)
    {
        var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
        if (account == null)
            return false;

        try
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException exception)
        {
            return false;
        }
    }

    public async Task<Account?> Register(Account account)
    {
        var entry = await _context.Accounts.AddAsync(account).ConfigureAwait(false);
        try
        {
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        catch (DbUpdateException exception)
        {
            return null;
        }
    }

    public async Task<bool> Withdraw(int id, decimal amount)
    {
        var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
        if (account == null)
            return false;

        if (account.Sum - amount < 0)
            return false;

        account.Sum -= amount;
        return true;
    }
}