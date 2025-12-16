namespace IAMModule.User.Features.GetByKey;

internal class GetByKeyEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/cache/api/v{apiVersion:apiVersion}/{key}", async (
                [FromRoute] string key,
                [FromServices] ISender sender,
                [FromServices] ILogger<GetByKeyEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(new CacheGetByKeyQuery(key), cancellationToken))
                    .ToApiResultSuccess());
            })
            .WithPermission(CachePermissions.Read)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("Cache-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<string>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get value of cache base a key")
            .WithDescription("");
    }
}