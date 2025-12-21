using ECommerceModule.Persistence.Context;
using IAMModule.Data.Context;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Test.Integration.Common;

public class WebAppFactory : WebApplicationFactory<Api.Program>, IAsyncLifetime
{
    private MsSqlContainer _dbContainer;
    public WebAppFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .Build();

    }
    internal async Task ClearDbAsync()
    {
        var scope = Services.CreateScope();
        var iamDb = scope.ServiceProvider.GetRequiredService<IAMModuleDbContext>();
        var ecommerceDb = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        await iamDb.Database.EnsureDeletedAsync();
        await ecommerceDb.Database.EnsureDeletedAsync();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connnectionString = _dbContainer.GetConnectionString()
            .Replace("master", "templatetestDb");

        builder.UseEnvironment("Development");

        // Configure test settings so they override any default configuration
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            // Clear existing sources to ensure our test config takes precedence
            configBuilder.Sources.Clear();

            var testConfig = new Dictionary<string, string?>
            {
                // E-Commerce module config
                ["ECommerceModuleConfig:ConnectionString"] = connnectionString,

                // IAM module config
                ["IAMModuleConfig:ConnectionString"] = connnectionString,
                ["IAMModuleConfig:SecretKey"] = "test-secret-key-with-at-least-32-characters-for-jwt",
                ["IAMModuleConfig:TokenExpiryInMinutes"] = "60",

                // Cache module config
                ["CacheModuleConfig:CacheType"] = "1",

                // Notification module config
                ["NotificationModuleConfig:SmsConfig:Provider"] = "TestProvider",
                ["NotificationModuleConfig:SmsConfig:UserName"] = "test-user",
                ["NotificationModuleConfig:SmsConfig:Password"] = "test-pass",
            };

            // Add test config first
            configBuilder.AddInMemoryCollection(testConfig);

            // Then add appsettings if needed for other settings
            configBuilder.AddJsonFile("appsettings.json", optional: true);
            configBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
            configBuilder.AddEnvironmentVariables();
        });

        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {

            services.RemoveAll<DbContextOptions<IAMModuleDbContext>>();
            services.AddDbContext<IAMModuleDbContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });

            services.RemoveAll<DbContextOptions<ECommerceDbContext>>();
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });
        });
    }
    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
    public async Task InitializeAsync()
    {

        await _dbContainer.StartAsync();

        var scope = Services.CreateScope();
        // Apply migrations before seeding to ensure schema exists
        var iamDb = scope.ServiceProvider.GetRequiredService<IAMModuleDbContext>();
        var ecommerceDb = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
        await iamDb.Database.MigrateAsync();
        await ecommerceDb.Database.MigrateAsync();

        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var item in seeders)
        {
            await item.SeedAsync();
        }
    }
}

[CollectionDefinition("Collection tests v1", DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<WebAppFactory>
{
}


