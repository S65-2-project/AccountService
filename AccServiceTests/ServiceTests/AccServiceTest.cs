using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Repositories;
using AccountService.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace AccountServiceTests.ServiceTests
{
    public class AccServiceTest
    {
        private readonly Mock<IAccountRepository> _repository;

        public AccServiceTest(ITestOutputHelper testOutputHelper)
        {
            _repository = new Mock<IAccountRepository>();
        }

        [Fact]
        public async Task CreateAccountSuccess()
        {
            const string email = "test@test.com";
            const string password = "#$%FGAvaffa4s";
            
            var account = new Account
            {
                Email = email,
                Password = password
            };

            _repository.Setup(r => r.Create(account))
                .ReturnsAsync(account);
            
            var service = new AccService(_repository.Object);

            var result = await service.CreateAccount(email, password);

            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task CreateAccountFail()
        {
            const string email = "test@test.com";
            const string password = "test";
            
            var account = new Account
            {
                Email = email,
                Password = password
            };

            _repository.Setup(r => r.Create(account))
                .ReturnsAsync(account);
            
            var service = new AccService(_repository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAccount(email, password));
            
            Assert.Equal("Password doesn't meet the requirements.", ex.Message);
        }
    }
}