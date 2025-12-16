namespace IAMModule.User.Features.ClearAll;

internal class ClearAllEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("/cache/api/v{apiVersion:apiVersion}", async (
                [FromServices] ISender sender,
                [FromServices] ILogger<ClearAllEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(new CacheClearAllCommand(), cancellationToken))
                    .ToApiResultSuccess());
            })
            .WithPermission(CachePermissions.Delete)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("Cache-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("delete all cache")
            .WithDescription("");
    }
}