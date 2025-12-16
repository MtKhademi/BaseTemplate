using IAMModule.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IAMModule.Auth.Authorization;

internal sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _scopeFactory;
    public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // 1️⃣ User must be authenticated
        if (!context.User.Identity?.IsAuthenticated ?? true)
            return;

        // 2️⃣ Get UserId
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return;

        using var scope = _scopeFactory.CreateScope();

        var _permissionService = scope.ServiceProvider
            .GetRequiredService<IUserPermissionRepository>();

        if (await _permissionService.HasPermissionByUserIdAsync(userId, requirement.PermissionName))
        {
            context.Succeed(requirement);
        }
    }
}
