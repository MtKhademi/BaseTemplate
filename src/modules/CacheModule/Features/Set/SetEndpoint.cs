using CacheModule.Contract.Requests;

namespace IAMModule.User.Features.Set;

internal class SetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/cache/api/v{apiVersion:apiVersion}/{key}", async (
                [FromRoute] string key,
                [FromBody] CacheSetRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<SetEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(CacheSetCommand<string>.Create(request), cancellationToken))
                    .ToApiResultSuccess());
            })
            .WithPermission(CachePermissions.Create)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("Cache-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get value of cache base a key")
            .WithDescription("");
    }
}