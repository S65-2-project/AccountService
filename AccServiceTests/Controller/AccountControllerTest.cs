using System;
using System.Threading.Tasks;
using AccountService.Controllers;
using AccountService.Domain;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AccountServiceTests.Controller
{
    public class AccountControllerTest
    {
        private readonly ITestOutputHelper  _testOutputHelper;
        private AccountController _accountController;

        public AccountControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task RegisterAccount_ReturnUserSuccess()
        {
            const string email = "test@test.com";
            const string password = "#$%FGAvaffa4s";
            
            var account = new Account()
            {
                Email = email,
                Password = password
            };

            var accountService = new Mock<IAccService>();
            _accountController = new AccountController(accountService.Object);

            var result = await _accountController.CreateAccount(account);
            
            _testOutputHelper.WriteLine(result.ToString());
            Assert.IsType<OkObjectResult>(result);
        }
    }
}