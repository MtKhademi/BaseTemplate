using IAMModule.IAM.Authorization;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Auth.Authorization;

public interface IAppModulePermission
{
    public AppModule Module { get; }
    public IEnumerable<AppPermission> Features { get; }
    public void AddFeature(AppPermission permission);
}

public static class ApplicationBuilderExtensions
{
    public static void PopulatePermissionsFromEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.DataSources.SelectMany(ds => ds.Endpoints);

        using var scope = app.ServiceProvider.CreateScope();
        var moduleProviders = scope.ServiceProvider
            .GetServices<IAppModulePermission>()
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

            provider.AddFeature(permissionAttr.Permission);
        }
    }
}
