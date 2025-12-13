using IAMModule;
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
    .AddNotificationModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option
    .AddSwaggerIAMModule()
    .AddSwaggerNotificationModule();
});


var app = builder.Build();

app.MapCarter();
app.UseRouting();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.UseSwaggerIAMModule()
    .UseSwaggerNotificationModule();
});


app.UseSerilogRequestLogging();
app.UseDeveloperExceptionPage();


app.UseStaticFiles();


app.UseCorsConfig();


app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();



app.UseIAMModule()
    .UseNotificationModule();


app.Run();
