using ECommerceModule.Persistence.Context;
using IAMModule.Data.Context;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Test.Integration.Common;

public class WebAppFactory : WebApplicationFactory<Api.Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    private string? _connectionString;
    private bool _isInitialized;
    private string ConnectionString => _connectionString ??= _dbContainer
        .GetConnectionString()
        .Replace("master", "templatetestDb");
    public WebAppFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .Build();

    }
    internal async Task ClearDbAsync()
    {
        await InitializeAsync();

        var sqlPath = Path.Combine(AppContext.BaseDirectory, "ClearDb.sql");
        var clearScript = await File.ReadAllTextAsync(sqlPath);

        await using (var connection = new SqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(clearScript, connection);
            await command.ExecuteNonQueryAsync();
        }

        await using var scope = Services.CreateAsyncScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var item in seeders)
        {
            await item.SeedAsync();
        }
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connnectionString = ConnectionString;

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
        if (_isInitialized)
            return;

        await _dbContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();
        // Apply migrations before seeding to ensure schema exists
        var iamDb = scope.ServiceProvider.GetRequiredService<IAMModuleDbContext>();
        var ecommerceDb = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
        // await iamDb.Database.MigrateAsync();
        // await ecommerceDb.Database.MigrateAsync();

        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var item in seeders)
        {
            await item.SeedAsync();
        }

        _isInitialized = true;
    }
}

[CollectionDefinition("Collection tests v1", DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<WebAppFactory>
{
}


