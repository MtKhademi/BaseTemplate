using ECommerceModule.Contract.Cart.Queries;
using ECommerceModule.Contract.Cart.Responses;

namespace ECommerceModule.Cart.Features.GetCartById;

internal class GetCartByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/cart/{cartId}", async (
                [FromRoute(Name = "cartId")] int cartId,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetCartByIdEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(GetCartByIdQuery.Create(cartId), cancellationToken);
                return Results.Ok(result.ToCartResponse().ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CART")
            .IncludeInOpenApi()
            .Produces<ApiResult<CartResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get cart by id")
            .WithDescription("Get a specific cart by its id");
    }
}
