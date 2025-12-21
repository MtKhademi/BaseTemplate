using System;
using System.Linq;
using ECommerceModule.Contract.Order.Models;
using ECommerceModule.Contract.Order.Responses;
using Infrastructure.Pagination;
using ECommerceModule.Orders.Entities;

namespace ECommerceModule.Orders.Extensions;

internal static class OrderMappingExtensions
{
    public static OrderModel ToOrderModel(this OrderEntity order)
    {
        return new OrderModel(
            order.Id,
            order.UserId,
            order.TotalAmount,
            order.Status,
            order.CreatedAt ?? DateTime.UtcNow,
            order.Items.Select(x => x.ToOrderItemModel()).ToList()
        );
    }

    public static OrderItemModel ToOrderItemModel(this OrderItemEntity item)
    {
        return new OrderItemModel(
            item.Id,
            item.OrderId,
            item.ProductId,
            item.Quantity,
            item.UnitPrice,
            item.Quantity * item.UnitPrice
        );
    }

    public static OrderResponse ToOrderResponse(this OrderModel model)
    {
        return new OrderResponse(
            model.OrderId,
            model.UserId,
            model.TotalAmount,
            model.Status,
            model.CreatedAt,
            model.Items.Select(x => x.ToOrderItemResponse()).ToList()
        );
    }

    public static OrderItemResponse ToOrderItemResponse(this OrderItemModel model)
    {
        return new OrderItemResponse(
            model.OrderItemId,
            model.ProductId,
            model.Quantity,
            model.UnitPrice,
            model.TotalPrice
        );
    }

    public static PaginatedList<OrderResponse> ToOrderResponsePaginated(
        this PaginatedList<OrderModel> result)
    {
        return result.ToPaginatedList(x => x.ToOrderResponse());
    }
}
