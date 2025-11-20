namespace IAMModule.Role.Features.RoleCreate;

internal class RoleCreateHandler(RoleManager<ApplicationRole> roleManager)
    : ICommandHandler<RoleCreateCommand, RoleModel>
{
    public async Task<RoleModel> Handle(RoleCreateCommand command, CancellationToken cancellationToken)
    {
        if (await roleManager.RoleExistsAsync(command.Name))
        {
            throw new RoleWithNameAlreadyExistException(command.Name);
        }

        var role = new ApplicationRole
        {
            Name = command.Name,
            Description = command.Description
        };

        await roleManager.CreateAsync(role);

        return role.ToRoleModel();
    }
}
