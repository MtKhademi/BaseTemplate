using ECommerceModule.Auth;
using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.DeleteProduct;

internal class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("/ecommerce/api/v{apiVersion:apiVersion}/catalog/products/{productId}", async (
                int productId,
                [FromServices] ISender sender,
                [FromServices] ILogger<DeleteProductEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteProductCommand(productId);
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
            .WithSummary("Delete product")
            .WithDescription("Deletes a product");
    }
}
