using System;
using System.Threading.Tasks;
using AccountService.Controllers;
using AccountService.Domain;
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
        public async Task CreateAccountTest()
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

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account.Id, ((Account) result.Value).Id);
        }

        [Fact]
        public async Task LoginTest()
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

            var result = await _accountController.Login(loginModel);
            var data = result as ObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(data);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account.Id, ((Account) data.Value).Id);
        }

        [Fact]
        public async Task LoginBadRequestTest()
        {
            const string email = "test@test.nl";
            const string password = "secure";

            var loginModel = new LoginModel()
            {
                Email = email,
                Password = password
            };

            _accountService.Setup(x => x.Login(loginModel))
                .Throws<ArgumentException>();

            var result = await _accountController.Login(loginModel);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}