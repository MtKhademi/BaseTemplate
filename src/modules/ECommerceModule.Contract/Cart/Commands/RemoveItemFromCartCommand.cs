namespace ECommerceModule.Contract.Cart.Commands;

public record RemoveItemFromCartCommand : ICommand<bool>
{
    public int CartId { get; init; }
    public int CartItemId { get; init; }

    private RemoveItemFromCartCommand(int cartId, int cartItemId)
    {
        var errors = new List<string>();
        if (cartId <= 0) errors.Add("CartId must be greater than zero.");
        if (cartItemId <= 0) errors.Add("CartItemId must be greater than zero.");
        if (errors.Any()) throw new RemoveItemFromCartCommandException(errors);
        CartId = cartId;
        CartItemId = cartItemId;
    }

    public static RemoveItemFromCartCommand Create(int cartId, int cartItemId) => new(cartId, cartItemId);
    public static RemoveItemFromCartCommand Create(RemoveItemFromCartRequest request)
        => new(request.CartId, request.CartItemId);

    internal class RemoveItemFromCartCommandException : NotValidDataException<RemoveItemFromCartCommandException>
    {
        public RemoveItemFromCartCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
