using ECommerceModule.Contract.Cart.Queries;

namespace ECommerceModule.Cart.Features.GetCartById;

internal class GetCartByIdHandler(ICartRepository repository)
    : IQueryHandler<GetCartByIdQuery, CartModel>
{
    public async Task<CartModel> Handle(GetCartByIdQuery query, CancellationToken cancellationToken)
    {
        var cart = await repository.GetByIDAsync(query.CartId)
            ?? throw new EntityNotFoundException<CartEntity, int>(query.CartId);
        return cart.ToCartModel();
    }
}
