using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Exceptions;
using AccountService.Helpers;
using AccountService.Models;
using AccountService.Repositories;
using AccountService.Services;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using MessageBroker;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Xunit;

namespace AccountServiceTests.ServiceTests
{
    public class AccountServiceTest
    {
        private readonly IAccountService _accountService;
        private readonly Mock<IAccountRepository> _repository;
        private readonly Mock<IHasher> _hasher;
        private readonly Mock<ITokenGenerator> _jwtGenerator;
        private readonly Mock<IRegexHelper> _regexHelper;
        private readonly Mock<IMessageQueuePublisher> _messageQueuePublisher;

        public AccountServiceTest()
        {
            _jwtGenerator = new Mock<ITokenGenerator>();
            _hasher = new Mock<IHasher>();
            _repository = new Mock<IAccountRepository>();
            _regexHelper = new Mock<IRegexHelper>();
            _messageQueuePublisher = new Mock<IMessageQueuePublisher>();

            _accountService = new AccountService.Services.AccountService(
                _repository.Object,
                _hasher.Object,
                _jwtGenerator.Object,
                _regexHelper.Object,
                _messageQueuePublisher.Object,
            Options.Create(new MessageQueueSettings())
                );
        }

        [Fact]
        public async Task CreateAccount_ValidAccount_ReturnsAccountWithoutSensitiveData()
        {
            const string email = "iemand@gmail.com";
            const string password = "MyV3rysecurepw!!2";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            var account = new Account
            {
                Email = email,
                Password = hashedPassword,
                Salt = salt,
            };

            var createModel = new CreateAccountModel
            {
                Email = "iemand@gmail.com",
                Password = "MyV3rysecurepw!!2"
            };

            _repository.Setup(x => x.Get(createModel.Email)).ReturnsAsync((Account) null);
            _hasher.Setup(x => x.CreateSalt()).Returns(salt);
            _hasher.Setup(x => x.HashPassword(password, salt)).ReturnsAsync(hashedPassword);
            _repository.Setup(x => x.Create(It.IsAny<Account>())).ReturnsAsync(account);
            _regexHelper.Setup(x => x.IsValidEmail(createModel.Email)).Returns(true);
            _regexHelper.Setup(x => x.IsValidPassword(createModel.Password)).Returns(true);
            _messageQueuePublisher.Setup(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new {Email = email})).Returns(Task.CompletedTask);

            var result = await _accountService.CreateAccount(createModel);

            Assert.Equal(account.Email, result.Email);
            Assert.Null(result.Password);
            Assert.Null(result.Salt);
        }
        
        [Fact]
        public async Task Login_ValidAccount_ReturnsAccountWithoutSensitiveData()
        {
            var id = Guid.NewGuid();
            const string email = "iemand@gmail.com";
            const string password = "MyV3rysecurepw!!2";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string token = "zqawsexdctvbyunimo";

            var account = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            var login = new LoginModel
            {
                Email = account.Email,
                Password = password
            };
            _repository.Setup(x => x.Get(email)).ReturnsAsync(account);
            _hasher.Setup(x => x.VerifyHash(password, salt, hashedPassword)).ReturnsAsync(true);
            _jwtGenerator.Setup(x => x.GenerateJwt(account.Id)).Returns(token);

            var result = await _accountService.Login(login);
            
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task Login_IncorrectPassword_ThrowsIncorrectPasswordException()
        {
            var id = Guid.NewGuid();
            const string email = "iemand@gmail.com";
            const string password = "MyV3rysecurepw!!2";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string token = "zqawsexdctvbyunimo";

            var account = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            var login = new LoginModel
            {
                Email = account.Email,
                Password = password
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync(account);
            _hasher.Setup(x => x.VerifyHash("NotMy!Password1", salt, hashedPassword)).ReturnsAsync(true);
            _jwtGenerator.Setup(x => x.GenerateJwt(account.Id)).Returns(token);

            var result = await Assert.ThrowsAsync<IncorrectPasswordException>(() => _accountService.Login(login));

            Assert.IsType<IncorrectPasswordException>(result);
        }

        [Fact]
        public async Task UpdatePassword_NewPassword_ReturnsAccountWithoutSensitiveData()
        {
            var id = Guid.NewGuid();
            const string email = "iemand@gmail.com";
            const string oldPassword = "MyV3rysecurepw!!2";
            const string newPassword = "Myn3V3ryS3cur!@";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            var account = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };
            
            var updatedAccount = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            var passwordModel = new ChangePasswordModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };
            
            _repository.Setup(x => x.Get(id)).ReturnsAsync(account);
            _hasher.Setup(x => x.VerifyHash(oldPassword, salt, hashedPassword)).ReturnsAsync(true);
            _regexHelper.Setup(x => x.IsValidPassword(newPassword)).Returns(true);
            _repository.Setup(x => x.Update(account.Id, account)).ReturnsAsync(updatedAccount);

            var result = await _accountService.UpdatePassword(account.Id, passwordModel);

            Assert.Equal(account.Id, result.Id);
            Assert.Null(result.Password);
            Assert.Null(result.Salt);
        }

