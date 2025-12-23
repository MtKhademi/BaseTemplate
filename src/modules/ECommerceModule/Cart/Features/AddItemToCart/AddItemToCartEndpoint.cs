using ECommerceModule.Contract.Cart.Requests;
using ECommerceModule.Contract.Cart.Responses;

namespace ECommerceModule.Cart.Features.AddItemToCart;

internal class AddItemToCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/ecommerce/api/v{apiVersion:apiVersion}/cart/items", async (
                [FromBody] AddItemToCartRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<AddItemToCartEndpoint> logger,
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
            .WithSummary("Add item to cart")
            .WithDescription("Add an item to a cart");
    }
}
