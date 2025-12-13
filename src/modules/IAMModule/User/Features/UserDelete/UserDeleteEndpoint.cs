using Infrastructure.Auth.Authorization;

namespace IAMModule.User.Features.UserDelete;

internal class UserDeleteEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("/iam/api/v{apiVersion:apiVersion}/users/{userId}", async (
                [FromRoute(Name = "userId")] string userId,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserDeleteEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                (await sender.Send(UserDeleteCommand.Create(userId), cancellationToken)).ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserCreate)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("delete a user")
            .WithDescription("");
    }
}