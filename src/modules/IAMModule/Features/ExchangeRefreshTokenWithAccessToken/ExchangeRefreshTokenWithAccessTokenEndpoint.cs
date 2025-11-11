namespace IAMModule.Features.Login;

internal class ExchangeRefreshTokenWithAccessTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPut("/api/BankingGateWay/v{apiVersion:apiVersion}/exchange-refresh-token-with-access-token", async (
                [FromBody] TokenCreateWithRefreshTokenRequest request,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                request.Validate();
                return (await sender.Send(request.ToCommand(), cancellationToken)).ToApiResultSuccess();
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("BankingGateWayV1")
            .MapToApiVersion(1)
            .WithTags("AUTH")
            .IncludeInOpenApi()
            .Produces<ApiResult<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("exchange refresh token with access token")
            .WithDescription("Exchanges a refresh token for a new access token");
    }
}