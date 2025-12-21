using Infrastructure.Web;

namespace Infrastructure.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddConfig<TConfig>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TConfig : BaseConfig<TConfig>
    {
        var sectionName = typeof(TConfig).Name;
        var section = configuration.GetSection(sectionName);

        if (!section.Exists())
            throw new ArgumentNullException(
                sectionName,
                $"Configuration section '{sectionName}' is not found.");

        // Bind + register options
        services.Configure<TConfig>(section);

        // Validate AFTER DI is built (correct lifecycle)
        services.AddOptions<TConfig>()
            .Bind(section)
            .Validate(config =>
            {
                config.IsValidAndThrow();
                return true;
            });

        return services;
    }


    public static TService? TryGetRequiredKeyedService<TService>(this IServiceProvider serviceProvider, object key)
    {
        try
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (key is null)
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            var service = serviceProvider.GetRequiredKeyedService<TService>(key);
            if (service is null)
                throw new ArgumentException($"Service of type {typeof(TService).Name} with key {key} is not registered.", nameof(key));
            return service;
        }
        catch (InvalidOperationException) { return default(TService); }

    }
}
