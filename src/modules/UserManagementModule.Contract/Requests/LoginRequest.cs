namespace UserManagementModule.Contract.Requests;

public record LoginRequest(string? UserName, string? Password)
{


    public LoginCommand ToCommand()
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(UserName))
            errors.Add($"{nameof(UserName)} is required.");

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            errors.Add($"{nameof(Password)} is invalid. It must be at least 6 characters long.");

        if (errors.Any())
            throw new LoginRequestException(errors);

        return new LoginCommand(UserName!, Password!);
    }

    internal class LoginRequestException : NotValidDataException<LoginRequestException>
    {
        public LoginRequestException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}