using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.SwaggerHelpers;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerGenConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            // options.EnableAnnotations();

            options.SwaggerDoc("SolutionSolverV1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "Solution Solver - API",
                Version = "v1",
                // Description = "Solution Solver API"
            });

            options.SwaggerDoc("ECommerceV1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "Solution Solver - ECommerce API",
                Version = "v1",
                // Description = "Solution Solver API"
            });

            options.DocInclusionPredicate((version, apiDescription) =>
            {
                var groupName = apiDescription.GroupName;
                return groupName != null && groupName.Equals(version, StringComparison.OrdinalIgnoreCase);
            });

            options.DocumentFilter<PerGroupSecurityDocumentFilter>();
            options.OperationFilter<CustomSecurityRequirementsFilter>();

        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/SolutionSolverV1/swagger.json", "Solution Solver v1");
            options.SwaggerEndpoint("/swagger/ECommerceV1/swagger.json", "ECommerce v1 API");
            options.RoutePrefix = "swagger"; // Optional: Set Swagger UI as the root page
        });

        return app;
    }

}

public class CustomSecurityRequirementsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var groupName = context.ApiDescription.GroupName;

        var security = new List<OpenApiSecurityRequirement>();

        // همه نیاز به Bearer_Banking دارن
        //var bankingScheme = new OpenApiSecurityScheme
        //{
        //    Reference = new OpenApiReference
        //    {
        //        Type = ReferenceType.SecurityScheme,
        //        Id = "Bearer"
        //    }
        //};

        security.Add(new OpenApiSecurityRequirement
        {
            //[bankingScheme] = new List<string>()
        });

        operation.Security = security;
    }
}

public class PerGroupSecurityDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var securitySchemes = new Dictionary<string, OpenApiSecurityScheme>();

        // همیشه Banking رو اضافه کن
        securitySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "توکن عمومی برای همه APIها (Solution solver)",
            //Reference = new OpenApiReference
            //{
            //    Type = ReferenceType.SecurityScheme,
            //    Id = "Bearer"
            //}
        });


        //foreach (var scheme in securitySchemes)
        //{
        //    if (!swaggerDoc.Components.SecuritySchemes.ContainsKey(scheme.Key))
        //        swaggerDoc.Components.SecuritySchemes.Add(scheme.Key, scheme.Value);
        //}
    }
}

