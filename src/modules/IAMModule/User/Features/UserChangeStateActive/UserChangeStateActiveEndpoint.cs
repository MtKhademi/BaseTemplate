namespace IAMModule.User.Features.UserChangeStateActive;

internal class UserChangeStateActiveEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPatch("/iam/api/v{apiVersion:apiVersion}/users/{userId}/change-state", async (
                [FromRoute(Name = "userId")] string userId,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                (await sender.Send(new UserChangeStateActiveCommand(userId), cancellationToken))
                .ToApplicationUserResponse()
                .ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserChangeState)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<ApplicationUserResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Change active state of an user")
            .WithDescription("");
    }
}