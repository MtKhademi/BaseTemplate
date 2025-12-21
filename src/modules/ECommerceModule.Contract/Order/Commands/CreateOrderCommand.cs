namespace ECommerceModule.Contract.Order.Commands;

public record CreateOrderCommand : ICommand<OrderModel>
{
    public Guid UserId { get; init; }
    public IList<CreateOrderItemCommand> Items { get; init; }

    private CreateOrderCommand(
        Guid userId,
        IList<CreateOrderItemCommand> items)
    {
        var errors = new List<string>();

        if (userId == Guid.Empty)
            errors.Add($"{nameof(UserId)} is required.");

        if (items == null || !items.Any())
            errors.Add($"{nameof(Items)} must contain at least one item.");

        if (errors.Any())
            throw new CreateOrderCommandException(errors);

        UserId = userId;
        Items = items ?? new List<CreateOrderItemCommand>();
    }

    public static CreateOrderCommand Create(
        Guid userId,
        IList<CreateOrderItemCommand> items) => new(userId, items);

    public static CreateOrderCommand Create(CreateOrderRequest request) =>
        new(
            request.UserId,
            request.Items.Select(x => CreateOrderItemCommand.Create(
                x.ProductId,
                x.Quantity,
                x.UnitPrice
            )).ToList()
        );

    public record CreateOrderItemCommand
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }

        private CreateOrderItemCommand(int productId, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public static CreateOrderItemCommand Create(int productId, int quantity, decimal unitPrice)
        {
            var errors = new List<string>();
            if (productId <= 0) errors.Add($"{nameof(productId)} must be greater than zero.");
            if (quantity <= 0) errors.Add($"{nameof(quantity)} must be greater than zero.");
            if (unitPrice <= 0) errors.Add($"{nameof(unitPrice)} must be greater than zero.");

            if (errors.Any())
                throw new CreateOrderCommandException(errors);

            return new CreateOrderItemCommand(productId, quantity, unitPrice);
        }
    };

    internal class CreateOrderCommandException : NotValidDataException<CreateOrderCommandException>
    {
        public CreateOrderCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
