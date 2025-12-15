using IAMModule.Auth.Authorization;
using IAMModule.Services;
using Infrastructure.Auth.Authorization;
using Microsoft.OpenApi;
using System.Reflection.Metadata;

namespace IAMModule;

public static class IAMModule
{
    public static IServiceCollection AddIAMModule(this IServiceCollection services, IConfiguration configuration)
    {

        var iamConfig = services.AddConfig<IAMModuleConfig>(configuration);
        var assembly = typeof(IAMModule).Assembly;
        services.AddCarterWithAssemblies(assembly)
            .AddMediatRWithAssemblies(assembly)
            .AddMassTransitWithAssemblies(configuration, assembly)
            .AddRecurringJobs(assembly)
            .RegistersServices<IBaseRepository>(assembly);

        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IUserPermissionService, UserPermissionSerivce>();

        services.AddScoped<IUserPermissionService, UserPermissionSerivce>();
        services.Decorate<IUserPermissionService, UserPermissionCacheService>();

        return services.AddServices(iamConfig)
            .AddIdentitySettings()
            .AddJwtRESTAuthentication(iamConfig)
            .AddSingleton<IAppModulePermission, AppModulePermissions>();
    }
    public static IApplicationBuilder UseIAMModule(this WebApplication app)
    {
        app.PopulatePermissionsFromEndpoints();
        app.UseMigration<IAMModuleDbContext>();
        return app;
    }

    public static SwaggerGenOptions AddSwaggerIAMModule(this SwaggerGenOptions option)
    {

        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid JWT token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOi...\""
        });

        option.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });

        //option.AddSecurityRequirement(new OpenApiSecurityRequirement
        //{

        //    //{
        //    //new OpenApiSecurityScheme
        //    //{
        //    //    Reference = new OpenApiReference
        //    //    {
        //    //        Type = ReferenceType.SecurityScheme,
        //    //        Id = "Bearer"
        //    //    }
        //    //},
        //    //Array.Empty<string>()
        //    //}
        //});


        option.SwaggerDoc("IAM-V1", new OpenApiInfo
        {
            Title = "I AM Module - API",
            Version = "v1",
        });
        //option.DocInclusionPredicate((version, apiDescription) =>
        //    {
        //        var groupName = apiDescription.GroupName;
        //        return groupName != null && groupName.Equals(version, StringComparison.OrdinalIgnoreCase);
        //    });
        //});

        return option;
    }
    public static SwaggerUIOptions UseSwaggerIAMModule(this SwaggerUIOptions option)
    {
        option.SwaggerEndpoint("/swagger/IAM-V1/swagger.json", "IAM V1");

        return option;
    }

    private static IServiceCollection AddIdentitySettings(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<IAMModuleDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
    private static IServiceCollection AddServices(this IServiceCollection services, IAMModuleConfig config)
    {
        services.AddDbContext<IAMModuleDbContext>(options =>
            options.UseSqlServer(config.ConnectionString));

        services.AddHttpContextAccessor();

        services.AddScoped<IUnitOfWork, IAMModuleUnitOfWork>();
        services.AddScoped<IDataSeeder, IAMModuleDbDataSeeder>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, IdentityTokenService>();

        return services;
    }
}