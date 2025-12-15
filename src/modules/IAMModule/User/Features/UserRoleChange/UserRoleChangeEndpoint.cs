using Infrastructure.Auth;

namespace IAMModule.User.Features.UserRoleChange;

internal class UserRoleChangeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/iam/api/v{apiVersion:apiVersion}/users/{userId}/roles", async (
                [FromRoute(Name = "userId")] string userId,
                [FromBody] UserRolesChangeRequest request,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                if(request.UserId != userId)
                {
                    return Results.BadRequest(
                        ApiResult.BadRequest("UserId in route does not match UserId in request body."));
                }
                return Results.Ok(
                (await sender.Send(UserRoleChangeCommand.Create(request), cancellationToken))
                .ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserRoleChange)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("update role a user")
            .WithDescription("");
    }
}