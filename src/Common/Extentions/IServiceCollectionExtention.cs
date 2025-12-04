using Infrastructure.Web;

namespace Infrastructure.Extentions;

public static class ServiceCollectionExtention
{
    public static ConfigType AddConfig<ConfigType>(this IServiceCollection services, IConfiguration configuration)
        where ConfigType : BaseConfig<ConfigType>
    {
        var config = configuration.GetSection(typeof(ConfigType).Name);
        var configConverted = config.Get<ConfigType>();

        if (config is null || configConverted is null)
            throw new ArgumentNullException(typeof(ConfigType).Name, $"Configuration section '{typeof(ConfigType).Name}' is not found or is not valid.");

        configConverted.IsValidAndThrow();

        services.Configure<ConfigType>(config);


        return configConverted;

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
            if(service is null)
                throw new ArgumentException($"Service of type {typeof(TService).Name} with key {key} is not registered.", nameof(key));
            return service;
        }
        catch (InvalidOperationException) { return default(TService); }

    }
}
