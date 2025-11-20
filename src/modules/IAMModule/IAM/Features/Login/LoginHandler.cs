using IAMModule.IAM.Extentions;
using IAMModule.IAM.Services;

namespace IAMModule.IAM.Features.Login;

internal class LoginHandler : ICommandHandler<LoginCommand, TokenModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<TokenModel> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameOrThrowAsync(command.UserName);

        if (!user.IsActive)
            throw new UserDontActiveException(command.UserName);

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
    }
}
