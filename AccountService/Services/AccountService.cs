using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Repositories;

namespace AccountService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            this._repository = repository;
        }

        public async Task CreateAccount(Account account)
        {
            var list = await _repository.Get();
            if (list.Count != 0) return;

            await _repository.Create(new Account()
            {
                Id = Guid.NewGuid(),
                Email = account.Email,
                Password = account.Password,
            });
        }

        public async Task<Account> Get(Account account)
        {
            Account acc = await _repository.Get(account.Email);

            return acc;
        }

        public async Task DeleteAccount(Account account)
        {
            var acc = await _repository.Get(account.Email);
            
        }
    }
}