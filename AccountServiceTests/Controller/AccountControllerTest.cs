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
        public async Task CreateAccountSuccess()
        {
            var accountService = new Mock<IAccountService>();
            _accountController = new AccountController(accountService.Object);

            var result = _accountController.CreateAccount(new CreateAccountModel()
            {
                Email = "test@test.com", Password = "Ba123xzc2342314!"
            });
            
            _testOutputHelper.WriteLine(result.ToString());
            Assert.IsType<OkResult>(result);
        }
    }
}