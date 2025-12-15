using Infrastructure.Auth;

namespace IAMModule.UserManagement.Features.UserGetPaginated;

internal class UserGetPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/iam/api/v{apiVersion:apiVersion}/users", async (
                [AsParameters] UserGetPaginatedRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserGetPaginatedEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToQuery(), cancellationToken))
                    .ToPaginatedList(x => x.ToApplicationUserResponse()).ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.UserRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("USER")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<ApplicationUserResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("get users paginated list")
            .WithDescription("");
    }
}