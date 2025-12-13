namespace IAMModule.User.Features.UserDelete;

internal class UserDeleteHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<UserDeleteCommand, bool>
{
    public async Task<bool> Handle(UserDeleteCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(command.UserId);
        await userManager.DeleteAsync(user);
        return true;
    }
}
