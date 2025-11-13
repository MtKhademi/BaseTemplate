namespace UserManagementModule.IAM.Features.Register;

internal class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/api/iam/v{apiVersion:apiVersion}/register", async (
                [FromBody] UserRegistrationRequest request,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok((await sender.Send(request.ToCommand(), cancellationToken)).ToResponse().ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("USER-MANAGEMENT-V1")
            .MapToApiVersion(1)
            .WithTags("IAM")
            .IncludeInOpenApi()
            .Produces<ApiResult<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("User Registration")
            .WithDescription("This endpoint allows a user to register by providing their details such as email, password, and other required information. Upon successful registration, a token is returned for authentication purposes.");
    }
}