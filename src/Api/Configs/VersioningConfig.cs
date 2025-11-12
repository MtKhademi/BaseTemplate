using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Configs
{
    internal static class VersioningConfig
    {
        internal static IServiceCollection AddVersioningConfig(this IServiceCollection services)
        {
            services
                .AddApiVersioning(opt =>
                    {
                        opt.DefaultApiVersion = new ApiVersion(1, 0);
                        opt.AssumeDefaultVersionWhenUnspecified = true;
                        opt.ReportApiVersions = true;
                        opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                        new HeaderApiVersionReader("x-api-version"),
                                                                        new MediaTypeApiVersionReader("x-api-version"));
                    })
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
