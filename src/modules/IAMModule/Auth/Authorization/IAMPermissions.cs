using Infrastructure.Auth;

namespace IAMModule.IAM.Authorization;

public static class IAMPermissions
{
    public const string Module = nameof(IAMModule);
    public const string RoleFeature = "Role";


    public static AppPermission UserCreate => new AppPermission(Module, RoleFeature, AppAction.Create);
    public static AppPermission UserRead => new AppPermission(Module, RoleFeature, AppAction.Read);
    public static AppPermission UserUpdate => new AppPermission(Module, RoleFeature, AppAction.Update);
    public static AppPermission UserDelete => new AppPermission(Module, RoleFeature, AppAction.Delete);
    public static AppPermission UserRoleChange => new AppPermission(Module, RoleFeature, AppAction.Update);
    public static AppPermission UserRoles => new AppPermission(Module, RoleFeature, AppAction.Read);
    public static AppPermission UserChangeState => new AppPermission(Module, RoleFeature, AppAction.Update);

    
    


    public static AppPermission RoleCreate => new AppPermission(Module, RoleFeature, AppAction.Create);
    public static AppPermission RoleRead => new AppPermission(Module, RoleFeature, AppAction.Read);
    public static AppPermission RoleUpdate => new AppPermission(Module, RoleFeature, AppAction.Update);
    public static AppPermission RoleDelete => new AppPermission(Module, RoleFeature, AppAction.Delete);
}
