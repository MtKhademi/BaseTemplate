using Infrastructure.Auth.Authorization;

namespace IAMModule.UserManagement.Features.UserGetById;

internal class UserGetByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/iam/api/v{apiVersion:apiVersion}/users/{userId}", async (
                [FromRoute(Name = "userId")] string userId,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserGetByIdEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(new UserGetByIdQuery(userId), cancellationToken))
                    .ToApplicationUserResponse().ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<ApplicationUserResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("get user by id")
            .WithDescription("");
    }
}