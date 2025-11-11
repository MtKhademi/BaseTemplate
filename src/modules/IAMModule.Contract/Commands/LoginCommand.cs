using IAMModule.Contract.Models;

namespace IAMModule.Contract.Commands;

public class LoginCommand : ICommand<TokenModel>
{
    public string UserName { get; init; }
    public string Password { get; init; }

    public LoginCommand(string userName, string password)
    {

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(userName))
        {
            errors.Add("UserName is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Password is required.");
        }

        if (errors.Any())
        {
            throw new LoginCommandException(errors);
        }

        UserName = userName;
        Password = password;

    }

    internal class LoginCommandException : NotValidDataException<LoginCommandException>
    {
        public LoginCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}