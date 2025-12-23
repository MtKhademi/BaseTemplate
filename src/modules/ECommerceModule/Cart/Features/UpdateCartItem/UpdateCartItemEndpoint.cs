using ECommerceModule.Contract.Cart.Requests;
using ECommerceModule.Contract.Cart.Responses;

namespace ECommerceModule.Cart.Features.UpdateCartItem;

internal class UpdateCartItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/ecommerce/api/v{apiVersion:apiVersion}/cart/items/{cartItemId}", async (
                [FromRoute(Name = "cartItemId")] int cartItemId,
                [FromBody] UpdateCartItemRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UpdateCartItemEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(request.ToCommand(), cancellationToken);
                return Results.Ok(result.ToCartItemResponse().ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CART")
            .IncludeInOpenApi()
            .Produces<ApiResult<CartItemResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update cart item")
            .WithDescription("Update the quantity of an item in a cart");
    }
}
