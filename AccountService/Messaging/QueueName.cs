namespace AccountService.Messaging
{
    public class QueueName
    {
        public string Name { get; }
        public QueueName(string name)
        {
            Name = name;
        }
    }
}