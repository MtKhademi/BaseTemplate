using ECommerceModule.Contract.Cart.Commands;

namespace ECommerceModule.Cart.Features.DeleteCart;

internal class DeleteCartHandler(ICartRepository repository)
    : ICommandHandler<DeleteCartCommand, bool>
{
    public async Task<bool> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await repository.GetByIDAsync(command.CartId)
            ?? throw new EntityNotFoundException<CartEntity, int>(command.CartId);

        await repository.DeleteHardAsync(command.CartId, cancellationToken);
        return true;
    }
}
