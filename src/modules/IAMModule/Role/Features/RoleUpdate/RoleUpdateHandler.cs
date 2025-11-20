using IAMModule.Extensions;

namespace IAMModule.Role.Features.RoleUpdate;

internal class RoleUpdateHandler(RoleManager<ApplicationRole> roleManager)
    : ICommandHandler<RoleUpdateCommand, RoleModel>
{
    public async Task<RoleModel> Handle(RoleUpdateCommand command, CancellationToken cancellationToken)
    {
        var roleEntity = await roleManager.FindByIdOrThrowAsync(command.RoleId);

        var roleWithSameName = await roleManager.FindByNameAsync(command.Name);
        if (roleWithSameName != null)
        {
            if (roleWithSameName.Id != roleEntity.Id)
            {
                throw new RoleWithNameAlreadyExistException(command.Name);
            }
        }

        roleEntity.Name = command.Name;
        roleEntity.Description = command.Description;

        await roleManager.UpdateAsync(roleEntity);

        return roleEntity.ToRoleModel();
    }
}
