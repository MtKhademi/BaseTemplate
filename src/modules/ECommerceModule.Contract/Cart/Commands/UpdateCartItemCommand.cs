namespace ECommerceModule.Contract.Cart.Commands;

public record UpdateCartItemCommand : ICommand<CartItemModel>
{
    public int CartId { get; init; }
    public int CartItemId { get; init; }
    public int Quantity { get; init; }

    private UpdateCartItemCommand(int cartId, int cartItemId, int quantity)
    {
        var errors = new List<string>();
        if (cartId <= 0) errors.Add("CartId must be greater than zero.");
        if (cartItemId <= 0) errors.Add("CartItemId must be greater than zero.");
        if (quantity <= 0) errors.Add("Quantity must be greater than zero.");
        if (errors.Any()) throw new UpdateCartItemCommandException(errors);
        CartId = cartId;
        CartItemId = cartItemId;
        Quantity = quantity;
    }

    public static UpdateCartItemCommand Create(int cartId, int cartItemId, int quantity)
        => new(cartId, cartItemId, quantity);
    public static UpdateCartItemCommand Create(UpdateCartItemRequest request)
        => new(request.CartId, request.CartItemId, request.Quantity);

    internal class UpdateCartItemCommandException : NotValidDataException<UpdateCartItemCommandException>
    {
        public UpdateCartItemCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
