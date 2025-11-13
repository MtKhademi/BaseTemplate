using UserManagementModule.IAM.Services;

namespace UserManagementModule.IAM.Features.SignOut;

internal class SignOutHandler : ICommandHandler<SignOutCommand, bool>
{
    private readonly ILogger<SignOutHandler> _logger;
    private readonly ITokenService _tokenService;

    public SignOutHandler(ITokenService tokenService, ILogger<SignOutHandler> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<bool> Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        return await _tokenService.SignOutAsync(command.UserId);
    }
}
