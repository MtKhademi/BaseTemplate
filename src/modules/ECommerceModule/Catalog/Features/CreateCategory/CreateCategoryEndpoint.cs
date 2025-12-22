using ECommerceModule.Auth;

namespace ECommerceModule.Catalog.Features.CreateCategory;

internal class CreateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/ecommerce/api/v{apiVersion:apiVersion}/catalog/categories", async (
                [FromBody] CreateCategoryRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<CreateCategoryEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(request.ToCommand(), cancellationToken);
                return Results.Ok(result.ToCategoryResponse().ToApiResultSuccess());
            })
            .WithPermission(ECommercePermissions.CatalogCreate)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CATALOG")
            .IncludeInOpenApi()
            .Produces<ApiResult<CategoryResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create category")
            .WithDescription("Creates a new category");
    }
}
