using IAMModule.Extensions;

namespace IAMModule.Role.Features.RoleDelete;

internal class RoleDeleteHandler(RoleManager<ApplicationRole> roleManager)
    : ICommandHandler<RoleDeleteCommand, Unit>
{
    public async Task<Unit> Handle(RoleDeleteCommand command, CancellationToken cancellationToken)
    {
        var roleEntity = await roleManager.FindByIdOrThrowAsync(command.RoleId);
        await roleManager.DeleteAsync(roleEntity);
        return Unit.Value;
    }
}
