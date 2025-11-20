using CacheModule;
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

//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("Redis");
//});

//builder.Services.AddControllers()
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.Converters.Add(new StringEnumConverter());
//        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
//        options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
//    });

builder.Services
    .AddIAMModule(builder.Configuration)
    .AddCacheModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option
    .AddSwaggerIAMModule()
    .AddSwaggerCacheModule();
});
builder.Services.AddVersioningConfig();


var app = builder.Build();

app.MapCarter();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.UseSwaggerIAMModule()
        .UseSwaggerCacheModule();

});


app.UseSerilogRequestLogging();
app.UseDeveloperExceptionPage();


app.UseStaticFiles();
app.UseRouting();
app.UseCorsConfig();

//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();

app.UseIAMModule()
    .UseCacheModule();

app.Run();
