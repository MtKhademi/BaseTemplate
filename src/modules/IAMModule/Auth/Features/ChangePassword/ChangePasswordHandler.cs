namespace IAMModule.IAM.Features.ChangePassword;

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if (userInDb is null)
            throw new UserNotFoundWithUserIdException(request.UserId);

        var changeResult =
            await _userManager.ChangePasswordAsync(userInDb, request.CurrentPassword, request.NewPassword);
        if (changeResult.Succeeded)
            return true;

        throw new ChangePasswordNotSuccessException(changeResult);
    }
}