namespace ECommerceModule.Contract.Cart.Requests;

public record UpdateCartItemRequest(
    int CartId,
    int CartItemId,
    int Quantity
)
{
    public UpdateCartItemCommand ToCommand() => UpdateCartItemCommand.Create(this);
}
