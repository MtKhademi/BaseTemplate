using ECommerceModule.Contract.Order.Commands;

namespace ECommerceModule.Orders.Features.MarkOrderAsPaid;

internal class MarkOrderAsPaidEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/ecommerce/api/v{apiVersion:apiVersion}/orders/{orderId}/mark-as-paid", async (
                int orderId,
                [FromServices] ISender sender,
                [FromServices] ILogger<MarkOrderAsPaidEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var command = MarkOrderAsPaidCommand.Create(orderId);
                return Results.Ok(
                    (await sender.Send(command, cancellationToken))
                    .ToOrderResponse()
                    .ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("ORDER")
            .IncludeInOpenApi()
            .Produces<ApiResult<OrderResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Mark order as paid")
            .WithDescription("Updates order status to paid");
    }
}
