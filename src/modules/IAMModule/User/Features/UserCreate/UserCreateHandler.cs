namespace IAMModule.User.Features.UserCreate;

internal class UserCreateHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<UserCreateCommand, ApplicationUserModel>
{
    public async Task<ApplicationUserModel> Handle(UserCreateCommand command, CancellationToken cancellationToken)
    {
        await userManager.CheckExistUserNameOrThrowAsync(command.UserName);

        if (!string.IsNullOrWhiteSpace(command.Email))
            await userManager.CheckExistEmailAndThrowAsync(command.Email);

        if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
            await userManager.CheckExistPhoneOrThrowAsync(command.PhoneNumber);

        var newUser = new ApplicationUser
        {
            Email = command.Email,
            LastName = command.LastName ?? "",
            FirstName = command.FirstName ?? "",
            UserName = command.UserName,
            PhoneNumber = command.PhoneNumber,
            IsActive = true,
            EmailConfirmed = false,
            RefreshToken = ""
        };

        var userResult = await userManager.CreateAsync(newUser, command.Password);

        if (!userResult.Succeeded)
            throw new UserCreateHandlerException(userResult);

        await userManager.AddToRoleAsync(newUser, AppRoles.Basic);

        return newUser.ToModel();
    }

    internal class UserCreateHandlerException : NotValidDataException<UserCreateHandlerException>
    {
        public UserCreateHandlerException(IdentityResult result)
            : this(result.Errors.Select(x => x.Description))
        {
            
        }
        public UserCreateHandlerException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
