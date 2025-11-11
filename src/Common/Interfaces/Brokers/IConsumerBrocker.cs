namespace Common.Interfaces.Brokers
{
    internal interface IConsumerBrocker<TMessageModel> : IDisposable where TMessageModel : class
    {
        public bool IsAlive { get; }
        Task ReadMessages(Action<TMessageModel> readMessageAction, CancellationToken cancellationToken);
    }
}
