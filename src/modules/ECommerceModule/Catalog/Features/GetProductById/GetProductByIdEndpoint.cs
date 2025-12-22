using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.GetProductById;

internal class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/ecommerce/api/v{apiVersion:apiVersion}/catalog/products/{productId}", async (
                int productId,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetProductByIdEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var query = new GetProductByIdQuery(productId);
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.ToProductResponse().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<ProductResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get product by ID")
            .WithDescription("Retrieves a product by ID");
    }
}
