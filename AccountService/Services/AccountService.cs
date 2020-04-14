using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Helpers;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly RegexUtils _regexUtils;

        public AccountService(IAccountRepository repository)
        {
            this._repository = repository;
        }

        public async Task CreateAccount(string email, string password)
        {
            var acc = await _repository.Get(email);
            
            //if (acc == null || _regexUtils.IsValidEmail(email) || _regexUtils.IsValidPassword(password)) return;
            
            await _repository.Create(new Account()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = password
            });
        }

        public async Task<Account> UpdateAccount(string email, string password)
        {
            if (_regexUtils.IsValidEmail(email) || _regexUtils.IsValidPassword(password)) return null;
            
            var acc = await GetAccount(email);
            if (_regexUtils.IsValidPassword(password))
            {
                acc.Password = password;
            }
            else
            {
                return null;
            }
            
            return await _repository.Update(acc.Id, acc);
        }

        public async Task<Account> UpdateRole(string email, bool isDelegate, bool isDAppOwner)
        {
            var acc = await GetAccount(email);
            
            acc.isDelegate = isDelegate;
            acc.isDAppOwner = isDAppOwner;

            return (await _repository.Update(acc.Id, acc));
        }

        public async Task<Account> GetAccount(string email)
        {
            var acc = await _repository.Get(email);
            return acc;
        }

        public async Task DeleteAccount(string email)
        {
            var acc = await _repository.Get(email);
            await _repository.Remove(acc.Id);
        }
    }
}