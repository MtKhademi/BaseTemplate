using Microsoft.AspNetCore.Authorization;

namespace IAMModule.Auth.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }
    public PermissionRequirement(string permission) => PermissionName = permission;
}

