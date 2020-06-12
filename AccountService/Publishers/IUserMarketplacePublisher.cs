using AccountService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Publishers
{
    public interface IUserMarketplacePublisher
    {
        public Task PublishUpdateUser(Account updatedAccount);
        public Task PublishDeleteUser(Guid id);
    }
}
