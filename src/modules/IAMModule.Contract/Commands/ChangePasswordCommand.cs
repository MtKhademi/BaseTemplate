namespace IAMModule.Contract.Commands;

public class ChangePasswordCommand : ICommand<bool>
{
    public string UserId { get; init; }
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }


    public ChangePasswordCommand(string userId, string currentPassword, string newPassword, string confirmNewPassword)
    {
        UserId = userId;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
        ConfirmNewPassword = confirmNewPassword;

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(UserId))
            errors.Add("User ID is required.");

        if (string.IsNullOrWhiteSpace(CurrentPassword))
            errors.Add("Current password is required.");

        if (string.IsNullOrWhiteSpace(NewPassword))
            errors.Add("New password is required.");

        if (NewPassword != ConfirmNewPassword)
            errors.Add("New password and confirmation do not match.");

        if (errors.Any())
            throw new ChangePasswordCommandException(errors);
    }


    internal class ChangePasswordCommandException : NotValidDataException<ChangePasswordCommandException>
    {
        public ChangePasswordCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}