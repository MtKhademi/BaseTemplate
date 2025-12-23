namespace IAMModule.Auth.Authentications;

internal static class JwtRESTAuthentication
{
    internal static IServiceCollection AddJwtRESTAuthentication(this IServiceCollection services)
    {

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();


        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<IAMModuleConfig>>((options, config) =>
            {
                var key = Encoding.UTF8.GetBytes(config.Value.SecretKey);

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = BuildJwtEvents();
            });

        return services;
    }

    internal static bool IsRESTRequest(this HttpRequest request)
    {
        return request.ContentType?.Contains("application/json") == true;
    }

    private static JwtBearerEvents BuildJwtEvents()
    {
        return new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<JwtBearerEvents>>();

                var tokenService = context.HttpContext.RequestServices
                    .GetRequiredService<ITokenService>();

                var userId = context.Principal?
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                var token = context.SecurityToken.ToString();

                if (!await tokenService.ValidTokenAsync(token, userId))
                {
                    context.Fail("Token is not valid or has been revoked.");
                }
            },

            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    context.Token = authHeader["Bearer ".Length..];

                return Task.CompletedTask;
            },

            OnChallenge = async context =>
            {
                context.HandleResponse();
                var exception = new JwtRESTUnauthorizedException(context.Request.Path);
                await exception.WriteResponseAsync(context.HttpContext);
            },

            OnForbidden = async context =>
            {
                var exception = new JwtRESTForbiddenException(context.Request.Path);
                await exception.WriteResponseAsync(context.HttpContext);
            }
        };
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