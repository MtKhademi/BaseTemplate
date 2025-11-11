namespace Common.Interfaces.Brokers
{
    internal interface IProducerBroker<TMessageModel> : IDisposable where TMessageModel : class
    {
        Task SendMessage(TMessageModel message);
    }
}
