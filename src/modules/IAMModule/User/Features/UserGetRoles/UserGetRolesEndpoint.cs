namespace IAMModule.User.Features.UserGetRoles;

internal class UserGetRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/api/IAM/v{apiVersion:apiVersion}/user/{user-name}/roles", async (
            [FromRoute(Name = "user-name")] string userName,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(UserGetRolesQuery.Create(userName), cancellationToken))
                    .Select(role => role.ToUserRoleResponse())
                    .ToApiResultSuccess());
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Read))
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