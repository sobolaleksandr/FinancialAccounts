namespace FinancialAccounts.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [ApiController]
    [Route("")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/v1/account/{id:int}")]
        public async Task<ActionResult<decimal>> CheckSum(int id)
        {
            var checkSum = await _repository.CheckSum(id).ConfigureAwait(false);
            return checkSum == default ? new NotFoundResult() : new ActionResult<decimal>(checkSum);
        }

        [HttpDelete("api/v1/account/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var withdraw = await _repository.Delete(id).ConfigureAwait(false);
            return withdraw ? Ok() : NotFound();
        }

        [HttpPost]
        [Route("api/v1/account")]
        public async Task<ActionResult<Account>> Register(Account account)
        {
            var entry = await _repository.Register(account).ConfigureAwait(false);
            return entry != null ? entry : BadRequest();
        }

        [HttpPut]
        [Route("api/v1/account/{id:int}")]
        public async Task<ActionResult> Withdraw(int id, decimal sum)
        {
            var withdraw = await _repository.Withdraw(id, sum).ConfigureAwait(false);
            return withdraw ? Ok() : NotFound();
        }
    }
}