using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CacheModule;

public static class CacheModule
{
    public static IServiceCollection AddCacheModule(this IServiceCollection services, IConfiguration configuration)
    {

        //var iamConfig = services.AddConfig<CacheModuleConfig>(configuration);
        var assembly = typeof(CacheModule).Assembly;
        services.AddCarterWithAssemblies(assembly)
            .AddMediatRWithAssemblies(assembly)
            .AddMassTransitWithAssemblies(configuration, assembly)
            .AddRecurringJobs(assembly)
            .RegistersServices<IBaseRepository>(assembly);

        return services;
        //return services.AddServices(configuration);
    }
    public static IApplicationBuilder UseCacheModule(this IApplicationBuilder app)
    {
        //app.UseMigration<CacheModuleDbContext>();
        return app;
    }

    public static SwaggerGenOptions AddSwaggerCacheModule(this SwaggerGenOptions option)
    {
        option.SwaggerDoc("CacheModuleV1", new Microsoft.OpenApi.OpenApiInfo
        {
            Title = "Cache Module - API",
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

    public static SwaggerUIOptions UseSwaggerCacheModule(this SwaggerUIOptions option)
    {
        option.SwaggerEndpoint("/swagger/CacheModuleV1/swagger.json", "Cache Module v1");

        return option;
    }
}