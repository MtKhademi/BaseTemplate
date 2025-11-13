namespace UserManagementModule.Contract.Requests;

public class ChangePasswordRequest
{
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }


    public void Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(CurrentPassword))
            errors.Add("Current password is required.");

        if (string.IsNullOrWhiteSpace(NewPassword))
            errors.Add("New password is required.");

        if (NewPassword != ConfirmNewPassword)
            errors.Add("New password and confirmation do not match.");

        if (errors.Any())
            throw new ChangePasswordRequestException(errors);
    }

    public ChangePasswordCommand ToCommand(string userId)
    {
        Validate();
        return new ChangePasswordCommand(
            userId,
            CurrentPassword,
            NewPassword,
            ConfirmNewPassword
        );
    }


    internal class ChangePasswordRequestException : NotValidDataException<ChangePasswordRequestException>
    {
        public ChangePasswordRequestException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}