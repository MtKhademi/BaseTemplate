using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.GetCategoriesPaginated;

internal class GetCategoriesPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/catalog/categories", async (
                [AsParameters] GetCategoriesPaginatedQuery query,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetCategoriesPaginatedEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.ToCategoryResponsePaginated().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<CategoryResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get categories paginated")
            .WithDescription("Retrieves paginated list of categories");
    }
}
