using Microsoft.CodeAnalysis;

namespace IAMModule.Authentications;

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

                    OnAuthenticationFailed = c =>
                    {
                        if (c.Exception is SecurityTokenValidationException)
                        {

                            if (c.Request.IsRESTRequest())
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                throw new JwtRESTNotValidTokenException($"Token validation failed: {c.Exception.Message}");
                            }
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            throw new JwtRESTUnauthorizedException(location: context.Request.Path);

                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        throw new JwtRESTForbiddenException(location: context.Request.Path);
                    }
                };
            });


        services.AddAuthorization(options =>
        {
            foreach (var prop in typeof(AppPermissions)
                         .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    var permissions = propertyValue as IReadOnlyList<AppPermission>;
                    foreach (var permission in permissions)
                    {
                        options.AddPolicy(permission.Name,
                            policy => policy.RequireClaim(AppClaim.Permission, permission.Name));
                    }
                }
            }
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
}

internal class JwtRESTUnauthorizedException : UnauthorizedException<JwtRESTUnauthorizedException>
{
    public JwtRESTUnauthorizedException(string location) : base(location)
    {
    }
}

internal class JwtRESTForbiddenException : NotValidDataException<JwtRESTForbiddenException>
{
    public JwtRESTForbiddenException(string location) : base(location)
    {
    }
}