namespace ECommerceModule.Contract.Cart.Requests;

public record AddItemToCartRequest(
    int CartId,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice
)
{
    public AddItemToCartCommand ToCommand() => AddItemToCartCommand.Create(this);
}
