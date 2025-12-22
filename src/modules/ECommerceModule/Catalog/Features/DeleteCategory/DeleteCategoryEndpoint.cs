using ECommerceModule.Auth;
using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.DeleteCategory;

internal class DeleteCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("/ecommerce/api/v{apiVersion:apiVersion}/catalog/categories/{categoryId}", async (
                int categoryId,
                [FromServices] ISender sender,
                [FromServices] ILogger<DeleteCategoryEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteCategoryCommand(categoryId);
                var result = await sender.Send(command, cancellationToken);
                return Results.Ok(result.ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogDelete)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete category")
            .WithDescription("Deletes a category");
    }
}
