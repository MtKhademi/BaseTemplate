using IAMModule.IAM.Authorization;

namespace Infrastructure.Auth.Authorization;

public record AppPermission(
    AppModule Module,
    AppFeature Feature,
    AppAction Action,
    string? description = default!)
{
    public string Name => NameFor(Module, Feature, Action);
    public static string NameFor(AppModule module, AppFeature feature, AppAction action)
    => $"Permission.{module.Name}.{feature.Name}.{action}";


    public ApiPermissionAttribute ToApiPermission()
        => new ApiPermissionAttribute(this);
}
