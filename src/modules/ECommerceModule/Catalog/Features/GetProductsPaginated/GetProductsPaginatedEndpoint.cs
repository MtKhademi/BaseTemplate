using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.GetProductsPaginated;

internal class GetProductsPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/catalog/products", async (
                [AsParameters] GetProductsPaginatedQuery query,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetProductsPaginatedEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.ToProductResponsePaginated().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<ProductResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get products paginated")
            .WithDescription("Retrieves paginated list of products");
    }
}
