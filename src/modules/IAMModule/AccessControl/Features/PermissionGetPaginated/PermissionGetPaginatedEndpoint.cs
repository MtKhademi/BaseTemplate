namespace IAMModule.AccessControl.Features.PermissionGetPaginated;

internal class PermissionGetPaginatedEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/iam/api/v{apiVersion:apiVersion}/access-controll/permissions", async (
                [AsParameters] PermissionGetPaginatedRequest request,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToQuery(), cancellationToken))
                    .ToPaginatedList(x => x.ToPermissionResponse()).ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.AccessControlRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("Access-Controll")
            .IncludeInOpenApi()
            .Produces<ApiResult<PaginatedList<PermissionResponse>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("get Permissions paginated list")
            .WithDescription("");
    }
}