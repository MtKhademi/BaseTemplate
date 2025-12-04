using IAMModule.IAM.Authorization;
using Infrastructure.Web.ApiResult;

namespace IAMModule.IAM.Features.ChangePassword;

internal class ChangePasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/api/BankingGateWay/v{apiVersion:apiVersion}/change-password", async (
                HttpContext context,
                [FromBody] ChangePasswordRequest request,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return (await sender.Send(request.ToCommand(userId), cancellationToken))
                    .ToApiResultSuccess();
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Update))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("BankingGateWayV1")
            .MapToApiVersion(1)
            .WithTags("AUTH")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Change Password")
            .WithDescription("Change user password");
    }
}