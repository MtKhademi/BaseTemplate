using ECommerceModule.Contract.Cart.Commands;

namespace ECommerceModule.Cart.Features.RemoveItemFromCart;

internal class RemoveItemFromCartHandler(ICartRepository repository)
    : ICommandHandler<RemoveItemFromCartCommand, bool>
{
    public async Task<bool> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await repository.GetByIDAsync(command.CartId)
            ?? throw new EntityNotFoundException<CartEntity, int>(command.CartId);

        cart.RemoveItem(command.CartItemId);

        await repository.UpdateAsync(cart);
        return true;
    }
}
