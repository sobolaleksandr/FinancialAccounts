namespace FinancialAccounts.Web;

using Microsoft.EntityFrameworkCore;

/// <inheritdoc />
public class AccountRepository : IAccountRepository
{
    /// <summary>
    /// Объект для синхронизации потоков.
    /// </summary>
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    /// <summary>
    /// Контекст приложения.
    /// </summary>
    private readonly ApplicationContext _context;

    /// <summary>
    /// Создать экземпляр <see cref="AccountRepository"/>.
    /// </summary>
    /// <param name="context"> Контекст приложения. </param>
    public AccountRepository(ApplicationContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<decimal> CheckSum(int id)
    {
        await SemaphoreSlim.WaitAsync().ConfigureAwait(false);
        try
        {
            var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
            return account?.Sum ?? AccountConstants.DEFAULT_VALUE;
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }

    /// <inheritdoc />
    public async Task<bool> Delete(int id)
    {
        var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
        if (account == null)
            return false;

        try
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> Deposit(int id, decimal amount)
    {
        await SemaphoreSlim.WaitAsync().ConfigureAwait(false);

        try
        {
            var account = await _context.Accounts.FindAsync(id).ConfigureAwait(false);
            if (account == null || account.Sum + amount < 0)
                return false;

            if (amount == 0)
                return true;

            account.Sum += amount;

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }

    /// <inheritdoc />
    public async Task<Account?> Register(Account account)
    {
        var entry = await _context.Accounts.AddAsync(account).ConfigureAwait(false);
        try
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return entry.Entity;
        }
        catch (DbUpdateException)
        {
            return null;
        }
    }
}