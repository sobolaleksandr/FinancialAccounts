namespace FinancialAccounts.Web;

using Microsoft.EntityFrameworkCore;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationContext _context;
    private readonly object _balanceLock = new();

    public AccountRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<decimal> CheckSum(int id)
    {
        var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
        lock (_balanceLock)
        {
            return account?.Sum ?? AccountConstants.DEFAULT_VALUE;
        }
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

    public async Task<decimal> Deposit(int id, decimal amount)
    {
        var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
        lock (_balanceLock)
        {
            if (account == null || account.Sum + amount < 0)
                return AccountConstants.DEFAULT_VALUE;

            account.Sum += amount;
        }

        try
        {
            await _context.SaveChangesAsync();
            return account.Sum;
        }
        catch (DbUpdateException exception)
        {
            return AccountConstants.DEFAULT_VALUE;
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
}