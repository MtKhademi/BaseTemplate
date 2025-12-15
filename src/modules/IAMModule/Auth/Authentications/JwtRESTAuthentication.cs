using IAMModule.Services;
using Infrastructure.Auth.Authorization;

namespace IAMModule.IAM.Authentications;

internal static class JwtRESTAuthentication
{
    internal static IServiceCollection AddJwtRESTAuthentication(this IServiceCollection services, IAMModuleConfig config)
    {
        var key = Encoding.UTF8.GetBytes(config.SecretKey);

        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                bearer.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        // Get UserManager from DI
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                        var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

                        logger.LogInformation("Validating token for user ID: {UserId}", userId);

                        // Get token from header
                        var token = context.SecurityToken.ToString();

                        if (!await tokenService.ValidTokenAsync(token, userId))
                        {
                            context.HttpContext.Items["AuthErrorMessage"] = "Token is not valid or has been revoked.";
                            context.Fail("Token is not valid or has been revoked.");
                        }
                    },
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader["Bearer ".Length..];
                        }
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = async context =>
                    {
                        if (context.Exception is SecurityTokenValidationException)
                        {

                            if (context.Request.IsRESTRequest())
                            {
                                var exception = new JwtRESTNotValidTokenException($"Token validation failed: {context.Exception.Message}");
                                await exception.WriteResponseAsync(context.HttpContext);
                            }
                        }
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            var exception = new JwtRESTUnauthorizedException(location: context.Request.Path);
                            await exception.WriteResponseAsync(context.HttpContext);
                        }
                    },
                    OnForbidden = async context =>
                    {
                        var exception = new JwtRESTForbiddenException(location: context.Request.Path);
                        await exception.WriteResponseAsync(context.HttpContext);
                    }
                };
            });

        return services;
    }

    internal static bool IsRESTRequest(this HttpRequest request)
    {
        return request.ContentType?.Contains("application/json") == true;
    }
}


internal class JwtRESTNotValidTokenException : NotValidDataException<JwtRESTNotValidTokenException>
{
    public JwtRESTNotValidTokenException(string errors) : base(errors)
    {
    }

    public async Task WriteResponseAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        var response = ApiResult.BadRequest(this);
        await context.Response.WriteAsync(response.ToJson());
    }
}

internal class JwtRESTUnauthorizedException : UnauthorizedException<JwtRESTUnauthorizedException>
{
    public JwtRESTUnauthorizedException(string location) : base(location)
    {
    }
    public async Task WriteResponseAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        var response = ApiResult.Unauthorized(this);
        await context.Response.WriteAsync(response.ToJson());
    }
}

internal class JwtRESTForbiddenException : ForbiddenException<JwtRESTForbiddenException>
{
    public JwtRESTForbiddenException(string location) : base(location)
    {
    }
    public async Task WriteResponseAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        var response = ApiResult.Forbidden(this);
        await context.Response.WriteAsync(response.ToJson());
    }
}