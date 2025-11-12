namespace SolutionSolverApi.Middlewares;

public static class JWTMiddlewareExtensions
{
    public static IApplicationBuilder UseSignalRCredentialHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SignalRMiddleware>();
    }

}
