namespace FinancialAccounts.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// ���������� ��� <see cref="Account"/>.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// ����������� <see cref="Account"/>.
    /// </summary>
    private readonly IAccountRepository _repository;

    /// <summary>
    /// ���������� ��� <see cref="Account"/>.
    /// </summary>
    /// <param name="repository"> ����������� <see cref="Account"/>.</param>
    public AccountController(IAccountRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// �������� ������� �������. 
    /// </summary>
    /// <param name="id"> ID-�������. </param>
    /// <returns> <see cref="NotFoundResult"/>, ���� ������������ ��� � �������, ����� ������. </returns>
    [HttpGet]
    [Route("api/v1/account/{id:int}")]
    public async Task<ActionResult<decimal>> CheckSum(int id)
    {
        var checkSum = await _repository.CheckSum(id).ConfigureAwait(false);
        return checkSum == AccountConstants.DEFAULT_VALUE ? NotFound() : Ok(checkSum);
    }

    /// <summary>
    /// ������� ������� �� �������.
    /// </summary>
    /// <param name="id"> ID-�������. </param>
    /// <returns> <see cref="NotFoundResult"/>, ���� ������������ ��� � �������, ����� <see cref="OkResult"/>. </returns>
    [HttpDelete("api/v1/account/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var isDeleted = await _repository.Delete(id).ConfigureAwait(false);
        return isDeleted ? Ok() : NotFound();
    }

    /// <summary>
    /// ����������/�������� �������� ����� � �������.
    /// </summary>
    /// <param name="id"> ID-�������. </param>
    /// <param name="sum"> ����� ���������� (������ 0) ��� �������� (������ 0). </param>
    /// <returns> ���������� <see cref="OkResult"/>, ���� �������� ������ �������, ����� <see cref="BadRequestResult"/>. </returns>
    [HttpPut]
    [Route("api/v1/account/{id:int}")]
    public async Task<ActionResult<bool>> Deposit(int id, decimal sum)
    {
        var isWithdrawn = await _repository.Deposit(id, sum).ConfigureAwait(false);
        return isWithdrawn ? Ok() : BadRequest();
    }

    /// <summary>
    /// ����������� �������.
    /// </summary>
    /// <param name="account"> ������ �������. </param>
    /// <returns> ������ �������, ���� ���������� ����������������, ����� <see cref="BadRequestResult"/>.</returns>
    [HttpPost]
    [Route("api/v1/account")]
    public async Task<ActionResult<Account>> Register(Account account)
    {
        var entry = await _repository.Register(account).ConfigureAwait(false);
        return entry != null ? entry : BadRequest();
    }
}