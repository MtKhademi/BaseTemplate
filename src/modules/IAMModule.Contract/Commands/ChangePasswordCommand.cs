namespace IAMModule.Contract.Commands;

public record ChangePasswordCommand : ICommand<bool>
{
    public string UserId { get; init; }
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }


    public ChangePasswordCommand(string userId, string currentPassword, string newPassword, string confirmNewPassword)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(userId))
            errors.Add($"{nameof(userId)} is required.");

        if (string.IsNullOrWhiteSpace(currentPassword))
            errors.Add($"{nameof(currentPassword)} is required.");

        if (string.IsNullOrWhiteSpace(newPassword))
            errors.Add($"{nameof(newPassword)} is required.");

        if (newPassword != confirmNewPassword)
            errors.Add($"{nameof(newPassword)} and {nameof(confirmNewPassword)} do not match.");

        if (errors.Any())
            throw new ChangePasswordCommandException(errors);

        UserId = userId;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
        ConfirmNewPassword = confirmNewPassword;

    }


    internal class ChangePasswordCommandException : NotValidDataException<ChangePasswordCommandException>
    {
        public ChangePasswordCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}