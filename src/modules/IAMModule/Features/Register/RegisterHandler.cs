using IAMModule.Contract.Models;

namespace IAMModule.Features.Login;


internal class RegisterHandler : ICommandHandler<UseRegistrationCommand, ApplicationUserModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IAMModuleConfig _UserManagementConfig;

    public RegisterHandler(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IAMModuleConfig> userManagementConfig)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _UserManagementConfig = userManagementConfig.Value;
    }

    public async Task<ApplicationUserModel> Handle(UseRegistrationCommand command, CancellationToken cancellationToken)
    {
        var userWithSameEmail = await _userManager.FindByEmailAsync(command.Email);

        if (userWithSameEmail is not null)
            throw new UserAlreadyExistWithEmailException(command.Email);

        var userWithSamePhone = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == command.PhoneNumber, cancellationToken);
        if (userWithSamePhone is not null)
            throw new UserAlreadyExistWithPhoneException(command.PhoneNumber);

        var userWithSameUserName = await _userManager.FindByNameAsync(command.UserName);
        if (userWithSameUserName is not null)
            throw new UserAlreadyExistWithUserNameException(command.UserName);

        var newUser = new ApplicationUser
        {
            Email = command.Email,
            LastName = command.LastName,
            FirstName = command.FirstName,
            UserName = command.UserName,
            PhoneNumber = command.PhoneNumber,
            IsActive = true,
            EmailConfirmed = true,
            RefreshToken = ""
        };

        var userResult = await _userManager.CreateAsync(newUser, command.Password);

        if (!userResult.Succeeded)
            throw new RegisterHandlerException(userResult.Errors.Select(x => x.Description));

        await _userManager.AddToRoleAsync(newUser, AppRoles.Basic);

        return newUser.ToModel();
    }


    internal class RegisterHandlerException : NotValidDataException<RegisterHandlerException>
    {
        public RegisterHandlerException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
