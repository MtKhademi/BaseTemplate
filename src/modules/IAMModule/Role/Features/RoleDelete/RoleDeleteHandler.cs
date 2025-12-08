namespace IAMModule.Role.Features.RoleDelete;

internal class RoleDeleteHandler(RoleManager<ApplicationRole> roleManager)
    : ICommandHandler<RoleDeleteCommand, bool>
{
    public async Task<bool> Handle(RoleDeleteCommand command, CancellationToken cancellationToken)
    {
        var roleEntity = await roleManager.FindByIdOrThrowAsync(command.RoleId);
        await roleManager.DeleteAsync(roleEntity);
        return true;
    }
}
