using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Exceptions;
using AccountService.Helpers;
using AccountService.Models;
using AccountService.Repositories;
using MessageBroker;
using Microsoft.Extensions.Options;

namespace AccountService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IHasher _hasher;
        private readonly IRegexHelper _regexHelper;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        private readonly MessageQueueSettings _messageQueueSettings;
        private readonly ITokenGenerator _tokenGenerator;

        public AccountService(IAccountRepository repository, IHasher hasher, ITokenGenerator tokenGenerator,
            IRegexHelper regexHelper, IMessageQueuePublisher messageQueuePublisher, IOptions<MessageQueueSettings> messageQueueSettings)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
            _regexHelper = regexHelper;
            _messageQueuePublisher = messageQueuePublisher;
            _messageQueueSettings = messageQueueSettings.Value;
        }

        public async Task<Account> CreateAccount(CreateAccountModel model)
        {
            var account = await _repository.Get(model.Email);

            if (account != null)
                throw new EmailAlreadyExistsException();

            if (!_regexHelper.IsValidEmail(model.Email))
                throw new InvalidEmailException();

            if (!_regexHelper.IsValidPassword(model.Password))
                throw new InvalidPasswordException("Password does not meet the minimum requirements.");

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

            newAccount = await _repository.Create(newAccount);
            
            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange, "EmailService", "RegisterUser", new {Email = newAccount.Email});

            return newAccount.WithoutSensitiveData();
        }
        
        public async Task<Account> Login(LoginModel loginModel)
        {
            var account = await _repository.Get(loginModel.Email);
            if (account == null) throw new EmailNotFoundException();

            if (!await _hasher.VerifyHash(loginModel.Password, account.Salt, account.Password))
                throw new IncorrectPasswordException();

            account.Token = _tokenGenerator.GenerateJwt(account.Id);
            
            return account.WithoutSensitiveData();
        }

        public async Task<Account> GetAccount(Guid id)
        {
            var account = await _repository.Get(id);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            return account.WithoutSensitiveData();
        }

        public async Task<Account> UpdatePassword(Guid id, ChangePasswordModel passwordModel)
        {
            var account = await _repository.Get(id);

            if (account == null)
            {
                throw new AccountNotFoundException();
            }
            
            if (!await _hasher.VerifyHash(passwordModel.OldPassword, account.Salt, account.Password))
            {
                throw new IncorrectPasswordException();
            }

            if (!_regexHelper.IsValidPassword(passwordModel.NewPassword))
            {
                throw new InvalidPasswordException("The new password does not meet the requirements.");
            }

            //hash the password. 
            var salt = _hasher.CreateSalt();
            var hashedPassword = await _hasher.HashPassword(passwordModel.NewPassword, salt);
            account.Salt = salt;
            account.Password = hashedPassword;
            var updatedAccount = await _repository.Update(account.Id, account);

            return updatedAccount.WithoutSensitiveData();
        }

        public async Task<Account> UpdateAccount(Guid id, UpdateAccountModel model)
        {
            if (!_regexHelper.IsValidEmail(model.Email)) throw new InvalidEmailException();
            
            var account = await _repository.Get(id);
            if (account == null) throw new AccountNotFoundException();
            
            var accountWithConflictingEmail = await _repository.Get(model.Email);
            if (accountWithConflictingEmail != null && account.Email != accountWithConflictingEmail.Email)
            {
                throw new EmailAlreadyExistsException();
            }
            
            account.Email = model.Email;
            account.isDelegate = model.isDelegate;
            account.isDAppOwner = model.isDAppOwner;

            var updatedAccount = await _repository.Update(id, account);
            if (updatedAccount == null) throw new AccountNotFoundException();
            return updatedAccount.WithoutSensitiveData();
        }

        public async Task DeleteAccount(Guid id)
        {
            if (await _repository.Get(id) == null)
            {
                throw new AccountNotFoundException();
            }
            
            await _repository.Remove(id);
        }
    }
}