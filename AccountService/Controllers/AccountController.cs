using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Helpers;
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
        private readonly IAccService _accService;

        public AccountController(IAccService accService)
        {
            _accService = accService;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> CreateAccount(Account account)
        {
            try
            {
                await _accService.CreateAccount(account.Email, account.Password);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(account.WithoutPassword());
        }
        
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAccount(string email)
        {
            var acc = await _accService.GetAccount(email);
            return Ok(acc);
        }

        [AllowAnonymous]
        [HttpPut("")]
        public async Task<IActionResult> UpdateAccount(string email, UpdateAccountModel account)
        {
            await _accService.UpdateAccount(account.Email, account.Password);
            return Ok(GetAccount(email));
        }
        
        [AllowAnonymous]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteAccount(Account account)
        {
            await _accService.DeleteAccount(account.Email);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("Role")]
        public async Task<IActionResult> AddRole(Account account)
        {
            var acc = await _accService.GetAccount(account.Email);
            await _accService.UpdateRole(acc.Email, account.isDelegate, account.isDelegate);
            return Ok(acc);
        }
    }
}