using IAMModule.Services;

namespace IAMModule.Auth.Features.SignOut;

internal class SignOutHandler(ITokenService tokenService) : ICommandHandler<SignOutCommand, bool>
{
    public async Task<bool> Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        return await tokenService.SignOutAsync(command.UserId);
    }
}
