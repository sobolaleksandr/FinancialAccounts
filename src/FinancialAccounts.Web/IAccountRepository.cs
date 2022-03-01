namespace FinancialAccounts.Web;

/// <summary>
/// Репозиторий <see cref="Account"/>.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Проверить баланс.
    /// </summary>
    /// <param name="id"> ID-клиента. </param>
    /// <returns> Баланс клиента. </returns>
    Task<decimal> CheckSum(int id);

    /// <summary>
    /// Удалить клиента.
    /// </summary>
    /// <param name="id"> ID-клиента. </param>
    /// <returns> <see langword="true"/>, если клиент успешно удален. </returns>
    Task<bool> Delete(int id);

    /// <summary>
    /// Начисление заданной суммы клиенту.
    /// </summary>
    /// <param name="id"> ID-клиента. </param>
    /// <param name="amount"> Сумма начисления.</param>
    /// <returns> <see langword="true"/>, если баланс успешно изменен. </returns>
    Task<bool> Deposit(int id, decimal amount);

    /// <summary>
    /// Создать клиента. 
    /// </summary>
    /// <param name="account"> Модель клиента. </param>
    /// <returns> Модель клиента. </returns>
    Task<Account?> Register(Account account);
}