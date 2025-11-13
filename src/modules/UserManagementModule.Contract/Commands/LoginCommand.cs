using UserManagementModule.Contract.Models;

namespace UserManagementModule.Contract.Commands;

public record LoginCommand : ICommand<TokenModel>
{
    public string UserName { get; }
    public string Password { get; }
    public LoginCommand(string userName, string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(userName))
            errors.Add("UserName is required.");

        if (string.IsNullOrWhiteSpace(password))
            errors.Add("Password is required.");

        if (errors.Any())
            throw new LoginCommandException(errors);

        UserName = userName!;
        Password = password!;
    }


    internal class LoginCommandException : NotValidDataException<LoginCommandException>
    {
        public LoginCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}