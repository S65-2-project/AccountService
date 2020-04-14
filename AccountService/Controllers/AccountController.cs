using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Models;
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
        [HttpPost("")]
        public async Task<IActionResult> CreateAccount(CreateAccountModel account)
        {
            await _accountService.CreateAccount(account.Email, account.Password);
            return Ok(account);
        }
        
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAccount(string email)
        {
            var acc = await _accountService.GetAccount(email);
            return Ok(acc);
        }

        [AllowAnonymous]
        [HttpPut("")]
        public async Task<IActionResult> UpdateAccount(string email, UpdateAccountModel account)
        {
            await _accountService.UpdateAccount(account.Email, account.Password);
            return Ok(GetAccount(email));
        }
        
        [AllowAnonymous]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteAccount(Account account)
        {
            await _accountService.DeleteAccount(account.Email);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("Role")]
        public async Task<IActionResult> AddRole(Account account)
        {
            var acc = await _accountService.GetAccount(account.Email);
            await _accountService.UpdateRole(acc.Email, account.isDelegate, account.isDelegate);
            return Ok(acc);
        }
    }
}