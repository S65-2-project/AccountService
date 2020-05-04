using System;
using System.Text;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Helpers;
using AccountService.Models;
using AccountService.Repositories;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace AccountServiceTests.ServiceTests
{
    public class AccServiceTest
    {        
        private readonly IAccService _accountService;
        private readonly Mock<IAccountRepository> _repository;
        private readonly Mock<IHasher> _hasher;
        private readonly Mock<IJWTokenGenerator> _jwtGenerator;
        private readonly Mock<IRegexHelper> _regexHelper;


        public AccServiceTest()
        {
            _jwtGenerator = new Mock<IJWTokenGenerator>();
            _hasher = new Mock<IHasher>();
            _repository = new Mock<IAccountRepository>();
            _regexHelper = new Mock<IRegexHelper>();
            _accountService = new AccService(_repository.Object,_hasher.Object, _jwtGenerator.Object, _regexHelper.Object);

        }

        [Fact]
        public async Task CreateAccountSuccess()
        {
            const string email = "iemand@gmail.com";
            const string password = "MyV3rysecurepw!!2";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "afdsafdsafsda";
            
            var account = new Account
            {
                Email = email,
                Password = encryptedPassword,
                Salt = salt,
            };
            
            var createModel = new CreateAccountModel
            {
                Email = "iemand@gmail.com",
                Password = "MyV3rysecurepw!!2"
            };

            _repository.Setup(x => x.Get(createModel.Email)).ReturnsAsync((Account)null);
            _hasher.Setup(x => x.CreateSalt()).Returns(salt);
            _hasher.Setup(x => x.HashPassword(password, salt)).ReturnsAsync(encryptedPassword);
            _repository.Setup(x => x.Create(It.IsAny<Account>())).ReturnsAsync(account);
            _regexHelper.Setup(x => x.IsValidEmail(createModel.Email)).Returns(true);
            _regexHelper.Setup(x => x.IsValidPassword(createModel.Password)).Returns(true);
            
            var result = await _accountService.CreateAccount(createModel);

            Assert.Equal(account.Email, result.Email);
            Assert.NotNull(result.Id);
            Assert.Null(result.Password);
            Assert.Null(result.Salt);
            Assert.NotNull(result);        
        }
        
    }
}