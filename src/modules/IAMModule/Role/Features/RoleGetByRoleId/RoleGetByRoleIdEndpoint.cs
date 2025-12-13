using Infrastructure.Auth.Authorization;

namespace IAMModule.Role.Features.RoleGetByRoleId;

internal class RoleGetByRoleIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapGet("/iam/api/v{apiVersion:apiVersion}/roles/{roleId}", async (
                [FromRoute(Name ="roleId")] string? roleId,
                [FromServices] ISender sender,
                [FromServices] ILogger<RoleGetByRoleIdEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(RoleGetByRoleIdQuery.Create(roleId), cancellationToken))
                        .ToRoleResponse().ToApiResultSuccess());
            })
            .WithPermission(IAMPermissions.RoleRead)
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("ROLE")
            .IncludeInOpenApi()
            .Produces<ApiResult<RoleResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("get role by role id")
            .WithDescription("");
    }
}