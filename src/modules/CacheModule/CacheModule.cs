using IAMModule.Auth.Authorization;

namespace CacheModule;

public static class CacheModule
{
    public static IServiceCollection AddCacheModule(this IServiceCollection services, IConfiguration configuration)
    {

        var iamConfig = services.AddConfig<CacheModuleConfig>(configuration);
        var assembly = typeof(CacheModule).Assembly;
        services.AddCarterWithAssemblies(assembly)
            .AddMediatRWithAssemblies(assembly)
            .AddMassTransitWithAssemblies(configuration, assembly)
            .AddRecurringJobs(assembly)
            .RegistersServices<IBaseRepository>(assembly);


        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddSingleton<IModulePermission, CacheModulePermissions>();

        return services;
    }
    public static IApplicationBuilder UseCacheModule(this IApplicationBuilder app)
    {
        //app.UseMigration<CacheModuleDbContext>();
        return app;
    }

    public static SwaggerGenOptions AddSwaggerCacheModule(this SwaggerGenOptions option)
    {
        option.SwaggerDoc("Cache-V1", new Microsoft.OpenApi.OpenApiInfo
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
        option.SwaggerEndpoint("/swagger/Cache-V1/swagger.json", "Cache V1");

        return option;
    }
}