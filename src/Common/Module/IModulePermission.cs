using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Module;

public interface IModulePermission
{
    public AppModule Module { get; }
    public IEnumerable<ApiPermission> Features { get; }
    public void AddFeature(ApiPermission permission);
}

public static class ApplicationBuilderExtensions
{
    public static void PopulatePermissionsFromEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.DataSources.SelectMany(ds => ds.Endpoints);

        using var scope = app.ServiceProvider.CreateScope();
        var moduleProviders = scope.ServiceProvider
            .GetServices<IModulePermission>()
            .ToList();

        foreach (var endpoint in endpoints)
        {
            var permissionAttr = endpoint.Metadata
                .GetMetadata<ApiPermissionAttribute>();
            if (permissionAttr == null)
                continue;

            var provider = moduleProviders
                .FirstOrDefault(p => p.Module.Equals(permissionAttr.Permission.Module));
            if (provider == null)
                continue;


            var apiPermission = permissionAttr.Permission;

            if (apiPermission.description is null)
            {
                // Try to get summary from IEndpointSummaryMetadata
                var summaryMeta = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Http.Metadata.IEndpointSummaryMetadata>();

                if (!string.IsNullOrWhiteSpace(summaryMeta?.Summary))
                {
                    apiPermission = apiPermission with { description = summaryMeta?.Summary };
                }
            }
            provider.AddFeature(permissionAttr.Permission);
        }
    }
}
