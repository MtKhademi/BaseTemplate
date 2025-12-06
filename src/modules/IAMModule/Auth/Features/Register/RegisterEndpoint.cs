namespace IAMModule.IAM.Features.Register;

internal class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/iam/api/v{apiVersion:apiVersion}/auth/register", async (
                [FromBody] UserRegistrationRequest request,
                [FromServices] ISender sender,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok((await sender.Send(request.ToCommand(), cancellationToken)).ToApplicationUserResponse().ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("IAM-V1")
            .MapToApiVersion(1)
            .WithTags("AUTH")
            .IncludeInOpenApi()
            .Produces<ApiResult<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("User registration")
            .WithDescription("");
    }
}