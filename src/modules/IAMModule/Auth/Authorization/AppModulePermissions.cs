namespace IAMModule.Auth.Authorization;

internal class AppModulePermissions : IModulePermission
{
    public AppModule Module => IAMPermissions.IamModule;
    private readonly List<ApiPermission> _features = new();
    public IEnumerable<ApiPermission> Features => _features;
    public void AddFeature(ApiPermission permission) => _features.Add(permission);
}
