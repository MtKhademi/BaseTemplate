using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.CreateProduct;

internal class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/ecommerce/api/v{apiVersion:apiVersion}/catalog/products", async (
                [FromBody] CreateProductRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<CreateProductEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(request.ToCommand(), cancellationToken);
                return Results.Ok(result.ToProductResponse().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogCreate)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<ProductResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create product")
            .WithDescription("Creates a new product");
    }
}
