namespace ECommerceModule.Contract.Cart.Requests;

public record CreateCartRequest(
    Guid UserId
)
{
    public CreateCartCommand ToCommand() => CreateCartCommand.Create(this);
}
