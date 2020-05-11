using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Helpers;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Impl;

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
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                return Ok(await _accountService.Login(loginModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountModel accountModel)
        {
            try
            {
                return Ok(await _accountService.CreateAccount(accountModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            try
            {
                return Ok(await _accountService.GetAccount(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [AllowAnonymous]
        [HttpPut("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(Guid id, ChangePasswordModel passwordModel)
        {
            try
            {
                return Ok(await _accountService.UpdatePassword(id, passwordModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody]UpdateAccountModel account)
        {
            try
            {
                return Ok(await _accountService.UpdateAccount(id, account));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                await _accountService.DeleteAccount(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}