using AccountService.Domain;
using AccountService.Helpers;
using MessageBroker;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Publishers
{
    public class UserMarketplacePublisher : IUserMarketplacePublisher
    {
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        private readonly MessageQueueSettings _messageQueueSettings;
        public UserMarketplacePublisher(IMessageQueuePublisher messageQueuePublisher, IOptions<MessageQueueSettings> messageQueueSettings)
        {
            _messageQueuePublisher = messageQueuePublisher;
            _messageQueueSettings = messageQueueSettings.Value;
        }
        public async Task PublishDeleteUser(Guid id)
        {
            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange, "MarketplaceService", "delete-user", new { Id = id });
        }

        public async Task PublishUpdateUser(Account updatedAccount)
        {
            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange, "MarketplaceService", "update-user", new { Id = updatedAccount.Id, NewEmail = updatedAccount.Email});
        }
    }
}
