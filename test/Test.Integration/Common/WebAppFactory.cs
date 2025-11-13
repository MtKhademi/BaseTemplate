namespace Test.Integration.Common;

public class WebAppFactory : WebApplicationFactory<Api.Program>, IAsyncLifetime
{
    private MsSqlContainer _dbContainer;
    //public UserManagementModel User => new UserManagementModel
    //{
    //    Name = TestConstants.USER_NAME
    //};
    public WebAppFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .Build();

    }
    //public Mock<IJWTUserHandlerService> IjwtUserHandlerServiceMoq
    //{
    //    get
    //    {
    //        var IjwtUserMock = new Mock<IJWTUserHandlerService>();
    //        IjwtUserMock.Setup(x => x.GetModelFromToken(It.IsAny<string>())).Returns(User);

    //        return IjwtUserMock;
    //    }
    //}

    internal async Task ClearDbAsync()
    {
        //var scope = Services.CreateScope();
        //var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
        //var script = File.ReadAllText("ClearDb.sql");
        //await dbContext.Database.ExecuteSqlRawAsync(script);
    }

    //internal DatabaseRepositoryFixture Repositories
    //{
    //    get
    //    {
    //        var scope = Services.CreateScope();
    //        var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
    //        return new DatabaseRepositoryFixture(dbContext);
    //    }
    //}

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connnectionString = _dbContainer.GetConnectionString()
            .Replace("master", "templatetestDb");

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


            //services.RemoveAll<IJWTUserHandlerService>();
            //services.AddScoped(_ => IjwtUserHandlerServiceMoq.Object);

            //services.RemoveAll<IMfHubService>();
            //services.AddScoped(_ => MfHubServiceMoq.Object);


            //var descriptorBGServices = services
            //.Where(x => !x.IsKeyedService)
            //.Where(x => x.ImplementationType == typeof(FundNavGetFromRabbitMQAndSaveBGService)).ToList();
            //if (descriptorBGServices.Any())
            //    descriptorBGServices.ForEach(des => services.Remove(des));
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

        //var script = File.ReadAllText("DUMP2.sql");
        //var result = await _dbContainer.ExecScriptAsync(script);

        //using (var scope = Services.CreateScope())
        //{
        //    var scopeProvider = scope.ServiceProvider;
        //    var _context = scopeProvider.GetRequiredService<DatabaseContext>();

        //    await _context.Database.EnsureCreatedAsync();

        //    script = File.ReadAllText("BaseDataInDP.sql");
        //    await _context.Database.ExecuteSqlRawAsync(script);
        //}

    }
}

[CollectionDefinition("Collection tests v1", DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<WebAppFactory>
{
}


