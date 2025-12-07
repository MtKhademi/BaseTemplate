namespace IAMModule.User.Features.UserUpdate;

internal class UserUpdateHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<UserUpdateCommand, ApplicationUserModel>
{
    public async Task<ApplicationUserModel> Handle(UserUpdateCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(command.UserId);

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var users = await userManager.Users.ToListAsync();
            var userEmail = await userManager.FindByEmailAsync(command.Email);
            if (userEmail is not null && userEmail.Id != user.Id)
                throw new UserUpdateHandlerException(new[] { "Email is already taken" });
        }

        if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            var userPhone = await userManager.FindByPhoneAsync(command.PhoneNumber);
            if (userPhone is not null && userPhone.Id != user.Id)
                throw new UserUpdateHandlerException(new[] { "Phone number is already taken" });
        }

        if (!string.IsNullOrWhiteSpace(command.UserName))
        {
            var userName = await userManager.FindByNameAsync(command.UserName);
            if (userName is not null && userName.Id != user.Id)
                throw new UserUpdateHandlerException(new[] { "Username is already taken" });
        }

        user.Update(command);
        await userManager.UpdateAsync(user);
        return user.ToModel();
    }


    internal class UserUpdateHandlerException : NotValidDataException<UserUpdateHandlerException>
    {
        public UserUpdateHandlerException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
