using ECommerceModule.Contract.Cart.Commands;
using ECommerceModule.Contract.Cart.Models;

namespace ECommerceModule.Cart.Features.AddItemToCart;

internal class AddItemToCartHandler(ICartRepository repository)
    : ICommandHandler<AddItemToCartCommand, CartItemModel>
{
    public async Task<CartItemModel> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await repository.GetByIDAsync(command.CartId)
            ?? throw new EntityNotFoundException<CartEntity, int>(command.CartId);

        var cartItem = new CartItemEntity(command.CartId, command.ProductId, command.Quantity, command.UnitPrice);
        cart.AddItem(cartItem);

        await repository.UpdateAsync(cart);
        return cartItem.ToCartItemModel();
    }
}
