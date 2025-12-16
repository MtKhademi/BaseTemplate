using Infrastructure.Module;

namespace IAMModule.IAM.Authorization;

internal static class IAMPermissions
{
    internal static AppModule IamModule = new AppModule("IAM", "Identity and Access Management Module");

    internal static AppFeature AccessControlFeature = new AppFeature("Access control", "Access control Management Feature");
    internal static AppFeature RoleFeature = new AppFeature("Role", "Role Management Feature");
    internal static AppFeature UserFeature = new AppFeature("User", "User Management Feature");


    //----------- User permissions
    internal static ApiPermission UserCreate => new ApiPermission(IamModule, UserFeature, ActionType.Create);
    internal static ApiPermission UserRead => new ApiPermission(IamModule, UserFeature, ActionType.Read);
    internal static ApiPermission UserUpdate => new ApiPermission(IamModule, UserFeature, ActionType.Update);
    internal static ApiPermission UserDelete => new ApiPermission(IamModule, UserFeature, ActionType.Delete);
    internal static ApiPermission UserRoleChange => new ApiPermission(IamModule, UserFeature, ActionType.Update);
    internal static ApiPermission UserRoles => new ApiPermission(IamModule, UserFeature, ActionType.Read);
    internal static ApiPermission UserChangeState => new ApiPermission(IamModule, UserFeature, ActionType.Update);

    //----------- Role permissions
    internal static ApiPermission RoleCreate => new ApiPermission(IamModule, RoleFeature, ActionType.Create);
    internal static ApiPermission RoleRead => new ApiPermission(IamModule, RoleFeature, ActionType.Read);
    internal static ApiPermission RoleUpdate => new ApiPermission(IamModule, RoleFeature, ActionType.Update);
    internal static ApiPermission RoleDelete => new ApiPermission(IamModule, RoleFeature, ActionType.Delete);


    //----------- Access control permissions
    internal static ApiPermission AccessControlRead => new ApiPermission(IamModule, AccessControlFeature, ActionType.Read);

}
