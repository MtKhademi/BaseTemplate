namespace IAMModule.Role.Features.RoleUpdate;

internal class RoleUpdateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/iam/api/v{apiVersion:apiVersion}/roles", async (
                [FromBody] RoleUpdateRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<RoleUpdateEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToRoleUpdateCommand(), cancellationToken))
                    .ToRoleResponse()
                    .ToApiResultSuccess());
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Update))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("ROLE")
            .IncludeInOpenApi()
            .Produces<ApiResult<RoleResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("update a role")
            .WithDescription("");
    }
}