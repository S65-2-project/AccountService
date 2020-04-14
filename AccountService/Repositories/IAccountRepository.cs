using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.Domain;

namespace AccountService.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> Get(string email);
        Task<Account> Get(Guid id);
        Task<Account> Create(Account account);
        Task<Account> Update(Guid id, Account account);
        Task Remove(Guid id);
    }
}