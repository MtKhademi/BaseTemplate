using ECommerceModule.Auth;
using Infrastructure.Module;
using Microsoft.Extensions.Options;

namespace ECommerceModule;

public static class ECommerceModule
{
    public static IServiceCollection AddECommerceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfig<ECommerceModuleConfig>(configuration);
        var assembly = typeof(ECommerceModule).Assembly;

        services.AddCarterWithAssemblies(assembly)
            .AddMediatRWithAssemblies(assembly)
            .AddMassTransitWithAssemblies(configuration, assembly)
            .AddRecurringJobs(assembly)
            .RegistersServices<IBaseRepository>(assembly);

        return services.AddServices();
    }

    public static IApplicationBuilder UseECommerceModule(this IApplicationBuilder app)
    {
        app.UseMigration<ECommerceDbContext>();
        return app;
    }

    public static SwaggerGenOptions AddSwaggerECommerceModule(this SwaggerGenOptions option)
    {
        option.SwaggerDoc("ECOMMERCE-V1", new Microsoft.OpenApi.OpenApiInfo
        {
            Title = "E-Commerce Module - API",
            Version = "v1",
        });

        return option;
    }

    public static SwaggerUIOptions UseSwaggerECommerceModule(this SwaggerUIOptions option)
    {
        option.SwaggerEndpoint("/swagger/ECOMMERCE-V1/swagger.json", "E-Commerce V1");
        return option;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddDbContext<ECommerceDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IOptions<ECommerceModuleConfig>>().Value;
            options.UseSqlServer(config.ConnectionString);
        });

        services.AddScoped<IUnitOfWork, ECommerceUnitOfWork>();
        services.AddScoped<IDataSeeder, ECommerceDbDataSeeder>();
        services.AddSingleton<IModulePermission, ECommerceModulePermissions>();

        return services;
    }
}
