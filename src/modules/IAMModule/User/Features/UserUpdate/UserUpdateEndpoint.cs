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

        app.MapPut("/iam/api/v{apiVersion:apiVersion}/users/{userId}", async (
                [FromRoute(Name = "userId")] string userId,
                [FromBody] UserUpdateRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserUpdateEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                if(userId!= request.UserId)
                {
                    logger.LogWarning("User id from route {RouteUserId} is different from user id from body {BodyUserId}", userId, request.UserId);
                    return Results.BadRequest(ApiResult.BadRequest("User id from route is different from user id from body"));
                }

                return Results.Ok(
                (await sender.Send(request.ToUserUpdateCommand(), cancellationToken))
                .ToApplicationUserResponse()
                .ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserUpdate)
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