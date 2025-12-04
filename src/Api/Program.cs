using IAMModule;

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

builder.Services
    .AddIAMModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option
    .AddSwaggerIAMModule();
});
builder.Services.AddVersioningConfig();


var app = builder.Build();

app.MapCarter();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.UseSwaggerIAMModule();
});


app.UseSerilogRequestLogging();
app.UseDeveloperExceptionPage();


app.UseStaticFiles();
app.UseRouting();
app.UseCorsConfig();


app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();

app.UseIAMModule();

app.Run();
