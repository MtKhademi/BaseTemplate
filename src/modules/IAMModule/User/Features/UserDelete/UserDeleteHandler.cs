namespace IAMModule.User.Features.UserDelete;

internal class UserDeleteHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<UserDeleteCommand, Unit>
{
    public async Task<Unit> Handle(UserDeleteCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(command.UserId);
        await userManager.DeleteAsync(user);
        return Unit.Value;
    }


    internal class UserDeleteHandlerException : NotValidDataException<UserDeleteHandlerException>
    {
        public UserDeleteHandlerException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
