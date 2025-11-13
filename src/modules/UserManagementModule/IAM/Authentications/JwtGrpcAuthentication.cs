//using Microsoft.AspNetCore.Http;
//using Microsoft.CodeAnalysis;

//namespace UserManagementModule.Authentications;

//internal static class JwtGrpcAuthentication
//{
//    internal static IServiceCollection AddJwtGrpcAuthentication(this IServiceCollection services, UserManagementModuleConfig config)
//    {
//        var key = Encoding.UTF8.GetBytes(config.Secret);

//        services.AddAuthentication(auth =>
//        {
//            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        })
//            .AddJwtBearer(bearer =>
//            {
//                bearer.RequireHttpsMetadata = false;
//                bearer.SaveToken = true;
//                bearer.TokenValidationParameters = new TokenValidationParameters()
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateAudience = false,
//                    ValidateIssuer = false,
//                    RoleClaimType = ClaimTypes.Role,
//                    ClockSkew = TimeSpan.Zero
//                };

//                bearer.Events = new JwtBearerEvents
//                {
//                    OnTokenValidated = async context =>
//                    {
//                        // Get UserManager from DI
//                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
//                        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
//                        var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

//                        logger.LogInformation("Validating token for user ID: {UserId}", userId);

//                        // Get token from header
//                        var token = context.SecurityToken.ToString();

//                        if (!await tokenService.ValidTokenAsync(token, userId))
//                        {
//                            context.HttpContext.Items["AuthErrorMessage"] = "Token is not valid or has been revoked.";
//                            context.Fail("Token is not valid or has been revoked.");
//                        }
//                    },
//                    OnMessageReceived = context =>
//                    {
//                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//                        {
//                            context.Token = authHeader["Bearer ".Length..];
//                        }
//                        return Task.CompletedTask;
//                    },

//                    OnAuthenticationFailed = c =>
//                    {
//                        if (c.Exception is SecurityTokenValidationException)
//                        {

//                            if (c.Request.ContentType?.Contains("application/grpc") == true)
//                            {
//                                // For gRPC requests, return a gRPC status code
//                                c.Response.StatusCode = (int)HttpStatusCode.OK; // gRPC always returns 200 OK for application-level errors
//                                var trailers = new Metadata
//                                {
//                                    { "grpc-status", ((int)StatusCode.Unauthenticated).ToString() },
//                                    { "grpc-message", "Authentication failed: " + c.Exception.Message }
//                                };
//                                foreach (var entry in trailers)
//                                {
//                                    c.Response.Headers.Add(entry.Key, entry.Value);
//                                }
//                            }
//                            else
//                            {
//                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                                c.Response.ContentType = "application/json";
//                                var result = Newtonsoft.Json.JsonConvert.SerializeObject(
//                                    ApiResult.UnAuthorize("Authentication failed: " + c.Exception.Message,
//                                        "BankingGateWayUnAuthorize"));
//                                return c.Response.WriteAsync(result);
//                            }
//                        }

//                        return Task.CompletedTask;
//                    },
//                    OnChallenge = context =>
//                    {
//                        context.HandleResponse();
//                        if (!context.Response.HasStarted)
//                        {
//                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                            context.Response.ContentType = "application/json";
//                            var errorMessage = context.HttpContext.Items.ContainsKey("AuthErrorMessage")
//                                ? context.HttpContext.Items["AuthErrorMessage"]?.ToString()
//                                : "BankingGateWay-You are not Authorized";
//                            var result = JsonConvert.SerializeObject(
//                                ApiResult.UnAuthorize(errorMessage, "BankingGateWayUnAuthorize"));

//                            return context.Response.WriteAsync(result);
//                        }

//                        return Task.CompletedTask;
//                    },
//                    OnForbidden = context =>
//                    {
//                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
//                        context.Response.ContentType = "application/json";
//                        var result = JsonConvert.SerializeObject(
//                                ApiResult.FailureResult("You are not Authorized to access this resource"));
//                        return context.Response.WriteAsync(result);
//                    }
//                };
//            });


//        services.AddAuthorization(options =>
//        {
//            foreach (var prop in typeof(AppPermissions)
//                         .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
//            {
//                var propertyValue = prop.GetValue(null);
//                if (propertyValue is not null)
//                {
//                    var permissions = propertyValue as IReadOnlyList<AppPermission>;
//                    foreach (var permission in permissions)
//                    {
//                        options.AddPolicy(permission.Name,
//                            policy => policy.RequireClaim(AppClaim.Permission, permission.Name));
//                    }
//                }
//            }
//        });

//        return services;
//    }
//}