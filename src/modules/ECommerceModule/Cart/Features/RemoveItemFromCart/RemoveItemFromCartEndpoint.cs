using ECommerceModule.Contract.Cart.Commands;

namespace ECommerceModule.Cart.Features.RemoveItemFromCart;

internal class RemoveItemFromCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("/ecommerce/api/v{apiVersion:apiVersion}/cart/items/{cartItemId}", async (
                [FromRoute(Name = "cartItemId")] int cartItemId,
                [FromQuery] int cartId,
                [FromServices] ISender sender,
                [FromServices] ILogger<RemoveItemFromCartEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(RemoveItemFromCartCommand.Create(cartId, cartItemId), cancellationToken);
                return Results.Ok(true.ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CART")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Remove item from cart")
            .WithDescription("Remove an item from a cart");
    }
}
