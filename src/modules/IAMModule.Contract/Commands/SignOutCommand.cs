namespace IAMModule.Contract.Commands;

public class SignOutCommand : ICommand<bool>
{
    public string UserId { get; init; }
    public SignOutCommand(string userId)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(userId))
            errors.Add("UserId is required.");

        if (errors.Any())
            throw new SignOutCommandException(errors);

        UserId = userId;
    }

    internal class SignOutCommandException : NotValidDataException<SignOutCommandException>
    {
        public SignOutCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}