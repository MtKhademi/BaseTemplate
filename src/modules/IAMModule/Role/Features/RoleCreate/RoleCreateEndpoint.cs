namespace IAMModule.Role.Features.RoleCreate;

internal class RoleCreateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/iam/api/v{apiVersion:apiVersion}/roles", async (
                [FromBody] RoleCreateRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<RoleCreateEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(request.ToRoleCreateCommand(), cancellationToken))
                    .ToRoleResponse()
                    .ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.RoleCreate)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("ROLE")
            .IncludeInOpenApi()
            .Produces<ApiResult<RoleResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("create a new role")
            .WithDescription("");
    }
}