        [Fact]
        public async Task UpdatePassword_IncorrectPassword_ThrowsIncorrectPasswordException()
        {
            var id = Guid.NewGuid();
            const string email = "iemand@gmail.com";
            const string password = "MyV3rysecurepw!!2";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string token = "zqawsexdctvbyunimo";

            var account = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            var updatedAccount = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            var passwordModel = new ChangePasswordModel
            {
                OldPassword = "NietHetGoedeW4chtwoord12!",
                NewPassword = "Myn3westV3ryS3cur3password12345!@"
            };

            _repository.Setup(x => x.Get(id)).ReturnsAsync(account);
            _repository.Setup(x => x.Update(account.Id, account)).ReturnsAsync(updatedAccount);
            _hasher.Setup(x => x.VerifyHash(password, salt, hashedPassword)).ReturnsAsync(true);
            _jwtGenerator.Setup(x => x.GenerateJwt(account.Id)).Returns(token);

            var result = await Assert.ThrowsAsync<IncorrectPasswordException>(() =>
                    _accountService.UpdatePassword(account.Id, passwordModel));

            Assert.IsType<IncorrectPasswordException>(result);
        }
        
        [Fact]
        public async Task UpdateAccount_NewAccountData_ReturnsAccountWithoutSensitiveData()
        {
            var id = Guid.NewGuid();
            const string emailOld = "iemand@gmail.com";
            const string emailNew = "mijn@nieuwe.nl";

            var account = new Account
            {
                Id = id,
                Email = emailOld,
                isDelegate = true,
                isDAppOwner = false
            };

            var updateModel = new UpdateAccountModel
            {
                Email = emailNew,
                isDelegate = false,
                isDAppOwner = true
            };
            
            var updatedAccount = new Account
            {
                Id = id,
                Email = emailNew,
                isDelegate = false,
                isDAppOwner = true
            };

            _regexHelper.Setup(r => r.IsValidEmail(emailNew)).Returns(true);
            _repository.Setup(x => x.Get(id)).ReturnsAsync(account);

            _repository.Setup(x => x.Get(updateModel.Email));
            _repository.Setup(x => x.Update(id, It.IsAny<Account>())).ReturnsAsync(updatedAccount);

            var result = await _accountService.UpdateAccount(id, updateModel);

            Assert.NotEqual(emailOld, result.Email);
            Assert.False(result.isDelegate);
            Assert.True(result.isDAppOwner);
        }

        [Fact]
        public async Task GetAccount_Id_ReturnsAccountWithoutSensitiveData()
        {
            var id = Guid.NewGuid();
            const string email = "iemand@gmail.com";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            var account = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            _repository.Setup(x => x.Get(id)).ReturnsAsync(account);

            var result = await _accountService.GetAccount(account.Id);

            Assert.Equal(id, result.Id);
            Assert.Equal(account.Email, result.Email);
            Assert.Null(result.Password);
            Assert.Null(result.Salt);
        }

        [Fact]
        public async Task GetAccount_InvalidId_ReturnsAccountWithoutSensitiveData()
        {
            var id = Guid.NewGuid();
            const string email = "iemand@gmail.com";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            var account = new Account
            {
                Id = id,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            _repository.Setup(x => x.Get(id)).ReturnsAsync(account);

            var result =
                await Assert.ThrowsAsync<AccountNotFoundException>(() =>
                    _accountService.GetAccount(Guid.Empty));

            Assert.IsType<AccountNotFoundException>(result);
        }
    }
}
