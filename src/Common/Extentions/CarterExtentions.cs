namespace Infrastructure.Extentions;

public static class CarterExtentions
{
    public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddCarter(configurator: cfg =>
        {
            var catalogModule = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
            cfg.WithModules(catalogModule);
        });

        return services;
    }
}
