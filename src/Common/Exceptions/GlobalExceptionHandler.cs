namespace Infrastructure.Exceptions;

public sealed class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next,
        ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
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

        var response = ApiResult.InternalServerError(ex);
        var json = response.ToJson();
        await context.Response.WriteAsync(json);
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