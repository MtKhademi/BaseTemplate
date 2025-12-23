using ECommerceModule.Contract.Cart.Models;
using ECommerceModule.Contract.Cart.Responses;

namespace ECommerceModule.Cart.Extensions;

internal static class CartMappingExtensions
{
    public static CartModel ToCartModel(this CartEntity entity)
        => new(
            entity.Id,
            entity.UserId,
            entity.CreatedAt,
            entity.Items.Select(x => x.ToCartItemModel()).ToList()
        );

    public static CartResponse ToCartResponse(this CartModel model)
        => new(
            model.CartId,
            model.UserId,
            model.CreatedAt,
            model.Items.Select(x => x.ToCartItemResponse()).ToList(),
            model.Items.Sum(x => x.TotalPrice)
        );

    public static CartItemResponse ToCartItemResponse(this CartItemModel model)
        => new(
            model.CartItemId,
            model.CartId,
            model.ProductId,
            model.Quantity,
            model.UnitPrice,
            model.TotalPrice
        );

    public static PaginatedList<CartResponse> ToCartResponsePaginated(this PaginatedList<CartModel> list)
        => list.ToPaginatedList(x => x.ToCartResponse());
}
