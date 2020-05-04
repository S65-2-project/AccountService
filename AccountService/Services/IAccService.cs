using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Models;

namespace AccountService.Services
{
    public interface IAccService
    {
        //TODO Documentation
        Task<Account> CreateAccount(CreateAccountModel model);
        Task<Account> UpdateAccount(Guid id, UpdateAccountModel model);
        Task<Account> UpdatePassword(Guid id, UpdateAccountModel model);
        Task<Account> GetAccount(Guid id);
        Task DeleteAccount(string email);
    }
}