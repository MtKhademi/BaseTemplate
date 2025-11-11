namespace CacheModule.Features.SetCache;

public class LoginRequest { }
internal class SetCacheEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/api/BankingGateWay/v{apiVersion:apiVersion}/login", async (
                [FromBody] LoginRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<SetCacheEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                //return (await sender.Send(request.ToCommand(), cancellationToken)).ToApiResultSuccess();
                throw new NotImplementedException();
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("CacheModuleV1")
            .MapToApiVersion(1)
            .WithTags("CACHE")
            .IncludeInOpenApi()
            .Produces<ApiResult<string>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("set cache")
            .WithDescription("set cache in gate way");
    }
}