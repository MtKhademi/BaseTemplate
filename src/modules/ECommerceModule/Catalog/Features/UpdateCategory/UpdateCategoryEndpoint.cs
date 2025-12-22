using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.UpdateCategory;

internal class UpdateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/ecommerce/api/v{apiVersion:apiVersion}/catalog/categories/{categoryId}", async (
                int categoryId,
                [FromBody] UpdateCategoryRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UpdateCategoryEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var command = request with { CategoryId = categoryId };
                var result = await sender.Send(command.ToCommand(), cancellationToken);
                return Results.Ok(result.ToCategoryResponse().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogUpdate)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<CategoryResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update category")
            .WithDescription("Updates an existing category");
    }
}
