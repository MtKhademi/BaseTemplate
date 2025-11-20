using IAMModule.IAM.Authorization;

namespace IAMModule.IAM.Features.SignOut;

internal class SignOutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/api/BankingGateWay/v{apiVersion:apiVersion}/signout", async (
                HttpContext context,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return (await sender.Send(new SignOutCommand(userId), cancellationToken)).ToApiResultSuccess();
            })
            .RequireAuthorization()
            .WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Delete))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("BankingGateWayV1")
            .MapToApiVersion(1)
            .WithTags("AUTH")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Sign Out")
            .WithDescription("Sign out user and invalidate refresh token");
    }
}
