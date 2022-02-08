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
            return checkSum == AccountConstants.DEFAULT_VALUE ? new NotFoundResult() : Ok(checkSum);
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
        public async Task<ActionResult<decimal>> Deposit(int id, decimal sum)
        {
            var withdraw = await _repository.Deposit(id, sum).ConfigureAwait(false);
            return withdraw != AccountConstants.DEFAULT_VALUE ? Ok(withdraw): BadRequest();
        }
    }
}