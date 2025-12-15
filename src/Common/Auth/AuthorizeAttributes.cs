using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Auth;


public class ApiPermissionAttribute : AuthorizeAttribute
{
    public AppPermission Permission { get; init; }
    public ApiPermissionAttribute(
        AppModule module,
        AppFeature feature,
        AppAction action) : this(new AppPermission(module, feature, action))
    {
    }

    public ApiPermissionAttribute(AppPermission permission)
    {
        Permission = permission;
        Policy = permission.Name;
    }
}

