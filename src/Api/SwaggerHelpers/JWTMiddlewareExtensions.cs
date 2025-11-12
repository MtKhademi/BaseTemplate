using Swashbuckle.AspNetCore.SwaggerUI;

namespace Api.SwaggerHelpers;

public static class JWTMiddlewareExtensions
{
    public static IApplicationBuilder UseSwaggerUIApiVersioning(this WebApplication builder)
    {

        //var provider = builder.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

        //builder.UseSwaggerUI(options =>
        //{
        //    foreach (var description in provider.ApiVersionDescriptions)
        //    {
        //        options.SwaggerEndpoint(
        //            $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        //    }
        //});

        //return builder;

        builder.UseSwaggerUI(options => ConfigureUI(builder, options));
        return builder;

    }

    private static void ConfigureUI(IEndpointRouteBuilder app, SwaggerUIOptions options)
    {
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    }
}
