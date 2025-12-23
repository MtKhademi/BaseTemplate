namespace ECommerceModule.Contract.Cart.Commands;

public record AddItemToCartCommand : ICommand<CartItemModel>
{
    public int CartId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }

    private AddItemToCartCommand(int cartId, Guid productId, int quantity, decimal unitPrice)
    {
        var errors = new List<string>();
        if (cartId <= 0) errors.Add("CartId must be greater than zero.");
        if (productId == Guid.Empty) errors.Add("ProductId is required.");
        if (quantity <= 0) errors.Add("Quantity must be greater than zero.");
        if (unitPrice < 0) errors.Add("UnitPrice cannot be negative.");
        if (errors.Any()) throw new AddItemToCartCommandException(errors);
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static AddItemToCartCommand Create(int cartId, Guid productId, int quantity, decimal unitPrice)
        => new(cartId, productId, quantity, unitPrice);
    public static AddItemToCartCommand Create(AddItemToCartRequest request)
        => new(request.CartId, request.ProductId, request.Quantity, request.UnitPrice);

    internal class AddItemToCartCommandException : NotValidDataException<AddItemToCartCommandException>
    {
        public AddItemToCartCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
