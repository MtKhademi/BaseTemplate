namespace Common.Hangfier;

public static class HangfireJobRegistrar
{
    public static IServiceCollection AddRecurringJobs(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        var jobTypes = assembliesToScan
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(IRecurringJob).IsAssignableFrom(t)
                    && !t.IsInterface
                    && !t.IsAbstract);

        foreach (var jobType in jobTypes)
        {
            services.AddScoped(typeof(IRecurringJob), jobType);
            // یا services.AddTransient(jobType);
        }

        return services;
    }

    public static void UseRecurringJobs(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("HangfireJobRegistrar");

        logger.LogInformation("RegisterAllRecurringJobs started...");

        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var jobs = scope.ServiceProvider.GetServices<IRecurringJob>();


        foreach (var job in jobs)
        {
            // Use the job’s type name (or something else) as the recurring job ID
            // so it’s unique per job implementation.
            recurringJobManager.AddOrUpdate(
                recurringJobId: job.Name,
                methodCall: () => job.ExecuteAsync(),
                cronExpression: job.CronExpression
            );
        }
    }

}