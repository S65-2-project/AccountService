using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost(("create"))]
        public async Task<IActionResult> CreateAccount(Account account)
        {
            await _accountService.CreateAccount(account);
            return Ok();
        }
    }
}