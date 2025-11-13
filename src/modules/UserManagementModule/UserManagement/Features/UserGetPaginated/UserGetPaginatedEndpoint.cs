using Common.Pagination;

namespace UserManagementModule.UserManagement.Features.UserGetPaginated;

internal class UserGetPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/api/user-management/v{apiVersion:apiVersion}/user", async (
                [AsParameters] UserGetPaginatedRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<UserGetPaginatedEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToQuery(), cancellationToken))
                    .ToPaginatedList(x => x.ToResponse()).ToApiResultSuccess());
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.UserManagementModule, AppActions.Read))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("USER-MANAGEMENT-V1")
            .MapToApiVersion(1)
            .WithTags("USER-MANAGEMENT")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<ApplicationUserResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("UserGetPaginated")
            .WithDescription("");
    }
}