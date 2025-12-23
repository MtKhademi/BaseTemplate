using ECommerceModule.Contract.Cart.Models;
using ECommerceModule.Contract.Cart.Queries;

namespace ECommerceModule.Cart.Features.GetCartsPaginated;

internal class GetCartsPaginatedHandler(ICartRepository repository)
    : IQueryHandler<GetCartsPaginatedQuery, PaginatedList<CartModel>>
{
    public async Task<PaginatedList<CartModel>> Handle(GetCartsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var items = await repository.QueryNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.ToCartModel())
            .ToPaginatedListAsync(query, cancellationToken);

        return items;
    }
}
