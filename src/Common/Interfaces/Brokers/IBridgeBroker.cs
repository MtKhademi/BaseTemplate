namespace Common.Interfaces.Brokers
{
    internal interface IBridgeBroker<TMessageModel> where TMessageModel : class
    {
        Task TransferMessage(
            IConsumerBrocker<TMessageModel> consumer,
            List<IProducerBroker<TMessageModel>> producers,
            CancellationToken cancellationToken);
    }
}
