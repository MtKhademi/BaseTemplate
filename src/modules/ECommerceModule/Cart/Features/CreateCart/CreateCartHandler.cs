namespace ECommerceModule.Cart.Features.CreateCart;

internal class CreateCartHandler(ICartRepository repository)
    : ICommandHandler<CreateCartCommand, CartModel>
{
    public async Task<CartModel> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var entity = new CartEntity(command.UserId);
        await repository.CreateAsync(entity, cancellationToken);
        return entity.ToCartModel();
    }
}
