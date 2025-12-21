using Microsoft.Extensions.Hosting;

namespace Infrastructure.Exceptions;

public sealed class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(RequestDelegate next,
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotValidDataException ex) { await HandleNotValidDataExceptionAsync(context, ex); }
        catch (AlreadyExistException ex) { await HandleAlreadyExistDataExceptionAsync(context, ex); }
        catch (NotFoundException ex) { await HandleNotExistDataExceptionAsync(context, ex); }
        catch (UnauthorizedException ex) { await HandleUnauthorizedExceptionAsync(context, ex); }
        catch (ForbiddenException ex) { await HandleForbiddenExceptionExceptionAsync(context, ex); }
        catch (Exception ex)
        {
            _logger.LogCritical(exception: ex, message: "=========== CRITICAL ERROR ================");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = 500;

        if (_env.IsDevelopment())
        {
            var devMessages = BuildDevelopmentErrorMessages(context, ex);
            var response = ApiResult.InternalServerError(devMessages);
            await context.Response.WriteAsync(response.ToJson());
            return;
        }

        var prodResponse = ApiResult.InternalServerError(ex);
        await context.Response.WriteAsync(prodResponse.ToJson());
    }

    private static string[] BuildDevelopmentErrorMessages(HttpContext context, Exception ex)
    {
        var messages = new List<string>
        {
            $"Exception: {ex.GetType().FullName}",
            $"Message: {ex.Message}",
            $"Path: {context.Request.Path}",
            $"Method: {context.Request.Method}",
            $"TraceId: {context.TraceIdentifier}"
        };

        if (ex.InnerException is not null)
            messages.Add($"InnerException: {ex.InnerException.GetType().FullName} | {ex.InnerException.Message}");

        if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            messages.Add($"StackTrace: {ex.StackTrace}");

        return messages.ToArray();
    }
    private async Task HandleForbiddenExceptionExceptionAsync(HttpContext context, ForbiddenException ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        var response = ApiResult.Forbidden(ex);
        await context.Response.WriteAsync(response.ToJson());
    }
    private async Task HandleUnauthorizedExceptionAsync(HttpContext context, UnauthorizedException ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        var response = ApiResult.Unauthorized(ex);
        await context.Response.WriteAsync(response.ToJson());
    }
    private async Task HandleNotExistDataExceptionAsync(HttpContext context, NotFoundException ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        var response = ApiResult.NotFound(ex);
        await context.Response.WriteAsync(response.ToJson());
    }
    private async Task HandleAlreadyExistDataExceptionAsync(HttpContext context, AlreadyExistException ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;

        var response = ApiResult.AlreadyExists(ex);
        await context.Response.WriteAsync(response.ToJson());
    }
    private async Task HandleNotValidDataExceptionAsync(HttpContext context, NotValidDataException ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = ApiResult.BadRequest(ex);
        await context.Response.WriteAsync(response.ToJson());
    }
}