namespace NotificationModule.Features.SmsNotification;

internal class SmsNotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("/notification/api/v{apiVersion:apiVersion}/sms", async (
                [FromBody] SmsNotificationRequest request,
                [FromServices] ISender sender,
                [FromServices] ILogger<SmsNotificationEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(
                    (await sender.Send(SmsNotificationCommand.Create(request), cancellationToken))
                    .ToResponse()
                    .ToApiResultSuccess());
            })
            .RequireAuthorization()
            ////.WithMetadata(new MustHavePermissionAttribute(AppFeature.IAMModule, AppActions.Create))
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("NOTIFICATION-V1")
            .MapToApiVersion(1)
            .WithTags("SMS")
            .IncludeInOpenApi()
            .Produces<ApiResult<NotificationResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("send a sms")
            .WithDescription("");
    }
}