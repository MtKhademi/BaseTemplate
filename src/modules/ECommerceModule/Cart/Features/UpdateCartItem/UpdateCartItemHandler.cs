using ECommerceModule.Contract.Cart.Commands;
using ECommerceModule.Contract.Cart.Models;

namespace ECommerceModule.Cart.Features.UpdateCartItem;

internal class UpdateCartItemHandler(ICartRepository repository)
    : ICommandHandler<UpdateCartItemCommand, CartItemModel>
{
    public async Task<CartItemModel> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
    {
        var cart = await repository.GetByIDAsync(command.CartId)
            ?? throw new EntityNotFoundException<CartEntity, int>(command.CartId);

        cart.UpdateItemQuantity(command.CartItemId, command.Quantity);

        await repository.UpdateAsync(cart);

        var updatedItem = cart.Items.FirstOrDefault(x => x.Id == command.CartItemId)
            ?? throw new EntityNotFoundException<CartItemEntity, int>(command.CartItemId);

        return updatedItem.ToCartItemModel();
    }
}
