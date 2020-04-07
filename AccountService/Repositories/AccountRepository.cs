using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.DatastoreSettings;
using AccountService.Domain;
using MongoDB.Driver;

namespace AccountService.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountRepository(IAccountDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _accounts = database.GetCollection<Account>(settings.AccountCollectionName);
        }

        public Task<List<Account>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Account> Get(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Account> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Account> Create(Account account)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, Account account)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}