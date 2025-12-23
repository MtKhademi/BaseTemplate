using ECommerceModule.Contract.Cart.Queries;
using ECommerceModule.Contract.Cart.Responses;

namespace ECommerceModule.Cart.Features.GetCartsPaginated;

internal class GetCartsPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/cart", async (
                [AsParameters] GetCartsPaginatedQuery query,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetCartsPaginatedEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.ToCartResponsePaginated().ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CART")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<CartResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get carts paginated")
            .WithDescription("Get all carts with pagination");
    }
}
