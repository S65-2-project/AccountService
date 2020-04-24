using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Helpers;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services
{
    public class AccService : IAccService
    {
        private readonly IAccountRepository _repository;
        private readonly IRegexHelper _regexHelper = new RegexHelper();

        public AccService(IAccountRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Account> CreateAccount(string email, string password)
        {
            var acc = await _repository.Get(email);

            if (acc != null)
                throw new ArgumentException("Email is already in use.");
            
            if(!_regexHelper.IsValidEmail(email))
                throw new ArgumentException("Email is not a valid email.");
            
            if (!_regexHelper.IsValidPassword(password))
                throw new ArgumentException("Password doesn't meet the requirements.");

            var newAccount = new Account()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = password
            };

            await _repository.Create(newAccount);
            
            return newAccount.WithoutPassword();
        }

        public async Task<Account> UpdateAccount(string email, string password)
        {
            if (_regexHelper.IsValidEmail(email) || _regexHelper.IsValidPassword(password)) return null;
            
            var acc = await GetAccount(email);
            if (_regexHelper.IsValidPassword(password))
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