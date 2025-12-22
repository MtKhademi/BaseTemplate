using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.GetCategoryById;

internal class GetCategoryByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/catalog/categories/{categoryId}", async (
                int categoryId,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetCategoryByIdEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCategoryByIdQuery(categoryId);
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.ToCategoryResponse().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<CategoryResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get category by ID")
            .WithDescription("Retrieves a category by ID");
    }
}
