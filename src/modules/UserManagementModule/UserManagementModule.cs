using UserManagementModule.IAM.Authentications;
using UserManagementModule.IAM.Services;

namespace UserManagementModule;

public static class UserManagementModule
{
    public static IServiceCollection AddUserManagementModule(this IServiceCollection services, IConfiguration configuration)
    {

        var iamConfig = services.AddConfig<UserManagementModuleConfig>(configuration);
        var assembly = typeof(UserManagementModule).Assembly;
        services.AddCarterWithAssemblies(assembly)
            .AddMediatRWithAssemblies(assembly)
            .AddMassTransitWithAssemblies(configuration, assembly)
            .AddRecurringJobs(assembly)
            .RegistersServices<IBaseRepository>(assembly);

        return services.AddServices(iamConfig)
            .AddIdentitySettings()
            .AddJwtRESTAuthentication(iamConfig);
    }
    public static IApplicationBuilder UseUserManagementModule(this IApplicationBuilder app)
    {
        app.UseMigration<UserManagementModuleDbContext>();
        return app;
    }

    public static SwaggerGenOptions AddSwaggerUserManagementModule(this SwaggerGenOptions option)
    {
        option.SwaggerDoc("USER-MANAGEMENT-V1", new Microsoft.OpenApi.OpenApiInfo
        {
            Title = "USER MANAGEMENT Module - API",
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
    public static SwaggerUIOptions UseSwaggerUserManagementModule(this SwaggerUIOptions option)
    {
        option.SwaggerEndpoint("/swagger/USER-MANAGEMENT-V1/swagger.json", "User Management v1");

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
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<UserManagementModuleDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
    private static IServiceCollection AddServices(this IServiceCollection services, UserManagementModuleConfig config)
    {
        services.AddDbContext<UserManagementModuleDbContext>(options =>
            options.UseSqlServer(config.ConnectionString));

        services.AddHttpContextAccessor();

        services.AddScoped<IUnitOfWork, UserManagementModuleUnitOfWork>();
        services.AddScoped<IDataSeeder, UserManagementModuleDbDataSeeder>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, IdentityTokenService>();

        return services;
    }
}