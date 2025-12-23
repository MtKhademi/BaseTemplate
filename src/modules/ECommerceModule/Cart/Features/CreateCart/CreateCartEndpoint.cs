using ECommerceModule.Contract.Cart.Requests;
using ECommerceModule.Contract.Cart.Responses;

namespace ECommerceModule.Cart.Features.CreateCart;

internal class CreateCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/ecommerce/api/v{apiVersion:apiVersion}/cart", async (
                [FromBody] CreateCartRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<CreateCartEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(request.ToCommand(), cancellationToken);
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
            .WithSummary("Create cart")
            .WithDescription("Creates a new cart for a user");
    }
}
