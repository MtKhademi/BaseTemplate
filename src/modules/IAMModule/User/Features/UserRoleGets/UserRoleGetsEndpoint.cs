using Infrastructure.Auth;

namespace IAMModule.User.Features.UserRoleGets;

internal class UserRoleGetsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/iam/api/v{apiVersion:apiVersion}/users/{userId}/roles", async (
            [FromRoute(Name = "userId")] string userId,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(UserRoleGetsQuery.Create(userId), cancellationToken))
                    .ToUserRoleResponse()
                    .ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserRoles)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<IEnumerable<UserRoleResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("get roles for a user paginated list")
            .WithDescription("");
    }
}