namespace ECommerceModule.Orders.Features.GetOrdersPaginated;

internal class GetOrdersPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/orders", async (
            [FromServices] ISender sender,
            [FromServices] ILogger<GetOrdersPaginatedEndpoint> logger,
            CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? userId = null) =>
            {
                var query = new GetOrdersPaginatedQuery(pageNumber, pageSize, userId);
                var result = await sender.Send(query, cancellationToken);

                return Results.Ok(
                    result
                    .ToOrderResponsePaginated()
                    .ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("ORDER")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<OrderResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get paginated orders")
            .WithDescription("Retrieves orders with pagination, optionally filtered by user ID");
    }
}
