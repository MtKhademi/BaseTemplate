namespace IAMModule.Contract.Requests;

public record ChangePasswordRequest(
    string? CurrentPassword = default!,
    string? NewPassword = default!,
    string? ConfirmNewPassword = default!)
{
    public ChangePasswordCommand ToCommand(string userId)
        => new ChangePasswordCommand(
            userId: userId,
            currentPassword: CurrentPassword,
            newPassword: NewPassword,
            confirmNewPassword: ConfirmNewPassword
        );
}