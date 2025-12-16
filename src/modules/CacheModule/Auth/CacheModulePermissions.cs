using CacheModule.Auth;
using Infrastructure.Module;

namespace IAMModule.Auth.Authorization;

internal class CacheModulePermissions : IModulePermission
{
    public AppModule Module => CachePermissions.CacheModule;
    private readonly List<ApiPermission> _features = new();
    public IEnumerable<ApiPermission> Features => _features;
    public void AddFeature(ApiPermission permission) => _features.Add(permission);
}
