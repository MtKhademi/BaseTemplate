namespace Infrastructure.Module;

public record ApiPermission(
    AppModule Module,
    AppFeature Feature,
    ActionType Action,
    string? description = default!)
{
    public string Name => NameFor(Module, Feature, Action);
    public static string NameFor(AppModule module, AppFeature feature, ActionType action)
    => $"Permission.{module.Name}.{feature.Name}.{action}";


    public ApiPermissionAttribute ToApiPermission()
        => new ApiPermissionAttribute(this);
}
