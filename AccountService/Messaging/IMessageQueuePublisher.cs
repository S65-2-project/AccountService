using System.Threading.Tasks;

namespace AccountService.Messaging
{
    public interface IMessageQueuePublisher
    {
        Task PublishMessageAsync<T>(string routingKey, string messageType, T value);

    }
}