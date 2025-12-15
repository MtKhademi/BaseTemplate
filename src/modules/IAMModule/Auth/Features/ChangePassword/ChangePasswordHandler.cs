using IAMModule.Services;

namespace IAMModule.Auth.Features.ChangePassword;

public class ChangePasswordHandler(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    ILogger<ChangePasswordHandler> logger) : ICommandHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.CreateTransactionAsync();
        try
        {
            var user = await userManager.FindByIdOrThrowAsync(request.UserId);

            var changeResult =
                await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changeResult.Succeeded)
                throw new ChangePasswordNotSuccessException(changeResult);

            await tokenService.RemoveTokenAsync(user);

            await unitOfWork.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error changing password for user {UserId}", request.UserId);
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}