using System.Collections.ObjectModel;

namespace IAMModule.Authorization;

public record AppPermission(string Feature, string Action, string Group, string Description, bool IsBasic = false)
{
    public string Name => NameFor(Feature, Action);
    public static string NameFor(string feature, string action)
    => $"Permission.{feature}.{action}";
}

public class AppPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new AppPermission(AppFeature.IAMModule,AppActions.Create,AppRoleGroup.SystemAccess,$"{nameof(AppActions.Read)} data from {nameof(AppFeature.IAMModule)}"),
        new AppPermission(AppFeature.IAMModule,AppActions.Read,AppRoleGroup.SystemAccess,$"{nameof(AppActions.Read)} data from {nameof(AppFeature.IAMModule)}"),
        new AppPermission(AppFeature.IAMModule,AppActions.Update,AppRoleGroup.SystemAccess,$"{nameof(AppActions.Read)} data from {nameof(AppFeature.IAMModule)}"),
        new AppPermission(AppFeature.IAMModule,AppActions.Delete,AppRoleGroup.SystemAccess,$"{nameof(AppActions.Read)} data from {nameof(AppFeature.IAMModule)}"),
    };

    public static IReadOnlyList<AppPermission> AdminPermissions
        => new ReadOnlyCollection<AppPermission>(_all.ToArray());

    public static IReadOnlyList<AppPermission> BasicPermissions
        => new ReadOnlyCollection<AppPermission>(_all.Where(per => per.IsBasic).ToArray());
}