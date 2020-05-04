using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Helpers;
using AccountService.Models;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services
{
    public class AccService : IAccService
    {
        //TODO Custom exception for every argument
        private readonly IAccountRepository _repository;
        private readonly IHasher _hasher;
        private readonly IRegexHelper _regexHelper = new RegexHelper();

        public AccService(IAccountRepository repository, IHasher hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        public async Task<Account> CreateAccount(CreateAccountModel model)
        {
            var acc = await _repository.Get(model.Email);

            if (acc != null)
                throw new ArgumentException("Email is already in use.");
            
            if(!_regexHelper.IsValidEmail(model.Email))
                throw new ArgumentException("Email is not a valid email.");
            
            if (!_regexHelper.IsValidPassword(model.Password))
                throw new ArgumentException("Password doesn't meet the requirements.");
            
            //hash the password. 
            var salt = _hasher.CreateSalt();
            var hashedPassword = await _hasher.HashPassword(model.Password, salt);
            
            //Create new User object and send to repository
            var newAccount = new Account()
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Password = hashedPassword,
                Salt = salt
            };

            await _repository.Create(newAccount);
            
            return newAccount.WithoutPassword();
        }

        public async Task<Account> UpdatePassword(Guid id, UpdateAccountModel model)
        {
            if (_regexHelper.IsValidPassword(model.Password)) return null;
            
            var acc = await GetAccount(id);
            if (_regexHelper.IsValidPassword(model.Password))
            {
                //hash the password. 
                var salt = _hasher.CreateSalt();
                var hashedPassword = await _hasher.HashPassword(model.Password, salt);
                acc.Salt = salt;
                acc.Password = hashedPassword;
            }
            else
            {
                return null;
            }
            
            return await _repository.Update(acc.Id, acc);
        }

        public async Task<Account> UpdateAccount(Guid id, UpdateAccountModel model)
        {
            if (_regexHelper.IsValidEmail(model.Email)) return null;

            var acc = await GetAccount(id);
            acc.Email = model.Email;
            acc.isDelegate = model.isDelegate;
            acc.isDAppOwner = model.isDelegate;
            
            return await _repository.Update(id, acc);
        }

        public async Task<Account> GetAccount(Guid id)
        {
            var acc = await _repository.Get(id);
            return acc;
        }

        public async Task DeleteAccount(string email)
        {
            var acc = await _repository.Get(email);
            await _repository.Remove(acc.Id);
        }
    }
}