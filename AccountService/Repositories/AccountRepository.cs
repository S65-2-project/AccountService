using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.DatastoreSettings;
using AccountService.Domain;
using AccountService.Messaging;
using AccountService.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AccountService.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;
        private IMessageQueuePublisher _publisher;

        public AccountRepository(IAccountDatabaseSettings settings, IMessageQueuePublisher publisher)
        {
            _publisher = publisher;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _accounts = database.GetCollection<Account>(settings.AccountCollectionName);
        }

        public async Task<List<Account>> Get() =>
            await _accounts.Find(f => true).ToListAsync();
        
        public async Task<Account> Get(string email) =>
            await _accounts.Find<Account>(f => f.Email == email).FirstOrDefaultAsync();

        public async Task<Account> Get(Guid id) =>
            await _accounts.Find<Account>(f => f.Id == id).FirstOrDefaultAsync();

        public async Task<Account> Create(Account account)
        {
            await _accounts.InsertOneAsync(account);
            var newAccount = new CreateAccountModel {Email = account.Email, Password = account.Password};
            await _publisher.PublishMessageAsync("AuthenticationService", "RegisterUser", newAccount);
            return account;
        }

        public async Task<Account> Update(Guid id, Account account)
        {
            await _accounts.ReplaceOneAsync(f => f == account, account);
            return account;
        }

        public async Task Remove(Guid id) =>
            await _accounts.DeleteManyAsync(f => f.Id == id);
    }
}