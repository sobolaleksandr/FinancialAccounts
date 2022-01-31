namespace FinancialAccounts.Web
{
    public interface IAccountRepository
    {
        Task<decimal> CheckSum(int id);

        Task<bool> Delete(int id);

        Task<Account?> Register(Account account);

        Task<bool> Withdraw(int id, decimal amount);
    }
}