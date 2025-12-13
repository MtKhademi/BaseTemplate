using IAMModule.Auth.Authorization;
using IAMModule.Auth.Services;
using Infrastructure.Auth.Authorization;

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
        option.SwaggerDoc("IAM-V1", new Microsoft.OpenApi.OpenApiInfo
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