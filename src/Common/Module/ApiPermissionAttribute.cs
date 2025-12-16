using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Module;


public class ApiPermissionAttribute : AuthorizeAttribute
{
    public ApiPermission Permission { get; init; }
    public ApiPermissionAttribute(
        AppModule module,
        AppFeature feature,
        ActionType action) : this(new ApiPermission(module, feature, action))
    {
    }

    public ApiPermissionAttribute(ApiPermission permission)
    {
        Permission = permission;
        Policy = permission.Name;
    }
}

