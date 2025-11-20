using IAMModule.Extensions;

namespace IAMModule.Role.Features.RoleGetByRoleId;

internal class RoleGetByRoleIdHandler(RoleManager<ApplicationRole> roleManager)
    : IQueryHandler<RoleGetByRoleIdQuery, RoleModel>
{
    public async Task<RoleModel> Handle(RoleGetByRoleIdQuery query, CancellationToken cancellationToken)
    {
        return (await roleManager.FindByIdOrThrowAsync(query.RoleId)).ToRoleModel();
    }
}
