namespace ECommerceModule.Contract.Order.Commands;

public record MarkOrderAsPaidCommand : ICommand<OrderModel>
{
    public int OrderId { get; init; }

    private MarkOrderAsPaidCommand(int orderId)
    {
        var errors = new List<string>();

        if (orderId <= 0)
            errors.Add($"{nameof(OrderId)} must be greater than 0.");

        if (errors.Any())
            throw new MarkOrderAsPaidCommandException(errors);

        OrderId = orderId;
    }

    public static MarkOrderAsPaidCommand Create(int orderId) => new(orderId);

    public static MarkOrderAsPaidCommand Create(MarkOrderAsPaidRequest request) => new(request.OrderId);

    internal class MarkOrderAsPaidCommandException : NotValidDataException<MarkOrderAsPaidCommandException>
    {
        public MarkOrderAsPaidCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
