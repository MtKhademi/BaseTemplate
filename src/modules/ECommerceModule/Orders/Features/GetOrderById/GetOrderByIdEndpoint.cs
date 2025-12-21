using ECommerceModule.Auth;

namespace ECommerceModule.Orders.Features.GetOrderById;

internal class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/orders/{orderId}", async (
                int orderId,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetOrderByIdEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var query = new GetOrderByIdQuery(orderId);
                return Results.Ok(
                    (await sender.Send(query, cancellationToken))
                    .ToOrderResponse()
                    .ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.Read)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("ORDER")
            .IncludeInOpenApi()
            .Produces<ApiResult<OrderResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get order by ID")
            .WithDescription("Retrieves a specific order by its ID");
    }
}
