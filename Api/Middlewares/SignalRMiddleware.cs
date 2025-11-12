namespace SolutionSolverApi.Middlewares;

public class SignalRMiddleware
{
    private readonly RequestDelegate _next;

    public SignalRMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("Origin", out var originValue))
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", originValue);

            if (httpContext.Request.Method == "OPTIONS")
            {
                httpContext.Response.StatusCode = 204;
                return httpContext.Response.WriteAsync(string.Empty);
            }
        }
        else
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        return _next(httpContext);
    }
}
