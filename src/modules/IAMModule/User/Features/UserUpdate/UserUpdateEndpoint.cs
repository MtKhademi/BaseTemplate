using Infrastructure.Web.ApiResult;

namespace IAMModule.User.Features.UserUpdate;

internal class UserUpdateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/api/IAM/v{apiVersion:apiVersion}/user/{user-id}", async (
                [FromRoute(Name = "user-id")] string userId,
                [FromBody] UserUpdateRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserUpdateEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                (await sender.Send(request.ToUserUpdateCommand(userId), cancellationToken))
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
            .WithSummary("update a user")
            .WithDescription("");
    }
}