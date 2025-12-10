namespace NotificationModule;

public static class NotificationModule
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
    {

        var iamConfig = services.AddConfig<NotificationModuleConfig>(configuration);
        var assembly = typeof(NotificationModule).Assembly;
        services.AddCarterWithAssemblies(assembly)
            .AddMediatRWithAssemblies(assembly)
            .AddMassTransitWithAssemblies(configuration, assembly)
            .AddRecurringJobs(assembly);
            //.RegistersServices<IBaseRepository>(assembly);



        return services;
    }
    public static IApplicationBuilder UseNotificationModule(this IApplicationBuilder app)
    {
        //app.UseMigration<NotificationModuleDbContext>();
        return app;
    }

    public static SwaggerGenOptions AddSwaggerNotificationModule(this SwaggerGenOptions option)
    {
        option.SwaggerDoc("NOTIFICATION-V1", new Microsoft.OpenApi.OpenApiInfo
        {
            Title = "Notification Module - API",
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
    public static SwaggerUIOptions UseSwaggerNotificationModule(this SwaggerUIOptions option)
    {
        option.SwaggerEndpoint("/swagger/NOTIFICATION-V1/swagger.json", "Notification V1");

        return option;
    }
}