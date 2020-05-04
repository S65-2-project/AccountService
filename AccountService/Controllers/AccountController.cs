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
        private readonly IAccService _accService;

        public AccountController(IAccService accService)
        {
            _accService = accService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                return Ok(await _accService.Login(loginModel));
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
                return Ok(await _accService.CreateAccount(accountModel));
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
                return Ok(await _accService.GetAccount(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [AllowAnonymous]
        [HttpPut("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(Guid id, UpdateAccountModel account)
        {
            try
            {
                return Ok(await _accService.UpdatePassword(id, account));
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
                return Ok(await _accService.UpdateAccount(id, account));
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
                await _accService.DeleteAccount(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}