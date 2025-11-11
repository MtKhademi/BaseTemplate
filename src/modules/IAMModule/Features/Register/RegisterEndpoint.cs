//namespace IAMModule.Features.Login;

//internal class RegisterEndpoint : ICarterModule
//{
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        var versionSet = app.NewApiVersionSet()
//            .HasApiVersion(new ApiVersion(1, 0))
//            .ReportApiVersions()
//            .Build();

//        app.MapPost("/api/BankingGateWay/v{apiVersion:apiVersion}/register", async (
//                [FromBody] UserRegistrationRequest request,
//                [FromServices] ISender sender,
//                CancellationToken cancellationToken) =>
//            {
//                request.Validate();
//                return (await sender.Send(request.ToCommand(), cancellationToken)).ToApiResultSuccess();
//            })
//            .WithMetadata(new ApiVersion(1, 0))
//            .WithApiVersionSet(versionSet)
//            .WithGroupName("BankingGateWayV1")
//            .MapToApiVersion(1)
//            .WithTags("USER MANAGEMENT")
//            .IncludeInOpenApi()
//            .Produces<ApiResult<TokenResponse>>(StatusCodes.Status200OK)
//            .ProducesProblem(StatusCodes.Status400BadRequest)
//            .ProducesProblem(StatusCodes.Status500InternalServerError)
//            .WithSummary("User Registration")
//            .WithDescription("This endpoint allows a user to register by providing their details such as email, password, and other required information. Upon successful registration, a token is returned for authentication purposes.");
//    }
//}