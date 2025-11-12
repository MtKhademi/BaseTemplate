
using IAMModule.Contract.Models;

namespace IAMModule.Features.Login;

internal class LoginHandler : ICommandHandler<LoginCommand, TokenModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IAMModuleConfig _UserManagementConfig;
    private readonly ITokenService _tokenService;

    public LoginHandler(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IAMModuleConfig> userManagementConfig,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _UserManagementConfig = userManagementConfig.Value;
        _tokenService = tokenService;
    }

    public async Task<TokenModel> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(command.UserName);
        if (user is null)
            throw new UserNotFoundWithUserNameException(command.UserName);

        if (!user.IsActive)
            throw new UserDontActiveException(command.UserName);

        if (!user.EmailConfirmed)
            throw new UserNotEmailConfirmException(command.UserName);

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!isPasswordValid)
            throw new UserNotCorrectPasswordException(command.UserName);

        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryDate = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(user);

        var accessToken = await _tokenService.GenerateTokenAsync(user);

        return new TokenModel(
            userId: user.Id,
            Token: accessToken,
            RefreshToken: user.RefreshToken,
            RefreshTokenExpiryTime: user.RefreshTokenExpiryDate
        );
        //{
        //    Token = accessToken,
        //    RefreshToken = user.RefreshToken,
        //    RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
        //};
    }


}
