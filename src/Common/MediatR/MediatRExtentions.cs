namespace Infrastructure.MediatR;

public static class MediatRExtentions
{
    public static IServiceCollection AddMediatRWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(LogginBehavior<,>));
        });

        return services;
    }
}
