namespace IAMModule.User.Features.UserChangeStateActive;

internal class UserChangeStateActiveHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<UserChangeStateActiveCommand, ApplicationUserModel>
{
    public async Task<ApplicationUserModel> Handle(UserChangeStateActiveCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(command.UserId);

        user.IsActive = !user.IsActive;
        await userManager.UpdateAsync(user);
        return user.ToModel();
    }
}
