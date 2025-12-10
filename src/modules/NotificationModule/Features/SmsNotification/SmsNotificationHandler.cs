//namespace NotificationModule.Features.SmsNotification;

//internal class SmsNotificationHandler(UserManager<ApplicationUser> userManager)
//    : ICommandHandler<SmsNotificationCommand, ApplicationUserModel>
//{
//    public async Task<ApplicationUserModel> Handle(SmsNotificationCommand command, CancellationToken cancellationToken)
//    {
//        await userManager.CheckExistUserNameOrThrowAsync(command.UserName);

//        if (!string.IsNullOrWhiteSpace(command.Email))
//            await userManager.CheckExistEmailAndThrowAsync(command.Email);

//        if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
//            await userManager.CheckExistPhoneOrThrowAsync(command.PhoneNumber);

//        var newUser = new ApplicationUser
//        {
//            Email = command.Email,
//            LastName = command.LastName ?? "",
//            FirstName = command.FirstName ?? "",
//            UserName = command.UserName,
//            PhoneNumber = command.PhoneNumber,
//            IsActive = true,
//            EmailConfirmed = false,
//            RefreshToken = ""
//        };

//        var userResult = await userManager.CreateAsync(newUser, command.Password);

//        if (!userResult.Succeeded)
//            throw new SmsNotificationHandlerException(userResult);

//        await userManager.AddToRoleAsync(newUser, AppRoles.Basic);

//        return newUser.ToModel();
//    }

//    internal class SmsNotificationHandlerException : NotValidDataException<SmsNotificationHandlerException>
//    {
//        public SmsNotificationHandlerException(IdentityResult result)
//            : this(result.Errors.Select(x => x.Description))
//        {
            
//        }
//        public SmsNotificationHandlerException(IEnumerable<string> errors) : base(errors)
//        {
//        }
//    }
//}
