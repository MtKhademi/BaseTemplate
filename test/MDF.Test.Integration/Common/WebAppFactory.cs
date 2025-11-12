using MDF.Modules.Modules.MutualFundModule.ExternalServices.Jobs;
using MDF.DAL.Modules.Entities.OldEntities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Thinktecture;
using Modules.MutualFundModule.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using SymbolModule.Data;
using AnnouncementModule.Data;
using MDF.Common.Authorization;

namespace Test.Integration.ModulesTest.Common;

public class WebAppFactory : WebApplicationFactory<MDF.API.Program>, IAsyncLifetime
{
    private MsSqlContainer _dbContainer;
    public UserManagementModel User => new UserManagementModel
    {
        Name = TestConstants.USER_NAME
    };
    public WebAppFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .Build();

    }
    public Mock<IJWTUserHandlerService> IjwtUserHandlerServiceMoq
    {
        get
        {
            var IjwtUserMock = new Mock<IJWTUserHandlerService>();
            IjwtUserMock.Setup(x => x.GetModelFromToken(It.IsAny<string>())).Returns(User);

            return IjwtUserMock;
        }
    }

    internal async Task ClearDbAsync()
    {
        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
        var script = File.ReadAllText("ClearDb.sql");
        await dbContext.Database.ExecuteSqlRawAsync(script);
    }

    internal DatabaseRepositoryFixture Repositories
    {
        get
        {
            var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
            return new DatabaseRepositoryFixture(dbContext);
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connnectionString = _dbContainer.GetConnectionString()
            .Replace("master", "DataProvider");

        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {

            services.RemoveAll<DbContextOptions<SharedDbContext>>();
            services.AddDbContext<SharedDbContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });

            services.RemoveAll<DbContextOptions<DatabaseContext>>();
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });

            services.RemoveAll<DbContextOptions<SymbolDbContext>>();
            services.AddDbContext<SymbolDbContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });

            services.RemoveAll<DbContextOptions<MutualFundDbContext>>();
            services.AddDbContext<MutualFundDbContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });


            services.RemoveAll<DbContextOptions<AnnouncementDbContext>>();
            services.AddDbContext<AnnouncementDbContext>(options =>
            {
                options.UseSqlServer(connnectionString,
                    options =>
                    {
                        options.AddWindowFunctionsSupport();
                    });
            });

            services.RemoveAll<IJWTUserHandlerService>();
            services.AddScoped(_ => IjwtUserHandlerServiceMoq.Object);

            //services.RemoveAll<IMfHubService>();
            //services.AddScoped(_ => MfHubServiceMoq.Object);


            var descriptorBGServices = services
            .Where(x => !x.IsKeyedService)
            .Where(x => x.ImplementationType == typeof(FundNavGetFromRabbitMQAndSaveBGService)).ToList();
            if (descriptorBGServices.Any())
                descriptorBGServices.ForEach(des => services.Remove(des));
        });
    }
    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
    public async Task InitializeAsync()
    {

        await _dbContainer.StartAsync();
       
        var connnectionString = _dbContainer.GetConnectionString();

        var script = File.ReadAllText("DUMP2.sql");
        var result = await _dbContainer.ExecScriptAsync(script);

        using (var scope = Services.CreateScope())
        {
            var scopeProvider = scope.ServiceProvider;
            var _context = scopeProvider.GetRequiredService<DatabaseContext>();

            await _context.Database.EnsureCreatedAsync();

            script = File.ReadAllText("BaseDataInDP.sql");
            await _context.Database.ExecuteSqlRawAsync(script);
        }

    }
}

[CollectionDefinition(TestConstants.COLLECTION_NAME, DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<WebAppFactory>
{
}


