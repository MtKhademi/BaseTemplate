namespace IAMModule.IAM.Features.Login;

internal class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/api/iam/v{apiVersion:apiVersion}/login", async (
                [FromBody] LoginRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<LoginEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return (await sender.Send(request.ToCommand(), cancellationToken)).ToApiResultSuccess();
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("IAM")
            .IncludeInOpenApi()
            .Produces<ApiResult<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("login")
            .WithDescription("login in gate way");
    }
}