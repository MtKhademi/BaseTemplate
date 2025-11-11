namespace Common.Messaging.Extentions;

public static class MassTransitExtentions
{
    public static IServiceCollection AddMassTransitWithAssemblies(
        this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        //services.AddMassTransit(cfg =>
        //{
        //    //cfg.SetKebabCaseEndpointNameFormatter();
        //    //cfg.SetInMemorySagaRepositoryProvider();

        //    //cfg.AddConsumers(assemblies);
        //    //cfg.AddSagaStateMachines(assemblies);
        //    //cfg.AddSagas(assemblies);
        //    //cfg.AddActivities(assemblies);

        //    //cfg.UsingRabbitMq((context, cfg) =>
        //    //{
        //    //    cfg.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
        //    //    {
        //    //        host.Username(configuration["MessageBroker:Username"]!);
        //    //        host.Password(configuration["MessageBroker:Password"]!);
        //    //    });

        //    //    cfg.ConfigureEndpoints(context);
        //    //});
        //});
        return services;
    }
}
