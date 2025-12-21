using ECommerceModule.Auth;

namespace ECommerceModule.Orders.Features.CreateOrder;

internal class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/ecommerce/api/v{apiVersion:apiVersion}/orders", async (
                [FromBody] CreateOrderRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<CreateOrderEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToCreateOrderCommand(), cancellationToken))
                    .ToOrderResponse()
                    .ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.Create)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("ORDER")
            .IncludeInOpenApi()
            .Produces<ApiResult<OrderResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new order")
            .WithDescription("Creates a new order with items");
    }
}
