using IAMModule.Contract.Models;

namespace IAMModule.Features.Login;


internal class ExchangeRefreshTokenWithAccessTokenHandler : ICommandHandler<TokenCreateWithRefreshTokenCommand, TokenModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IAMModuleConfig _UserManagementConfig;

    private readonly IUnitOfWork _unitOfWork;

    public ExchangeRefreshTokenWithAccessTokenHandler(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IAMModuleConfig> userManagementConfig,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _UserManagementConfig = userManagementConfig.Value;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenModel> Handle(TokenCreateWithRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var userPrincipal = _tokenService.GetPrincipalFromExpiredToken(command.Token);
        var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null)
            throw new UserNotFoundWithEmailException(userEmail);
        if (user.RefreshToken != command.RefreshToken ||
            user.RefreshTokenExpiryDate <= DateTime.Now)
            throw new NotValidDataException<ExchangeRefreshTokenWithAccessTokenHandler>("Invalid refresh token.");


        await _unitOfWork.CreateTransactionAsync();

        var token = await _tokenService.GenerateTokenAsync(user);

        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryDate = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        await _unitOfWork.CommitAsync();

        return new TokenModel(
            userId: user.Id,
            Token: token,
            RefreshToken: user.RefreshToken,
            RefreshTokenExpiryTime: user.RefreshTokenExpiryDate
        );
        //{
        //    Token = token,
        //    RefreshToken = user.RefreshToken,
        //    RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
        //};
    }

}
