namespace IAMModule.User.Features.UserRoleChange;

internal class UserRoleChangeHandler(
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager)
    : ICommandHandler<UserRoleChangeCommand, Unit>
{
    public async Task<Unit> Handle(UserRoleChangeCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(command.UserId);

        using var transaction = await unitOfWork.CreateTransactionAsync();
        try
        {
            foreach (var roleId in command.RoleIds)
            {

                var role = await roleManager.FindByIdOrThrowAsync(roleId);

                var result = await userManager.AddToRoleAsync(user, role.Name!);
                if (result.Succeeded is false)
                    throw new UserRoleChangeHandlerException(result);
            }
        }
        catch 
        {
            transaction.Rollback();
            throw;
        }

        transaction.Commit();
        return Unit.Value;
    }


    internal class UserRoleChangeHandlerException : NotValidDataException<UserRoleChangeHandlerException>
    {
        public UserRoleChangeHandlerException(IdentityResult result)
            : this(result.Errors.Select(e => e.Description))
        {

        }
        public UserRoleChangeHandlerException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
