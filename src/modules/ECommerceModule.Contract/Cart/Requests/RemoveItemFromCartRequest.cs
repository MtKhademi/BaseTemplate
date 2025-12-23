namespace ECommerceModule.Contract.Cart.Requests;

public record RemoveItemFromCartRequest(
    int CartId,
    int CartItemId
)
{
    public RemoveItemFromCartCommand ToCommand() => RemoveItemFromCartCommand.Create(this);
}
