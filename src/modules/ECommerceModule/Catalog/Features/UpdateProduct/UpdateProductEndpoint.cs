using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.UpdateProduct;

internal class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/ecommerce/api/v{apiVersion:apiVersion}/catalog/products/{productId}", async (
                int productId,
                [FromBody] UpdateProductRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UpdateProductEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                if (productId != request.ProductId)
                {
                    return Results.BadRequest(
                            ApiResult.BadRequest($"Product ID in the URL does not match the ID in the request body.")
                        );
                }
                var command = request with { ProductId = productId };
                var result = await sender.Send(command.ToCommand(), cancellationToken);
                return Results.Ok(result.ToProductResponse().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogUpdate)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<ProductResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update product")
            .WithDescription("Updates an existing product");
    }
}
