namespace IAMModule.Role.Features.RoleGetPaginated;

internal class RoleGetPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/api/IAM/v{apiVersion:apiVersion}/role", async (
                [AsParameters] RoleGetPaginatedRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<RoleGetPaginatedEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToQuery(), cancellationToken))
                    .ToPaginatedList(x => x.ToRoleResponse()).ToApiResultSuccess());
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Read))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("ROLE")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<RoleResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("get role paginated list")
            .WithDescription("");
    }
}