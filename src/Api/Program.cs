using CacheModule;
using IAMModule;
using Infrastructure.Module;
using NotificationModule;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1048576000;
    serverOptions.ListenAnyIP(8585); // Set default port to 5000
});

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

builder.WebHost.UseKestrel().UseIIS();

builder.Services.AddCorsConfig();

builder.Services.AddVersioningConfig();

builder.Services
    .AddIAMModule(builder.Configuration)
    .AddNotificationModule(builder.Configuration)
    .AddCacheModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option
    .AddSwaggerIAMModule()
    .AddSwaggerNotificationModule()
    .AddSwaggerCacheModule();
});


var app = builder.Build();

app.MapCarter();
app.UseRouting();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.UseSwaggerIAMModule()
    .UseSwaggerNotificationModule()
    .UseSwaggerCacheModule();
});


app.UseSerilogRequestLogging();
app.UseDeveloperExceptionPage();


app.UseStaticFiles();


app.UseCorsConfig();


app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();



app.PopulatePermissionsFromEndpoints();
app.UseIAMModule()
    .UseNotificationModule()
    .UseCacheModule();


app.Run();
