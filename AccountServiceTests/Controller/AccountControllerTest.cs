using System;
using System.Threading.Tasks;
using AccountService.Controllers;
using AccountService.Domain;
using AccountService.Exceptions;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AccountServiceTests.Controller
{
    public class AccountControllerTest
    {
        private readonly AccountController _accountController;
        private readonly Mock<IAccountService> _accountService;

        public AccountControllerTest()
        {
            _accountService = new Mock<IAccountService>();
            _accountController = new AccountController(_accountService.Object);
        }

        [Fact]
        public async Task CreateAccount_ValidAccount_ReturnsAccount()
        {
            const string email = "test@test.nl";
            const string password = "MyV3ryS3cureP";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string token = "testjwttoken";

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = hashedPassword,
                isDelegate = false,
                isDAppOwner = false,
                Salt = salt,
                Token = token
            };

            var createAccountModel = new CreateAccountModel()
            {
                Email = email,
                Password = password
            };

            _accountService.Setup(x => x.CreateAccount(createAccountModel)).ReturnsAsync(account);

            var result = await _accountController.CreateAccount(createAccountModel) as ObjectResult;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account.Id, ((Account) result.Value).Id);
        }

        [Fact]
        public async Task Login_ValidAccount_ReturnsAccount()
        {
            const string email = "test@test.nl";
            const string password = "MyV3ryS3cureP!W!";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string token = "testjwttoken";

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = hashedPassword,
                isDelegate = false,
                isDAppOwner = false,
                Salt = salt,
                Token = token
            };

            var loginModel = new LoginModel()
            {
                Email = email,
                Password = password
            };

            _accountService.Setup(x => x.Login(loginModel)).ReturnsAsync(account);

            var result = await _accountController.Login(loginModel) as ObjectResult;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account.Id, ((Account) result.Value).Id);
        }

        [Fact]
        public async Task Login_InvalidAccount_ThrowsNotFound()
        {
            const string email = "test@test.nl";
            const string password = "secure";

            var loginModel = new LoginModel()
            {
                Email = email,
                Password = password
            };

            _accountService.Setup(x => x.Login(loginModel))
                .Throws<EmailNotFoundException>();

            var result = await _accountController.Login(loginModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}