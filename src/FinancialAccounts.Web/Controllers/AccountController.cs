namespace FinancialAccounts.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер для <see cref="Account"/>.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Репозиторий <see cref="Account"/>.
    /// </summary>
    private readonly IAccountRepository _repository;

    /// <summary>
    /// Контроллер для <see cref="Account"/>.
    /// </summary>
    /// <param name="repository"> Репозиторий <see cref="Account"/>.</param>
    public AccountController(IAccountRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Проверка баланса клиента. 
    /// </summary>
    /// <param name="id"> ID-клиента. </param>
    /// <returns> <see cref="NotFoundResult"/>, если пользователя нет в системе, иначе баланс. </returns>
    [HttpGet]
    [Route("api/v1/account/{id:int}")]
    public async Task<ActionResult<decimal>> CheckSum(int id)
    {
        var checkSum = await _repository.CheckSum(id).ConfigureAwait(false);
        return checkSum == AccountConstants.DEFAULT_VALUE ? NotFound() : Ok(checkSum);
    }

    /// <summary>
    /// Удалить клиента из системы.
    /// </summary>
    /// <param name="id"> ID-клиента. </param>
    /// <returns> <see cref="NotFoundResult"/>, если пользователя нет в системе, иначе <see cref="OkResult"/>. </returns>
    [HttpDelete("api/v1/account/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var isDeleted = await _repository.Delete(id).ConfigureAwait(false);
        return isDeleted ? Ok() : NotFound();
    }

    /// <summary>
    /// Начисление/Списание заданной суммы с клиента.
    /// </summary>
    /// <param name="id"> ID-клиента. </param>
    /// <param name="sum"> Сумма зачисления (больше 0) или списания (меньше 0). </param>
    /// <returns> Возвращает <see cref="OkResult"/>, если операция прошла успешно, иначе <see cref="BadRequestResult"/>. </returns>
    [HttpPut]
    [Route("api/v1/account/{id:int}")]
    public async Task<ActionResult<bool>> Deposit(int id, decimal sum)
    {
        var isWithdrawn = await _repository.Deposit(id, sum).ConfigureAwait(false);
        return isWithdrawn ? Ok() : BadRequest();
    }

    /// <summary>
    /// Регистрация клиента.
    /// </summary>
    /// <param name="account"> Модель клиента. </param>
    /// <returns> Модель клиента, если получилось зарегистрировать, иначе <see cref="BadRequestResult"/>.</returns>
    [HttpPost]
    [Route("api/v1/account")]
    public async Task<ActionResult<Account>> Register(Account account)
    {
        var entry = await _repository.Register(account).ConfigureAwait(false);
        return entry != null ? entry : BadRequest();
    }
}