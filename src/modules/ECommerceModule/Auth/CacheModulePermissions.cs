using Infrastructure.Module;

namespace ECommerceModule.Auth;

internal class ECommerceModulePermissions : IModulePermission
{
    public AppModule Module => ECommercePermissions.ECommerceModule;
    private readonly List<ApiPermission> _features = new();
    public IEnumerable<ApiPermission> Features => _features;
    public void AddFeature(ApiPermission permission) => _features.Add(permission);
}
