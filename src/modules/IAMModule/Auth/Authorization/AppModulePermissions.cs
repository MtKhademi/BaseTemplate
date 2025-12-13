using Infrastructure.Auth.Authorization;

namespace IAMModule.Auth.Authorization;

internal class AppModulePermissions : IAppModulePermission
{
    public AppModule Module => IAMPermissions.Module;
    private readonly List<AppPermission> _features = new();
    public IEnumerable<AppPermission> Features => _features;
    public void AddFeature(AppPermission permission) => _features.Add(permission);
}
