namespace IAMModule.User.Features.UserCreate;

internal class UserCreateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/api/IAM/v{apiVersion:apiVersion}/user", async (
                [FromBody] UserCreateRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserCreateEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToUserCreateCommand(), cancellationToken))
                    .ToApplicationUserResponse()
                    .ToApiResultSuccess());
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Create))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<ApplicationUserResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("create a new user")
            .WithDescription("");
    }
}